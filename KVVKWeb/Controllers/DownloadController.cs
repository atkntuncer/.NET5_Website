using KVVKWeb.API;
using KVVKWeb.Models;
using KVVKWeb.Models.Dto;
using KVVKWeb.Models.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace KVVKWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class DownloadController : ControllerBase
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly UserManager<CustomerInfo> _userManager;
        private readonly SignInManager<CustomerInfo> _signInManager;
        private readonly IGetFromAPI _api;
        private readonly URLs _urls;

        public DownloadController(IWebHostEnvironment hostEnvironment, UserManager<CustomerInfo> userManager, SignInManager<CustomerInfo> signInManager, IOptions<URLs> urls, IGetFromAPI api)
        {
            _hostEnvironment = hostEnvironment;
            _userManager = userManager;
            _signInManager = signInManager;
            _api = api;
            _urls = urls.Value;
        }
        [Route("service")]
        [HttpGet]
        [Authorize(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme)]
        public async Task<string> DownloadService()
        {
            var servicePath = Path.Combine(_hostEnvironment.WebRootPath, "launcher", "KVKKAgentSetup.msi");
            var bytes = await System.IO.File.ReadAllBytesAsync(servicePath);
            string base64 = Convert.ToBase64String(bytes);
            return base64;
        }
        [Route("authenticate")]
        [HttpPost]
        public async Task<DownloadReturnModel> Authenticate([FromBody] DownloadAuthorizeModel dam)
        {
            var user = await _userManager.FindByEmailAsync(dam.Email);

            if (user != null)
            {
                var result = await _signInManager.CheckPasswordSignInAsync(user, dam.Password, false);
                if (result.Succeeded)
                {
                    var claims = new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sub,user.Id),
                        new Claim(JwtRegisteredClaimNames.Email,user.Email)
                    };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Constants.Secret));
                    var algorithm = SecurityAlgorithms.HmacSha256;

                    var credentials = new SigningCredentials(key,algorithm);

                    var token = new JwtSecurityToken(Constants.Issuer, Constants.Audience, claims,notBefore: DateTime.Now,expires: DateTime.Now.AddMinutes(10),credentials);

                    var tokenJson = new JwtSecurityTokenHandler().WriteToken(token);

                    DownloadReturnModel drm = new DownloadReturnModel
                    {
                        Email = user.Email,
                        ID = user.Id,
                        CustomerId = user.CustomerID,
                        TotalPackagesCount = GetTotalPackagesCount(user.Id),
                        RemainingPackagesCount = 0,
                        AccessToken=tokenJson
                    };
                    return drm;
                }
            }
            return null;
        }

        private int GetTotalPackagesCount(string userId)
        {
            List<Packages> LP = _api.GetMethod<List<Packages>>($"{_urls.Get.GetCurrentLicences}?customerid={userId}");
            int result = 0;
            if (LP != null)
            {
                foreach (var item in LP)
                {
                    result += item.ClientCount;
                }
            }
            return result;
        }
    }
}

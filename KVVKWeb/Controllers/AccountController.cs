using KVVKWeb.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KVVKWeb.Models.Dto;
using KVVKWeb.API;
using System.Security.Claims;
using System.Text.Json;
using KVVKWeb.Models;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace KVVKWeb.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<CustomerInfo> _userManager;
        private readonly SignInManager<CustomerInfo> _signInManager;
        private readonly IGetFromAPI _api;
        private readonly URLs _urls;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AccountController(UserManager<CustomerInfo> userManager, SignInManager<CustomerInfo> signInManager, IGetFromAPI api, IOptions<URLs> urls, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _api = api;
            _urls = urls.Value;
            _roleManager = roleManager;
        }
        [AllowAnonymous]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Create(RegisterModel crm)
        {
            if (ModelState.IsValid)
            {
                CustomerInfo ci = new();
                ci.Email = crm.Email;
                ci.UserName = crm.Email;
                ci.Name = crm.Name;
                ci.LastName = crm.LastName;
                ci.NationalID = crm.NationalID;
                ci.DateofBirth = crm.DateofBirth;
                ci.PhoneNumber = crm.Phone;
                ci.MobilePhone = crm.MobilePhone;
                ci.Adress = crm.Adress;
                ci.CompanyName = crm.CompanyName;
                ci.Status = false;
                ci.CustomerType = 1;
                var result = await _userManager.CreateAsync(ci, crm.Password);
                var roleResult = await _userManager.AddToRoleAsync(ci, "PassiveUser");
                if (result.Succeeded && roleResult.Succeeded)
                {
                    var confirmcode = await _userManager.GenerateEmailConfirmationTokenAsync(ci);
                    var EmailConfirmationUrl = Url.Action(
                 "ConfirmEmail", "Account",
                 values: new { userId = ci.Id, confirmcode = confirmcode },
                 protocol: Request.Scheme);
                    return Content("<html><a href=\"" + EmailConfirmationUrl + "\">" + "Click here for email confirmation" + "</a><html>", "text/html");
                }
                else
                {
                    result.Errors.ToList().ForEach(i => ModelState.AddModelError("", i.Description));

                }
            }
            return View(crm);
        }
        [AcceptVerbs("Get", "Post")]
        [AllowAnonymous]
        public async Task<IActionResult> IsEmailInUse(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return Json(true);
            }
            else
            {
                return Json($"Email {email} is already in use!");
            }
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string confirmcode)
        {
            if (userId == null || confirmcode == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                ViewBag.Message = "User is invalaid!";
                return View("Message");
            }
            var result = await _userManager.ConfirmEmailAsync(user, confirmcode);
            if (result.Succeeded)
            {
                return View();
            }
            return Content("Error");
        }
        [AllowAnonymous]
        public IActionResult Login(string returnurl)
        {
            ViewBag.ReturnUrl = returnurl;
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel lm)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(lm.Email, lm.Password, lm.RememberMe, true);
                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(lm.ReturnUrl))
                    {
                        return LocalRedirect(lm.ReturnUrl);
                    }
                    return RedirectToAction("Index", "Home");
                }
                else if (result.IsLockedOut)
                {
                    ViewBag.Message = "Too many bad attempt. Account is locked out for some time.";
                    return View("Message");
                }
                else
                {
                    ModelState.AddModelError("", "Wrong email address or password");
                    return View(lm);
                }
            }
            else
            {
                return View(lm);
            }

        }
        [AllowAnonymous]
        public IActionResult Logout()
        {
            if (User.Identity.IsAuthenticated)
            {
                _signInManager.SignOutAsync();
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index", "Home");

        }

        [Authorize]
        public IActionResult Buy(int id)
        {
            Packages P = _api.GetMethod<Packages>($"{_urls.Get.GetLicencePackages}?id={id}");
            return View(P);
        }
        [HttpPost]
        public async Task<IActionResult> Buy(Packages P)
        {
            bool roleControl = true;
            Licences L = new Licences();
            L.Status = true;
            L.CustomerID = _userManager.GetUserId(User);
            L.LicencePackageID = P.ID;
            L.StartDate = DateTime.UtcNow;
            L.EndDate = DateTime.UtcNow.AddYears(1);
            int returnValue = _api.PostMethod<int>(L, _urls.Add.AddCustomerLicences);//ekleme
            var user = await _userManager.FindByEmailAsync(User.Identity.Name);//çekme
            user.Status = true;
            var result = await _userManager.UpdateAsync(user);//düzenleme
            //var kontrol = await _userManager.GetRolesAsync(user);
            if (!User.IsInRole("ActiveUser"))
            {
                var roleResult = await _userManager.AddToRoleAsync(user, "ActiveUser");
                if (!roleResult.Succeeded)
                {
                    roleControl = false;
                }
            }
            if (returnValue == 1 && result.Succeeded && roleControl)
            {
                await _signInManager.RefreshSignInAsync(user);//logout olmadan etkiler gözüksün diye yapıldı
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Message = "Bir hata oluştu tekrar deneyiniz";
                return View("Message");
            }
        }
        public async Task<IActionResult> UserInfo()
        {
            var user = await _userManager.FindByEmailAsync(User.Identity.Name);
            ChangeInfoModel rm = new ChangeInfoModel
            {
                UserID = user.Id,
                Email = user.Email,
                Name = user.Name,
                LastName = user.LastName,
                NationalID = user.NationalID,
                DateofBirth = user.DateofBirth,
                Phone = user.PhoneNumber,
                MobilePhone = user.MobilePhone,
                Adress = user.Adress,
                CompanyName = user.CompanyName
            };
            return View(rm);
        }
        [HttpPost]
        public async Task<IActionResult> UserInfo(ChangeInfoModel rm)
        {
            if (ModelState.IsValid)
            {
                var ci = await _userManager.FindByEmailAsync(User.Identity.Name);
                bool kontrol = ci.Email.Equals(rm.Email);
                ci.Email = rm.Email;
                ci.UserName = rm.Email;
                ci.Name = rm.Name;
                ci.LastName = rm.LastName;
                ci.NationalID = rm.NationalID;
                ci.DateofBirth = rm.DateofBirth;
                ci.PhoneNumber = rm.Phone;
                ci.MobilePhone = rm.MobilePhone;
                ci.Adress = rm.Adress;
                ci.CompanyName = rm.CompanyName;
                var result = await _userManager.UpdateAsync(ci);
                if (result.Succeeded)
                {
                    if (kontrol)
                    {
                        return RedirectToAction("UserInfo");
                    }
                    else
                    {
                        Logout();
                        return RedirectToAction("index", "home");
                    }
                }
                else
                {
                    result.Errors.ToList().ForEach(e => ModelState.AddModelError("Error", e.Description));
                    return View(ci);
                }
            }
            else
            {
                return View(rm);
            }

        }
        public IActionResult ChangePassword(string id)
        {
            ViewBag.UserId = id;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ChangePassword(PasswordModel pm)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            else
            {
                var user = await _userManager.FindByEmailAsync(User.Identity.Name);
                var checkpass = await _signInManager.CheckPasswordSignInAsync(user, pm.OldPassword, false);
                if (checkpass.Succeeded)
                {
                    var result = await _userManager.ChangePasswordAsync(user, pm.OldPassword, pm.NewPassword);
                    if (result.Succeeded)
                    {
                        Logout();
                        return RedirectToAction("Login");
                    }
                    else
                    {
                        result.Errors.ToList().ForEach(e => ModelState.AddModelError("", e.Description));
                    }

                }
                else
                {
                    ModelState.AddModelError("OldPassword", "Wrong old password");
                    //return View();
                }

            }
            return View();
        }
        [HttpGet]
        public IActionResult GetLicences()
        {
            string id = _userManager.GetUserId(User);
            List<Packages> LP = _api.GetMethod<List<Packages>>($"{_urls.Get.GetCurrentLicences}?customerid={id}");
            return View(LP);
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(PasswordForgotModel pfm)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(pfm.Email);
                if (user != null)
                {
                    var passToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var passUrl = Url.Action("UpdatePassword", "Account", new { userid = user.Id, passtoken = passToken }, Request.Scheme);
                    return Content("<html><a href=\"" + passUrl + "\">" + "Click here for pasasword reset" + "</a><html>", "text/html");
                }
                ViewBag.Message = "Böyle bir kullanıcı bulunamadı!";
                return View("Message");
            }
            return View();
        }
        [AcceptVerbs("Get", "Post")]
        [AllowAnonymous]
        public async Task<IActionResult> IsUserExist(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                return Json(true);
            }
            else
            {
                return Json($"User {email} doesn't exist!");
            }
        }
        [AllowAnonymous]
        public IActionResult UpdatePassword(string userid, string passToken)
        {
            if (userid != null && passToken != null)
            {
                PasswordResetModel pm = new();
                pm.Id = userid;
                pm.PasswordCode = passToken;
                return View(pm);
            }
            else
            {
                ViewBag.Message = "Bir hata oluştu lütfen tekrar deneyiniz";
                return View("Message");
            }
        }
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdatePassword(PasswordResetModel pm)
        {
            if (ModelState.IsValid)
            {
                if (pm.NewPassword != null)
                {
                    var user = await _userManager.FindByIdAsync(pm.Id);
                    var result = await _userManager.ResetPasswordAsync(user, pm.PasswordCode, pm.NewPassword);
                    if (result.Succeeded)
                    {
                        if (await _userManager.IsLockedOutAsync(user))
                        {
                            await _userManager.SetLockoutEndDateAsync(user, DateTime.UtcNow);
                        }
                        return RedirectToAction("Login");//değişir
                    }
                    else
                    {
                        ViewBag.Message = "Bir hata oluştu lütfen tekrar deneyiniz";
                        return View("Message");
                    }
                }
                else
                {
                    ViewBag.Message = "Bir hata oluştu lütfen tekrar deneyiniz";
                    return View("Message");
                }

            }
            else
            {
                return View();
            }
        }
    }
}

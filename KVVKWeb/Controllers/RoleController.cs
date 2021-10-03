using KVVKWeb.Models.Dto;
using KVVKWeb.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KVVKWeb.Controllers
{
    [Authorize(Roles ="Admin")]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<CustomerInfo> _userManager;
        public RoleController(RoleManager<IdentityRole> rolemanager, UserManager<CustomerInfo> userManager)
        {
            _roleManager = rolemanager;
            _userManager = userManager;
        }
        public IActionResult CreateRole()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateRole(string role)
        {
            var result = await _roleManager.CreateAsync(new IdentityRole(role));
            if (result.Succeeded)
            {
                ViewBag.Message = "Rol eklendi";
                return View("Message");
            }
            else
            {
                ViewBag.Message = "Rol eklenemedi!!";
                return View("Message");
            }
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AddRoleToUser()
        {
            RoleModel roleUser = new();
            roleUser.Customers = _userManager.Users.Select(i => new LoginModel() { Email = i.Email, Userid = i.Id }).ToList();

            roleUser.RoleNames = _roleManager.Roles.Select(i => i.Name).ToList();
            return View(roleUser);
        }
        [HttpPost]
        public async Task<IActionResult> AddRoleToUser(RoleModel clrv)
        {
            var user = await _userManager.FindByIdAsync(clrv.UserID);
            var result = await _userManager.AddToRoleAsync(user, clrv.RoleName);
            if (result.Succeeded)
            {
                ViewBag.Message = "Yetki başarı ile atanmıştır";
                return View("Message");
            }
            else
            {
                ViewBag.Message = "Bir hata oluştu lütfen tekrar deneyiniz";
                return View("Message");
            }
        }
    }
}

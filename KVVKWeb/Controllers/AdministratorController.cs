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
    [Authorize(Roles = "Admin")]
    public class AdministratorController : Controller
    {
        private readonly UserManager<CustomerInfo> _userManager;
        public AdministratorController(UserManager<CustomerInfo> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> UsersList()
        {
            var users = _userManager.Users.ToList();
            List<CustomerInfo> ci = new List<CustomerInfo>();
            foreach (var item in users)
            {
                if (!await _userManager.IsInRoleAsync(item,"Admin"))
                {
                    ci.Add(item);
                }
            }

            return View(ci);
        }
        public async Task<IActionResult> EditUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                return View(user);
            }
            else
            {
                ViewBag.Message = "Böyle bie kullanıcı bulunamadı!";
                return View("Message");
            }
        }
        [HttpPost]
        public async Task<IActionResult> EditUser(CustomerInfo ci)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(ci.Email);
                user.Email = ci.Email;
                user.Name = ci.Name;
                user.LastName = ci.LastName;
                user.NationalID = ci.NationalID;
                user.DateofBirth = ci.DateofBirth;
                user.PhoneNumber = ci.PhoneNumber;
                user.MobilePhone = ci.MobilePhone;
                user.Adress = ci.Adress;
                user.CompanyName = ci.CompanyName;
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("UsersList");
                }
                else
                {
                    result.Errors.ToList().ForEach(e => ModelState.AddModelError("Error", e.Description));
                    return View(ci);
                }
            }
            else
            {
                return View();
            }
        }

    }
}

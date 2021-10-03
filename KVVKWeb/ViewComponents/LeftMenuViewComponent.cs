using KVVKWeb.API;
using KVVKWeb.Models;
using KVVKWeb.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KVVKWeb.ViewComponents
{
    [Authorize]
    public class LeftMenuViewComponent : ViewComponent
    {
        private readonly UserManager<CustomerInfo> _userManager;
        private readonly IGetFromAPI _api;
        private readonly URLs _urls;
        public LeftMenuViewComponent(UserManager<CustomerInfo> userManager, IGetFromAPI api, IOptions<URLs> urls)
        {
            _userManager = userManager;
            _api = api;
            _urls = urls.Value;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<Menu> mmlist = _api.GetMethod<List<Menu>>(_urls.Get.GetMenu);
            var user = await _userManager.FindByEmailAsync(User.Identity.Name);
            // var role = await _userManager.GetRolesAsync(user);

            if (!User.IsInRole("ActiveUser") && !User.IsInRole("Admin"))
            {
                List<Menu> passiveMenu = new List<Menu>();
                foreach (var item in mmlist)
                {
                    if (item.ControllerName == "Account" || item.ActionName == "Logout")
                    {
                        passiveMenu.Add(item);
                    }
                }
                return View(passiveMenu);
            }
            else if (User.IsInRole("Admin"))
            {
                List<Menu> adminMenu = new List<Menu>();
                foreach (var item in mmlist)
                {
                    if (item.ControllerName == "Administrator" || item.ControllerName == "Role" || item.ActionName == "Logout" || item.ControllerName == "Account")
                    {
                        adminMenu.Add(item);
                    }
                }
                return View(mmlist);
            }
            else if (User.IsInRole("ActiveUser")&&!User.IsInRole("Admin"))
            {
                List<Menu> activeMenu = new List<Menu>();
                foreach (var item in mmlist)
                {
                    if (item.ControllerName == "Key" || item.ControllerName == "Access" || item.ActionName == "Logout" || item.ControllerName == "Account")
                    {
                        activeMenu.Add(item);
                    }
                }
                return View(activeMenu);
            }
            return View(mmlist);
        }
    }
}

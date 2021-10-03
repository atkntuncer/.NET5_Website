using KVVKWeb.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using KVVKWeb.API;
using System.Collections.Generic;
using KVVKWeb.Models;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.DataProtection;
using KVVKWeb.Security;
using System;

namespace KVVKWeb.Controllers
{
    [Authorize(Roles = "ActiveUser")]
    public class AccessController : Controller
    {
        private readonly UserManager<CustomerInfo> _userManager;
        private readonly IGetFromAPI _api;
        private readonly URLs _urls;
        private readonly IDataProtector _dataProtector;
        public AccessController(UserManager<CustomerInfo> userManager, SignInManager<CustomerInfo> signInManager, IGetFromAPI api, IOptions<URLs> urls, IDataProtectionProvider dataProtector, DataProtectorPurposeStrings dataProtectorPurposeStrings)//dataprotectprovider olarak ekliyorsun önemli
        {
            _userManager = userManager;
            _api = api;
            _urls = urls.Value;
            _dataProtector = dataProtector.CreateProtector(dataProtectorPurposeStrings.AccessManagementEditPurpose);
        }

        public IActionResult AddAccess()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddAccess(Management M)
        {
            if (ModelState.IsValid)
            {
                M.CustomerClientID = GetClientInfoID();
                M.UserId = _userManager.GetUserId(User);
                int returnValue = _api.PostMethod<int>(M, _urls.Add.AddAccessManagement);
                if (returnValue == 1)
                {
                    return RedirectToAction("ListAccess");
                }
                else
                {
                    ViewBag.Message = "Bir hata oluştu lütfen tekrar deneyiniz";
                    return View("Message");
                }
            }
            else
            {
                ViewBag.Message = "Bilgileri tekrardan giriniz";
                return View("Message");
            }
        }
        public IActionResult ListAccess()
        {
            string customerid = GetID();
            List<Management> M = _api.GetMethod<List<Management>>($"{_urls.Get.GetAccessManagement}?customerid={customerid}");
            foreach (var item in M)
            {
                item.EncryptedID = _dataProtector.Protect(item.ID.ToString());
            }
            return View(M);
        }
        public IActionResult EditAccess(string id)
        {
            string customerid = GetID();
            int ID = Convert.ToInt32(_dataProtector.Unprotect(id));
            List<int> ids = _api.GetMethod<List<int>>($"{_urls.Get.GetAccessManagementID}?customerid={customerid}");
            if (ids.Contains(ID))//burada kendi dışındakilerine karışamasın diye engel konuldu
            {
                Management M = _api.GetMethod<Management>($"{_urls.Get.GetAccessManagement}?ID={ID}");
                return View(M);
            }
            else
            {
                return RedirectToAction("ListAccess");
            }
        }
        [HttpPost]
        public IActionResult EditAccess(Management M)
        {
            if (ModelState.IsValid)
            {
                M.CustomerClientID = GetClientInfoID();
                int returnValue = _api.PostMethod<int>(M, _urls.Edit.EditAccessManagement);
                if (returnValue > 0)
                {
                    return RedirectToAction("ListAccess");
                }
                else
                {
                    ViewBag.Message = "Bilgileri tekrardan giriniz";
                    return View("Message");
                }
            }
            else
            {
                ViewBag.Message = "Bilgileri tekrardan giriniz";
                return View("Message");
            }
        }

        public IActionResult Download()
        {
            return View();
        }
        private int GetClientInfoID()
        {
            string customerid = GetID();
            ClientInfo CI = _api.GetMethod<ClientInfo>($"{_urls.Get.GetClientInfo}?customerid={customerid}");
            return CI.ID;
        }
        private string GetID()
        {
            return _userManager.GetUserId(User);
        }

    }
}

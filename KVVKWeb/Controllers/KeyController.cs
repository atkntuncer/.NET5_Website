using KVVKWeb.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using KVVKWeb.API;
using System;
using KVVKWeb.Models;
using Microsoft.Extensions.Options;

namespace KVVKWeb.Controllers
{
    [Authorize(Roles = "ActiveUser")]
    public class KeyController : Controller
    {

        private readonly UserManager<CustomerInfo> _userManager;
        private readonly IGetFromAPI _api;
        private readonly URLs _urls;
        public KeyController(UserManager<CustomerInfo> userManager, SignInManager<CustomerInfo> signInManager, IGetFromAPI api, IOptions<URLs> urls)
        {
            _userManager = userManager;
            _api = api;
            _urls = urls.Value;
        }
        public IActionResult CreateKey()
        {
            KeyInfo KI = new();
            KI = CheckKey();
            if (String.IsNullOrEmpty(KI.KeyLabel))
            {
                return View();
            }
            else
            {
                return RedirectToAction("ListKey");
            }
            
        }
        [HttpPost]
        public IActionResult CreateKey(KeyInfo KI)
        {
            if (ModelState.IsValid)
            {
                KI.CustomerID = _userManager.GetUserId(User);
                int returnValue = _api.PostMethod<int>(KI, _urls.Add.AddKeyInfo);
                if (returnValue==1)
                {
                    return RedirectToAction("CreateKey");
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
        [HttpGet]
        public IActionResult ListKey()//anahtar yoksa gösterme
        {
           KeyInfo KI = CheckKey();
            if (!String.IsNullOrEmpty(KI.KeyLabel))
            {
                KI.KeyBit = "128-bit";
                KI.KeyType = "AES";
            }
            return View(KI);
        }
        private KeyInfo CheckKey()
        {
            string customerid = _userManager.GetUserId(User);
            KeyInfo KI = new();
            KI = _api.GetMethod<KeyInfo>($"{_urls.Get.GetKeyInfo}?customerid={customerid}");
            return KI;
        }
    }
}

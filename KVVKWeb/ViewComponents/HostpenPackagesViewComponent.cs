using KVVKWeb.API;
using KVVKWeb.Models;
using KVVKWeb.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KVVKWeb.ViewComponents
{
    public class HostpenPackagesViewComponent:ViewComponent
    {
        private readonly IGetFromAPI _api;
        private readonly URLs _urls;
        public HostpenPackagesViewComponent(IGetFromAPI api, IOptions<URLs> urls)
        {
            _api = api;
            _urls = urls.Value;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<Packages> P = _api.GetMethod<List<Packages>>(_urls.Get.GetLicencePackages);
            return View(P);
        }
    }
}

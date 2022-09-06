using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using UserInterfaceService.Models;
using UserInterfaceService.Models.ViewModels;

namespace UserInterfaceService.Controllers
{
    public class ShareHolderController : Controller
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IConfiguration config;
        private readonly UserManager<IdentityUser> userManager;

        public ShareHolderController(IHttpClientFactory httpClientFactory, IConfiguration configuration, UserManager<IdentityUser> userManager)
        {
            this.httpClientFactory = httpClientFactory;
            this.config = configuration;
            this.userManager = userManager;
        }

        [Authorize]
        public async Task<IActionResult> Index(int shareId)
        {
            ViewData["ShareFilter"] = shareId;
            var model = new ShareHolderIndexViewModel();
            var _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri(config["ShareHolderUrl"]);
            var response = await _httpClient.GetAsync("");
            var responebody = await response.Content.ReadAsStringAsync();
            model.ShareHolders = JsonConvert.DeserializeObject<List<ShareHolderModel>>(responebody);

            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri(config["ShareUrl"]);
            response = await _httpClient.GetAsync("");
            responebody = await response.Content.ReadAsStringAsync();
            model.Shares = JsonConvert.DeserializeObject<List<ShareModel>>(responebody);

            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri(config["BrokerUrl"]);
            response = await _httpClient.GetAsync("");
            responebody = await response.Content.ReadAsStringAsync();
            model.Brokers = JsonConvert.DeserializeObject<List<BrokerModel>>(responebody);

            model.Users = userManager.Users.ToList();
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri(config["TraderInfoUrl"]);
            response = await _httpClient.GetAsync("");
            responebody = await response.Content.ReadAsStringAsync();
            model.Trades = JsonConvert.DeserializeObject<List<TraderInfoModel>>(responebody);

            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri(config["ShareAlertUrl"]);
            response = await _httpClient.GetAsync("");
            responebody = await response.Content.ReadAsStringAsync();
            model.Alerts = JsonConvert.DeserializeObject<List<ShareAlertModel>>(responebody);

            if (shareId != 0)
            {
                foreach (var share in model.Shares)
                { share.Selected = false; }
                model.Shares.Find(a => a.Id == shareId).Selected = true;

                model.ShareHolders = model.ShareHolders.Where(a => a.ShareId == shareId).ToList();
                model.Trades = model.Trades.Where(a => a.TradingCode == model.Shares.Find(a => a.Id == shareId).TradingCode).ToList();
                model.Alerts = model.Alerts.Where(a => a.ShareId == shareId).ToList();
            }
            else
            {
                model.ShareHolders = new List<ShareHolderModel>();
                model.Trades = new List<TraderInfoModel>();
                model.Alerts = new List<ShareAlertModel>();
            }

            return View(model);
        }

        public async Task<IActionResult> Details()
        {
            return View();
        }
    }
}

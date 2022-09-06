using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UserInterfaceService.Models;
using UserInterfaceService.Models.ViewModels;

namespace UserInterfaceService.Controllers
{
    public class BuyShareController : Controller
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IConfiguration config;
        private readonly UserManager<IdentityUser> userManager;

        public BuyShareController(IHttpClientFactory httpClientFactory, IConfiguration configuration, UserManager<IdentityUser> userManager)
        {
            this.httpClientFactory = httpClientFactory;
            this.config = configuration;
            this.userManager = userManager;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Index(BuyShareViewModel model, int brokerId, int shareId)
        {
            ViewData["ShareId"] = shareId;
            ViewData["BrokerId"] = brokerId;
            if (model.Brokers == null)
            {
                model = new BuyShareViewModel();
                var _httpClient = httpClientFactory.CreateClient();
                _httpClient.BaseAddress = new Uri(config["BrokerUrl"]);
                var response = await _httpClient.GetAsync("");
                var responebody = await response.Content.ReadAsStringAsync();
                model.Brokers = JsonConvert.DeserializeObject<List<BrokerModel>>(responebody);
            }

            if (brokerId != 0)
            {
                foreach(var broker in model.Brokers)
                { broker.Selected = false; }
                model.Brokers.Find(a => a.Id == brokerId).Selected = true;
                var _httpClient = httpClientFactory.CreateClient();
                _httpClient.BaseAddress = new Uri(config["ShareUrl"]);
                var response = await _httpClient.GetAsync("");
                var responebody = await response.Content.ReadAsStringAsync();
                model.Shares = JsonConvert.DeserializeObject<List<ShareModel>>(responebody);

                model.Shares = model.Shares.Where(a => a.BrokerId == brokerId).ToList();
                model.BrokerSelected = model.Brokers.Find(a => a.Id == brokerId);

                if (shareId != 0)
                {
                    foreach (var share in model.Shares)
                    { share.Selected = false; }
                    model.Shares.Find(a => a.Id == shareId).Selected = true;
                    model.ShareSelected = model.Shares.Find(a => a.Id == shareId);

                }
                else
                {
                    model.ShareSelected = null;
                }
            }  
            else
            {
                model.BrokerSelected = null;
                model.ShareSelected = null;
            }

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Index(string amount, string price, string brokerId, string shareId, BuyShareViewModel model)
        {
            try
            {
                int temp = int.Parse(amount);
            }
            catch { return View("Index", model); }
            try
            {
                double temp = double.Parse(price);
            }
            catch { return View("Index", model); }

            var _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri(config["ShareIdUrl"]);
            var response = await _httpClient.GetAsync(shareId.ToString());
            var responseBody = await response.Content.ReadAsStringAsync();
            var share = JsonConvert.DeserializeObject<ShareModel>(responseBody);

            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri(config["BrokerIdUrl"]);
            response = await _httpClient.GetAsync(brokerId.ToString());
            responseBody = await response.Content.ReadAsStringAsync();
            var broker = JsonConvert.DeserializeObject<BrokerModel>(responseBody);

            var user = await userManager.FindByNameAsync(User.Identity.Name);

            share.Quantity = (int.Parse(share.Quantity) - int.Parse(amount)).ToString();

            var json = JsonConvert.SerializeObject(share);
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri(config["ShareIdUrl"]);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            response = await _httpClient.PutAsync(share.Id.ToString(), content);

            var tradeInfo = new TraderInfoModel();
            tradeInfo.TradeDate = DateTime.Now;
            tradeInfo.TradingCode = share.TradingCode;
            tradeInfo.SellerId = broker.Id;
            tradeInfo.BuyerId = user.Id;
            tradeInfo.Amount = int.Parse(amount);
            tradeInfo.Price = double.Parse(price);

            json = JsonConvert.SerializeObject(tradeInfo);
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri(config["TraderInfoUrl"]);
            content = new StringContent(json, Encoding.UTF8, "application/json");
            response = await _httpClient.PostAsync("", content);

            var shareHolder = new ShareHolderModel();
            shareHolder.BrokerId = broker.Id;
            shareHolder.ShareId = share.Id;
            shareHolder.UserId = user.Id;

            json = JsonConvert.SerializeObject(shareHolder);
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri(config["ShareHolderUrl"]);
            content = new StringContent(json, Encoding.UTF8, "application/json");
            response = await _httpClient.PostAsync("", content);

            return RedirectToAction("Index", "Home");
        }
    }
}

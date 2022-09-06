using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using UserInterfaceService.Data;
using UserInterfaceService.Models;
using UserInterfaceService.Models.ViewModels;

namespace UserInterfaceService.Controllers
{
    public class TraderInfoController : Controller
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IConfiguration config;
        private readonly UserManager<IdentityUser> userManager;

        public TraderInfoController(IHttpClientFactory httpClientFactory, IConfiguration configuration, UserManager<IdentityUser> userManager)
        {
            this.httpClientFactory = httpClientFactory;
            this.config = configuration;
            this.userManager = userManager;
        }

        // GET: TraderInfo
        [Authorize]
        public async Task<IActionResult> Index(int shareId, DateTime startDate, DateTime endDate)
        {
            var model = new TraderIndexViewModel();
            var _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri(config["TraderInfoUrl"]);
            var response = await _httpClient.GetAsync("");
            var responebody = await response.Content.ReadAsStringAsync();
            model.Traders = JsonConvert.DeserializeObject<List<TraderInfoModel>>(responebody);
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri(config["ShareUrl"]);
            response = await _httpClient.GetAsync("");
            responebody = await response.Content.ReadAsStringAsync();
            model.Shares = JsonConvert.DeserializeObject<List<ShareModel>>(responebody);
            model.Users = await userManager.Users.ToListAsync();
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri(config["BrokerUrl"]);
            response = await _httpClient.GetAsync("");
            responebody = await response.Content.ReadAsStringAsync();
            model.Brokers = JsonConvert.DeserializeObject<List<BrokerModel>>(responebody);

            if (startDate.Ticks != 0)
            {
                model.startDate = startDate;
            }
            if (endDate.Ticks != 0)
            {
                model.endDate = endDate;
            }
            if (shareId != 0)
            {
                foreach (var share in model.Shares)
                { share.Selected = false; }
                model.Shares.Find(a => a.Id == shareId).Selected = true;
                model.Traders = model.Traders.Where(a => a.TradingCode == model.Shares.Find(a => a.Id == shareId).TradingCode).ToList();
                
                if (startDate.Ticks != 0 && endDate.Ticks != 0)
                {
                    ViewData["StartDate"] = startDate;
                    ViewData["EndDate"] = endDate;
                    model.Traders = model.Traders.Where(a => a.TradeDate.Ticks <= endDate.Ticks && a.TradeDate.Ticks >= startDate.Ticks).ToList();
                }
            }
            return View(model);
        }

        // GET: TraderInfo/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            var _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri(config["TraderInfoIdUrl"]);
            var response = await _httpClient.GetAsync(id.ToString());
            var responseBody = await response.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<TraderInfoModel>(responseBody);
            return View(model);
        }

        // GET: TraderInfo/Create
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Create()
        {
            var model = new TraderCreateViewModel();
            var _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri(config["ShareUrl"]);
            var response = await _httpClient.GetAsync("");
            var responebody = await response.Content.ReadAsStringAsync();
            model.Shares = JsonConvert.DeserializeObject<List<ShareModel>>(responebody);
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri(config["BrokerUrl"]);
            response = await _httpClient.GetAsync("");
            responebody = await response.Content.ReadAsStringAsync();
            model.Brokers = JsonConvert.DeserializeObject<List<BrokerModel>>(responebody);
            model.Users = userManager.Users.ToList();
            return View(model);
        }

        // POST: TraderInfo/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Create(TraderCreateViewModel model, int shareId, int userIndex, int brokerId)
        {
            ViewData["ShareFilter"] = shareId;
            ViewData["UserFilter"] = shareId;
            ViewData["BrokerFilter"] = shareId;
            if (ModelState.IsValid)
            {
                if (shareId == -1)
                { return View(model); }
                if (userIndex == -1)
                { return View(model); }
                if (brokerId == -1)
                { return View(model); }

                model.Trader.TradingCode = model.Shares.Find(a => a.Id == shareId).TradingCode;
                model.Trader.BuyerId = model.Users[userIndex].Id;
                model.Trader.SellerId = brokerId;
                model.Trader.TradeDate = DateTime.Now;

                var json = JsonConvert.SerializeObject(model.Trader);
                var _httpClient = httpClientFactory.CreateClient();
                _httpClient.BaseAddress = new Uri(config["TraderInfoUrl"]);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("", content);
                return RedirectToAction("index");
            }

            return View(model);
        }

        // GET: TraderInfo/Edit/5
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Edit(int? id)
        {
            var _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri(config["TraderInfoIdUrl"]);
            var response = await _httpClient.GetAsync(id.ToString());
            var responseBody = await response.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<TraderInfoModel>(responseBody);
            return View(model);
        }

        // POST: TraderInfo/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TradeDate,Price,Amount,BuyerId,SellerId")] TraderInfoModel traderInfoModel)
        {
            var json = JsonConvert.SerializeObject(traderInfoModel);
            var _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri(config["TraderInfoIdUrl"]);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync(traderInfoModel.Id.ToString(), content);
            return RedirectToAction("index");
        }

        // GET: TraderInfo/Delete/5
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Delete(int? id)
        {
            var _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri(config["TraderInfoIdUrl"]);
            var response = await _httpClient.GetAsync(id.ToString());
            var responseBody = await response.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<TraderInfoModel>(responseBody);
            return View(model);
        }

        // POST: TraderInfo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var json = JsonConvert.SerializeObject(id);
            var _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri(config["TraderInfoIdUrl"]);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.DeleteAsync(id.ToString());
            if (response.StatusCode != System.Net.HttpStatusCode.NotFound)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        private async Task<bool> TraderInfoModelExists(int id)
        {
            var _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri(config["TraderInfoIdUrl"]);
            var response = await _httpClient.GetAsync(id.ToString());
            var responseBody = await response.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<TraderInfoModel>(responseBody);
            if (model != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}

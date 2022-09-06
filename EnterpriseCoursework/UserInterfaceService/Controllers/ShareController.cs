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
    public class ShareController : Controller
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IConfiguration config;
        private readonly UserManager<IdentityUser> userManager;

        public ShareController(IHttpClientFactory httpClientFactory, IConfiguration configuration, UserManager<IdentityUser> userManager)
        {
            this.httpClientFactory = httpClientFactory;
            this.config = configuration;
            this.userManager = userManager;
        }

        // GET: Share
        [Authorize]
        public async Task<IActionResult> Index(string searchString)
        {
            ViewData["CurrentFilter"] = searchString;
            var _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri(config["ShareUrl"]);
            var response = await _httpClient.GetAsync("");
            var responebody = await response.Content.ReadAsStringAsync();
            var shares = JsonConvert.DeserializeObject<List<ShareModel>>(responebody);

            if (!String.IsNullOrEmpty(searchString))
            {                
                shares = shares.Where(s => s.CompanyName.ToLower().Contains(searchString.ToLower())
                || s.CompanyMarketValue.ToString().Contains(searchString)
                || s.Price.ToString().Contains(searchString)
                || s.TradingCode.ToString().Contains(searchString)
                || s.Quantity.Contains(searchString)).ToList();
            }
            return View(shares);
        }

        // GET: Share/Details/5
        [Authorize]
        [Route("Details")]
        public async Task<IActionResult> Details(int id)
        {
            var _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri(config["ShareIdUrl"]);
            var response = await _httpClient.GetAsync(id.ToString());
            var responseBody = await response.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<ShareModel>(responseBody);
            return View(model);
        }

        // GET: Share/Create
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Create()
        {
            ShareCreateViewModel model = new ShareCreateViewModel();
            var _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri(config["BrokerUrl"]);
            var response = await _httpClient.GetAsync("");
            var responebody = await response.Content.ReadAsStringAsync();
            model.Brokers = JsonConvert.DeserializeObject<List<BrokerModel>>(responebody);
            return View(model);
        }

        // POST: Share/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Create(ShareCreateViewModel shareModel, int brokerId)
        {
            if (ModelState.IsValid)
            {
                shareModel.Share.BrokerId = brokerId;
                var json = JsonConvert.SerializeObject(shareModel.Share);
                var _httpClient = httpClientFactory.CreateClient();
                _httpClient.BaseAddress = new Uri(config["ShareUrl"]);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("", content);
                return RedirectToAction("index");
            }

            return View(shareModel);
        }

        // GET: Share/Edit/5
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Edit(int? id)
        {
            var _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri(config["ShareIdUrl"]);
            var response = await _httpClient.GetAsync(id.ToString());
            var responseBody = await response.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<ShareModel>(responseBody);
            return View(model);
        }

        // POST: Share/Edit/5
        [Authorize(Roles = "Manager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TradingCode,Quantity,Price,CompanyName,CompanyMarketValue")] ShareModel shareModel)
        {
            var json = JsonConvert.SerializeObject(shareModel);
            var _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri(config["ShareIdUrl"]);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync(shareModel.Id.ToString(), content);
            return RedirectToAction("index");
        }

        // GET: Share/Delete/5
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Delete(int id)
        {
            var _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri(config["ShareIdUrl"]);
            var response = await _httpClient.GetAsync(id.ToString());
            var responseBody = await response.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<ShareModel>(responseBody);
            return View(model);
        }

        // POST: Share/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var json = JsonConvert.SerializeObject(id);
            var _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri(config["ShareIdUrl"]);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.DeleteAsync(id.ToString());
            if (response.StatusCode != System.Net.HttpStatusCode.NotFound)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        [Authorize]
        private async Task<bool> ShareModelExists(int id)
        {
            var _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri(config["ShareIdUrl"]);
            var response = await _httpClient.GetAsync(id.ToString());
            var responseBody = await response.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<ShareModel>(responseBody);
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

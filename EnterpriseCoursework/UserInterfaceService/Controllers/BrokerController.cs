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

namespace UserInterfaceService.Controllers
{
    public class BrokerController : Controller
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IConfiguration config;
        private readonly UserManager<IdentityUser> userManager;

        public BrokerController(IHttpClientFactory httpClientFactory, IConfiguration configuration, UserManager<IdentityUser> userManager)
        {
            this.httpClientFactory = httpClientFactory;
            this.config = configuration;
            this.userManager = userManager;
        }

        // GET: Broker
        [Authorize]
        public async Task<IActionResult> Index()
        {            
            var _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri(config["BrokerUrl"]);
            var response = await _httpClient.GetAsync("");
            var responebody = await response.Content.ReadAsStringAsync();
            var brokers = JsonConvert.DeserializeObject<List<BrokerModel>>(responebody);

            return View(brokers);
        }

        // GET: Broker/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            var _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri(config["BrokerIdUrl"]);
            var response = await _httpClient.GetAsync(id.ToString());
            var responseBody = await response.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<BrokerModel>(responseBody);
            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> Recommend()
        {
            List<BrokerModel> model = new List<BrokerModel>();
            var _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri(config["BrokerUrl"]);
            var response = await _httpClient.GetAsync("");
            var responebody = await response.Content.ReadAsStringAsync();
            var brokers = JsonConvert.DeserializeObject<List<BrokerModel>>(responebody);
            var user = await userManager.FindByNameAsync(User.Identity.Name);

            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri(config["TraderInfoUrl"]);
            response = await _httpClient.GetAsync("");
            responebody = await response.Content.ReadAsStringAsync();
            var traders = JsonConvert.DeserializeObject<List<TraderInfoModel>>(responebody);

            traders = traders.Where(a => a.BuyerId == user.Id).ToList();
            if(traders.Count == 0)
            {
                return View(model);
            }

            List<BrokerModel> brokersByTrader = new List<BrokerModel>();
            foreach (var trader in traders)
            {
                brokersByTrader.AddRange(brokers.Where(a => a.Id == trader.SellerId).ToList());
            }

            if(brokersByTrader.Count == 0)
            {
                return View(model);
            }

            List<string> domains = new List<string>();
            foreach (var broker in brokersByTrader)
            {
                if(!domains.Contains(broker.Domain.ToLower()))
                {
                    domains.Add(broker.Domain.ToLower());
                }
            }

            foreach (var domain in domains)
            {
                model.AddRange(brokers.Where(a => a.Domain.ToLower() == domain).ToList());
            }

            return View(model);
        }

        // GET: Broker/Create
        [Authorize(Roles = "Manager")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Broker/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Create([Bind("Id,Name,PhoneNumber,Email,Address1,Address2,Address3,Postcode,Domain,TradingRecord,ServiceQualityGrade")] BrokerModel brokerModel)
        {
            var json = JsonConvert.SerializeObject(brokerModel);
            var _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri(config["BrokerUrl"]);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("", content);
            return RedirectToAction("index");
        }

        // GET: Broker/Edit/5
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Edit(int? id)
        {
            var _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri(config["BrokerIdUrl"]);
            var response = await _httpClient.GetAsync(id.ToString());
            var responseBody = await response.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<BrokerModel>(responseBody);
            return View(model);
        }

        // POST: Broker/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,PhoneNumber,Email,Address1,Address2,Address3,Postcode,Domain,TradingRecord,ServiceQualityGrade")] BrokerModel brokerModel)
        {
            var json = JsonConvert.SerializeObject(brokerModel);
            var _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri(config["BrokerIdUrl"]);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync(brokerModel.Id.ToString(), content);
            return RedirectToAction("index");
        }

        // GET: Broker/Delete/5
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Delete(int? id)
        {
            var _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri(config["BrokerIdUrl"]);
            var response = await _httpClient.GetAsync(id.ToString());
            var responseBody = await response.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<BrokerModel>(responseBody);
            return View(model);
        }

        // POST: Broker/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var json = JsonConvert.SerializeObject(id);
            var _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri(config["BrokerIdUrl"]);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.DeleteAsync(id.ToString());
            if (response.StatusCode != System.Net.HttpStatusCode.NotFound)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        private async Task<bool> BrokerModelExists(int id)
        {
            var _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri(config["BrokerIdUrl"]);
            var response = await _httpClient.GetAsync(id.ToString());
            var responseBody = await response.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<BrokerModel>(responseBody);
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

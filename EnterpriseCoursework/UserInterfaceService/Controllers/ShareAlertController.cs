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
    public class ShareAlertController : Controller
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IConfiguration config;
        private readonly UserManager<IdentityUser> userManager;

        public ShareAlertController(IHttpClientFactory httpClientFactory, IConfiguration configuration, UserManager<IdentityUser> userManager)
        {
            this.httpClientFactory = httpClientFactory;
            this.config = configuration;
            this.userManager = userManager;
        }

        // GET: ShareAlert
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Index()
        {
            var _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri(config["ShareAlertUrl"]);
            var response = await _httpClient.GetAsync("");
            var responebody = await response.Content.ReadAsStringAsync();
            var models = JsonConvert.DeserializeObject<List<ShareAlertModel>>(responebody);
            return View(models);
        }

        // GET: ShareAlert/Details/5
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Details(int? id)
        {
            var _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri(config["ShareAlertIdUrl"]);
            var response = await _httpClient.GetAsync(id.ToString());
            var responseBody = await response.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<ShareAlertModel>(responseBody);
            return View(model);
        }

        // GET: ShareAlert/Create
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Create()
        {
            CreateAlertViewModel model = new CreateAlertViewModel();
            var _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri(config["ShareUrl"]);
            var response = await _httpClient.GetAsync("");
            var responebody = await response.Content.ReadAsStringAsync();
            model.Shares = JsonConvert.DeserializeObject<List<ShareModel>>(responebody);
            return View(model);
        }

        // POST: ShareAlert/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Create(int shareId, CreateAlertViewModel shareAlertModel)
        {
            shareAlertModel.ShareAlert.Created = DateTime.Now;
            shareAlertModel.ShareAlert.ShareId = shareId;
            var json = JsonConvert.SerializeObject(shareAlertModel.ShareAlert);
            var _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri(config["ShareAlertUrl"]);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("", content);
            return RedirectToAction("index");
        }

        // GET: ShareAlert/Edit/5
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Edit(int? id)
        {
            var _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri(config["ShareAlertIdUrl"]);
            var response = await _httpClient.GetAsync(id.ToString());
            var responseBody = await response.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<ShareAlertModel>(responseBody);
            return View(model);
        }

        // POST: ShareAlert/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ShareId,Subject,Message")] ShareAlertModel shareAlertModel)
        {
            var json = JsonConvert.SerializeObject(shareAlertModel);
            var _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri(config["ShareAlertIdUrl"]);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync(shareAlertModel.Id.ToString(), content);
            return RedirectToAction("index");
        }

        // GET: ShareAlert/Delete/5
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Delete(int? id)
        {
            var _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri(config["ShareAlertIdUrl"]);
            var response = await _httpClient.GetAsync(id.ToString());
            var responseBody = await response.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<ShareAlertModel>(responseBody);
            return View(model);
        }

        // POST: ShareAlert/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var json = JsonConvert.SerializeObject(id);
            var _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri(config["ShareAlertIdUrl"]);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.DeleteAsync(id.ToString());
            if (response.StatusCode != System.Net.HttpStatusCode.NotFound)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpGet]
        [Route("/ShareAlert/GetAlerts")]
        public async Task<JsonResult> GetAlerts(string userId)
        {
            var _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri(config["ShareAlertUrl"]);
            var response = await _httpClient.GetAsync("");
            var responebody = await response.Content.ReadAsStringAsync();
            var tempAlerts = JsonConvert.DeserializeObject<List<ShareAlertModel>>(responebody);
            DateTime now = DateTime.Now;
            tempAlerts = tempAlerts.Where(a => a.Created.Ticks < now.Ticks && a.Created.Ticks > now.AddDays(-2).Ticks).ToList();
            JsonResponseModel model = null;
            if (tempAlerts.Count > 0)
            {
                _httpClient = httpClientFactory.CreateClient();
                _httpClient.BaseAddress = new Uri(config["ShareHolderUrl"]);
                response = await _httpClient.GetAsync("");
                responebody = await response.Content.ReadAsStringAsync();
                var shareHolders = JsonConvert.DeserializeObject<List<ShareHolderModel>>(responebody);
                shareHolders = shareHolders.Where(a => a.UserId == userId).ToList();
                if(shareHolders.Count == 0)
                {
                    model = new JsonResponseModel() { status = false, msg = "No alerts" };
                    return new JsonResult(model);
                }

                List<ShareAlertModel> alerts = new List<ShareAlertModel>();
                foreach (var holder in shareHolders)
                {
                    alerts.AddRange(tempAlerts.Where(a => a.ShareId == holder.ShareId));
                }

                if(alerts.Count == 0)
                {
                    model = new JsonResponseModel() { status = false, msg = "No alerts" };
                    return new JsonResult(model);
                }

                model = new JsonResponseModel() { status = true };
                model.msgs = new string[alerts.Count];
                for (int i = 0; i < alerts.Count; i++)
                {
                    _httpClient = httpClientFactory.CreateClient();
                    _httpClient.BaseAddress = new Uri(config["ShareIdUrl"]);
                    response = await _httpClient.GetAsync(alerts[i].ShareId.ToString());
                    var responseBody = await response.Content.ReadAsStringAsync();
                    var share = JsonConvert.DeserializeObject<ShareModel>(responseBody);

                    model.msgs[i] = "<div><h1>News For Share: " + share.TradingCode + "</h1></div><div><h3>Subject: " + alerts[i].Subject + "</h3></div><div><p>Message: "
                        + alerts[i].Message + "</p></div><div><button class='btn btn-primary alert-view-button' href='Share/Details/" + share.Id + "'>View Share</button></div>";
                }
                return new JsonResult(model);
            }
            else
            {
                model = new JsonResponseModel() { status = false, msg = "No alerts" };
                return new JsonResult(model);
            }
        }

        private async Task<bool> ShareAlertModelExists(int id)
        {
            var _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri(config["ShareAlertIdUrl"]);
            var response = await _httpClient.GetAsync(id.ToString());
            var responseBody = await response.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<ShareAlertModel>(responseBody);
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

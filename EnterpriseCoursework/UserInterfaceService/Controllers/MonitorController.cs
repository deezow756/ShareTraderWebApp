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

namespace UserInterfaceService.Controllers
{
    public class MonitorController : Controller
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IConfiguration config;
        private readonly UserManager<IdentityUser> userManager;

        public MonitorController(IHttpClientFactory httpClientFactory, IConfiguration configuration, UserManager<IdentityUser> userManager)
        {
            this.httpClientFactory = httpClientFactory;
            this.config = configuration;
            this.userManager = userManager;
        }

        // GET: Monitors
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Index()
        {
            var _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri(config["MonitorUrl"]);
            var response = await _httpClient.GetAsync("");
            var responebody = await response.Content.ReadAsStringAsync();
            var models = JsonConvert.DeserializeObject<List<MonitorModel>>(responebody);
            return View(models);
        }

        // GET: Monitor/Details/5
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Details(int id)
        {
            var _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri(config["MonitorIdUrl"]);
            var response = await _httpClient.GetAsync(id.ToString());
            var responseBody = await response.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<MonitorModel>(responseBody);
            return View(model);
        }

        // GET: Monitor/Create/2
        [Authorize]
        [HttpGet]
        [Route("/Monitor/Create/{id}")]
        public async Task<IActionResult> Create(int id)
        {            
            MonitorModel model = new MonitorModel();
            var _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri(config["ShareIdUrl"]);
            var response = await _httpClient.GetAsync(id.ToString());
            var responseBody = await response.Content.ReadAsStringAsync();
            var share = JsonConvert.DeserializeObject<ShareModel>(responseBody);
            model.ShareId = id;
            model.CurValue = share.Price;
            
            return View(model);
        }

        // POST: Monitor/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        [Route("/Monitor/Create/{id}")]
        public async Task<IActionResult> Create(int id, MonitorModel model)
        {
            model.Id = 0;
            var _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri(config["ShareIdUrl"]);
            var response = await _httpClient.GetAsync(id.ToString());
            var responseBody = await response.Content.ReadAsStringAsync();
            var share = JsonConvert.DeserializeObject<ShareModel>(responseBody);
            model.CurValue = share.Price;
            model.ShareId = id;
            IdentityUser user = await userManager.FindByNameAsync(User.Identity.Name);
            model.UserId = user.Id;
            if (ModelState.IsValid)
            {
                if(model.Min >= model.CurValue)
                {
                    ModelState.AddModelError("", "Enter a valid Minimum value, must be lower than the current value");
                    return View(model);
                }
                if (model.Max <= model.CurValue)
                {
                    ModelState.AddModelError("", "Enter a valid Maximum value, must be higher than the current value");
                    return View(model);
                }                
                var json = JsonConvert.SerializeObject(model);
                _httpClient = httpClientFactory.CreateClient();
                _httpClient.BaseAddress = new Uri(config["MonitorUrl"]);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var post_response = await _httpClient.PostAsync("", content);
                if(post_response.StatusCode == System.Net.HttpStatusCode.Created)
                {
                    return RedirectToAction("Index", "Interested");
                }
                else
                {
                    return View(model);
                }
            }

            return View(model);
        }

        // GET: Monitor/Edit/5
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Edit(int? id)
        {
            var _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri(config["MonitorIdUrl"]);
            var response = await _httpClient.GetAsync(id.ToString());
            var responseBody = await response.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<MonitorModel>(responseBody);
            return View(model);
        }

        // POST: Monitor/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ShareId,UserId,Min,Max")] MonitorModel monitorModel)
        {
            var json = JsonConvert.SerializeObject(monitorModel);
            var _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri(config["MonitorIdUrl"]);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync(monitorModel.Id.ToString(), content);
            return RedirectToAction("index");
        }

        // GET: Monitor/Delete/5
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Delete(int? id)
        {
            var _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri(config["MonitorIdUrl"]);
            var response = await _httpClient.GetAsync(id.ToString());
            var responseBody = await response.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<MonitorModel>(responseBody);
            return View(model);
        }

        // POST: Monitor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id, string prevPage)
        {
            var json = JsonConvert.SerializeObject(id);
            var _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri(config["MonitorIdUrl"]);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.DeleteAsync(id.ToString());
            if (response.StatusCode != System.Net.HttpStatusCode.NotFound)
            {
                if (String.IsNullOrEmpty(prevPage))
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return Redirect(prevPage);
                }
            }
            return View();
        }

        [Authorize]
        private async Task<bool> MonitorModelExists(int id)
        {
            var _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri(config["MonitorIdUrl"]);
            var response = await _httpClient.GetAsync(id.ToString());
            var responseBody = await response.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<MonitorModel>(responseBody);
            if (model != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        [HttpGet]
        [Route("/Monitor/GetMonitors")]
        public async Task<JsonResult> GetMonitors(string userId)
        {
            var _httpClientMonitor = httpClientFactory.CreateClient();
            _httpClientMonitor.BaseAddress = new Uri(config["MonitorUrl"]);
            var response = await _httpClientMonitor.GetAsync("");
            var responebody = await response.Content.ReadAsStringAsync();
            var monitors = JsonConvert.DeserializeObject<List<MonitorModel>>(responebody);
            monitors = monitors.Where(a => a.UserId.Equals(userId) && a.Viewed == false).ToList();
            JsonResponseModel model = null;
            if (monitors.Count > 0)
            {
                var _httpClient = httpClientFactory.CreateClient();
                _httpClient.BaseAddress = new Uri(config["ShareUrl"]);
                response = await _httpClient.GetAsync("");
                responebody = await response.Content.ReadAsStringAsync();
                var shares = JsonConvert.DeserializeObject<List<ShareModel>>(responebody);
                List<MonitorModel> monitorsToShow = new List<MonitorModel>();
                
                for (int i = 0; i < monitors.Count; i++)
                {
                    ShareModel share = shares.Find(a => a.Id == monitors[i].ShareId);
                    if (share.Price < monitors[i].Min || share.Price > monitors[i].Max)
                    {
                        monitorsToShow.Add(monitors[i]);
                    }
                }

                if(monitorsToShow.Count == 0)
                {
                    model = new JsonResponseModel() { status = false, msg = "No monitors" };
                    return new JsonResult(model);
                }

                model = new JsonResponseModel() { status = true };
                model.msgs = new string[monitorsToShow.Count];
                for (int i = 0; i < monitorsToShow.Count; i++)
                {
                    ShareModel share = shares.Find(a => a.Id == monitorsToShow[i].ShareId);
                    if (share.Price < monitorsToShow[i].Min)
                    {
                        model.msgs[i] = "<div>Share price is now lower than the minimum</div><div>The share price is now lower than the minimum price that you set</div><div>The price for " + share.TradingCode + " is now £" + share.Price
                                                    + "</div><div><button class='btn btn-primary monitor-view-button' href='/Share/Details/" + monitorsToShow[i].Id + "'>View Share</button></div>";
                    }
                    else if (share.Price > monitorsToShow[i].Max)
                    {
                        model.msgs[i] = "<div>Share price is now higher than the maximum</div><div>The share price is now higher than the maximum price that you set</div><div>The price for " + share.TradingCode + " is now £" + share.Price
                            + "</div><div><button class='btn btn-primary monitor-view-button' asp-controller='Share' asp-action='Details' asp-route-id='" + monitorsToShow[i].Id + "'>View Share</button></div>";
                    }
                    monitorsToShow[i].Viewed = true;
                    var json = JsonConvert.SerializeObject(monitorsToShow[i]);
                    _httpClientMonitor.Dispose();
                    _httpClientMonitor = httpClientFactory.CreateClient();
                    _httpClientMonitor.BaseAddress = new Uri(config["MonitorIdUrl"]);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var responseUpdate = await _httpClientMonitor.PutAsync(monitorsToShow[i].Id.ToString(), content);
                }
                return new JsonResult(model);
            }
            else
            {
                model = new JsonResponseModel() { status = false, msg = "No monitors" };
                return new JsonResult(model);
            }
        }
    }
}

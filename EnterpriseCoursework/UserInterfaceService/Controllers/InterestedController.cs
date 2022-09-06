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
    public class InterestedController : Controller
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IConfiguration config;
        private readonly UserManager<IdentityUser> userManager;

        public InterestedController(IHttpClientFactory httpClientFactory, IConfiguration configuration, UserManager<IdentityUser> userManager)
        {
            this.httpClientFactory = httpClientFactory;
            this.config = configuration;
            this.userManager = userManager;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            InterestedViewModel model = new InterestedViewModel();
            var _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri(config["ShareUrl"]);
            var response = await _httpClient.GetAsync("");
            var responebody = await response.Content.ReadAsStringAsync();
            var shares = JsonConvert.DeserializeObject<List<ShareModel>>(responebody);
            model.Shares = shares;
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri(config["MonitorUrl"]);
            response = await _httpClient.GetAsync("");
            responebody = await response.Content.ReadAsStringAsync();
            var monitors = JsonConvert.DeserializeObject<List<MonitorModel>>(responebody);
            IdentityUser user = await userManager.FindByNameAsync(User.Identity.Name);
            model.Monitors = monitors.Where(a => a.UserId == user.Id).ToList();
            return View(model);
        }
    }
}

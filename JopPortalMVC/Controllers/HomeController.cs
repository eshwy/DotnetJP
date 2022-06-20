using JopPortalMVC.Helper;
using JopPortalMVC.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using X.PagedList;

namespace JopPortalMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IJobPortalUrl _jobPortalUrl;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            _jobPortalUrl = new JobPortalUrl();

        }

        
        public async Task<IActionResult> Index(String sort_order,string searchString,string gender,String skill,int? page)
        {
            var token = HttpContext.Session.GetString("JWToken");
            HttpClient cli = _jobPortalUrl.initial();
            cli.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",token);

            List<SelectListItem> Gender = new List<SelectListItem>();
            Gender.Add(new SelectListItem { Text = "Select", Value = "0" });
            Gender.Add(new SelectListItem { Text = "Male", Value = "Male" });
            Gender.Add(new SelectListItem { Text = "FeMale", Value = "FeMale" });
            TempData["Gender"] = Gender;
            List<SelectListItem> Skill = new List<SelectListItem>();
            Skill.Add(new SelectListItem { Text = "Select", Value = "0" });
            Skill.Add(new SelectListItem { Text = "Dot net Core", Value = "Dot net Core" });
            Skill.Add(new SelectListItem { Text = "Sql", Value = "Sql" });
            Skill.Add(new SelectListItem { Text = "Python", Value = "Python" });
            TempData["Skill"] = Skill;

            ViewData["EmailSortParm"] = String.IsNullOrEmpty(sort_order) ? "Email_desc" : "";
            ViewData["ExperienceSortParm"] = String.IsNullOrEmpty(sort_order) ? "Experience_desc" : "";
            ViewData["SkillsSortParm"] = String.IsNullOrEmpty(sort_order) ? "Skills_desc" : "";
            ViewData["ValueFilter"] = searchString;



            List<UserPersonalDetailsTable> JPDT = new List<UserPersonalDetailsTable>();
            var result = await cli.GetAsync(cli.BaseAddress +"api/AllUserDetails");
            
            if (result.IsSuccessStatusCode)
            {
                var res = result.Content.ReadAsStringAsync().Result;                
                JPDT = JsonConvert.DeserializeObject<List<UserPersonalDetailsTable>>(res);                
            }
            if (gender == "Male" || gender == "FeMale")
            {

                JPDT = JPDT.Where(c => c.Gender.Contains(gender)).ToList();
            }
            switch (skill)
            {
                case "Dot net Core":
                    JPDT = JPDT.Where(s => s.Skills.Contains(skill)).ToList();
                    break;
                case "Sql":
                    JPDT = JPDT.Where(s => s.Skills.Contains(skill)).ToList();
                    break;
                case "Python":
                    JPDT = JPDT.Where(s => s.Skills.Contains(skill)).ToList();
                    break;
            }
            if (!String.IsNullOrEmpty(searchString))
            {                
                //JPDT = JPDT.Where(s => s.LastName.ToLower().Contains(searchString.ToLower()) ).ToList();
                JPDT = JPDT.Where(s => s.FirstName.ToLower().Any(value => s.FirstName.ToLower().Contains(searchString))||
                                       s.LastName.ToLower().Any(value => s.LastName.ToLower().Contains(searchString)) ||
                                       s.Email.ToLower().Any(value => s.Email.ToLower().Contains(searchString)) ||
                                       s.City.ToLower().Any(value => s.City.ToLower().Contains(searchString)) ||
                                       s.Skills.ToLower().Any(value => s.Skills.ToLower().Contains(searchString))||
                                       s.Experience.ToString().Contains(searchString)).ToList();
                
            }   
            switch (sort_order)
            {
                case "Email_desc":
                    JPDT = JPDT.OrderByDescending(s => s.Email).ToList();
                    break;
                case "Experience_desc":
                    JPDT = JPDT.OrderByDescending(s => s.Experience).ToList();
                    break;
                case "Skills_desc":
                    JPDT = JPDT.OrderByDescending(s => s.Skills).ToList();
                    break;
                default:
                    JPDT = JPDT.OrderBy(s => s.Experience).ToList();
                    break;
            }

            var pageNumber = page ?? 1;
            
            return View(JPDT.ToList().ToPagedList(pageNumber,2));
        }
        public async Task<IActionResult> Details(string id)
        {
            
            var token = HttpContext.Session.GetString("JWToken");
            var handler = new JwtSecurityTokenHandler();
            var decodedValue = handler.ReadJwtToken(token);
            var data = decodedValue.Claims.First(c => c.Type == "nameid").Value;
            ViewBag.IdValue= Int32.Parse(data);

            HttpClient cli = _jobPortalUrl.initial();
            cli.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);          

            List<ParticularDetails> UPD = new List<ParticularDetails>();
            List<UserJobDetails> UJD = new List<UserJobDetails>();
            List<UserFollowers> userFollowersDetails = new List<UserFollowers>();
            List<UserFollowing> userFollowingDetails = new List<UserFollowing>();
            List<Article> ArticleDetails = new List<Article>();


            HttpResponseMessage ParticularResult = await cli.GetAsync("/api/ParticularDetails/"+id);
            if (ParticularResult.IsSuccessStatusCode)
            {
                var ParticularRes = ParticularResult.Content.ReadAsStringAsync().Result;
                UPD = JsonConvert.DeserializeObject<List<ParticularDetails>>(ParticularRes);

                HttpResponseMessage JobDetailsResult = await cli.GetAsync("/api/UserJobDetails/" + id);
                if (JobDetailsResult.IsSuccessStatusCode)
                {
                    var jobDeatilsRes = JobDetailsResult.Content.ReadAsStringAsync().Result;
                    UJD = JsonConvert.DeserializeObject<List<UserJobDetails>>(jobDeatilsRes);
                    var result = await cli.GetAsync(cli.BaseAddress + "api/UserFollowerandFollowedBies");
                    if (result.IsSuccessStatusCode)
                    {
                        var res = result.Content.ReadAsStringAsync().Result;
                        userFollowersDetails = JsonConvert.DeserializeObject<List<UserFollowers>>(res);
                        userFollowingDetails = JsonConvert.DeserializeObject<List<UserFollowing>>(res);
                        userFollowingDetails = userFollowingDetails.Where(c => c.FollowerId.ToString() == id).ToList();
                        userFollowersDetails = userFollowersDetails.Where(c => c.UserId.ToString() == id).ToList();
                        var ArticleResult = await cli.GetAsync("api/Articles/" + id);
                        if (ArticleResult.IsSuccessStatusCode)
                        {
                            var RawArticleResult = ArticleResult.Content.ReadAsStringAsync().Result;
                            ArticleDetails = JsonConvert.DeserializeObject<List<Article>>(RawArticleResult);
                        }
                    }
                }
                Console.WriteLine(UJD);
                }
            DeatilsView DeatilsView = new DeatilsView()
            {
                ParticularDetails = UPD,
                UserJobDetails = UJD,
                UserFollowers = userFollowersDetails,
                UserFollowing = userFollowingDetails,
                Article = ArticleDetails
            };
            
            

            return View(DeatilsView);
        }



        //[HttpGet]
        //public async Task<IActionResult> Followers()
        //{
        //    int id = Int32.Parse(TempData["id"].ToString());
        //    Console.WriteLine(id);
        //    var token = HttpContext.Session.GetString("JWToken");
        //    HttpClient cli = _jobPortalUrl.initial();
        //    cli.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        //    List<UserFollowers> userFollowersDetails = new List<UserFollowers>();
        //    List<UserFollowing> userFollowingDetails = new List<UserFollowing>();

        //    var result = await cli.GetAsync(cli.BaseAddress + "api/UserFollowerandFollowedBies");
        //    if (result.IsSuccessStatusCode)
        //    {
        //        var res = result.Content.ReadAsStringAsync().Result;
        //        userFollowersDetails = JsonConvert.DeserializeObject<List<UserFollowers>>(res);
        //        userFollowingDetails = JsonConvert.DeserializeObject<List<UserFollowing>>(res);
        //        userFollowingDetails = userFollowingDetails.Where(c => c.FollowerId == id).ToList();                
        //        userFollowersDetails = userFollowersDetails.Where(c => c.UserId == id).ToList();

        //    }

        //    DeatilsView Deatils = new DeatilsView()
        //    {
        //        UserFollowers = userFollowersDetails,
        //        UserFollowing= userFollowingDetails
        //    };
        //    return PartialView(Deatils);
        //}


        [HttpGet]
        public async Task<IActionResult> ViewArticle(Article art)
        {
            List<Article> ParticularArticleDetails = new List<Article>();
            HttpClient cli = _jobPortalUrl.initial();
            var token = HttpContext.Session.GetString("JWToken");
            cli.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            Console.WriteLine(art);
            string ArticleData = JsonConvert.SerializeObject(art);

            StringContent content = new StringContent(ArticleData, Encoding.UTF8, "application/json");
            var ParticularArticleResult = await cli.PostAsync(cli.BaseAddress + "api/Articles/ParticularArticle", content);
            if (ParticularArticleResult.IsSuccessStatusCode)
            {
                var RawArticleResult = ParticularArticleResult.Content.ReadAsStringAsync().Result;
                ParticularArticleDetails = JsonConvert.DeserializeObject<List<Article>>(RawArticleResult);
                return View(ParticularArticleDetails);
            }
            return RedirectToAction("Index");

            
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserGetisterAndLoginTable user)
        {
            HttpClient cli = _jobPortalUrl.initial();
            //Console.WriteLine(user.Name);
            //Console.WriteLine(user.Password);
            
            string UserLoginDetails = JsonConvert.SerializeObject(user);
            StringContent content = new StringContent(UserLoginDetails, Encoding.UTF8, "application/json");
            var response = await cli.PostAsync(cli.BaseAddress + "api/UserGetisterAndLoginTables", content);

            if (response.IsSuccessStatusCode)
            {
                string token = await response.Content.ReadAsStringAsync();
                HttpContext.Session.SetString("JWToken",token);
                return RedirectToAction("Index");
            }
             return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Article(string id)
        {
            ViewBag.ID = id;
            var token = HttpContext.Session.GetString("JWToken");
            var handler = new JwtSecurityTokenHandler();
            var decodedValue = handler.ReadJwtToken(token);
            var data = decodedValue.Claims.First(c => c.Type == "nameid").Value;
            Console.WriteLine(data);            
            return View();
        }

        [HttpPost]
        [System.Web.Mvc.ValidateInput(false)]
        public async Task<IActionResult> Article(Article article)
        {
            HttpClient cli = _jobPortalUrl.initial();
            var token = HttpContext.Session.GetString("JWToken");
            cli.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);            
            Console.WriteLine(article);
            string ArticleData = JsonConvert.SerializeObject(article);

            StringContent content = new StringContent(ArticleData, Encoding.UTF8, "application/json");
            var response = await cli.PostAsync(cli.BaseAddress + "api/Articles", content);
            if (response.IsSuccessStatusCode)
            {                
                return RedirectToAction("Index");
            }            

            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

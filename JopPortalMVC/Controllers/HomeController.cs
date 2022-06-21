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

        public string UserClaim()
        {
            var token = HttpContext.Session.GetString("JWToken");
            var handler = new JwtSecurityTokenHandler();
            var decodedValue = handler.ReadJwtToken(token);
            var data = decodedValue.Claims.First(c => c.Type == "nameid").Value;
            return data;
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
            
            string UserLoginDetails = JsonConvert.SerializeObject(user);
            StringContent content = new StringContent(UserLoginDetails, Encoding.UTF8, "application/json");
            var response = await cli.PostAsync(cli.BaseAddress + "api/UserGetisterAndLoginTables", content);

            if (response.IsSuccessStatusCode)
            {
                string token = await response.Content.ReadAsStringAsync();
                HttpContext.Session.SetString("JWToken", token);
                
                return RedirectToAction("Index");
            }
            ViewBag.Error = "***  Invalid Valid User or Password  ***";
            return View();
        }

        public IActionResult LogOff()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
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
            
            ViewBag.IdValue= Int32.Parse(UserClaim());

            HttpClient cli = _jobPortalUrl.initial();
            cli.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);          

            List<ParticularDetails> UPD = new List<ParticularDetails>();
            List<UserJobDetails> UJD = new List<UserJobDetails>();

            List<FollowerAndFollowing> userFollowersDetails = new List<FollowerAndFollowing>();
            List<FollowerAndFollowing> userFollowingDetails = new List<FollowerAndFollowing>();

            List<FollowDetails> FollowingDetails = new List<FollowDetails>();
            List<FollowDetails> FollowersDetails = new List<FollowDetails>();

            List<UserPersonalDetailsTable> JPDT = new List<UserPersonalDetailsTable>();
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
                    
                    var result = await cli.GetAsync("api/UserFollowerandFollowedBy");
                    if (result.IsSuccessStatusCode)
                    {
                        var res = result.Content.ReadAsStringAsync().Result;
                        userFollowersDetails = JsonConvert.DeserializeObject<List<FollowerAndFollowing>>(res);
                        userFollowingDetails = JsonConvert.DeserializeObject<List<FollowerAndFollowing>>(res);
                        userFollowingDetails = userFollowingDetails.Where(c => c.UserId.ToString() == id).ToList();
                        userFollowersDetails = userFollowersDetails.Where(c => c.FollowerId.ToString() == id).ToList();
                        var UserDetails = await cli.GetAsync("api/ParticularDetails");
                        if(UserDetails.IsSuccessStatusCode)
                        {
                            var RawUserDetails = UserDetails.Content.ReadAsStringAsync().Result;
                            
                            JPDT = JsonConvert.DeserializeObject<List<UserPersonalDetailsTable>>(RawUserDetails);

                            var value1 = userFollowingDetails.Join(JPDT, t1 => t1.FollowerId,
                                                                    t2 => t2.User_Id,
                                                                    (t1, t2) => new
                                                                    {
                                                                        UserId = t1.FollowerId,
                                                                        UserName = t2.FirstName +" " + t2.LastName
                                                                    });
                            var value2 = userFollowersDetails.Join(JPDT, t1 => t1.UserId,
                                                                    t2 => t2.User_Id,
                                                                    (t1, t2) => new
                                                                    {
                                                                        UserId = t1.UserId,
                                                                        UserName = t2.FirstName + " " + t2.LastName
                                                                    });
                            FollowersDetails = (from i in value1
                                                select new FollowDetails
                                                { 
                                                    UserId= i.UserId,
                                                    UserName = i.UserName                                                
                                                }).ToList();
                            FollowingDetails = (from p in value2
                                                select new FollowDetails
                                                {
                                                    UserId = p.UserId,
                                                    UserName = p.UserName
                                                }).ToList();

                        }
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
                UserFollowersDetails = FollowersDetails,
                UserFollowingDetails = FollowingDetails,
                Article = ArticleDetails
            };         
            return View(DeatilsView);
        }


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
        public IActionResult ArticlePost()
        {         
            return View();
        }

        [HttpPost]
        [System.Web.Mvc.ValidateInput(false)]
        public async Task<IActionResult> ArticlePost(Article article)
        {
            
            HttpClient cli = _jobPortalUrl.initial();
            var token = HttpContext.Session.GetString("JWToken");
            cli.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var handler = new JwtSecurityTokenHandler();            
            article.UserId = Int32.Parse(UserClaim());
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
        public async Task<IActionResult> ListArticle(string searchString)
        {
            ViewData["ValueFilter"] = searchString;
            HttpClient cli = _jobPortalUrl.initial();
            var token = HttpContext.Session.GetString("JWToken");
            cli.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            List<ArticleView> TotalArticleDetails = new List<ArticleView>();
            List<ArticleView> TotalArticleDetails1 = new List<ArticleView>();
            List<Article> ArticleDetails = new List<Article>();
            List<UserPersonalDetailsTable> PersonalDetails = new List<UserPersonalDetailsTable>();
            
            var ArticleResponse = await cli.GetAsync("api/Articles");
            if (ArticleResponse.IsSuccessStatusCode)
            {
                var ArtileResult = ArticleResponse.Content.ReadAsStringAsync().Result;
                ArticleDetails = JsonConvert.DeserializeObject<List<Article>>(ArtileResult);
                var UserDetailsResponse = await cli.GetAsync("api/AllUserDetails");
                if (UserDetailsResponse.IsSuccessStatusCode)
                {
                    var UserResult = UserDetailsResponse.Content.ReadAsStringAsync().Result;
                    PersonalDetails = JsonConvert.DeserializeObject<List<UserPersonalDetailsTable>>(UserResult);

                    var LinqJoinArticle = ArticleDetails.Join(PersonalDetails, t1 => t1.UserId,
                                                                    t2 => t2.User_Id,
                                                                    (t1, t2) => new
                                                                    {
                                                                        UserId = t1.UserId,
                                                                        Name = t2.FirstName + " " + t2.LastName,
                                                                        Title = t1.Title,
                                                                        Category = t1.Category,
                                                                        Gender =t2.Gender,
                                                                        Experience =t2.Experience,
                                                                        Skills = t2.Skills
                                                                    });
                    TotalArticleDetails = (from p in LinqJoinArticle
                                           select new ArticleView
                                           {
                                               UserId = p.UserId,
                                               Name = p.Name,
                                               Title =p.Title,
                                               Category =p.Category,
                                               Gender =p.Gender,
                                               Experience=p.Experience,
                                               Skills=p.Skills
                                           }).ToList();
                }
                if (!String.IsNullOrEmpty(searchString))
                {
                    var Title = searchString.Split(',').Select(p => p.ToLower());
                                      
                    TotalArticleDetails = TotalArticleDetails.Where(c => Title.Contains(c.Title.ToLower())).ToList();
                    

                }


            }
            return View(TotalArticleDetails);
        }

        public async Task<IActionResult> UserAddedList(SoretedProfiles soreted)
        {
            HttpClient cli = _jobPortalUrl.initial();
            var token = HttpContext.Session.GetString("JWToken");
            var handler = new JwtSecurityTokenHandler();
            var decodedValue = handler.ReadJwtToken(token);
            var data = decodedValue.Claims.First(c => c.Type == "nameid").Value;
            soreted.SortedBy = Int32.Parse(UserClaim());
            string SortData = JsonConvert.SerializeObject(soreted);

            StringContent content = new StringContent(SortData, Encoding.UTF8, "application/json");
            Console.WriteLine(data);
            
            List<SoretedProfiles> SoretedProfilesList = new List<SoretedProfiles>();
            
            var result = await cli.PostAsync(cli.BaseAddress + "api/SoretedProfiles" , content);
            if (result.IsSuccessStatusCode)
            {
                return RedirectToAction("SortedView");
            }
            return RedirectToAction("SortedView");
        }

        public async Task<IActionResult> SortedView()
        {
            HttpClient cli = _jobPortalUrl.initial();
            var token = HttpContext.Session.GetString("JWToken");
            cli.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            
            var data = Int32.Parse(UserClaim());

            List<UserPersonalDetailsTable> UserSorderList = new List<UserPersonalDetailsTable>();
            List<SoretedProfiles> SorderList = new List<SoretedProfiles>();
            List<SortedView> ParticularUserSorderList = new List<SortedView>();

            var result = await cli.GetAsync(cli.BaseAddress + "api/SoretedProfiles/" + data);
            if (result.IsSuccessStatusCode)
            {
                var RawSortData = result.Content.ReadAsStringAsync().Result;

                SorderList = JsonConvert.DeserializeObject<List<SoretedProfiles>>(RawSortData);
                var ProfileResult = await cli.GetAsync(cli.BaseAddress + "api/AllUserDetails");
                if (ProfileResult.IsSuccessStatusCode)
                {
                    var RawProfileData = ProfileResult.Content.ReadAsStringAsync().Result;
                    UserSorderList = JsonConvert.DeserializeObject<List<UserPersonalDetailsTable>>(RawProfileData);

                    var LinqSortData = SorderList.Join(UserSorderList, t1 => t1.SeletedId,
                                                                    t2 => t2.User_Id,
                                                                    (t1, t2) => new
                                                                    {
                                                                        UserId = t1.SeletedId,
                                                                        FirstName = t2.FirstName ,
                                                                        LastName = t2.LastName,
                                                                        Email = t2.Email,
                                                                        Gender = t2.Gender,
                                                                        City = t2.City,
                                                                        Experience= t2.Experience,
                                                                        Skills = t2.Skills
                                                                    });
                    ParticularUserSorderList = (from p in LinqSortData
                                                select new SortedView
                                                {
                                                    UserId = p.UserId,
                                                    FirstName = p.FirstName,
                                                    LastName = p.LastName,
                                                    Email = p.Email,
                                                    Gender = p.Gender,
                                                    City = p.City,
                                                    Experience = p.Experience,
                                                    Skills = p.Skills
                                                }).ToList();
                }
            }

            return View(ParticularUserSorderList);
        }

        public async Task<IActionResult> RemoveFromSortList(SoretedProfiles Sp)
        {
            HttpClient cli = _jobPortalUrl.initial();
            var token = HttpContext.Session.GetString("JWToken");
            cli.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            
            Sp.SortedBy = Int32.Parse(UserClaim());

            string RemoveData = JsonConvert.SerializeObject(Sp);

            StringContent content = new StringContent(RemoveData, Encoding.UTF8, "application/json");
            var RemoveResult = await cli.PostAsync(cli.BaseAddress + "api/SoretedProfiles/Remove", content);
            if (RemoveResult.IsSuccessStatusCode)
            {
                return RedirectToAction("SortedView");
            }
            return RedirectToAction("SortedView");
        }
        
        public async Task<IActionResult> RemoveAndUnFollow(FollowerAndFollowing UnFollow)
        {
            Console.WriteLine(UnFollow.FollowerId);
            HttpClient cli = _jobPortalUrl.initial();
            var token = HttpContext.Session.GetString("JWToken");
            cli.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            
            string UnFollowData = JsonConvert.SerializeObject(UnFollow);

            StringContent content = new StringContent(UnFollowData, Encoding.UTF8, "application/json");
            var RemoveResult = await cli.PostAsync(cli.BaseAddress + "api/UserFollowerandFollowedBy/Remove", content);
            if (RemoveResult.IsSuccessStatusCode)
            {
                return RedirectToAction("Details", new { id = UnFollow.UserId });
            }
            return RedirectToAction("Details", new { id = UnFollow.UserId });
        }

        public async Task<IActionResult> Follow(FollowerAndFollowing UnFollow)
        {
            
            
            HttpClient cli = _jobPortalUrl.initial();
            var token = HttpContext.Session.GetString("JWToken");
            cli.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);            

            UnFollow.UserId = Int32.Parse(UserClaim());


            string UnFollowData = JsonConvert.SerializeObject(UnFollow);

            StringContent content = new StringContent(UnFollowData, Encoding.UTF8, "application/json");
            var RemoveResult = await cli.PostAsync(cli.BaseAddress + "api/UserFollowerandFollowedBy", content);
            if (RemoveResult.IsSuccessStatusCode)
            {
                return RedirectToAction("Details", new { id = UnFollow.FollowerId });
            }
            return RedirectToAction("Details", new { id = UnFollow.FollowerId });
        }

        public IActionResult Privacy()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

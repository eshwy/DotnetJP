
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace JopPortalMVC.Helper
{
    public class JobPortalUrl : IJobPortalUrl
    {
        public HttpClient initial()
        {
            var client = new HttpClient();
            client.BaseAddress=new Uri("https://localhost:44327/");          
            
            
            return client;
        }
    }
}

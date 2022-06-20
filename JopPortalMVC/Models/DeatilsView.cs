using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JopPortalMVC.Models
{
    public class DeatilsView
    {
        public List<ParticularDetails> ParticularDetails { get; set; }
        public IEnumerable<UserJobDetails> UserJobDetails { get; set; }
        public IEnumerable<UserFollowers> UserFollowers { get; set; }
        public IEnumerable<UserFollowing> UserFollowing { get; set; }
        public IEnumerable<Article> Article { get; set; }
    }
}

using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace emo_back.Models
{
    public class ApplicationUser : IdentityUser<int>
    {
        public UserInfo UserInfo { get; set; }
        public List<Message> Messages { get; set; }
    }
}
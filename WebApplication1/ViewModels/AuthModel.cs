using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.ViewModels
{
    public class AuthModel
    {
        public string Token { get; set; }
        public DateTime ExpiresOn { get; set; }
    }
}

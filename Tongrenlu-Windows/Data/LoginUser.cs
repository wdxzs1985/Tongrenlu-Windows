using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tongrenlu_Windows.Data
{
    public class LoginUserList
    {
        public List<LoginUser> list { get; set; }
    }

    public class LoginUser
    {
        public string token { get; set; }

        public string name { get; set; }
    }
}

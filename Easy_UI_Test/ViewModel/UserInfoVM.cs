using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public class UserInfoVM
    {
        public int Id { get; set; }

        public string LoginName { get; set; }

        public string Password { get; set; }

        public int Type { get; set; }
    }
}

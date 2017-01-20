using BusinessEntities;
using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel;

namespace BusinessLayer
{
    public class UserInfoBus
    {
        public List<UserInfo> GetUserInfos()
        {
            return new List<UserInfo>();
        }

        //public int Add( UserInfoVM model)
        //{
        //    model = new UserInfoVM() { LoginName = "b", Password = "b", Type = 3 };
        //    return new UserInfoDAL().Add(model.Map<UserInfo, UserInfoVM>());
        //}

        public bool Update(UserInfoVM model)
        {
            return new UserInfoDAL().Update(model.Map<UserInfo, UserInfoVM>());
        }

        public UserInfoVM GetByName(string name)
        {
            var result = new UserInfoDAL().GetModel(name).Map<UserInfoVM, UserInfo>();
            return result;
        }

        public UserInfoVM GetById(int uId)
        {
            var result = new UserInfoDAL().GetModel(uId).Map<UserInfoVM, UserInfo>();
            return result;
        }
    }
}

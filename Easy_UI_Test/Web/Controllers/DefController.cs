using BusinessEntities;
using BusinessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ViewModel;
using ViewModel.Common;

namespace MVC_T.Controllers
{
    public class DefController : Controller
    {
        /// <summary>
        /// 登录页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Login()
        {
            Session.Remove("username"); // 清除登录session
            return View();
        }

        /// <summary>
        /// 退出登录
        /// </summary>
        /// <returns></returns>
        public ActionResult Loginout()
        {
            return RedirectToAction("Login", "Def");
        }

        /// <summary>
        /// 获取session用户
        /// </summary>
        /// <returns></returns>
        public ActionResult GetSessionUser()
        {
            var curUser = Session["username"] as UserInfoVM;
            if (curUser != null && curUser.Id > 0)
            {
                return Json(new JsonData(true, curUser));
            }
            return Json(new JsonData(false));
        }

        /// <summary>
        /// 登录系统
        /// </summary>
        /// <param name="name"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public ActionResult CheckUser(string name, string password)
        {
            UserInfoVM curUser = new UserInfoBus().GetByName(name);
            if (curUser == null)
            {
                return Json(new JsonData(false, "该帐号不存在！"));
            }
            if (!curUser.Password.Equals(password))
            {
                return Json(new JsonData(false, "密码错误！"));
            }
            Session["username"] = curUser;
            return Json(new JsonData(true));
            //return RedirectToAction("Index", "Def");
        }

        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public ActionResult GetDatas(int area, int control)
        {
            var result = new EquipmentBus().GetList(new CustomFilter());
            result = result.FindAll(x => (x.AreaId == area || area == 0) && (x.ControlId == control || control == 0));
            return Json(result);
        }

        public ActionResult Index2()
        {
            
            DateTime dtNow = DateTime.Now; 
            int days = DateTime.DaysInMonth(dtNow.Year, dtNow.Month);
            string[] daysInMonth = new string[days];
            for (int i = 1; i <= days; i++)
            {
                daysInMonth[i - 1] = i.ToString();
            }
            ViewData["daysInMonth"] = daysInMonth;

            return View();
        }

        public ActionResult Index3()
        {
            return View();
        }

        public ActionResult Index4()
        {
            return View();
        }

        public ActionResult ResetPwd(int uId, string oldPwd, string newPwd)
        {
            UserInfoVM curUser = new UserInfoBus().GetById(uId);
            if (curUser.Password.Equals(oldPwd))
            {
                curUser.Password = newPwd;
                if (new UserInfoBus().Update(curUser))
                {
                    return Json(new JsonData(true));
                }
                else
                {
                    return Json(new JsonData(false, "密码修改失败！"));
                }
            }
            else
            {
                return Json(new JsonData(false, "原密码错误！"));
            }
        }
    }
}

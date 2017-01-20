using BusinessEntities;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public partial class EquipmentDAL
    {
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<Equipment> GetList(CustomFilter filter)
        {
            StringBuilder strSql = new StringBuilder();
            var param = new DynamicParameters();

            strSql.Append("select * ");
            strSql.Append(" FROM Equipment");
            strSql.Append(" where 1=1 ");
            strSql.Append(GetStrWhere(filter));
            strSql.Append(" order by AreaId,ControlId");

            List<Equipment> list = new List<Equipment>();
            using (var conn = DBHelper.CreateConnection())
            {
                conn.Open();
                list = conn.Query<Equipment>(strSql.ToString(), param).ToList();
            }
            return list;
        }

        /// <summary>
        /// 根据CustomFilter获取where语句
        /// </summary>
        private string GetStrWhere(CustomFilter filter)
        {
            StringBuilder strSql = new StringBuilder();
            if (filter.CollegeId != 0)
                strSql.Append(" and UserInfo.collegeId=" + filter.CollegeId);            // 按照学校Id进行数据隔离
            if (filter.Id.HasValue)
            {
                strSql.AppendFormat(" and UserInfo.Id={0}", filter.Id);
            }
            if (filter.IdList != null)
            {
                strSql.AppendFormat(" and UserInfo.Id in('{0}')", string.Join("','", filter.IdList));
            }
            if (filter.StatusId.HasValue)
            {
                strSql.AppendFormat(" and UserInfo.Status={0}", filter.StatusId);
            }
            if (filter.TypeId.HasValue)
            {
                strSql.AppendFormat(" and UserInfo.RoleId={0}", filter.TypeId);
            }
            if (!string.IsNullOrWhiteSpace(filter.KeyWord))
            {
                if (filter.KeyWord1Ex)
                {
                    strSql.AppendFormat(" and ( UserInfo.UserName like '%{0}%' or UserInfo.SchoolNumber like '%{0}%' )", filter.KeyWord.Replace("'", "''"));
                }
                else
                {
                    // 查询模糊匹配账号，姓名，学校，手机号码
                    strSql.AppendFormat(" and (UserInfo.UserName like '%{0}%' or UserInfo.IDCard like '%{0}%' or Account.AccountNo like '%{0}%' or UserInfo.CollegeName like '%{0}%' or UserInfo.Phone like '%{0}%')", filter.KeyWord.Replace("'", "''"));
                }
            }
            if (!string.IsNullOrWhiteSpace(filter.KeyWord2))
            {
                strSql.AppendFormat(" and UserInfo.SchoolNumber like '%{0}%'", filter.KeyWord2.Replace("'", "''"));
            }

            // 判断用户的注册类型
            if (filter.RegistrationType.HasValue)
            {
                // 判断是否查询管理员添加和审核通过的
                if (filter.RegistrationType == -1)
                {
                    strSql.Append(" and ( IsPageRegistration=0  or IsAudit=1) ");
                }
                else if (filter.RegistrationType == 0)
                {
                    strSql.Append(" and IsPageRegistration=0 ");
                }
                else if (filter.RegistrationType == 1)
                {
                    strSql.Append(" and IsPageRegistration=1 ");

                    // 注册用户的审核状态
                    if (filter.IsAudit.HasValue)
                        strSql.AppendFormat(" and IsAudit={0} ", filter.IsAudit);
                }
            }

            return strSql.ToString();
        }
    }
}

using System;
using System.Data;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using Dapper;
using BusinessEntities;
using MySql.Data.MySqlClient;

namespace DataAccessLayer
{
    /// <summary>
    /// 数据访问类:UserInfo
    /// </summary>
    public partial class UserInfoDAL
    {
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public bool Exists(int Id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from UserInfo where Id=@Id ");

            int result = 0;
            var param = new DynamicParameters();
            using (var conn = DBHelper.CreateConnection())
            {
                conn.Open();
                param.Add("@Id", Id, dbType: DbType.Int32);
                result = conn.Query<int>(strSql.ToString(), param).FirstOrDefault();
            }
            return result > 0;
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(UserInfo model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into UserInfo(");
            strSql.Append("LoginName,Password,Type)");
            strSql.Append(" values (");
            strSql.Append("@LoginName,@Password,@Type)");
            int result = 0;

            var param = new DynamicParameters();
            using (var conn = DBHelper.CreateConnection())
            {
                conn.Open();
                param.Add("@LoginName", model.LoginName, dbType: DbType.String);
                param.Add("@Password", model.Password, dbType: DbType.String);
                param.Add("@Type", model.Type, dbType: DbType.Int32);

                result = conn.Execute(strSql.ToString(), param);
            }
            return result;
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(UserInfo model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update UserInfo set ");
            strSql.Append("LoginName=@LoginName,");
            strSql.Append("Password=@Password,");
            strSql.Append("Type=@Type ");
            strSql.Append(" where Id=@Id ");

            int result = 0;
            var param = new DynamicParameters();
            using (var conn = DBHelper.CreateConnection())
            {
                conn.Open();
                param.Add("@Id", model.Id, dbType: DbType.Int32);
                param.Add("@LoginName", model.LoginName, dbType: DbType.String);
                param.Add("@Password", model.Password, dbType: DbType.String);
                param.Add("@Type", model.Type, dbType: DbType.Int32);
                result = conn.Execute(strSql.ToString(), param);
            }
            return result > 0;
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int Id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from UserInfo ");
            strSql.Append(" where Id=@Id ");

            int result = 0;
            var param = new DynamicParameters();
            using (var conn = DBHelper.CreateConnection())
            {
                conn.Open();
                param.Add("@Id", Id, dbType: DbType.Int32);
                result = conn.Execute(strSql.ToString(), param);
            }
            return result > 0;
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public UserInfo GetModel(int Id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from UserInfo ");
            strSql.Append(" where Id=@Id ");

            UserInfo model = null;
            var param = new DynamicParameters();
            using (var conn = DBHelper.CreateConnection())
            {
                conn.Open();
                param.Add("@Id", Id, dbType: DbType.Int32);
                model = conn.Query<UserInfo>(strSql.ToString(), param).FirstOrDefault();
            }
            return model;
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public UserInfo GetModel(string loginName)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from UserInfo ");
            strSql.Append(" where LoginName=@loginName ");

            UserInfo model = null;
            var param = new DynamicParameters();
            using (var conn = DBHelper.CreateConnection())
            {
                conn.Open();
                param.Add("@LoginName", loginName, dbType: DbType.String);
                model = conn.Query<UserInfo>(strSql.ToString(), param).FirstOrDefault();
            }
            return model;
        }

        #region  根据查询条件获取列表
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<UserInfo> GetList(CustomFilter filter)
        {
            StringBuilder strSql = new StringBuilder();
            var param = new DynamicParameters();

            strSql.Append("select Account.AccountNo,UserInfo.* ");
            strSql.Append(" FROM UserInfo, Account");
            strSql.Append(" where UserInfo.id=Account.UserId ");
            strSql.Append(GetStrWhere(filter));
            strSql.Append(" order by " + GetStrSort(filter));

            List<UserInfo> list = new List<UserInfo>();
            using (var conn = DBHelper.CreateConnection())
            {
                conn.Open();
                list = conn.Query<UserInfo>(strSql.ToString(), param).ToList();
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
            //if (filter.IdList != null && filter.IdList.Count > 0)
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
            //if (filter.IdList != null && filter.IdList.Count > 0)
            //{
            //    string StrValue = string.Empty;
            //    foreach (var item in filter.IdList)
            //    {
            //        StrValue += item + ",";
            //    }
            //    StrValue = StrValue.TrimEnd(',');
            //    strSql.AppendFormat(" and Id in({0}) ", StrValue);
            //}

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
            //if (filter.RegistrationType.HasValue && filter.RegistrationType != -1)
            //    strSql.AppendFormat(" and IsPageRegistration={0} ", filter.RegistrationType);



            return strSql.ToString();
        }

        private string GetStrSort(CustomFilter filter)
        {
            StringBuilder strSql = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(filter.SortName))
            {
                strSql.Append(filter.SortName);
            }
            else
            {
                strSql.Append("UserInfo.Id desc");              // 新添加的用户显示在前
            }
            if (filter.SortWay.HasValue)
            {
                strSql.Append(filter.SortWay.Value ? " " : " desc");
            }
            return strSql.ToString();
        }

        #endregion

        #region  获取分页参数
        /// <summary>
        /// 获取分页参数
        /// </summary>
        public PageModel GetUserInfoPageParams(CustomFilter filter = null)
        {
            bool crossTable = filter != null && filter.Id2List != null && filter.Id2List.Count > 0;

            PageModel model = new PageModel();
            model.Tables = "UserInfo" + (crossTable ? " left join UserClass on UserInfo.Id = UserClass.UserId" : "");
            model.PKey = "UserInfo.Id";
            model.Sort = GetStrSort(filter);
            model.Fields = "UserInfo.Id,UserName,Sex,Phone,Email,Status,UserInfo.RoleId,CreateTime,ModifyTime";
            model.Filter = GetStrWhere(filter) + (crossTable ? string.Format(" and UserClass.ClassId in({0})", string.Join(",", filter.Id2List)) : "");
            return model;
        }

        #endregion

        /*===============================自定义分界线============================*/

        /// <summary>
        /// 更新用户
        /// </summary>
        /// <param name="model"></param>
        /// <param name="author"></param>
        /// <returns></returns>
        public bool UpdateUserInfo(UserInfo model)
        {
            int result = 0;
            StringBuilder strSql = new StringBuilder();
            var param = new DynamicParameters();
            using (var conn = DBHelper.CreateConnection())
            {
                conn.Open();
                //var tran = conn.BeginTransaction();
                try
                {
                    #region 更新用户

                    strSql.Append("update UserInfo set ");
                    strSql.Append("LoginName=@LoginName,");
                    strSql.Append("Password=@Password,");
                    strSql.Append("Type=@Type");
                    strSql.Append(" where Id=@Id ");

                    param.Add("@Id", model.Id, dbType: DbType.Int32);
                    param.Add("@LoginName", model.LoginName, dbType: DbType.String);
                    param.Add("@Password", model.Password, dbType: DbType.String);
                    param.Add("@Type", model.Type, dbType: DbType.Int32);
                    result = conn.Execute(strSql.ToString(), param);

                    #endregion
                }
                catch (Exception ex)
                {
                }
            }
            return result > 0;
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids">将用户id批量传人，用逗号分开</param>
        public bool DeleteUserInfoBulk(List<int> ids)
        {
            int result = 0;
            StringBuilder strSql = new StringBuilder();
            using (var conn = DBHelper.CreateConnection())
            {
                conn.Open();
                var tran = conn.BeginTransaction();
                try
                {
                    strSql.Append("delete from UserInfo where Id in @Ids ");
                    result = conn.Execute(strSql.ToString(), new { ids = ids.ToArray() }, tran);

                    if (result > 0)
                    {
                        strSql.Clear();
                        strSql.Append("delete from Account where UserId in @Ids ");
                        conn.Execute(strSql.ToString(), new { ids = ids.ToArray() }, tran);

                        strSql.Clear();
                        strSql.Append("delete from UserClass where UserId in @ids ");
                        conn.Execute(strSql.ToString(), new { ids = ids.ToArray() }, tran);
                    }
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    result = 0;
                }
                finally
                {
                    if (conn != null)
                        conn.Close();
                }
            }
            return result > 0;
        }

        /// <summary>
        /// 更新用户状态
        /// </summary>
        /// <param name="ids">用户ID集合</param>
        /// <param name="status">用户操作状态</param>
        public bool UpdateUserStatus(List<int> ids, int status)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update UserInfo set Status={0} where Id in @ids");
            int result = 0;
            using (var conn = DBHelper.CreateConnection())
            {
                conn.Open();
                result = conn.Execute(string.Format(strSql.ToString(), status), new { ids = ids.ToArray() });
            }
            return result > 0;
        }

        public bool UpdateUserView(int IsView, int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update UserInfo set IsView={0} where Id ={1}");
            int result = 0;
            using (var conn = DBHelper.CreateConnection())
            {
                conn.Open();
                result = conn.Execute(string.Format(strSql.ToString(), IsView, id));
            }
            return result > 0;
        }


        #region cww 新增


        /// <summary>
        /// cww-用户分页，关联账号表和用户信息表
        /// </summary>
        public PageModel GetUserInfoPage(CustomFilter filter = null)
        {
            PageModel model = new PageModel();
            model.Tables = "UserInfo join Account on UserInfo.Id = Account.UserId";
            model.PKey = "UserInfo.Id";
            model.Sort = GetStrSort(filter);
            model.Fields = "UserInfo.*,Account.AccountNo";
            model.Filter = GetStrWhere(filter);
            return model;
        }

        /// <summary>
        /// 账号是否存在，返回true=账号存在
        /// </summary>
        /// <param name="accountNo">账号</param>
        /// <param name="userType">用户类型，3=评委，4=用户</param>
        /// <returns>true=账号存在</returns>
        public bool ExistsAccountNo(string accountNo, int userType, int collegeId, int userId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from Account a,UserInfo b where a.UserId=b.id and accountNo=@accountNo and Status<>3 and collegeId=@collegeId and b.id<>@userId");

            int result = 0;
            var param = new DynamicParameters();
            using (var conn = DBHelper.CreateConnection())
            {
                conn.Open();
                param.Add("@accountNo", accountNo, dbType: DbType.String);
                param.Add("@roleId", userType, dbType: DbType.Int32);
                param.Add("@collegeId", collegeId, dbType: DbType.Int32);
                param.Add("@userId", userId, dbType: DbType.Int32);
                result = conn.Query<int>(strSql.ToString(), param).FirstOrDefault();
            }
            return result > 0;
        }


        /// <summary>
        /// 判断账号是否存在
        /// </summary>
        /// <param name="accountNoList">账号列表</param>
        /// <param name="userType">用户类型，3=评委，4=用户</param>
        public List<string> ExistsAccountNo(IEnumerable<string> accountNoList, int userType, int collegeId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("select AccountNo from Account a, UserInfo b where a.userid=b.id and collegeId=" + collegeId + " and AccountNo in ('{0}') and Status<>3", string.Join("','", accountNoList));
            List<string> result = new List<string>();
            using (var conn = DBHelper.CreateConnection())
            {
                conn.Open();
                result = conn.Query<string>(strSql.ToString()).ToList();
            }
            return result;
        }


        /// <summary>
        /// 身份证号码是否存在，返回true=账号存在
        /// </summary>
        /// <param name="idCard">身份证号码</param>
        /// <param name="userType">用户类型，3=评委，4=用户</param>
        /// <returns>true=idCode</returns>
        public bool ExistsIdCard(string idCard, int userType, int collegeId, int userId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from UserInfo where IDCard=@idCard and Status<>3 and CollegeId=@collegeId and id<>@userId");

            int result = 0;
            var param = new DynamicParameters();
            using (var conn = DBHelper.CreateConnection())
            {
                conn.Open();
                param.Add("@idCard", idCard, dbType: DbType.String);
                param.Add("@roldId", userType, dbType: DbType.Int32);
                param.Add("@collegeId", collegeId, dbType: DbType.Int32);
                param.Add("@userId", userId, dbType: DbType.Int32);
                result = conn.Query<int>(strSql.ToString(), param).FirstOrDefault();
            }
            return result > 0;
        }

        /// <summary>
        /// 身份证号码是否存在，返回true=账号存在
        /// </summary>
        /// <param name="idCardList">身份证号码列表</param>
        /// <param name="userType">用户类型，3=评委，4=用户</param>
        public List<string> ExistsIDCardNoList(IEnumerable<string> idCardList, int userType, int collegeId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("select IDCard from UserInfo where IDCard in ('{0}') and Status<>3 and collegeId=" + collegeId + " and roleid=" + userType, string.Join("','", idCardList));
            List<string> result = new List<string>();
            using (var conn = DBHelper.CreateConnection())
            {
                conn.Open();
                result = conn.Query<string>(strSql.ToString()).ToList();
            }
            return result;
        }


        /// <summary>
        /// 批量修改用户审核状态
        /// </summary>
        /// <param name="ids">用户ID集合</param>
        /// <param name="audit">审核状态，1通过、2拒绝</param>
        public bool UpdateAudit(List<int> ids, int audit)
        {
            bool result = false;
            StringBuilder strSql = new StringBuilder();
            var param = new DynamicParameters();

            using (var conn = DBHelper.CreateConnection())
            {
                conn.Open();
                var tran = conn.BeginTransaction();
                try
                {
                    foreach (var item in ids)
                    {
                        strSql.Clear();
                        strSql.Append("update UserInfo set isaudit=@isAudit where Id =@userId");

                        param.Add("@isAudit", audit, dbType: DbType.Int32);
                        param.Add("@userId", item, dbType: DbType.Int32);

                        conn.Execute(strSql.ToString(), param, tran);
                    }
                    tran.Commit();
                    result = true;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                }
            }
            return result;
        }


        /// <summary>
        /// 返回未分组用户信息，但包括手动分组-右侧列表的用户信息
        /// </summary>
        /// <param name="collegeId">学校ID</param>
        /// <param name="competitionId">比赛Id</param>
        /// <param name="groupIds">忽略的用户分组</param>
        public List<UserInfo> NotGroupUser(int collegeId, int competitionId, int groupId, string query, int? pageIndex, int? pageSize, out int totalCount)
        {
            List<UserInfo> result = new List<UserInfo>();
            StringBuilder strSql = new StringBuilder();

            totalCount = 0;

            // 查询总行数
            strSql.Append("select count(1) from [GTA_FPBT_Structure_V1.5].dbo.UserInfo a, [GTA_FPBT_Structure_V1.5].dbo.Account b ");
            strSql.Append(" where RoleId=4 and a.IsAudit=1 and a.Id=b.UserId and a.Status=2 and a.CollegeId=" + collegeId);
            strSql.Append(" and ( a.id not in ( ");
            strSql.Append(" select userid ");
            strSql.Append(" from [GTA_FPBT_Match_V1.5].dbo.CompetitionUser where CompetitionId=" + competitionId);
            strSql.Append(" )or a.id in(select userid from [GTA_FPBT_Match_V1.5].dbo.CompetitionUser where CompetitionId=" + competitionId + "  ");

            if (groupId != 0)
                strSql.Append(" and GroupId=" + groupId + "))");
            else
                strSql.Append(" and GroupSouce=2 ))");

            if (!string.IsNullOrEmpty(query))
                strSql.Append(" and (a.UserName like '%" + query + "%' or b.AccountNo like '%" + query + "%')");

            using (var conn = DBHelper.CreateConnection())
            {
                conn.Open();
                totalCount = conn.Query<int>(strSql.ToString()).FirstOrDefault();
            }

            // 查询分页数据
            strSql.Clear();
            strSql.Append("select * from (");
            strSql.Append("select row_number() over (order by a.id) as no, a.*,b.AccountNo from [GTA_FPBT_Structure_V1.5].dbo.UserInfo a, [GTA_FPBT_Structure_V1.5].dbo.Account b ");
            strSql.Append(" where RoleId=4 and a.IsAudit=1 and a.Id=b.UserId and a.Status=2 and a.CollegeId=" + collegeId);
            strSql.Append(" and ( a.id not in ( ");
            strSql.Append(" select userid ");
            strSql.Append(" from [GTA_FPBT_Match_V1.5].dbo.CompetitionUser where CompetitionId=" + competitionId);
            strSql.Append(" )or a.id in(select userid from [GTA_FPBT_Match_V1.5].dbo.CompetitionUser where CompetitionId=" + competitionId + "  ");

            if (groupId != 0)
                strSql.Append(" and GroupId=" + groupId + "))");
            else
                strSql.Append(" and GroupSouce=2 ))");

            if (!string.IsNullOrEmpty(query))
                strSql.Append(" and (a.UserName like '%" + query + "%' or b.AccountNo like '%" + query + "%')");

            strSql.Append(" ) x  where no BETWEEN " + ((pageIndex - 1) * pageSize + 1) + " and " + (pageIndex * pageSize) + "  order by x.id");

            //strSql.Append("select top " + pageSize + " a.*,b.AccountNo from [GTA_FPBT_Structure_V1.5].dbo.UserInfo a, [GTA_FPBT_Structure_V1.5].dbo.Account b ");
            //strSql.Append(" where RoleId=4 and a.Id=b.UserId and a.Status=2 and a.CollegeId=" + collegeId);
            //strSql.Append(" and ( a.id not in ( ");
            //strSql.Append(" select top " + (pageIndex - 1) * pageSize + " a.id from [GTA_FPBT_Structure_V1.5].dbo.UserInfo a, [GTA_FPBT_Structure_V1.5].dbo.Account b");
            //strSql.Append(" where RoleId=4 and a.Id=b.UserId and a.Status=2 and a.CollegeId=" + collegeId + " and(  a.id not in (select userid ");
            //strSql.Append(" from [GTA_FPBT_Match_V1.5].dbo.CompetitionUser where CompetitionId=" + competitionId);
            //strSql.Append(" )or a.id in(select userid from [GTA_FPBT_Match_V1.5].dbo.CompetitionUser where CompetitionId=" + competitionId + "  ");

            //if (groupId != 0)
            //    strSql.Append(" and GroupId=" + groupId + ")))");
            //else
            //    strSql.Append(" and GroupSouce=2 )))");

            //strSql.Append(" and	a.id not in (select userid from [GTA_FPBT_Match_V1.5].dbo.CompetitionUser where CompetitionId=" + competitionId);
            //strSql.Append(" )or a.id in(select userid from [GTA_FPBT_Match_V1.5].dbo.CompetitionUser where CompetitionId=6 ");
            //if (groupId != 0)
            //    strSql.Append(" and GroupId=" + groupId + "))");
            //else
            //    strSql.Append(" and GroupSouce=2 ))");

            //if (!string.IsNullOrEmpty(query))
            //    strSql.Append(" and (a.UserName like '%" + query + "%' or b.AccountNo like '%" + query + "%')");

            using (var conn = DBHelper.CreateConnection())
            {
                conn.Open();
                result = conn.Query<UserInfo>(strSql.ToString()).ToList();
            }
            return result;
        }


        /// <summary>
        /// 根据账号，查询用户信息
        /// </summary>
        /// <param name="accountNo"></param>
        /// <param name="collegeId"></param>
        /// <returns></returns>
        public UserInfo GetAccountNoToInfo(string accountNo, int collegeId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select a.*,b.accountNo from UserInfo a,Account b where a.id=b.userid and AccountNo=@AccountNo and Status<>3 and CollegeId=@collegeId");

            UserInfo result = new UserInfo();
            var param = new DynamicParameters();
            using (var conn = DBHelper.CreateConnection())
            {
                conn.Open();
                param.Add("@AccountNo", accountNo, dbType: DbType.String);
                param.Add("@collegeId", collegeId, dbType: DbType.Int32);
                result = conn.Query<UserInfo>(strSql.ToString(), param).FirstOrDefault();
            }
            return result;
        }

        #endregion

        /// <summary>
        /// 根据身份证ID获取单个用户信息
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="CollegeId"></param>
        /// <returns></returns>
        public UserInfo GetUserInfoByID(string ID, int CollegeId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from UserInfo where IDCard=@IDCard and RoleId=4 and Status=2 and IsAudit=1 and CollegeId=@CollegeId");

            UserInfo result = null;
            var param = new DynamicParameters();
            using (var conn = DBHelper.CreateConnection())
            {
                conn.Open();
                param.Add("@IDCard", ID, dbType: DbType.String);
                param.Add("@CollegeId", CollegeId, dbType: DbType.Int32);
                result = conn.Query<UserInfo>(strSql.ToString(), param).FirstOrDefault();
            }
            return result;
        }

        #region Test
        public UserInfo Login(string name)
        {

            MySqlCommand sqlcom = new MySqlCommand();
            sqlcom.CommandText = "select * from common_members where LoginName =@LoginName";
            MySqlParameter commandParameters = new MySqlParameter("@LoginName", name);
            MySqlDataReader reader = MySqlHelper.ExecuteReader(MySqlHelper.ConnectionStringManager, CommandType.Text, sqlcom.CommandText, commandParameters);

            return new UserInfo();
        }
        #endregion
    }
}


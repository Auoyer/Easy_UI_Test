using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace BusinessEntities
{
    /// <summary>
    /// 通用查询条件过滤器
    /// </summary>
    public class CustomFilter
    {
        public CustomFilter()
        {
            KeyWord1Ex = false;
        }

        /// <summary>
        /// 主键Id
        /// </summary>
        public Nullable<int> Id { get; set; }

        /// <summary>
        /// 2号Id（搜账户时传用户ID）
        /// </summary>
        public Nullable<int> Id2 { get; set; }

        /// <summary>
        /// 主键Id列表(搜账号/用户班级关系时传用户Id列表）
        /// </summary>
        public List<int> IdList { get; set; }
        /// <summary>
        /// 次键Id列表(搜用户时传班级ID列表）
        /// </summary>
        public List<int> Id2List { get; set; }

        /// <summary>
        /// 类型Id（用户角色类型）
        /// </summary>
        public Nullable<int> TypeId { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        public Nullable<int> UserId { get; set; }

        /// <summary>
        /// 状态Id
        /// </summary>
        public Nullable<int> StatusId { get; set; }

        /// <summary>
        /// 关键字1（学生姓名）
        /// </summary>
        public string KeyWord { get; set; }
        /// <summary>
        /// 关键字2（学生学号）
        /// </summary>
        public string KeyWord2 { get; set; }

        /// <summary>
        /// 排序字段
        /// </summary>
        public string SortName { get; set; }
        /// <summary>
        /// true 升序 false 降序
        /// </summary>
        public bool? SortWay { get; set; }

        /// <summary>
        /// 只针对姓名和学号
        /// </summary>
        public bool KeyWord1Ex { get; set; }

        /// <summary>
        /// 注册类型，1=官网注册，0=管理员创建
        /// </summary>
        public Nullable<int> RegistrationType { get; set; }

        /// <summary>
        /// 是否审核通过，0=未审核，1=已通过，2=拒绝
        /// </summary>
        public Nullable<int> IsAudit { get; set; }

        /// <summary>
        /// 学校ID
        /// </summary>
        public int CollegeId { get; set; }


        /// <summary>
        /// 学校名称，域名，学校编码
        /// </summary>
        public string CollegeKey { get; set; }

        /// <summary>
        /// 用户类型（1试用帐号、2普通帐号）
        /// </summary>
        public int? UserType { get; set; }

        /// <summary>
        /// 是否查看（1，已查看；0，为查看），针对首页到期帐号数量，点查看以后数量递减。
        /// </summary>
        public string IsView { get; set; }

        public string DateKey { get; set; }
    }
}

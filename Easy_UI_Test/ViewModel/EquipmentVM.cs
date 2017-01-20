using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel.Enum;

namespace ViewModel
{
    /// <summary>
    /// 设备信息
    /// </summary>
    public class EquipmentVM
    {
        public int Id { get; set; }

        public int AreaId { get; set; }

        public string StrArea
        {
            get { return EnumHelper.GetEnumDesc((Areas)AreaId); }
        }

        public int ControlId { get; set; }

        public string StrControl
        {
            get { return EnumHelper.GetEnumDesc((Controls)ControlId); }
        }

        public string Equip { get; set; }

        public int ControlType { get; set; }

        public string StrControlType
        {
            get { return EnumHelper.GetEnumDesc((ControlType)ControlType); }
        }

        public int Status { get; set; }

        public string StrStatus
        {
            get { return EnumHelper.GetEnumDesc((LightPara)Status); }
        }

        public DateTime LastTime { get; set; }

        public string StrLastTime {
            get { return LastTime.ToString("yyyy-MM-dd HH:mm:ss"); }
        }

        public List<string> Operations { get; set; }
    }
}

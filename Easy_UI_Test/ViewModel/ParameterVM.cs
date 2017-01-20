using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    /// <summary>
    /// 设备参数
    /// </summary>
    public class ParameterVM
    {
        public int Id { get; set; }

        public int EquipmentId { get; set; }

        public int ParaType { get; set; }

        public int ParaValue { get; set; }
    }
}

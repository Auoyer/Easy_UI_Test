using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    /// <summary>
    /// 设备能耗
    /// </summary>
    public class EnergyVM
    {
        public int Id { get; set; }

        public int EquipmentId { get; set; }

        public float Voltage { get; set; }

        public float Strength { get; set; }

        public float Power { get; set; }

        public DateTime Time { get; set; }
    }
}

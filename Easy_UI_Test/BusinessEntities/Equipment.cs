using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities
{
    public class Equipment
    {
        public int Id { get; set; }

        public int AreaId { get; set; }

        public int ControlId { get; set; }

        public string Equip { get; set; }

        public int ControlType { get; set; }

        public int Status { get; set; }

        public DateTime LastTime { get; set; }
    }
}

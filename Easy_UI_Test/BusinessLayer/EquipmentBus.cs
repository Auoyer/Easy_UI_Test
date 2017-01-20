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
    public class EquipmentBus
    {
        public List<EquipmentVM> GetList(CustomFilter filter)
        {
            var result = new EquipmentDAL().GetList(filter).MapList<EquipmentVM, Equipment>();
            return result;
        }
    }
}

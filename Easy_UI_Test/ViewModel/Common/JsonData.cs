using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.Common
{
    /// <summary>
    /// 自定义Json返回值
    /// </summary>
    public class JsonData
    {
        /// <summary>
        /// 操作结果
        /// </summary>
        public bool JResult { get; set; }

        /// <summary>
        /// 返回数据
        /// </summary>
        public object JData { get; set; }

        public JsonData(bool result)
        {
            JResult = result;
        }

        public JsonData(bool result,object date)
        {
            JResult = result;
            JData = date;
        }
    }
}

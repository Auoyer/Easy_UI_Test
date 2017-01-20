using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.Enum
{
    /// <summary>
    /// 灯光设备参数
    /// </summary>
    public enum LightPara
    {
        /// <summary>
        /// 启动阀值
        /// </summary>
        [Description("启动")]
        Open = 0,

        /// <summary>
        /// 空闲阀值
        /// </summary>
        [Description("空闲")]
        Idle = 1,

        /// <summary>
        /// 工作阀值
        /// </summary>
        [Description("工作")]
        Busy = 2,

        /// <summary>
        /// 关闭阀值
        /// </summary>
        [Description("关闭")]
        Close = 3,

        /// <summary>
        /// 延迟时间
        /// </summary>
        [Description("延迟")]
        Delay = 4
    }

    /// <summary>
    /// 控制方式
    /// </summary>
    public enum ControlType
    {
        /// <summary>
        /// 自动
        /// </summary>
        [Description("自动")]
        Automatic = 0,

        /// <summary>
        /// 半自动
        /// </summary>
        [Description("半自动")]
        Semi_auto = 1,

        /// <summary>
        /// 手动
        /// </summary>
        [Description("手动")]
        Manual = 2,
    }

    /// <summary>
    /// 区域
    /// </summary>
    public enum Areas
    {
        /// <summary>
        /// 加油站
        /// </summary>
        [Description("加油站")]
        Workstation = 1,

        /// <summary>
        /// 便利店
        /// </summary>
        [Description("便利店")]
        Store = 2,

        /// <summary>
        /// 办公间
        /// </summary>
        [Description("办公间")]
        Office = 3,

        /// <summary>
        /// 餐厅
        /// </summary>
        [Description("餐厅")]
        Restaurant = 4,

        /// <summary>
        /// 厕所
        /// </summary>
        [Description("厕所")]
        Toilet = 5,

        /// <summary>
        /// 其他
        /// </summary>
        [Description("其他")]
        Other = 6

    }

    /// <summary>
    /// 控制器
    /// </summary>
    public enum Controls
    {
        /// <summary>
        /// 光感设备
        /// </summary>
        [Description("光感设备")]
        Light = 1,

        /// <summary>
        /// 压感设备
        /// </summary>
        [Description("压感设备")]
        Pressure = 2,

        /// <summary>
        /// 声感设备
        /// </summary>
        [Description("声感设备")]
        Voice = 3,

        /// <summary>
        /// 其他
        /// </summary>
        [Description("其他")]
        Other = 4
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ScadaWcfLibrary
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的类名“SvcManage”。
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class SvcManage : ISvcManage
    {
        private List<DevInfo> devList = new List<DevInfo>();

        /// <summary>
        /// 获取接入设备的信息
        /// </summary>
        /// <returns>返回设备信息列表</returns>
        public List<DevInfo> GetDeviceList()
        {
            return devList;
        }

        public uint GetDeviceCount()
        {
            return (uint)devList.Count();
        }

        /// <summary>
        /// 添加一个设备
        /// </summary>
        /// <param name="dev">某个设备的接入信息</param>
        public void AddDevice(DevInfo dev)
        {
            if(dev != null)
            {
                devList.Add(dev);
            }
        }

    }
}

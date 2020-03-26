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
        private Queue<string> mylog = new Queue<string>();

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

        string pbuf;
        public string PullData()
        {
            return pbuf;
        }


        public string PullLog()
        {
            if (mylog.Count == 0)
                return string.Empty;
            else
            {
                return mylog.Dequeue();
            }           
        }

        #region 宿主程序使用

        public void PushLog(string log)
        {
            mylog.Enqueue(log);
        }


        public void PushData(byte[] data)
        {
            pbuf = Encoding.Default.GetString(data);
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

        /// <summary>
        /// 根据connId获取设备连接信息
        /// </summary>
        /// <param name="_connId"></param>
        /// <returns></returns>
        public DevInfo GetDevInfo(uint _connId)
        {
            DevInfo ret;
            try
            {
                ret = devList.Where(x => x.ConnId == _connId).First();
            }
            catch(Exception)
            {
                ret = null;
            }
            return ret;
        }

        /// <summary>
        /// 根据连接ID获取产品名称
        /// </summary>
        /// <param name="_connId"></param>
        /// <returns></returns>
        public string GetProdName(uint _connId)
        {
            DevInfo dev = GetDevInfo(_connId);
            return (dev != null) ? dev.ProdName : string.Empty;
        }

        /// <summary>
        /// 根据连接ID获取设备ID
        /// </summary>
        /// <param name="_connId"></param>
        /// <returns></returns>
        public uint GetDevId(uint _connId)
        {
            DevInfo dev = GetDevInfo(_connId);
            return (dev != null) ? dev.DevId : 0;
        }

        /// <summary>
        /// 更新设备的最后活跃时间
        /// </summary>
        /// <param name="_connId">连接ID</param>
        /// <returns>true:成功； false:未查询到该连接</returns>
        public bool UpdateActiveTime(uint _connId)
        {
            for(int i = 0; i < devList.Count; i++)
            {
                if( devList[i].ConnId == _connId )
                {
                    devList[i].LastActiveTime = DateTime.Now;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 设备登录
        /// </summary>
        /// <param name="_connId">连接ID</param>
        /// <param name="_prodName">产品名称</param>
        /// <param name="_devId">设备ID</param>
        /// <returns>登录是否成功</returns>
        public bool Login(DevInfo dev)
        {
            if(dev != null)
            {
                for (int i = 0; i < devList.Count; i++)
                {
                    if (devList[i].DevId == dev.DevId)  //设备Id存在，则进行更新
                    {
                        devList[i].ConnId = dev.ConnId;
                        devList[i].IsOnline = true;
                        devList[i].ProdName = dev.ProdName;
                        devList[i].ConnTime = dev.ConnTime;
                        devList[i].LastActiveTime = dev.LastActiveTime;
                        return true;
                    }
                }
                AddDevice(dev); //设备不存在，则添加一个设备
            }
            return false;
        }

        /// <summary>
        /// 更新连接状态
        /// </summary>
        /// <param name="_connId">连接ID</param>
        /// <param name="_isOnline">是否在线</param>
        public void UpdateConnStatus(uint _connId, bool _isOnline)
        {
            for(int i = 0; i < devList.Count; i++)
            {
                if( devList[i].ConnId == _connId )
                {
                    devList[i].IsOnline = _isOnline;
                    break;
                }
            }
        }

        #endregion

    }
}

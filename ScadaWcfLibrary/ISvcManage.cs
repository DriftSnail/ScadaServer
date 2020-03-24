using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ScadaWcfLibrary
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的接口名“ISvcManage”。
    [ServiceContract]
    public interface ISvcManage
    {
        [OperationContract]
        List<DevInfo> GetDeviceList();
        [OperationContract]
        uint GetDeviceCount();
        [OperationContract]
        void AddDevice(DevInfo dev);

    }

    /// <summary>
    /// 描述设备接入信息
    /// </summary>
    [DataContract]
    public class DevInfo
    {
        [DataMember]
        public uint DevId { get; set; }
        [DataMember]
        public uint ConnId { get; set; }
        [DataMember]
        public string ProdName { get; set; }
        [DataMember]
        public bool IsOnline { get; set; }

        public DevInfo(uint _devId, uint _connId, string _prodName, bool _isOnline)
        {
            DevId = _devId;
            ConnId = _connId;
            ProdName = _prodName;
            IsOnline = _isOnline;
        }
    }

}

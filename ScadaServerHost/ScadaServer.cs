using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using HPSocket;
using HPSocket.Tcp;
using ScadaWcfLibrary;

namespace ScadaServerHost
{

    /// <summary>
    /// 描述服务器状态
    /// </summary>
    public enum ServerState
    {
        Starting,
        Started,
        Stopping,
        Stoped,
        Error
    }


    class ScadaServer
    {
        private ServerState state = ServerState.Stoped; //服务器状态
        private string serverName;
        private SvcManage Svc;

        #region 服务器相关属性

        /// <summary>
        /// 服务器状态
        /// </summary>
        public ServerState State
        {
            get { return state; }
        }

        /// <summary>
        /// 服务器名称
        /// </summary>
        public string ServerName
        {
            get { return serverName; }
        }

        /// <summary>
        /// 服务器IP
        /// </summary>
        public string ServerIp
        {
            get { return server.Address; }
        }

        /// <summary>
        /// 服务器端口号
        /// </summary>
        public uint ServerPort
        {
            get { return server.Port; }
        }


        #endregion

        public TcpPullServer server = new TcpPullServer();
        private HPSocket.Thread.ThreadPool receiveThreadPool = new HPSocket.Thread.ThreadPool();

        public ScadaServer(string _name, string _ip, ushort _port, SvcManage svc)
        {

            state = ServerState.Stoped;
            serverName = _name;
            server.Address = _ip;
            server.Port = _port;
            Svc = svc;
            //绑定事件
            //绑定监听地址前触发
            server.OnPrepareListen += new ServerPrepareListenEventHandler(server_OnPrepareListen);
            //客户端连接请求被接受后触发
            server.OnAccept += new ServerAcceptEventHandler(server_OnAccept);
            //发送消息后触发
            server.OnSend += new ServerSendEventHandler(server_OnSend);
            //收到消息后触发
            server.OnReceive += new PullServerReceiveEventHandler(server_OnReceive);
            //连接关闭后触发（服务端的连接通常是多个，只要某一个连接关闭了都会触发）
            server.OnClose += new ServerCloseEventHandler(server_OnClose);
            //组件停止后触发
            server.OnShutdown += new ServerShutdownEventHandler(server_OnShutdown);

        }

        public void Start()
        {
            try
            {
                state = ServerState.Starting;

                if (server.Start())
                {
                    state = ServerState.Started;
                }
                else
                {
                    state = ServerState.Stoped;
                    throw new Exception(string.Format("服务端启动失败：{0}，{1}", server.ErrorMessage, server.ErrorCode));
                }
                receiveThreadPool.Start(5, HPSocket.Thread.RejectedPolicy.WaitFor, 0, 0);
            }
            catch (Exception exc)
            {
                Console.Write(exc.Message);
            }


        }
        public void Stop() { }

        #region Server事件处理方法

        private HandleResult server_OnPrepareListen(IServer sender, IntPtr listen)
        {
#if DEBUG
            Console.Write("开始监听");
#endif
            return HandleResult.Ok;
        }

        /// <summary>
        /// 服务器接收一个连接时触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="connId"></param>
        /// <param name="client"></param>
        /// <returns></returns>
        private HandleResult server_OnAccept(IServer sender, IntPtr connId, IntPtr client)
        {
            //Svc.AddDevice(new DevInfo((uint)connId, 0, "Unknown", true, DateTime.Now, DateTime.Now));     //在连接列表添加一个设备

            return HandleResult.Ok;
        }

        private HandleResult server_OnSend(IServer sender, IntPtr connId, byte[] bytes)
        {
            return HandleResult.Ok;
        }

        private HandleResult server_OnReceive(IServer sender, IntPtr connId, int length)
        {
            //string recievedStr = Encoding.Default.GetString(data);
            byte[] data;
            server.Fetch(connId, length, out data);
            RecvPkg pkg = new RecvPkg((uint)connId, data);
            receiveThreadPool.Submit(AnalyseReciveData, pkg, -1);

#if DEBUG
            Console.WriteLine(string.Format("收到连接ID：{0} 的信息，长度：{1}，内容：{2}", connId, length, BitConverter.ToString(data)));
#endif
            return HandleResult.Ok;
        }

        //当触发了OnClose事件时，表示连接已经被关闭，并且OnClose事件只会被触发一次
        //通过errorCode参数判断是正常关闭还是异常关闭，0表示正常关闭
        private HandleResult server_OnClose(IServer sender, IntPtr connId, SocketOperation enOperation, int errorCode)
        {

            if (errorCode == 0)
            {
#if DEBUG
                Console.Write(string.Format("连接已断开，连接ID：{0}", connId));
#endif
            }
            else
            {
#if DEBUG
                Console.Write(string.Format("客户端连接发生异常，已经断开连接，连接ID：{0}，错误代码：{1}", connId, errorCode));
#endif
            }

            return HandleResult.Ok;
        }

        private HandleResult server_OnShutdown(IServer sender)
        {
            state = ServerState.Stoped;
#if DEBUG
            Console.WriteLine("服务端已经停止服务");
#endif

            return HandleResult.Ok;
        }

        #endregion Server事件处理方法


        #region  数据处理与解析部分

        public class RecvPkg
        {
            public uint connId;
            public byte[] data;
            
            public RecvPkg(uint _connId, byte[] _data)
            {
                connId = _connId;
                data = _data;
            }

        }


        /// <summary>
        /// 用于分析和处理接收的数据
        /// </summary>
        /// <param name="obj"></param>
        private void AnalyseReciveData(object obj)
        {
            RecvPkg pkg = (RecvPkg)obj;
            DevInfo tmp;
            if( Svc.UpdateActiveTime(pkg.connId) ) //查询到连接，更新活跃时间
            {
                
            }
            else if( ParseLoginPkg(pkg, out tmp) )//未查询到该连接，但解析登录包正确，则进行登录操作
            {
                Svc.Login(tmp);
            }
            else //登录不成功，把设备踢下线
            {
                server.Disconnect(new IntPtr(pkg.connId), true);
            }

        }

        /// <summary>
        /// 解析登录包
        /// </summary>
        /// <param name="data"></param>
        /// <param name="info"></param>
        /// <returns>登录是否成功</returns>
        private bool ParseLoginPkg(RecvPkg pkg, out DevInfo info)
        {
            info = new DevInfo();
            info.ConnId = pkg.connId;
            info.IsOnline = true;
            info.ConnTime = DateTime.Now;
            info.LastActiveTime = DateTime.Now;

            int i;
            for (i = 0; i < pkg.data.Length; i++)
            {
                if (pkg.data[i] == '#')
                    break;
            }
            if (pkg.data.First() == '*' && pkg.data.Last() == '*' && i > 0)
            {
                string s = Encoding.Default.GetString(pkg.data);
                try
                {
                    info.ProdName = s.Substring(1, i-1);
                    info.DevId = Convert.ToUInt32(s.Substring(i + 1, s.Length - i - 2));
                    return true;
                }
                catch(Exception)
                {
                    info = null;
                    return false;
                }

            }
            else
            {
                info = null;
                return false;
            }
        }
        

        #endregion



    }
}

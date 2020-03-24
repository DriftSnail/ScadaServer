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
            get{return state;}
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
            }
            catch (Exception exc)
            {
                Console.Write(exc.Message);
            }
            

        }
        public void Stop() {  }

        #region Server事件处理方法

        private HandleResult server_OnPrepareListen(IServer sender, IntPtr listen)
        {
#if DEBUG
            Console.Write("开始监听");
#endif
            return HandleResult.Ok;
        }

        private HandleResult server_OnAccept(IServer sender, IntPtr connId, IntPtr client)
        {
            Svc.AddDevice(new DevInfo(1,3,"",false));
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


    }
}

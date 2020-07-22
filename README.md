## SCADA Server Demo

### 一，简介

​		这是一个数据采集服务中心的Demo，基于.NET framework 4.5。它可以处理大量客户端的连接，客户端连接成功后，需发送自身的注册包，才视为注册成功。数据解析部分只实现了一个简单的示例：Server把接收到的数据按协议格式解析后存入MySQL数据库。

​		该项目的只是一个采集中心的Demo，一般用于测试，或初步参考。因本人初学C#，程序尚有很多不足之处，还望见谅。



### 二，开发环境

**平台**：Visual Studio 2019

**运行环境**：.NET Framework 4.5.2

**程序架构**：WCF框架 (Windows Communication Foundation)



### 三，架构设计

![](.\Image\scada.png)

注：消息队列，暂未采用ZeroMQ框架。

### 四，效果图

以下是给Scada Server后台服务写的一个管控界面程序（基于WCF框架）：

![](.\Image\scada HMI .png)

以下是数据库更新的内容：

![](.\Image\database.png)
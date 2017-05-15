using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace LEDScreenPrint_Client
{
    public partial class FrmMain : Form
    {
        private String socketIPAddress;
        private int socketPort;
        private byte[] socketResult = new byte[1024];
        private Socket socketClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private String configFilePath;

        public FrmMain()
        {
            InitializeComponent();
            //获取Ini配置文件绝对路径
            configFilePath = System.Windows.Forms.Application.StartupPath + "\\Config.ini";
            //获取Server服务IP地址
            socketIPAddress = ConfigHelper.ReadIniData("Setting", "ServerIP", "127.0.0.1", configFilePath);
            //获取Server服务端口号
            socketPort = Convert.ToInt32(ConfigHelper.ReadIniData("Setting", "ServerPort", "8888", configFilePath));
            //初始化连接
            initSocket();
        }

        private void initSocket()
        {
            //设定服务器IP地址
            IPAddress IP = IPAddress.Parse(socketIPAddress);
            try
            {
                //配置服务器IP与端口
                socketClient.Connect(new IPEndPoint(IP, socketPort));
            }
            catch
            {
                MessageBox.Show("连接服务器失败，请重新设置！");
                Application.Exit();
            }
            Thread receiveThread = new Thread(ReceiveMessage);
            receiveThread.Start();
        }

        private void ReceiveMessage()
        {
            while (true)
            {
                try
                {
                    //通过clientSocket接收数据
                    int receiveNumber = socketClient.Receive(socketResult);
                    string strContent = Encoding.ASCII.GetString(socketResult, 0, receiveNumber);
                    Console.WriteLine("接收服务端{0}消息{1}", socketClient.RemoteEndPoint.ToString(), Encoding.ASCII.GetString(socketResult, 0, receiveNumber));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    socketClient.Shutdown(SocketShutdown.Both);
                    socketClient.Close();
                    break;
                }
            }
        }

        private void SendMessage(string message)
        {
            string sendMessage = message;
            socketClient.Send(Encoding.ASCII.GetBytes(sendMessage));
        }

    }
}

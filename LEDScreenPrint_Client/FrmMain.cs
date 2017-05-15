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
        private int sendMessage = 0;

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
            //载入配置文件
            LoadSetting();
            
        }


        private void LoadSetting()
        {
            //设置位置信息
            nudNowX.Value = Convert.ToDecimal(ConfigHelper.ReadIniData("Location", "NowX", "0", configFilePath));
            nudNowY.Value = Convert.ToDecimal(ConfigHelper.ReadIniData("Location", "NowY", "0", configFilePath));
            nudNowDateX.Value = Convert.ToDecimal(ConfigHelper.ReadIniData("Location", "NowDateX", "0", configFilePath));
            nudNowDateY.Value = Convert.ToDecimal(ConfigHelper.ReadIniData("Location", "NowDateY", "0", configFilePath));
            nudNowWeekX.Value = Convert.ToDecimal(ConfigHelper.ReadIniData("Location", "NowWeekX", "0", configFilePath));
            nudNowWeekY.Value = Convert.ToDecimal(ConfigHelper.ReadIniData("Location", "NowWeekY", "0", configFilePath));
            nudNowTimeX.Value = Convert.ToDecimal(ConfigHelper.ReadIniData("Location", "NowTimeX", "0", configFilePath));
            nudNowTimeY.Value = Convert.ToDecimal(ConfigHelper.ReadIniData("Location", "NowTimeY", "0", configFilePath));
            nudTitleX.Value = Convert.ToDecimal(ConfigHelper.ReadIniData("Location", "TitleX", "0", configFilePath));
            nudTitleY.Value = Convert.ToDecimal(ConfigHelper.ReadIniData("Location", "TitleY", "0", configFilePath));
            nudFightX.Value = Convert.ToDecimal(ConfigHelper.ReadIniData("Location", "FightX", "0", configFilePath));
            nudFightY.Value = Convert.ToDecimal(ConfigHelper.ReadIniData("Location", "FightY", "0", configFilePath));
            nudFightDateX.Value = Convert.ToDecimal(ConfigHelper.ReadIniData("Location", "FightDateX", "0", configFilePath));
            nudFightDateY.Value = Convert.ToDecimal(ConfigHelper.ReadIniData("Location", "FightDateY", "0", configFilePath));
            nudFightWeekX.Value = Convert.ToDecimal(ConfigHelper.ReadIniData("Location", "FightWeekX", "0", configFilePath));
            nudFightWeekY.Value = Convert.ToDecimal(ConfigHelper.ReadIniData("Location", "FightWeekY", "0", configFilePath));
            nudFightTimeX.Value = Convert.ToDecimal(ConfigHelper.ReadIniData("Location", "FightTimeX", "0", configFilePath));
            nudFightTimeY.Value = Convert.ToDecimal(ConfigHelper.ReadIniData("Location", "FightTimeY", "0", configFilePath));
            //设置字体大小
            nudNowSize.Value = Convert.ToDecimal(ConfigHelper.ReadIniData("FrontSize", "NowSize", "9", configFilePath));
            nudNowDateSize.Value = Convert.ToDecimal(ConfigHelper.ReadIniData("FrontSize", "NowDateSize", "9", configFilePath));
            nudNowWeekSize.Value = Convert.ToDecimal(ConfigHelper.ReadIniData("FrontSize", "NowWeekSize", "9", configFilePath));
            nudNowTimeSize.Value = Convert.ToDecimal(ConfigHelper.ReadIniData("FrontSize", "NowTimeSize", "9", configFilePath));
            nudFightSize.Value = Convert.ToDecimal(ConfigHelper.ReadIniData("FrontSize", "FightSize", "9", configFilePath));
            nudFightDateSize.Value = Convert.ToDecimal(ConfigHelper.ReadIniData("FrontSize", "FightDateSize", "9", configFilePath));
            nudFightWeekSize.Value = Convert.ToDecimal(ConfigHelper.ReadIniData("FrontSize", "FightWeekSize", "9", configFilePath));
            nudFightTimeSize.Value = Convert.ToDecimal(ConfigHelper.ReadIniData("FrontSize", "FightTimeSize", "9", configFilePath));
            nudTitleSize.Value = Convert.ToDecimal(ConfigHelper.ReadIniData("FrontSize", "TitleSize", "9", configFilePath));
            //设置作战时间
            nudFightTimeYear.Value = Convert.ToDecimal(ConfigHelper.ReadIniData("FightTimeSetting", "FightTimeYear", DateTime.Now.ToString("yyyy"), configFilePath));
            nudFightTimeMonth.Value = Convert.ToDecimal(ConfigHelper.ReadIniData("FightTimeSetting", "FightTimeMonth", DateTime.Now.ToString("MM"), configFilePath));
            nudFightTimeDay.Value = Convert.ToDecimal(ConfigHelper.ReadIniData("FightTimeSetting", "FightTimeDay", DateTime.Now.ToString("dd"), configFilePath));
            nudFightTimeHour.Value = Convert.ToDecimal(ConfigHelper.ReadIniData("FightTimeSetting", "FightTimeHour", DateTime.Now.ToString("HH"), configFilePath));
            nudFightTimeMinute.Value = Convert.ToDecimal(ConfigHelper.ReadIniData("FightTimeSetting", "FightTimeMinute", DateTime.Now.ToString("mm"), configFilePath));
            nudFightTimeSecond.Value = Convert.ToDecimal(ConfigHelper.ReadIniData("FightTimeSetting", "FightTimeSecond", DateTime.Now.ToString("ss"), configFilePath));
            //设置显示文字
            tbxTitle.Text = ConfigHelper.ReadIniData("Title", "Title", "", configFilePath);
            //设置窗体颜色及大小
            nudScreenHeight.Value = Convert.ToDecimal(ConfigHelper.ReadIniData("Screen", "ScreenHeight", "200", configFilePath));
            sendMessage = 1;
        }
        private void initSocket()
        {
            //设定服务器IP地址
            IPAddress ip = IPAddress.Parse(socketIPAddress);
            try
            {
                socketClient.Connect(new IPEndPoint(ip, socketPort)); //配置服务器IP与端口
                Console.WriteLine("连接服务器成功");
            }
            catch
            {
                Console.WriteLine("连接服务器失败，请按回车键退出！");
                return;
            }
        }

        private void SendMessage(string message)
        {
            try
            {
                if (socketClient.Connected)
                {
                    socketClient.Send(Encoding.UTF8.GetBytes(message));
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            
        }

        private void nudNowX_ValueChanged(object sender, EventArgs e)
        {
            if (sendMessage == 1)
            {
                ConfigHelper.WriteIniData("Location", "NowX", nudNowX.Value.ToString(), configFilePath);
                SendMessage("NowX|" + nudNowX.Value.ToString());
            }
            
        }

        private void nudNowDateX_ValueChanged(object sender, EventArgs e)
        {
            if (sendMessage == 1)
            {
                ConfigHelper.WriteIniData("Location", "NowDateX", nudNowDateX.Value.ToString(), configFilePath);
                SendMessage("NowDateX|" + nudNowDateX.Value.ToString());
            }
            
        }

        private void nudNowWeekX_ValueChanged(object sender, EventArgs e)
        {
            if (sendMessage == 1)
            {
                ConfigHelper.WriteIniData("Location", "NowWeekX", nudNowWeekX.Value.ToString(), configFilePath);
                SendMessage("NowWeekX|" + nudNowWeekX.Value.ToString());
            }
        }

        private void nudNowTimeX_ValueChanged(object sender, EventArgs e)
        {
            if (sendMessage == 1)
            {
                ConfigHelper.WriteIniData("Location", "NowTimeX", nudNowTimeX.Value.ToString(), configFilePath);
                SendMessage("NowTimeX|" + nudNowTimeX.Value.ToString());
            } 
        }


        private void nudFightX_ValueChanged(object sender, EventArgs e)
        {
            if (sendMessage == 1)
            {
                ConfigHelper.WriteIniData("Location", "FightX", nudFightX.Value.ToString(), configFilePath);
                SendMessage("FightX|" + nudFightX.Value.ToString());
            }
        }

        private void nudFightDateX_ValueChanged(object sender, EventArgs e)
        {
            if (sendMessage == 1)
            {
                ConfigHelper.WriteIniData("Location", "FightDateX", nudFightDateX.Value.ToString(), configFilePath);
                SendMessage("FightDateX|" + nudFightDateX.Value.ToString());
            }
        }

        private void nudFightWeekX_ValueChanged(object sender, EventArgs e)
        {
            if (sendMessage == 1)
            {
                ConfigHelper.WriteIniData("Location", "FightWeekX", nudFightWeekX.Value.ToString(), configFilePath);
                SendMessage("FightWeekX|" + nudFightWeekX.Value.ToString());
            }
        }

        private void nudFightTimeX_ValueChanged(object sender, EventArgs e)
        {
            if (sendMessage == 1)
            {
                ConfigHelper.WriteIniData("Location", "FightTimeX", nudFightTimeX.Value.ToString(), configFilePath);
                SendMessage("FightTimeX|" + nudFightTimeX.Value.ToString());
            }
        }

        private void nudTitleX_ValueChanged(object sender, EventArgs e)
        {
            if (sendMessage == 1)
            {
                ConfigHelper.WriteIniData("Location", "TitleX", nudTitleX.Value.ToString(), configFilePath);
                SendMessage("TitleX|" + nudTitleX.Value.ToString());
            }
        }

        private void nudNowY_ValueChanged(object sender, EventArgs e)
        {
            if (sendMessage == 1)
            {
                ConfigHelper.WriteIniData("Location", "NowY", nudNowY.Value.ToString(), configFilePath);
                SendMessage("NowY|" + nudNowY.Value.ToString());
            }
        }

        private void nudNowDateY_ValueChanged(object sender, EventArgs e)
        {
            if (sendMessage == 1)
            {
                ConfigHelper.WriteIniData("Location", "NowDateY", nudNowDateY.Value.ToString(), configFilePath);
                SendMessage("NowDateY|" + nudNowDateY.Value.ToString());
            }
        }

        private void nudNowWeekY_ValueChanged(object sender, EventArgs e)
        {
            if (sendMessage == 1)
            {
                ConfigHelper.WriteIniData("Location", "NowWeekY", nudNowWeekY.Value.ToString(), configFilePath);
                SendMessage("NowWeekY|" + nudNowWeekY.Value.ToString());
            }
        }

        private void nudNowTimeY_ValueChanged(object sender, EventArgs e)
        {
            if (sendMessage == 1)
            {
                ConfigHelper.WriteIniData("Location", "NowTimeY", nudNowTimeY.Value.ToString(), configFilePath);
                SendMessage("NowTimeY|" + nudNowTimeY.Value.ToString());
            }
        }

        private void nudFightY_ValueChanged(object sender, EventArgs e)
        {
            if (sendMessage == 1)
            {
                ConfigHelper.WriteIniData("Location", "FightY", nudFightY.Value.ToString(), configFilePath);
                SendMessage("FightY|" + nudFightY.Value.ToString());
            }
        }

        private void nudFightDateY_ValueChanged(object sender, EventArgs e)
        {
            if (sendMessage == 1)
            {
                ConfigHelper.WriteIniData("Location", "FightDateY", nudFightDateY.Value.ToString(), configFilePath);
                SendMessage("FightDateY|" + nudFightDateY.Value.ToString());
            }
        }

        private void nudFightWeekY_ValueChanged(object sender, EventArgs e)
        {
            if (sendMessage == 1)
            {
                ConfigHelper.WriteIniData("Location", "FightWeekY", nudFightWeekY.Value.ToString(), configFilePath);
                SendMessage("FightWeekY|" + nudFightWeekY.Value.ToString());
            }
        }

        private void nudFightTimeY_ValueChanged(object sender, EventArgs e)
        {
            if (sendMessage == 1)
            {
                ConfigHelper.WriteIniData("Location", "FightTimeY", nudFightTimeY.Value.ToString(), configFilePath);
                SendMessage("FightTimeY|" + nudFightTimeY.Value.ToString());
            }
        }

        private void nudTitleY_ValueChanged(object sender, EventArgs e)
        {
            if (sendMessage == 1)
            {
                ConfigHelper.WriteIniData("Location", "TitleY", nudFightY.Value.ToString(), configFilePath);
                SendMessage("TitleY|" + nudTitleY.Value.ToString());
            }
        }

        private void nudNowSize_ValueChanged(object sender, EventArgs e)
        {
            if (sendMessage == 1)
            {
                ConfigHelper.WriteIniData("FrontSize", "NowSize", nudNowSize.Value.ToString(), configFilePath);
                SendMessage("NowSize|" + nudNowSize.Value.ToString());
            }
        }

        private void nudNowDateSize_ValueChanged(object sender, EventArgs e)
        {
            if (sendMessage == 1)
            {
                ConfigHelper.WriteIniData("FrontSize", "NowDateSize", nudNowDateSize.Value.ToString(), configFilePath);
                SendMessage("NowDateSize|" + nudNowDateSize.Value.ToString());
            }
        }

        private void nudNowWeekSize_ValueChanged(object sender, EventArgs e)
        {
            if (sendMessage == 1)
            {
                ConfigHelper.WriteIniData("FrontSize", "NowWeekSize", nudNowWeekSize.Value.ToString(), configFilePath);
                SendMessage("NowWeekSize|" + nudNowWeekSize.Value.ToString());
            }
        }

        private void nudNowTimeSize_ValueChanged(object sender, EventArgs e)
        {
            if (sendMessage == 1)
            {
                ConfigHelper.WriteIniData("FrontSize", "NowTimeSize", nudNowTimeSize.Value.ToString(), configFilePath);
                SendMessage("NowTimeSize|" + nudNowTimeSize.Value.ToString());
            }
        }

        private void nudFightSize_ValueChanged(object sender, EventArgs e)
        {
            if (sendMessage == 1)
            {
                ConfigHelper.WriteIniData("FrontSize", "FightSize", nudFightSize.Value.ToString(), configFilePath);
                SendMessage("FightSize|" + nudFightSize.Value.ToString());
            }
        }

        private void nudFightDateSize_ValueChanged(object sender, EventArgs e)
        {
            if (sendMessage == 1)
            {
                ConfigHelper.WriteIniData("FrontSize", "FightDateSize", nudFightDateSize.Value.ToString(), configFilePath);
                SendMessage("FightDateSize|" + nudFightDateSize.Value.ToString());
            }
        }

        private void nudFightWeekSize_ValueChanged(object sender, EventArgs e)
        {
            if (sendMessage == 1)
            {
                ConfigHelper.WriteIniData("FrontSize", "FightWeekSize", nudFightWeekSize.Value.ToString(), configFilePath);
                SendMessage("FightWeekSize|" + nudFightWeekSize.Value.ToString());
            }
        }

        private void nudFightTimeSize_ValueChanged(object sender, EventArgs e)
        {
            if (sendMessage == 1)
            {
                ConfigHelper.WriteIniData("FrontSize", "FightTimeSize", nudFightTimeSize.Value.ToString(), configFilePath);
                SendMessage("FightTimeSize|" + nudFightTimeSize.Value.ToString());
            }
        }

        private void nudTitleSize_ValueChanged(object sender, EventArgs e)
        {
            if (sendMessage == 1)
            {
                ConfigHelper.WriteIniData("FrontSize", "TitleSize", nudTitleSize.Value.ToString(), configFilePath);
                SendMessage("TitleSize|" + nudTitleSize.Value.ToString());
            }
        }

        private void nudScreenHeight_ValueChanged(object sender, EventArgs e)
        {
            if (sendMessage == 1)
            {
                ConfigHelper.WriteIniData("Screen", "ScreenHeight", nudScreenHeight.Value.ToString(), configFilePath);
                SendMessage("ScreenHeight|" + nudScreenHeight.Value.ToString());
            }
        }

        private void nudFightTimeYear_ValueChanged(object sender, EventArgs e)
        {
            if (sendMessage == 1)
            {
                ConfigHelper.WriteIniData("FightTimeSetting", "FightTimeYear", nudFightTimeYear.Value.ToString(), configFilePath);
                SendMessage("FightTimeYear|" + nudFightTimeYear.Value.ToString());
            }
        }

        private void nudFightTimeMonth_ValueChanged(object sender, EventArgs e)
        {
            if (sendMessage == 1)
            {
                ConfigHelper.WriteIniData("FightTimeSetting", "FightTimeMonth", nudFightTimeMonth.Value.ToString(), configFilePath);
                SendMessage("FightTimeMonth|" + nudFightTimeMonth.Value.ToString());
            }
        }

        private void nudFightTimeDay_ValueChanged(object sender, EventArgs e)
        {
            if (sendMessage == 1)
            {
                ConfigHelper.WriteIniData("FightTimeSetting", "FightTimeDay", nudFightTimeDay.Value.ToString(), configFilePath);
                SendMessage("FightTimeDay|" + nudFightTimeDay.Value.ToString());
            }
        }

        private void nudFightTimeHour_ValueChanged(object sender, EventArgs e)
        {
            if (sendMessage == 1)
            {
                ConfigHelper.WriteIniData("FightTimeSetting", "FightTimeHour", nudFightTimeHour.Value.ToString(), configFilePath);
                SendMessage("FightTimeHour|" + nudFightTimeHour.Value.ToString());
            }
        }

        private void nudFightTimeMinute_ValueChanged(object sender, EventArgs e)
        {
            if (sendMessage == 1)
            {
                ConfigHelper.WriteIniData("FightTimeSetting", "FightTimeMinute", nudFightTimeMinute.Value.ToString(), configFilePath);
                SendMessage("FightTimeMinute|" + nudFightTimeMinute.Value.ToString());
            }
        }

        private void nudFightTimeSecond_ValueChanged(object sender, EventArgs e)
        {
            if (sendMessage == 1)
            {
                ConfigHelper.WriteIniData("FightTimeSetting", "FightTimeSecond", nudFightTimeSecond.Value.ToString(), configFilePath);
                SendMessage("FightTimeSecond|" + nudFightTimeSecond.Value.ToString());
            }
        }

        private void tbxTitle_TextChanged(object sender, EventArgs e)
        {
            ConfigHelper.WriteIniData("Title", "Title", tbxTitle.Text, configFilePath);
            SendMessage("Title|" + tbxTitle.Text);
        }

        private void btnSync_Click(object sender, EventArgs e)
        {
            SendMessage("SyncFightTime|" + DateTime.Now.ToString());
        }

        private void FrmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                socketClient.Shutdown(SocketShutdown.Both);
                socketClient.Close();
            }
            catch(Exception ex)
            {

            }
            
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}

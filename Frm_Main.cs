using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Wnmp
{
    public partial class Frm_Main : Form
    {
        delegate void UiHandle(Object obj); 
        Thread PhpTh;
        Thread NginxTh;
        string dir = @Application.StartupPath; //@"D:/wnmp";
        public Frm_Main()
        {
            InitializeComponent();
            IniService();
            Log("当前路径："+dir);
        }

        /// <summary>
        /// 实例化线程
        /// </summary>
        public void IniService()
        { 
            PhpTh = new Thread(RunPhpTh);
            NginxTh = new Thread(RunNginx);
        }

        public void RunPhpTh()
        {
            try
            {
                Log("准备PHP脚本。。。");
                DirectoryInfo di = new DirectoryInfo(dir + "/php");
                foreach (DirectoryInfo dd in di.GetDirectories())
                {
                    //判断文件夹下是否有配置文件wei.ini，有则打开启动线程
                    if (File.Exists(dd.FullName + "/wei.ini"))
                    {
                        Log("启动："+dd.Name);
                        ThreadPool.QueueUserWorkItem(new WaitCallback(RunPhp), dd.Name.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Log("启动PHP时出现异常:" + ex.Message);
            }
        }
        /// <summary>
        /// 打开Php的php-cgi.exe程序方法
        /// </summary>
        public void RunPhp(object phpFolderName)
        {
            string phpVerFolderName = phpFolderName.ToString().Trim();
            string thisPhpPath = dir + "/php/" + phpVerFolderName;
            Wei.File.IniHelper ini = new Wei.File.IniHelper(thisPhpPath + "/wei.ini");
            try
            {
                int setPort = 9000;
                int.TryParse(ini.IniReadValue("Run","Port"), out setPort);
                if (setPort > 0 && IsPortAvailable(setPort))
                { 
                    Process Php = new Process();
                    Php.StartInfo.UseShellExecute = false;
                    Php.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    Php.StartInfo.FileName = thisPhpPath+ "/php-cgi.exe";
                    Php.StartInfo.Arguments = "-b 127.0.0.1:" + setPort + " -c " + thisPhpPath + "/php.ini";
                    Php.StartInfo.CreateNoWindow = true;
                    Php.Start();
                    Php.WaitForExit();
                }
                else
                {
                    Log(phpVerFolderName+" 启动失败，端口号 "+setPort+" 未配置或已被占用");
                }
            }
            catch (Exception ex)
            {
                Log(ex.Message);
            }
        }
        /// <summary>
        /// 关闭PHP进程方法
        /// </summary>
        public void ClosePhp()
        {
            try
            {
                List<Process> pro = ServiceProcess();
                foreach (Process p in pro)
                {
                    if (p.ProcessName.ToLower() == "php-cgi")
                    {
                        if (!p.CloseMainWindow()) { p.Kill(); }
                    }
                }
            }
            catch (Exception ex) { Log(ex.Message); }
        }
        /// <summary>
        /// 开启nginx方法
        /// </summary>
        public void RunNginx()
        {
            Log("启动Nginx");
            RunCmd("D:&cd "+dir+"&nginx.exe", "启动Nginx");//必须先启动，否则后面无限等待 
        }
        /// <summary>
        /// 关闭nginx方法
        /// </summary>
        public void CloseNginx()
        {
            try
            {
                List<Process> pro = ServiceProcess();
                foreach (Process p in pro)
                {
                    if (p.ProcessName.ToLower() == "nginx")
                    {
                        RunCmd("D:&cd "+dir+"&nginx -s stop");
                    }
                }
            }
            catch (Exception ex) { Log(ex.Message); }
        }

        /// <summary>
        /// 打开服务的方法
        /// </summary>
        public void RunService()
        {
            try
            {
                //启动线程执行方法
                NginxTh.Start();
                PhpTh.Start(); 
            }
            catch (Exception ex)
            {
                Log(ex.Message);
            }
        } 
        /// <summary>
        /// 关闭服务
        /// </summary>
        private void CloseService()
        {
            try
            { 
                PhpTh.Abort();
                NginxTh.Abort();
                ClosePhp();
                CloseNginx();
            }
            catch (Exception ex) { }
        }
        public void RunCmd(string cmds)
        {
            RunCmd(cmds, "");
        }
        public void RunCmd(string cmds, string retMsg)
        {
            string outPut = "";
            string errMsg = "";
            try
            {
                Process pc = new Process();
                pc.StartInfo.FileName = "cmd.exe";
                pc.StartInfo.CreateNoWindow = true;//隐藏窗口运行
                pc.StartInfo.RedirectStandardInput = true;//重定向输入流
                pc.StartInfo.RedirectStandardOutput = true;//重定向输出流
                pc.StartInfo.RedirectStandardError = true;//重定向错误流
                pc.StartInfo.UseShellExecute = false;
                pc.Start();
                pc.StandardInput.WriteLine(cmds + "&exit");//输入CMD命令  
                //pc.StandardInput.WriteLine("exit");//结束执行，很重要的
                pc.StandardInput.AutoFlush = true;
                outPut = pc.StandardOutput.ReadToEnd();//读取结果   
                Log(outPut); //正式使用时不需要把这个信息显示出来
                errMsg = pc.StandardError.ReadToEnd();//读取结果    
                if (!string.IsNullOrEmpty(errMsg.Trim()))
                {
                    Log("错误信息：" + errMsg);
                }
                else
                {
                    if (!string.IsNullOrEmpty(retMsg))
                    {
                        Log("成功" + retMsg);
                    }
                }
                pc.WaitForExit();
                //pc.CloseMainWindow();
                //pc.Dispose();
            }
            catch (Exception ex)
            {
                Log(ex.Message);
            }
        }
        public void Log(object obj)
        {
            try
            {
                if (this.UiLogBox.InvokeRequired)
                {
                    UiHandle ui = new UiHandle(Log);
                    this.UiLogBox.BeginInvoke(ui, new object[] { obj });
                }
                else
                {
                    this.UiLogBox.AppendText(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + obj.ToString() + "\r\n");
                }
            }
            catch (Exception e)
            {
                Log(e.Message);
            }
        }
        /// <summary>
        /// 获取服务进程并返回进程Process集合
        /// </summary>
        /// <returns></returns>
        public List<Process> ServiceProcess()
        {
            List<Process> pro = new List<Process> { };
            Process[] ps = Process.GetProcesses();
            foreach (Process p in ps)
            {
                try
                {
                    if (p.ProcessName.ToLower() == "php-cgi" || p.ProcessName.ToLower() == "nginx")
                    {
                        pro.Add(p);
                    }
                }
                catch (Exception ex) { }
            }
            return pro;
        }

        public bool IsPortAvailable(int port)
        {
            bool available = true;
            try
            {
                IPGlobalProperties ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
                TcpConnectionInformation[] tcpConnInfoArray = ipGlobalProperties.GetActiveTcpConnections();
                foreach (TcpConnectionInformation tcpi in tcpConnInfoArray)
                {
                    if (tcpi.LocalEndPoint.Port == port)
                    {
                        available = false;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                available = false;
                Log("端口检查异常，本次定义端口为不可用。"+ex.Message);
            }
            return available;
        }

        #region UI操作事件 
        private void UiStart_Click(object sender, EventArgs e)
        {
            RunService();
        }

        private void UiReload_Click(object sender, EventArgs e)
        {
            Log("正在关闭服务......");
            CloseService();
            Log("正在启动服务......");
            IniService();
            RunService();
        }
        private void UiQuit_Click(object sender, EventArgs e)
        {
            CloseService();
            RunCmd("D:&cd "+dir+"&nginx -s quit", "退出服务");
        }
         
        private void BtnProcesses_Click(object sender, EventArgs e)
        {
            Process[] ps = Process.GetProcesses();
            foreach (Process p in ps)
            {
                try
                {
                    if (p.ProcessName.ToLower() == "php-cgi" || p.ProcessName.ToLower() == "nginx")
                    {
                        //if (!p.CloseMainWindow()) { p.Kill(); }
                        Log(p.Id + "  " + p.ProcessName + "  " + p.MainModule.ModuleName + "  " + p.MainModule.FileName+" " );
                    }
                }
                catch (Exception ex)
                {
                    Log(ex.Message);
                }
            }
        }

        private void Frm_Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            List<Process> pro = ServiceProcess();
            foreach (Process p in pro)
            {
                //if (p.ProcessName.ToLower() == "php-cgi")
                //{
                //    if (!p.CloseMainWindow()) { p.Kill(); }
                //}
                //if (p.ProcessName.ToLower() == "nginx")
                //{
                //    RunCmd("D:&cd "+dir+"&nginx -s stop");
                //}
                //退出程序后直接结束进程
                if (!p.CloseMainWindow()) { p.Kill(); }
            }
            CloseService();
            //关闭托盘图标
            this.notifyIcon1.Visible = false;
            this.notifyIcon1.Dispose();
        }

        private void Frm_Main_SizeChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.WindowState == FormWindowState.Minimized)
                {
                    this.Hide();
                    notifyIcon1.Visible = true;
                }
            }catch(InvalidEnumArgumentException ex) { Log("隐藏到托盘时异常！"+ex.Message); }
        }
         

        private void notifyIcon1_Click(object sender, EventArgs e)
        {
            try
            {
                this.Visible = true;
                this.WindowState = FormWindowState.Normal;
            }
            catch (InvalidEnumArgumentException ex)
            {
                Log("从托盘显示时异常！" + ex.Message);
            } 
        }
    }
    #endregion
}

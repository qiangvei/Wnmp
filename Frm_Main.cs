using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
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
        private Process Php;
        Thread PhpTh;
        Thread NginxTh;
        public Frm_Main()
        {
            InitializeComponent();
            PhpTh = new Thread(RunPhp);
            NginxTh = new Thread(RunNginx);
        }

        public void RunPhp()
        {
            try
            {  
                Php = new Process();
                Php.StartInfo.UseShellExecute = false;
                Php.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                Php.StartInfo.FileName = @"D:\wnmp\apps\php\php5.2.17\php-cgi.exe";
                Php.StartInfo.Arguments = "-b 127.0.0.1:9000 -c php.ini";
                Php.StartInfo.CreateNoWindow = true;
                Php.Start();
                Php.WaitForExit();

            }
            catch(Exception ex)
            {
                Log(ex.Message);
            }
        }
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
            }catch(Exception ex) { Log(ex.Message); }
        }
        public void RunNginx()
        {
            RunCmd("D:&cd D:/wnmp&nginx.exe", "启动Nginx");//必须先启动，否则后面无限等待 
        }
        public void CloseRunNginx()
        {
            try
            {
                List<Process> pro = ServiceProcess();
                foreach (Process p in pro)
                { 
                    if (p.ProcessName.ToLower() == "nginx")
                    {
                        RunCmd("D:&cd D:/wnmp&nginx -s stop");
                    }
                }
            }
            catch (Exception ex) { Log(ex.Message); }
        }

        private void UiStart_Click(object sender, EventArgs e)
        {
            try
            {
                NginxTh.Start();
                PhpTh.Start();
            }catch(Exception ex)
            {
                Log(ex.Message);
            }
        } 
         

        private void UiReload_Click(object sender, EventArgs e)
        {
            RunCmd("D:&cd D:/wnmp&nginx -s reload","重启服务");
        } 
        private void UiQuit_Click(object sender, EventArgs e)
        {
            CloseService();
            RunCmd("D:&cd D:/wnmp&nginx -s quit", "退出服务");
        } 

        private void UiStop_Click(object sender, EventArgs e)
        {
            CloseService();
            RunCmd("D:&cd D:/wnmp&nginx -s stop", "强制退出");
        }

        private void CloseService()
        {
            try
            {
                ClosePhp();
                if (Php != null)
                {
                    Php.CloseMainWindow();
                    Php.Dispose();
                }
                PhpTh.Abort();
                NginxTh.Abort();
            }catch(Exception ex) { }
        }
        public void RunCmd(string cmds)
        {
            RunCmd(cmds, "");
        }
        public void RunCmd(string cmds , string retMsg)
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
                pc.StandardInput.WriteLine(cmds+"&exit");//输入CMD命令  
                //pc.StandardInput.WriteLine("exit");//结束执行，很重要的
                pc.StandardInput.AutoFlush = true;
                outPut = pc.StandardOutput.ReadToEnd();//读取结果   
                Log(outPut); //正式使用时不需要把这个信息显示出来
                errMsg = pc.StandardError.ReadToEnd();//读取结果    
                if(!string.IsNullOrEmpty(errMsg.Trim()))
                {
                    Log("错误信息："+errMsg);
                } 
                else
                {
                    if (!string.IsNullOrEmpty(retMsg))
                    {
                        Log("成功"+ retMsg);
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
                    this.UiLogBox.AppendText(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + obj.ToString()+"\r\n"); 
                }
            }
            catch (Exception e)
            {
                Log(e.Message);
            } 
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
                        Log(p.Id + "  " + p.ProcessName + "  " + p.MainModule.ModuleName + "  " + p.MainModule.FileName);
                    }
                }
                catch (Exception ex)
                {
                    Log(ex.Message);
                } 
            }
        }
        public List<Process> ServiceProcess()
        {
            List<Process> pro = new List<Process> { }; 
            Process[] ps = Process.GetProcesses();
            foreach (Process p in ps)
            {
                try
                {
                    if (p.ProcessName.ToLower() == "php-cgi" || p.ProcessName.ToLower()=="nginx")
                    {
                        pro.Add(p); 
                    }
                }
                catch (Exception ex){ }
            }
            return pro;
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
                //    RunCmd("D:&cd D:/wnmp&nginx -s stop");
                //}
                //退出程序后直接结束进程
                if (!p.CloseMainWindow()) { p.Kill(); }
            }
            CloseService();
        }
    }
}

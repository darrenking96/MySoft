using System;
using System.Diagnostics;
using System.IO;
using System.Text;

/*********************
 * 
 *  自动解压MySql文件，并完成安装与设置
  * 
 **********************/
namespace instdb
{
    internal class InstallMySql
    {
        private readonly string mysqlStr = "MySql";
        private readonly string serviceName = "CobanDb";


        /// <summary>
        /// 安装目录
        /// </summary>
        public string TargetDir { get; set; }

        public int Port { get; set; }

        public string Password { get; set; }

        public string MySqlDir 
        {
            get
            {
                return Path.Combine(Directory.GetParent(TargetDir).ToString(), mysqlStr);
            }
        }

        public InstallMySql()
        {
            Port = 3306;
        }

        /// <summary>
        /// 一键完成完成安装
        /// </summary>
        public void Onekey()
        {
            UpdateCfg();
            InstService(MySqlDir, Password);
        }


        /// <summary>
        /// 初始化INI文件
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="port"></param>
        private void UpdateCfg()
        {
            string str = File.ReadAllText(Path.Combine(MySqlDir, @"my_template.ini"));


            string bd = MySqlDir.Replace("\\","/");

            File.WriteAllText(Path.Combine(MySqlDir, @"my.ini"),
                str.Replace("$basedir$", bd).Replace("$port$", Port.ToString()), Encoding.ASCII);
        }

        public void DeleteService()
        {
            Process cmd = new Process();
            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.UseShellExecute = false;
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.RedirectStandardError = true;
            cmd.StartInfo.CreateNoWindow = false;
            cmd.Start();

            string str1 = string.Format(@"cd/d {0}", Path.Combine(MySqlDir, "bin"));

            cmd.StandardInput.WriteLine(str1);

            cmd.StandardInput.WriteLine(string.Format("net stop {0}",serviceName));
            Console.WriteLine("关闭服务中...");

            cmd.StandardInput.WriteLine(string.Format("mysqld -remove {0}",serviceName));
            Console.WriteLine("移除服务中...");

            cmd.StandardInput.WriteLine("exit");
            cmd.WaitForExit();
            cmd.Close();
            Console.WriteLine("删除完毕...");
        }

        private void InstService(string dir, string pwd)
        {
            if(string.IsNullOrEmpty(dir))
                return;
            if(!Directory.Exists(dir))
                return;
         
            Process cmd = new Process();
            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.UseShellExecute = false;
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.RedirectStandardError = true;
            cmd.StartInfo.CreateNoWindow = false;
            cmd.Start();

            string str1 = string.Format("cd/d {0}", Path.Combine(dir, "bin"));

            cmd.StandardInput.WriteLine(str1);

            cmd.StandardInput.WriteLine(string.Format("net stop {0}", serviceName));
            Console.WriteLine("关闭服务中...");

            cmd.StandardInput.WriteLine(string.Format("mysqld -remove {0}", serviceName));
            Console.WriteLine("移除服务中...");


            string oriPath = Path.Combine(dir, "my.ini");
            string filePath = oriPath.Replace("\\", "/");

            string buildStr = string.Format("mysqld --install {0} --defaults-file=\"{1}\"",serviceName, filePath);

            cmd.StandardInput.WriteLine(buildStr);
            Console.WriteLine("注册服务中...");

            cmd.StandardInput.WriteLine(string.Format("net start {0}",serviceName));
            Console.WriteLine("打开服务中...");

            //string updateSql = "use mysql;\r\n update user set password=password(" + "\"" + pwd + "\"" +
            //") where user=\"root\";\r\n flush privileges; \r\n exit \r\n";
            //File.WriteAllText(Path.Combine(dir, "temp.sql"), updateSql,Encoding.ASCII);
            //string callSql = string.Format("mysql -uroot -proot -duser<{0}", Path.Combine(dir, "temp.sql"));

            //cmd.StandardInput.WriteLine(str1);
            //string chanPwdCommand = string.Format("mysqladmin -uroot -proot password {0}", pwd);
            //cmd.StandardInput.WriteLine(chanPwdCommand);
            //Console.WriteLine("登陆数据库，更改数据库密码...");
            //cmd.StandardInput.WriteLine("exit");
            //Console.WriteLine("登出数据库...");

            cmd.WaitForExit();
            cmd.Close();
            //File.Delete(Path.Combine(dir, "temp.sql"));
            //Console.WriteLine("删除脚本...");
        }

    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace instdb
{
    /*
     * 安装与配置MySQL
     * 
     *************/
    class Program
    {
        static void Main(string[] args)
        {
            //string pwd = PasswordCreator.CreateNewPassword(4);
            string pwd = "aa456";


            string baseDir = Environment.CurrentDirectory;
            string storagePath = Path.Combine(baseDir, "MyPassword.txt");

            var cmd = new InstallMySql();
            cmd.Password = pwd;
            cmd.TargetDir = baseDir;

            //cmd.DeleteService();


            cmd.Onekey();



            PasswordStoreage.SavePasswordToFile(storagePath, pwd);
        }
    }
}

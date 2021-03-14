using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace xulingling
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            string s = null;//用于存放传入的文件名。
            if (args.Length > 0)//有实参，即有文件名传入。
            {
                //如果文件名或路径中存在空格，经过参数后会被分割成多个字符串，因此必须把分割的字符串数组重新组合成一个字符串。
                //例如:文件名为: c:lprogram filesltest.rtf，则实际传递后变为://args[0]=@"x:lprogram" args[1]=@"files\test.rtf"
                s = string.Join(" ", args);//
                string ex = s.Substring(s.LastIndexOf('.') + 1).ToLower();//双引号里是一 个空格,不多不少
                if (ex.Equals("rtf") || ex.Equals("txt"))
                    Application.Run(new xll_Form1(s));//这里暂时报错是因为xll. form1还没有带参构造函数
                else
                {
                    MessageBox.Show("无法打开指定文件" + s);
                    Application.Exit();//结束应用程序
                }
            }
            else//无参数，即没有文件名传入
            Application.Run(new xll_Form1());
        }
    }
}

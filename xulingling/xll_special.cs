using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace xulingling
{
   
    public partial class xll_special : Form
    {
        RichTextBox rtb = null;
        string ch1 = "♫", ch2 = "+", ch3 = "①", ch4 = "mg";//每种特殊符号的默认选中项字符

        public xll_special(RichTextBox rh)//修改为带参数的构造函数
        {
            InitializeComponent(); 
            rtb = rh;
        }
       

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {//选中内容发生改变
            RadioButton rb = sender as RadioButton;
            if(rb.Checked)
            {
                特殊展示label.Text = rb.Text;
                switch(tabControl1.SelectedIndex)
                {
                    case 0: ch1 = rb.Text; break;
                    case 1: ch2 = rb.Text; break;
                    case 2: ch3 = rb.Text; break;
                    case 3: ch4 = rb.Text; break;
                }
            }

        }

       private void tabControl1_SelectedIndexChanged(object sender,EventArgs e)
        {//选项卡发生变化
           switch(tabControl1.SelectedIndex)
           {
               case 0: 特殊展示label.Text = ch1; break;
               case 1: 特殊展示label.Text = ch2; break;
               case 2: 特殊展示label.Text = ch3; break;
               case 3: 特殊展示label.Text = ch4; break;
           }

        }

       private void 插入button_Click(object sender, EventArgs e)
       {
           rtb.SelectedText = 特殊展示label.Text;
       }

       private void xll_special_FormClosed(object sender, FormClosedEventArgs e)
       {
           xll_global.sp = null;//窗体关闭时，设置sp为null，下次需要新建特殊字符窗体
       }

       

    

      
    }
}

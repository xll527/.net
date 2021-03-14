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
    public partial class xll_bulletForm1 : Form
    {
        RichTextBox rtb = null;
        bool rh_wordwrap;//保存版本rh是否启用了自动换行
        bool have_bullet = false;//当前设置段落是否已经有项目符号

        public xll_bulletForm1(RichTextBox rh)//改写为带参的构造函数
        {
            InitializeComponent();
            rtb = rh;//接收传递过来的控件，方便设置项目符号
        }
        //注意顺序段radiobutton的命名顺序一致
        string[] bulletlist = new string[] { "●", "△", "○", ",▽", "◁", "♢", "▷", "☆", "✔", "✘", "°", "%", "÷", "✚", "☺", "□", "∞", "☀" };

        private void xll_bulletForm1_Load(object sender, EventArgs e)
        {
            rh_wordwrap = rtb.WordWrap;//保存原来的自动换行设置
            if (rtb.Lines.Length == 0) return;

            rtb.WordWrap = false;
            if(rtb.Lines[rtb.GetLineFromCharIndex(rtb.SelectionStart)].Length>0)//本段落有字符
            {
                string s = rtb.Text[rtb.GetFirstCharIndexOfCurrentLine()].ToString();//提取段落首字符
                int index = Array.IndexOf(bulletlist, s);//是否是规定的项目符号
                if(index>-1)//首字符是项目符号
                {//将选中的项目符号设置为当前段落的项目符号
                    展示.Text = s;
                    ((RadioButton)Controls["radioButton" + (index + 1).ToString()]).Checked = true;
                    rtb.SelectionStart = rtb.GetFirstCharIndexOfCurrentLine();
                    rtb.SelectionLength = 1;
                    nud_bulletsize.Value = Convert.ToDecimal(rtb.SelectionFont.Size);
                    颜色button1.BackColor = rtb.SelectionColor;
                    展示.ForeColor = rtb.SelectionColor;
                    have_bullet = true;//已有项目符号

                }
            }
            rtb.WordWrap = rh_wordwrap;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {//选定的项目符号发生改变
            RadioButton ra = sender as RadioButton;
            if (ra.Checked) 展示.Text = ra.Text;
        }

        private void 颜色button1_Click(object sender, EventArgs e)
        {//设置项目符号颜色
            ColorDialog cd = new ColorDialog();
            cd.FullOpen = true;
            cd.Color = (sender as Button).BackColor;
            if(cd.ShowDialog()==DialogResult.OK)
            {
                (sender as Button).BackColor = cd.Color;
                展示.ForeColor = cd.Color;
            }
        }

        private void nud_bulletsize_ValueChanged(object sender, EventArgs e)
        {//根据设置实时更新项目符号大小
           展示.Font=new Font(展示.Font.Name,float.Parse(nud_bulletsize.Value.ToString()));
        }

        private void 确定button_Click(object sender, EventArgs e)
        {
            rtb.WordWrap = false;
            int index = rtb.GetFirstCharIndexOfCurrentLine();//获取段首字符位置
            rtb.SelectionStart = index;
            if (have_bullet) rtb.SelectionLength = 1;
            else rtb.SelectionLength = 0;
            rtb.SelectedText = 展示.Text;
            rtb.SelectionStart = index;
            rtb.SelectionLength = 1;

            rtb.SelectionFont = new Font(展示.Font.Name, float.Parse(nud_bulletsize.Value.ToString()));
            rtb.SelectionColor =颜色button1.BackColor;

            rtb.WordWrap = rh_wordwrap;
            this.Close();
        }

        private void 取消button_Click(object sender, EventArgs e)
        {
            this.Close();
        }


       
    }
}

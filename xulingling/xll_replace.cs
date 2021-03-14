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
    public partial class xll_replace : Form
    {
        RichTextBox rtb = null;
        int beginindex = 0;//特定内容的首次查找替换开始位置，默认是替换窗体打开时光标的当前位置或查找内容发生变化时，光标所在的位置
        bool frombegin = true;//本次特定内容的查找是否是冲文档开始处的，是为true
        public xll_replace(RichTextBox rh)
        {//修改为带参数的构造函数
            InitializeComponent();
            rtb = rh;
        }

        //替换窗体-事件-formClosed—双击属性值空白处
        //关闭替换窗体时，将全局静态变量rp设置为null，表明目前已经没有打开的替换窗体对象了
        private void xll_replace_FormClosed(object sender, FormClosedEventArgs e)
        {
            xll_global.rp = null;
        }

        private void xll_replace_Load(object sender, EventArgs e)
        {//查找替换窗体load方法：
            if(rtb.SelectedText!="")//如果编辑区有选中的文本，则将其设置为默认的查找内容
            {
                tb_findtext.Text = rtb.SelectedText;
            }
            beginindex = rtb.SelectionStart;//记住首次查找替换的位置，以便文末后重新开头查找替换，到此处汇合

        }

        private void 查找button_Click(object sender, EventArgs e)
        {
            if(tb_findtext.Text=="")
            {
                MessageBox.Show("请输入待查找的内容。");
                return;
            }
            if(rtb.Text.Length==0)
            {
                MessageBox.Show("你目前的文档没有文字内容，查找替换结束。");
                return;
            }
            int startIndex, endIndex, index;
            endIndex = rtb.Text.Length;
            if (rtb.SelectedText == "")
            {
                startIndex = rtb.SelectionStart;
            }
            else startIndex = rtb.SelectionStart + rtb.SelectionLength;
            if(startIndex>=endIndex||(beginindex!=0&&startIndex>=beginindex&&frombegin==true))
            {
                //startIndex>=endIndex 指开始位置超过结束位置，或两者重合，说明查找到文末了，不需要在查找
                //(startIndex>beginindex&&frombegin==true)  在从头开始查找时，开始位置与上- 查找开始位置已经成功汇合了，也不用再查找了
                index = -1;
            }
            else
            {
                index = rtb.Find(tb_findtext.Text, startIndex, endIndex, RichTextBoxFinds.None);
            }
            if (index >= 0) rtb.Focus();//找到
            else//没查找到或没找(已经到文件末尾或从头开始时与上-次查找开始位置汇合， 则没有查找)
            {
                rtb.SelectionStart += rtb.SelectionLength;//将光标移至最后选中项的右侧,不处理
                rtb.SelectionLength = 0;//取消上一次的选中。不处理关系不大
                if(startIndex>=beginindex&&frombegin==false)//如果刚才不是从文件开始处查找的，则询问是否回到文档开头进行完整查找
                {
                    if(MessageBox.Show("已到达文档末尾，没有发现匹配的内容，是否尝试从文档开始处查找?","询问",MessageBoxButtons.YesNo)==DialogResult.Yes)
                    {
                        frombegin = true;
                        rtb.SelectionStart = 0;//光标移动至文档开头
                        查找button.PerformClick();//用程序激发一次查找按钮的单击事件
                    }
                }
                else//
                {
                    if(MessageBox.Show("没有找到内容。已经完成对文档的一次遍历搜索。是否从头开始新一轮查找?","查找完毕！",MessageBoxButtons.YesNo)==DialogResult.Yes)
                    {
                        frombegin = true;
                        beginindex = 0;
                        rtb.SelectionStart = 0;
                    }
                }
            }
        }

        private void 替换button_Click(object sender, EventArgs e)
        {
            if(tb_findtext.Text=="")
            {
                MessageBox.Show("请输入替换的内容。");
                return;
            }
            if(rtb.Text.Length==0)
            {
                MessageBox.Show("你目前的文档没有文字内容，替换结束.");
                return;
            }
            int startIndex, endIndex, index;
            endIndex = rtb.Text.Length;
            //如果选中内容和替换结果相同,则开始查找替换位置从选中内容右侧开始
            if (rtb.SelectedText != "" && rtb.SelectedText.Equals(tb_replace.Text))
                startIndex = rtb.SelectionStart + tb_replace.Text.Length;
            else//否则从选中内容第一个字符开始查找替换
                startIndex = rtb.SelectionStart;
            //如果没有选中内容或选中内容与待替换内容不符合，则先查找待替换内容
            if(rtb.SelectedText==""||rtb.SelectedText!=tb_findtext.Text)
            {
                rtb.SelectionLength = 0;
                if(startIndex<endIndex&&!(beginindex!=0&&startIndex>=beginindex&&frombegin==true))
                {
                    //startindex >= endIndex指开始位置超过结束位置，或两者重合.说明查找到文末了.不需要在查找
                    //(startIndex> =beginindex& &frombegin==true) ,在从头开始查找时，开始位置与上-查找开始位置已经成功汇合了， 也不用再查找了
                    index = rtb.Find(tb_findtext.Text, startIndex, endIndex, RichTextBoxFinds.None);
                }
            }
            if(rtb.SelectedText!="")//已经找到替换内容
            {
                rtb.SelectedText = tb_replace.Text;
                rtb.SelectionStart = rtb.SelectionStart - tb_replace.Text.Length;
                rtb.SelectionLength = tb_replace.Text.Length;
                rtb.Focus();
            }
            else//没有找到替换内容
            {
                if(startIndex>=beginindex&&frombegin==false)//如果刚才不是从文件开始处查找替换的，则询问是否回到文档开头进行完整查找替换
                {
                    if(MessageBox.Show("已到达文档末尾，没有发现匹配的内容，是否尝试从文档开始处查找替换?","询问",MessageBoxButtons.YesNo)==DialogResult.Yes)
                    {
                        frombegin = true;
                        rtb.SelectionStart = 0;//光标移动至文档开头
                        查找button.PerformClick();//用程序激发一次查找按钮的单击事件(这里考虑从文未到开始跳跃过大，先查找，由用户再次确认替换操作
                    }
                }
                else//已经完成从文档开头到结束的完整查找替换
                {
                    if(MessageBox.Show("没有找到可替换内容。已经完成-次对文档的遍历查找替换，是否重新开始一次查找替换?","询问",MessageBoxButtons.YesNo)==DialogResult.Yes)
                    {
                        frombegin = true;
                        beginindex = 0;
                        rtb.SelectionStart = 0;
                    }
                }
            }
        }

        private void 全部替换button_Click(object sender, EventArgs e)
        {
            if(tb_findtext.Text=="")
            {
                MessageBox.Show("请输入替换的原内容");
                return;
            }
            if(rtb.Text.Length==0)
            {
                MessageBox.Show("你目前的文档没有文字内容，替换结束");
                return;
            }
            int n = 0;//统计替换次数
            int startindex = 0;
            while((startindex=rtb.Find(tb_findtext.Text,startindex,rtb.Text.Length,RichTextBoxFinds.None))>=0)
            {
                n++;
                rtb.SelectedText = rtb.SelectedText.Replace(tb_findtext.Text, tb_replace.Text);
                startindex += tb_replace.Text.Length;
                if (startindex >= rtb.Text.Length) break;
            }
            rtb.ScrollToCaret();//将滚动条的位置设置到当前关心的位置
            MessageBox.Show("替换完成，总共替换了" + n.ToString() + "处。");
        }
    }
}

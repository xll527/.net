using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace xulingling
{
    public partial class xll_Form1 : Form
    {
        public xll_Form1(string s)//带参数构造函数  新增
        {
            InitializeComponent();
            filename = s;//获取文件名
        }

        public xll_Form1()
        {
            InitializeComponent();
        }



        private void xll_Form1_Load(object sender, EventArgs e)
        {
            //收起左侧
            splitContainer1.Panel1Collapsed = true;
            //读取系统字体
            tcb_font.Items.Clear();
            InstalledFontCollection f = new InstalledFontCollection();
            foreach (FontFamily ff in f.Families)
            {
                tcb_font.Items.Add(ff.Name);
            }
            //设置默认值
            tcb_font.Text = "宋体";
            tcb_fontsize.SelectedIndex = 9;//五号字体
            //安装事件
            tcb_font.SelectedIndexChanged += tcb_font_SelectedIndexChanged;
            tcb_fontsize.SelectedIndexChanged += tcb_fontsize_SelectedIndexChanged;
            tcb_fontsize.KeyUp += tcb_fontsize_KeyUp;

            if (filename == null) return;
            string ex = filename.Substring(filename.LastIndexOf('.') + 1);
            try
            {
                if (ex.ToLower().Equals("txt"))
                    rh.LoadFile(filename, RichTextBoxStreamType.PlainText);
                else
                    rh.LoadFile(filename);
                rh.Modified = false;
                string t = filename.Substring(filename.LastIndexOf(@"\") + 1);//
                this.Text = "xll的富记事本" + t;
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
                filename = null;
            }

        }

        void tcb_fontsize_KeyUp(object sender, KeyEventArgs e)
        {
            //工具栏字体大小输入一键松开事件,回车键输入后，正式改变大小,因为非列表字体大小, selectedindex均为- 1
            if (tcb_fontsize.SelectedIndex != -1) return;
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    float fontsize = float.Parse(tcb_fontsize.Text.Trim());
                    int len = rh.SelectionLength;
                    int selectstart = rh.SelectionStart;
                    bool toolflag = false;
                    for (int i = 0; i < len; i++)
                    {
                        rh.Select(selectstart + i, 1);
                        rh.SelectionFont = new Font(rh.SelectionFont.FontFamily, fontsize, rh.SelectionFont.Style);
                    }
                    rh.SelectionStart = selectstart;
                    rh.SelectionLength = len;
                    toolflag = true;
                    rh.Focus();
                }
                catch
                {
                    MessageBox.Show("属性值不正确");
                    tcb_fontsize.Focus();
                }
            }
        }

        void tcb_fontsize_SelectedIndexChanged(object sender, EventArgs e)
        {
            //工具栏大小改变
            if (size_flag == false) return;//程序调整字体大小选中项，以保持和菜单字体设置一 致，不需要响应事件
            float fontsize = 0;
            if (tcb_fontsize.SelectedIndex == -1) return;
            else//不为-1，选中项是列表内容
            {
                int k = Array.IndexOf(d, tcb_fontsize.Text.Trim());
                fontsize = v[k];
            }//选中项不为-1
            int len = rh.SelectionLength;
            int selectstart = rh.SelectionStart;
            bool toolflag = false;
            for (int i = 0; i < len; i++)
            {
                rh.Select(selectstart + i, 1);
                rh.SelectionFont = new Font(rh.SelectionFont.FontFamily, fontsize, rh.SelectionFont.Style);
            }
            rh.SelectionStart = selectstart;
            rh.SelectionLength = len;
            toolflag = true;
            rh.Focus();
        }

        void tcb_font_SelectedIndexChanged(object sender, EventArgs e)
        {
            //工具栏字体种类改变
            //程序调整字体种类选中项,以保持和菜单字体设置-致, 不需要响应事件,字体选中为-1,不需要处理
            if (size_flag == false || tcb_font.SelectedIndex == -1) return;
            //字体种类不同，就会使SelectionFont为null,因此只能单个字符循环处理
            int len = rh.SelectionLength;
            int selectstart = rh.SelectionStart;
            bool toolflag = false;//在字体设置期间的光标位置改变引起的字体变化，具栏字体设置控件内容保持不变
            for (int i = 0; i < len; i++)//
            {
                rh.Select(selectstart + i, 1);
                rh.SelectionFont = new Font(tcb_font.Text, rh.SelectionFont.Size, rh.SelectionFont.Style);
            }
            rh.SelectionStart = selectstart;
            rh.SelectionLength = len;
            toolflag = true;
            rh.Focus();
        }

        private void 复制ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (rh.SelectedRtf != "") rh.Copy();
        }

        private void 粘贴ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Clipboard.GetDataObject().GetDataPresent(DataFormats.Rtf) == true) rh.Paste();
        }

        private void 剪切ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (rh.SelectedRtf != "") rh.Cut();
        }

        private void 自动换行ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem ts = sender as ToolStripMenuItem;
            ts.Checked = !ts.Checked;
            rh.WordWrap = !rh.WordWrap;
        }

        private void 背景设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            cd.FullOpen = true;
            cd.Color = rh.BackColor;
            if (cd.ShowDialog() == DialogResult.OK)
            {
                rh.BackColor = cd.Color;
            }
        }

        private void 全选ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rh.SelectAll();
        }

        private void 撤销ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rh.Undo();
        }

        private void 恢复ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rh.Redo();
        }

        private void 左对齐ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rh.SelectionAlignment = HorizontalAlignment.Left;
        }

        private void 右对齐ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rh.SelectionAlignment = HorizontalAlignment.Right;
        }

        private void 居中ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rh.SelectionAlignment = HorizontalAlignment.Center;
        }

        private void 日期时间ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rh.SelectedText = DateTime.Now.ToString();
        }

        private void 图片ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog of = new OpenFileDialog();
            of.Filter = "BMP,JPEG,GIF,icon,png|*.bmp;*.jpg;*.gif;*.icon;*.png";
            if (of.ShowDialog() == DialogResult.OK)
            {
                Image img = Image.FromFile(of.FileName);
                Clipboard.SetDataObject(img);
                rh.Paste(DataFormats.GetFormat(DataFormats.Bitmap));
            }
        }


        float[] v = new float[] { 42, 36, 26.25f, 24, 21.75f, 18, 16, 15.75f, 14.25f, 12, 10.5f, 9, 7.5f, 6.75f, 5.25f };
        string[] d = new string[] { "初号", "小初", "一号", "小一", "二号", "小二", "三号", "小三", "四号", "小四", "五号", "小五", "六号", "小六", "七号" };
        //事件有效标记
        bool font_flag = true;//快捷工具栏-字体种类选中内容发生变化事件响应
        bool size_flag = true;//快捷工具栏-字体大小选中内容发生变化事件响应
        private void 字体ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //新建字体对话框并初始化
            FontDialog fd = new FontDialog();
            fd.Font = rh.SelectionFont;
            fd.Color = rh.SelectionColor;
            fd.ShowColor = true;

            //打开字体对话框并进行设置
            if (fd.ShowDialog() == DialogResult.OK)
            {
                rh.SelectionColor = fd.Color;
                rh.SelectionFont = fd.Font;
            }
            //处理快捷工具栏的字体信息同步设置
            font_flag = false;
            size_flag = false;
            ts_color.BackColor = fd.Color;
            tcb_font.Text = fd.Font.FontFamily.Name;
            //tcb_fontsize.Text = fd.Font.Size.ToString();
            int k = Array.IndexOf(v, fd.Font.Size);
            if (k > -1)//找到了
            {
                tcb_fontsize.Text = d[k];
            }
            else//没找到
            {
                tcb_fontsize.Text = fd.Font.Size.ToString();
            }

            ts_fontbold.Checked = fd.Font.Bold;
            ts_fontitalics.Checked = fd.Font.Italic;
            ts_fontunderline.Checked = fd.Font.Underline;
            font_flag = size_flag = true;

        }

        private void 字体颜色toolStripButton1_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            cd.FullOpen = true;
            cd.Color = rh.SelectionColor;
            if (cd.ShowDialog() == DialogResult.OK)
            {
                rh.SelectionColor = cd.Color;
                ts_color.BackColor = cd.Color;
            }
        }

        private void 项目符号ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            xll_bulletForm1 f = new xll_bulletForm1(rh);
            f.ShowDialog();
        }

        private void rh_TextChanged(object sender, EventArgs e)
        {//当文本内容发生变化时，状态栏字数统计实时更新
            tlb_words.Text = rh.Text.Replace("\n", "").Length.ToString();
        }

        private void 特殊字符ToolStripMenuItem_Click(object sender, EventArgs e)
        {//上面jhr_global是新定义的class，sp该类（class）是静态变量。
            if (xll_global.sp == null)
            {
                xll_global.sp = new xll_special(rh);
                xll_global.sp.Show();
            }
            else
            {
                xll_global.sp.Activate();
                //以下两句仅仅是防止窗体被我们放置到边缘后，一下子看不到，当激活时，放到显眼的位置而已
                xll_global.sp.Left = Screen.PrimaryScreen.WorkingArea.Width / 3;
                xll_global.sp.Top = Screen.PrimaryScreen.WorkingArea.Height / 3;
            }
        }

        private void 查找ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //菜单的查找，打开左侧的查找界面
            if (rh.SelectedText != null) td_findtext.Text = rh.SelectedText;//如果有选中的内容，默认为查找内容
            lbl_findnum.Text = "";//清空结果
            splitContainer1.Panel1Collapsed = false;//展开左侧
        }

        string lastfindtext = "";//记录已经完成查找的上一次待查找内容
        //自定义方法：清除查找过程设置的选中高亮显示效果：
        private void clear_findbackcolor()//清除查找到的内容高亮显示
        {
            if (rh.Text.Length != 0)
            {
                int startindex = 0, endindex = rh.Text.Length;//查找范围
                while ((startindex = rh.Find(lastfindtext, startindex, endindex, RichTextBoxFinds.None)) >= 0)
                {
                    rh.SelectionStart = startindex;
                    rh.SelectionLength = lastfindtext.Length;
                    rh.SelectionBackColor = rh.BackColor;//bug：会使局部的文字底纹消失，可以思考其他解决方案，不过有点复杂
                    startindex += lastfindtext.Length;
                    if (startindex >= endindex) break;
                }
            }
            rh.SelectionLength = 0;
            lastfindtext = "";
        }
        private void 查找button_Click(object sender, EventArgs e)
        {
            //查找按钮处理代码
            if (td_findtext.Text == "")
            {
                MessageBox.Show("请输入待查找的内容");
                td_findtext.Focus();
                return;
            }
            if (lastfindtext != "") clear_findbackcolor();//清除上一次的查找结果高亮显示
            if (rh.Text.Length == 0)
            {
                lbl_findnum.Text = "不存在查找的内容";
                return;
            }
            bool toolflag = false;
            int i = 0;//
            int startindex = 0, endindex = rh.Text.Length;//
            while ((startindex = rh.Find(td_findtext.Text, startindex, endindex, RichTextBoxFinds.None)) >= 0)
            {
                rh.SelectionStart = startindex;
                rh.SelectionLength = td_findtext.Text.Length;
                rh.SelectionBackColor = Color.YellowGreen;
                i++;
                startindex += td_findtext.Text.Length;
                if (startindex >= endindex) break;
            }
            if (i > 0) lastfindtext = td_findtext.Text;
            lbl_findnum.Text = "查找到" + i.ToString() + "个结果";
            rh.ScrollToCaret();//将滚动条的位置设置到最后一个查找到的内容位置、
            toolflag = true;

        }

        //查找界面关闭按钮处理代码：
        //splitcontainer.panel1关闭按钮，关闭左侧界面panel1
        private void pb_leftclose_Click(object sender, EventArgs e)
        {
            splitContainer1.Panel1Collapsed = true;
            //
            if (lastfindtext != "") clear_findbackcolor();
        }

        private void splitContainer1_Panel1_ClientSizeChanged(object sender, EventArgs e)
        {
            Panel p1 = sender as Panel;
            td_findtext.Width = p1.Width - 10;
        }

        private void 替换ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (xll_global.rp == null)
            {
                xll_global.rp = new xll_replace(rh);
                xll_global.rp.Show();
            }
            else
            {
                xll_global.rp.Activate();
                xll_global.rp.Left = Screen.PrimaryScreen.WorkingArea.Width / 3;
                xll_global.rp.Top = Screen.PrimaryScreen.WorkingArea.Height / 3;
            }
        }


        private void ts_fontbold_Click(object sender, EventArgs e)
        {
            bool toolflag = false;
            //字体种类不同，就会使SelectionFont为null, 因此只能单个字符循环处理
            int len = rh.SelectionLength;
            int selectstart = rh.SelectionStart;
            toolflag = false;//在字体设置期间的光标位置改变引起的字体变化，項栏字体相关设置控件内容保持不变
            bool isbold = true;
            for (int i = 0; i < len; i++)//逐宇修改字体的加粗与否,其余样式保留
            {
                rh.Select(selectstart + i, 1);
                if (i == 0) isbold = rh.SelectionFont.Bold;//以第一个字符为基准，第一个字符为加粗,则此次全部选中文本撤销加粗,反之同理。
                if (isbold)
                    rh.SelectionFont = new Font(rh.SelectionFont, rh.SelectionFont.Style & ~FontStyle.Bold);//撤销加粗
                else
                    rh.SelectionFont = new Font(rh.SelectionFont, rh.SelectionFont.Style | FontStyle.Bold);//设置加粗
            }
            ts_fontbold.Checked = !isbold;
            rh.SelectionStart = selectstart;
            rh.SelectionLength = len;
            toolflag = true;
            rh.Focus();
        }

        private void ts_fontitalics_Click(object sender, EventArgs e)
        {
            bool toolflag = false;
            //字体种类不同，就会使SelectionFont为null, 因此只能单个字符循环处理
            int len = rh.SelectionLength;
            int selectstart = rh.SelectionStart;
            toolflag = false;//在字体设置期间的光标位置改变引起的字体变化，項栏字体相关设置控件内容保持不变
            bool isitalic = true;
            for (int i = 0; i < len; i++)//逐宇修改字体的加粗与否,其余样式保留
            {
                rh.Select(selectstart + i, 1);
                if (i == 0) isitalic = rh.SelectionFont.Italic;//以第一个字符为基准，第一个字符为加粗,则此次全部选中文本撤销加粗,反之同理。
                if (isitalic)
                    rh.SelectionFont = new Font(rh.SelectionFont, rh.SelectionFont.Style & ~FontStyle.Italic);//撤销加粗
                else
                    rh.SelectionFont = new Font(rh.SelectionFont, rh.SelectionFont.Style | FontStyle.Italic);//设置加粗
            }
            ts_fontbold.Checked = !isitalic;
            rh.SelectionStart = selectstart;
            rh.SelectionLength = len;
            toolflag = true;
            rh.Focus();
        }

        private void ts_fontunderline_Click(object sender, EventArgs e)
        {
            bool toolflag = false;
            //字体种类不同，就会使SelectionFont为null, 因此只能单个字符循环处理
            int len = rh.SelectionLength;
            int selectstart = rh.SelectionStart;
            toolflag = false;//在字体设置期间的光标位置改变引起的字体变化，項栏字体相关设置控件内容保持不变
            bool isunderline = true;
            for (int i = 0; i < len; i++)//逐宇修改字体的加粗与否,其余样式保留
            {
                rh.Select(selectstart + i, 1);
                if (i == 0) isunderline = rh.SelectionFont.Underline;//以第一个字符为基准，第一个字符为加粗,则此次全部选中文本撤销加粗,反之同理。
                if (isunderline)
                    rh.SelectionFont = new Font(rh.SelectionFont, rh.SelectionFont.Style & ~FontStyle.Underline);//撤销加粗
                else
                    rh.SelectionFont = new Font(rh.SelectionFont, rh.SelectionFont.Style | FontStyle.Underline);//设置加粗
            }
            ts_fontbold.Checked = !isunderline;
            rh.SelectionStart = selectstart;
            rh.SelectionLength = len;
            toolflag = true;
            rh.Focus();
        }

        private void ts_fontshadow_Click(object sender, EventArgs e)
        {
            if (rh.SelectionBackColor == null)//如果无法获取底纹，则直接设置底纹
            {
                rh.SelectionBackColor = Color.LightPink;
            }
            else//有底纹
            {
                if (rh.SelectionBackColor.Equals(Color.LightPink))//底纹正好是设置的标准色，则取消底纹
                {
                    rh.SelectionBackColor = rh.BackColor;
                }
                else//当前底纹不是标准色，设置底纹为标准色
                {
                    rh.SelectionBackColor = Color.LightPink;
                }
            }
        }
        bool toolflag;
        private void rh_SelectionChanged(object sender, EventArgs e)
        {
            //复制按钮不可用
            int len = rh.SelectionLength;
            if (len > 0)
                复制CToolStripButton.Enabled = true;
            else if (len == 0)
                复制CToolStripButton.Enabled = false;
            //粘贴按钮不可用
            if (len > 0)
                粘贴PToolStripButton.Enabled = true;
            else if (len == 0)
                粘贴PToolStripButton.Enabled = false;
            //剪切按钮不可用
            if (len > 0)
                剪切UToolStripButton.Enabled = true;
            else if (len == 0)
                剪切UToolStripButton.Enabled = false;


            //行号，列号
            //if (toolflag == false) return;//如果非用户操作引起的事件，忽略
            tlb_rows.Text = (rh.GetLineFromCharIndex(rh.SelectionStart) + 1).ToString();//行号
            tlb_column.Text = (rh.SelectionStart - rh.GetFirstCharIndexOfCurrentLine() + 1).ToString();//列号
            //工具栏字体信息显示的实时更新:字体颜色控件,字体种类,字体大小，加粗,斜体，下划线
            font_flag = false;
            size_flag = false;
            if (rh.SelectionLength == 0)//没有选中任何文本
            {
                tcb_font.Text = rh.SelectionFont.FontFamily.Name;
                tcb_fontsize.Text = rh.SelectionFont.Size.ToString();//后面一起处理别名显示
                ts_fontbold.Checked = rh.SelectionFont.Bold;
                ts_fontitalics.Checked = rh.SelectionFont.Italic;
                ts_fontunderline.Checked = rh.SelectionFont.Underline;
                ts_color.BackColor = rh.SelectionColor;
            }
            else//有选中文本，则考虑选中文本的字体各属性可能是不一致的，只贿某属性-致时才设置快捷工具栏的对应字体属性值,
            {
                string u_fontfamily = "";
                string u_fontsize = "";
                bool u_bold = false;
                bool u_italic = false;
                bool u_underline = false;
                char[] flags = new char[] { '1', '1', '1', '1', '1' };//分别表示字体种类，大小，加粗，斜体，下划线是否一致，1为一致
                RichTextBox rhtemp = new RichTextBox();
                rhtemp.Rtf = rh.SelectedRtf;
                //toolflag=false;
                //int oldselectstart=rh.SelectionStart;
                //int oldlwn=rh.SelectionLength;
                for (int i = 0; i < rh.SelectionLength; i++)//逐字判断，各字体属性是否一致
                {
                    rhtemp.Select(i, 1);
                    if (i == 0)
                    {
                        u_fontfamily = rhtemp.SelectionFont.FontFamily.Name;
                        u_fontsize = rhtemp.SelectionFont.Size.ToString();
                        u_bold = rhtemp.SelectionFont.Bold;
                        u_italic = rhtemp.SelectionFont.Italic;
                        u_underline = rhtemp.SelectionFont.Underline;
                        ts_color.BackColor = rhtemp.SelectionColor;//颜色处理为和第一个字符相同
                    }
                    else
                    {
                        if (!u_fontfamily.Equals(rhtemp.SelectionFont.FontFamily.Name)) flags[0] = '0';
                        if (!u_fontsize.Equals(rhtemp.SelectionFont.Size.ToString())) flags[1] = '0';
                        if (!u_bold.Equals(rhtemp.SelectionFont.Bold)) flags[2] = '0';
                        if (!u_italic.Equals(rhtemp.SelectionFont.Italic)) flags[3] = '0';
                        if (!u_underline.Equals(rhtemp.SelectionFont.Underline)) flags[4] = '0';
                    }
                    if ((new string(flags)).Equals("00000")) break;//如果每个属性都发现存在不一致，则跳出循环
                }//for结束
                //rh.SelectionStart=oldselectstart;
                //rh.SelectionLength=oldlen;
                //toolflag=true;
                if (flags[0].Equals('1')) tcb_font.Text = u_fontfamily;
                else tcb_font.SelectedIndex = -1;
                if (flags[1].Equals('1')) tcb_fontsize.Text = u_fontsize;
                else tcb_fontsize.Text = "";
                if (flags[2].Equals('1') && u_bold == true) ts_fontbold.Checked = true;
                else ts_fontbold.Checked = false;
                if (flags[3].Equals('1') && u_italic == true) ts_fontitalics.Checked = true;
                else ts_fontitalics.Checked = false;
                if (flags[4].Equals('1') && u_underline == true) ts_fontunderline.Checked = true;
                else ts_fontunderline.Checked = false;
            }//有选中文本结束
            if (tcb_fontsize.Text != "")//字体大小不为空，则处理下别名显示
            {
                int k = Array.IndexOf(v, float.Parse(tcb_fontsize.Text.ToString()));
                if (k > -1) tcb_fontsize.Text = d[k];
            }
            font_flag = true;
            size_flag = true;
        }

        private void 字数统计ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string s;
            if (rh.SelectedText == "") s = rh.Text;
            else s = rh.SelectedText;
            int c_all = 0;//计空格字符数
            int c = 0;//不计空格字符数
            int c_ch = 0;//中文字数
            c_all = s.Replace("\n", "").Length;//总字数中除了回车
            s = s.Replace(" ", "").Replace("\n", "");//去除空格和回车
            c = s.Length;
            CharEnumerator chenumeator = s.GetEnumerator();
            Regex regex = new Regex("^[\u4E00-\u9FA5]{0,}$");
            while (chenumeator.MoveNext())
            {
                if (regex.IsMatch(chenumeator.Current.ToString(), 0)) c_ch++;
            }
            MessageBox.Show("字符总数：" + c_all + "\n不计空格字符数：" + c + "\n中文字符数：" + c_ch, "字数统计");
        }
        string filename = null;//
        private void 保存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int i = dosave();
        }
        //对原文档保存的统一 处理保存返回1,失败-1,没有执行保存返回0
        private int dosave()
        {
            try
            {
                if (filename == null)//如果是第一次保存，则选择保存路径和文件名
                {
                    SaveFileDialog sd = new SaveFileDialog();
                    sd.Filter = "RTF文件|*.rtf|TXT文本|*.txt";
                    sd.Title = "保存文件";
                    sd.OverwritePrompt = true;//如果是第- 次保存,则选择保存路径和文件名
                    if (sd.ShowDialog() == DialogResult.OK)//单击保存对话框的确定或保存按钮
                    {
                        string s = sd.FileName;//不要提前改变filename的内容, 也许保存过程发生意外终止

                        if (sd.FilterIndex == 1) rh.SaveFile(s);//Filterlndex==1 ,即rtf格式, 2 txt格式
                        else rh.SaveFile(s, RichTextBoxStreamType.PlainText);
                        rh.Modified = false;//设置控件为未修改状态
                        filename = s;
                        string t = filename.Substring(filename.LastIndexOf(@"\") + 1);//去除路径
                        this.Text = "xll的记事本  " + t;
                        return 1;
                    }
                    else//单击保存对话框的取消按钮
                    {
                        return 0;
                    }
                }//第一次保存结束
                else//非第一次保存
                {
                    string a = filename.Substring(filename.LastIndexOf('.') + 1).ToLower();//扩展名
                    if (a.Equals("rtf")) rh.SaveFile(filename);//rtf格式
                    else rh.SaveFile(filename, RichTextBoxStreamType.PlainText);//TXT格式
                    rh.Modified = false;//设置空间为未修改状态
                    return 1;
                }//非第一次保存结束

            }//try
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return -1;
            }
        }

        private void 新建ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (rh.Modified == true)//如果原编辑内容发生过变化，则询问是否保存
            {
                DialogResult result = MessageBox.Show("文件内容已经发生改变,是否保存？", "提示", MessageBoxButtons.YesNoCancel);
                if (result == DialogResult.Cancel) return;//如果选择取消，则返回
                if (result == DialogResult.Yes)//如果选择是,则保存原文件
                {
                    int i = dosave();
                    if (i < 1) return;//保存失败或在保存时选择取消，则都返回,放弃新建操作
                }//询问结果是
            }//有变化处理结束
            //新建
            filename = null;
            rh.Clear();
            rh.Modified = false;
            this.Text = "xll的记事本  " + filename;
        }

        private void 打开ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (rh.Modified == true)//如果原编辑内容发生过变化，则询问是否保存
            {
                DialogResult result = MessageBox.Show("文件内容已经发生改变,是否保存？", "提示", MessageBoxButtons.YesNoCancel);
                if (result == DialogResult.Cancel) return;//如果选择取消，则返回
                if (result == DialogResult.Yes)//如果选择是,则保存原文件
                {
                    int i = dosave();
                    if (i < 1) return;//保存失败或在保存时选择取消，则都返回,放弃新建操作
                }//询问结果是
            }//有变化处理结束
            //打开文件
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "RTF文件|*.rtf|TXT文本|*.txt";
            ofd.Title = "打开文件";
            if (filename != null) ofd.InitialDirectory = filename.Substring(0, filename.LastIndexOf(@"\"));
            if (ofd.ShowDialog() == DialogResult.OK)//打开对话框选择是
            {
                filename = ofd.FileName;
                rh.Clear();
                string a = filename.Substring(filename.LastIndexOf('.') + 1).ToLower();
                try//修复文件打开冲突引起的打开失败处理
                {
                    if (a.Equals("rtf"))
                        rh.LoadFile(filename);
                    else
                        rh.LoadFile(filename, RichTextBoxStreamType.PlainText);
                    this.Text = "xll的记事本   " + ofd.SafeFileName;
                }
                catch (Exception ee)
                {
                    MessageBox.Show(ee.Message);
                    filename = null;//打开失败，文档空白，文件名清空
                    this.Text = "xll的记事本";
                }
                rh.Modified = false;//设置文本框为未修改状态
            }//打开对话框选择是结束
        }

        private void 另存为ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int i = dosave另存();
        }

        private int dosave另存()
        {
            try
            {
                if (filename != null)//如果是第一次保存，则选择保存路径和文件名
                {
                    SaveFileDialog sd = new SaveFileDialog();
                    sd.Filter = "RTF文件|*.rtf|TXT文本|*.txt";
                    sd.Title = "保存文件";
                    sd.OverwritePrompt = true;//如果是第- 次保存,则选择保存路径和文件名
                    if (sd.ShowDialog() == DialogResult.OK)//单击保存对话框的确定或保存按钮
                    {
                        string s = sd.FileName;//不要提前改变filename的内容, 也许保存过程发生意外终止

                        if (sd.FilterIndex == 1) rh.SaveFile(s);//Filterlndex==1 ,即rtf格式, 2 txt格式
                        else rh.SaveFile(s, RichTextBoxStreamType.PlainText);
                        rh.Modified = false;//设置控件为未修改状态
                        filename = s;
                        string t = filename.Substring(filename.LastIndexOf(@"\") + 1);//去除路径
                        this.Text = "xll的记事本  " + t;
                        return 1;
                    }
                    else//单击保存对话框的取消按钮
                    {
                        return 0;
                    }
                }//第一次保存结束
                else//非第一次保存
                {
                    SaveFileDialog sd = new SaveFileDialog();
                    sd.Filter = "RTF文件|*.rtf|TXT文本|*.txt";
                    sd.Title = "保存文件";
                    sd.OverwritePrompt = true;//如果是第- 次保存,则选择保存路径和文件名
                    if (sd.ShowDialog() == DialogResult.OK)//单击保存对话框的确定或保存按钮
                    {
                        string s = sd.FileName;//不要提前改变filename的内容, 也许保存过程发生意外终止

                        if (sd.FilterIndex == 1) rh.SaveFile(s);//Filterlndex==1 ,即rtf格式, 2 txt格式
                        else rh.SaveFile(s, RichTextBoxStreamType.PlainText);
                        rh.Modified = false;//设置控件为未修改状态
                        filename = s;
                        string t = filename.Substring(filename.LastIndexOf(@"\") + 1);//去除路径
                        this.Text = "xll的记事本  " + t;
                        return 1;
                    }
                    else//单击保存对话框的取消按钮
                    {
                        return 0;
                    }
                }//非第一次保存结束

            }//try
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return -1;
            }
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void xll_Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (rh.Modified == true)//如果原编辑内容发生过变化，则询问是否保存
            {
                DialogResult result = MessageBox.Show("文件内容已经发生改变,是否保存？", "提示", MessageBoxButtons.YesNoCancel);
                if (result == DialogResult.Cancel)//如果选择取消，则取消关闭窗体事件并返回
                {
                    e.Cancel = true;
                    return;
                }
                if (result == DialogResult.Yes)//如果选择是，保存原文件
                {
                    int i = dosave();
                    if (i <= 0)//保存失败或在保存对话框单击取消，则取消关闭窗体事件并返回
                    {
                        e.Cancel = true;
                        return;
                    }
                }//询问选择是
            }//有变化处理结束
        }

        private void xll_Form1_DragEnter(object sender, DragEventArgs e)
        {
            //鼠标拖放式-进入
            if (e.Data.GetDataPresent(DataFormats.FileDrop))//预览有没有文件拖入，有
                e.Effect = DragDropEffects.Link;
            else//无
                e.Effect = DragDropEffects.None;
        }

        private void xll_Form1_DragDrop(object sender, DragEventArgs e)
        {//鼠标松开事件处理代码：
            string[] ss = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (ss.Length == 0) return;
            foreach (string s in ss)
            {
                try
                {
                    Process p = new Process();
                    p.StartInfo.FileName = Process.GetCurrentProcess().ProcessName;
                    p.StartInfo.Arguments = s;
                    p.Start();
                }
                catch (Exception ex)
                {
                    xll_Form1 xll = new xll_Form1(s);
                    xll.Show();
                        
                }
            }
        }


        private void 打印ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //为了简化处理, 这直接将文件送到打印机打印，不是别好
            //有兴趣的话可以尝试打印richtextbox的内容, 要留图和格式
            if (filename == null || filename == "")
            {
                MessageBox.Show("save first,please!");
                return;
            }
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            try
            {
                p.StartInfo.FileName = filename;
                p.StartInfo.WorkingDirectory = (new FileInfo(filename)).DirectoryName;
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                p.StartInfo.Verb = "Print";
                p.Start();
                if (!p.HasExited)
                {
                    p.WaitForInputIdle(10000);
                    int i = 1;
                    bool running = true;
                    while (running && i <= 20)
                    {
                        System.Threading.Thread.Sleep(500);
                        if (p.HasExited)
                            running = false;
                        else
                            running = !p.CloseMainWindow();
                        i++;
                    }
                    if (running && !p.HasExited)
                        p.Kill();
                }
                p.Dispose();
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
        }

        private void 关于ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("作者：徐玲玲");
            return;
        }

        private void 隐藏快捷工具栏ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem ts = sender as ToolStripMenuItem;
            ts.Checked = !ts.Checked;
           toolStrip1.Visible = !toolStrip1.Visible;
        }

        private void 隐藏任务栏ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem ts = sender as ToolStripMenuItem;
            ts.Checked = !ts.Checked;
            statusStrip1.Visible = !statusStrip1.Visible;
        }

        private void 查看帮助ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("版本：1.0。");
            return;
        }
    }
}



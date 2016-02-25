using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tetris
{
    public partial class FrmConfig : Form
    {
        public FrmConfig()
        {
            InitializeComponent();
        }

        private bool[,] struArr = new bool[5, 5];
        private Color blockColor = Color.Red;

        private void lblMode_Paint(object sender, PaintEventArgs e)
        {
            Graphics gp = e.Graphics;
            gp.Clear(Color.Black);
            Pen p = new Pen(Color.White);
            //SolidBrush s = new SolidBrush(Color.White);
            for (int i = 31; i < 155; i = i + 31)//画横白线
                gp.DrawLine(p, 1, i, 155, i);
            for (int i = 31; i < 155; i = i + 31)//画竖白线
                gp.DrawLine(p, i, 1, i, 155);

            //填充矩阵中的方块
            SolidBrush s = new SolidBrush(blockColor);
            for (int x = 0; x < 5; x++)
            {
                for (int y = 0; y < 5; y++)
                {
                    if (struArr[x, y])
                    {
                        gp.FillRectangle(s, 31 * x + 1, 31 * y + 1, 30, 30);
                    }
                }
            }

        }

        private void lblMode_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)//判断是否点击了鼠标左键，如果不是就退出
                return;
            int xPos, yPos;//xPos表示数组第一个下标，yPos表示数组第二个下标
            xPos = e.X / 31;
            yPos = e.Y / 31;
            struArr[xPos, yPos] = !struArr[xPos, yPos];
            bool b = struArr[xPos, yPos];
            Graphics gp = lblMode.CreateGraphics(); //得到lblMode的Graphics
            SolidBrush s = new SolidBrush(b ? blockColor : Color.Black);//创建一个刷子，并确定它的颜色
            gp.FillRectangle(s, 31 * xPos + 1, 31 * yPos + 1, 30, 30);//给所点击的方块上色
            gp.Dispose();//释放Graphics
        }

        private void lblColor_Click(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();//打开颜色对话框
            blockColor = colorDialog1.Color;
            lblColor.BackColor = colorDialog1.Color;
            lblMode.Invalidate();//使lblMode重画，即执行它的Paint事件
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            bool isEmpty = false;//首先查找图案是否为空
            foreach(bool i in struArr)
            {
                if(i)
                {
                    isEmpty = true;
                    break;
                }
            }
            if(!isEmpty)
            {
                MessageBox.Show("图案为空，请先用鼠标点击左边窗口绘制图案！", "提示窗口", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            StringBuilder sb = new StringBuilder(25);
            foreach(bool i in struArr)
            {
                sb.Append(i ? "1" : "0");
            }
            string blockString = sb.ToString();
            //再检查是否有重复图案
            foreach(ListViewItem item in lsvBlockSet.Items)
            {
                if(item.SubItems[0].Text == blockString)
                {
                    MessageBox.Show("该图案已经存在！", "提示窗口", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            //把新砖块图案信息添加进ListView
            ListViewItem myItem = new ListViewItem();
            myItem = lsvBlockSet.Items.Add(blockString);
            myItem.SubItems.Add(Convert.ToString(blockColor.ToArgb()));

        }

        private void lsvBlockSet_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if(e.IsSelected)//避免重复执行事件
            {
                blockColor = Color.FromArgb(int.Parse(e.Item.SubItems[1].Text));//把字符串信息转换为颜色类
                lblColor.BackColor = blockColor;
                string s = e.Item.SubItems[0].Text;//取砖块样式信息
                for(int i = 0; i < s.Length; i++)//把砖块样式信息从字符串转换为二维数组
                {
                    struArr[i / 5, i % 5] = (s[i] == '1') ? true : false;
                }
                lblMode.Invalidate();
            }
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            if(lsvBlockSet.SelectedItems.Count == 0)
            {
                MessageBox.Show("请在右边窗口选择一个条目进行删除！", "提示窗口", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            lsvBlockSet.Items.Remove(lsvBlockSet.SelectedItems[0]);
            btnClear.PerformClick();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            for(int x = 0; x < 5; x++)//把struArr内所有元素置为false，实现清空
            {
                for(int y = 0; y < 5; y++)
                {
                    struArr[x, y] = false;
                }
            }
            lblMode.Invalidate();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if(lsvBlockSet.SelectedItems.Count == 0)//判断是否有项目被选中
            {
                MessageBox.Show("请在右边窗口选择一个条目进行修改！", "提示窗口", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            bool isEmpty = false;//判断图案是否为空
            foreach(bool i in struArr)
            {
                if(i)
                {
                    isEmpty = true;
                    break;
                }
            }
            if(!isEmpty)
            {
                MessageBox.Show("图案为空，请先用鼠标点击左边窗口绘制图案再进行修改！", "提示窗口", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            StringBuilder sb = new StringBuilder(25);
            foreach(bool i in struArr)//把图案的字符串信息转换为二维数组信息
            {
                sb.Append(i ? "1" : "0");
            }
            lsvBlockSet.SelectedItems[0].SubItems[0].Text = sb.ToString();//改变图案信息
            lsvBlockSet.SelectedItems[0].SubItems[1].Text = Convert.ToString(blockColor.ToArgb());//改变颜色信息
        }

        private void txtContra_KeyDown(object sender, KeyEventArgs e)
        {
            //首先排除一些不适合的键值
            if((e.KeyValue >= 33 && e.KeyValue <= 36) || (e.KeyValue >= 45 && e.KeyValue <= 46) ||
                (e.KeyValue >= 48 && e.KeyValue <= 57) || (e.KeyValue >= 65 && e.KeyValue <= 90) ||
                (e.KeyValue >96 && e.KeyValue <= 107) || (e.KeyValue >= 109 && e.KeyValue <= 111) ||
                (e.KeyValue >= 186 && e.KeyValue <= 192) ||
                (e.KeyValue >= 219 && e.KeyValue <= 222))
            {
                //检查是否存在冲突的快捷键
                foreach(Control c in gbKeySet.Controls)
                {
                    Control TempC = c as TextBox;
                    if(TempC != null && ((TextBox)TempC).Text != "")
                    {
                        if(((int)((TextBox)TempC).Tag) == e.KeyValue)
                        {
                            ((TextBox)TempC).Text = "";
                            ((TextBox)TempC).Tag = Keys.None;
                        }
                    }
                }
                ((TextBox)sender).Text = e.KeyCode.ToString();
                ((TextBox)sender).Tag =(Keys)e.KeyValue;
            }
        }

        private void lblBackColor_Click(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();//打开颜色对话框
            lblBackColor.BackColor = colorDialog1.Color;//把选中的颜色做为标签的背景色
        }
    }
}

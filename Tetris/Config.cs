using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using System.Reflection;

namespace Tetris
{
    class Config
    {
        private Keys _downKey;//向下键
        private Keys _dropKey;//丢下降
        private Keys _moveLeftKey;//左移键
        private Keys _moveRightKey;//右移键
        private Keys _deasilRotateKey;//顺时针旋转键
        private Keys _contraRotateKey;//逆时针旋转键
        private int _coorWidth;//水平格子数
        private int _coorHeight;//垂直格子数
        private int _rectPix;//方块像素
        private Color _backColor;//背景色
        private InfoArr info = new InfoArr();//砖块信息集合
        #region 私有变量相应的属性
        public Keys DownKey
        {
            get
            {
                return _downKey;
            }
            set
            {
                _downKey = value;
            }
        }
        public Keys DropKey
        {
            get
            {
                return _dropKey;
            }
            set
            {
                _dropKey = value;
            }
        }
        public Keys MoveLeftKey
        {
            get
            {
                return _moveLeftKey;
            }
            set
            {
                _moveLeftKey = value;
            }
        }
        public Keys MoveRightKey
        {
            get
            {
                return _moveRightKey;
            }
            set
            {
                _moveRightKey = value;
            }
        }
        public Keys DeasilRotateKey
        {
            get
            {
                return _deasilRotateKey;
            }
            set
            {
                _deasilRotateKey = value;
            }
        }
        public Keys ContraRotateKey
        {
            get
            {
                return _contraRotateKey;
            }
            set
            {
                _contraRotateKey = value;
            }
        }
        public int CoorWidth
        {
            get
            {
                return _coorWidth;
            }
            set
            {
                if (value >= 10 && value <= 50)
                    _coorWidth = value;
            }
        }
        public int CoorHeight
        {
            get
            {
                return _coorHeight;
            }
            set
            {
                if (value >= 10 && value <= 50)
                    _coorHeight = value;
            }
        }
        public int RectPix
        {
            get
            {
                return _rectPix;
            }
            set
            {
                if (value >= 10 && value <= 30)
                    _rectPix = value;
            }
        }
        public Color BackColor
        {
            get
            {
                return _backColor;
            }
            set
            {
                _backColor = value;
            }
        }
        public InfoArr Info
        {
            get
            {
                return info;
            }
            set
            {
                info = value;
            }
        }
        #endregion
        public void LoadFormXmlFile()
        {
            XmlTextReader reader;
            if(File.Exists("BlockSet.xml"))
            {
                //优先读取外部BlockSet.xml文件并把信息写入blockInfo内
                reader = new XmlTextReader("BlockSet.xml");
            }
            else
            {
                //如果BlockSet.xml文件不存在，则从嵌入资源内读取BlockSet.xml
                Assembly asm = Assembly.GetExecutingAssembly();
                Stream sm = asm.GetManifestResourceStream("Tetris.BlockSet.xml");
                reader = new XmlTextReader(sm);
            }
            string key = "";
            try 
            { 
                while(reader.Read())
                {
                    if(reader.NodeType == XmlNodeType.Element)
                    {
                        if(reader.Name == "ID")
                        {
                            key = reader.ReadElementString().Trim();
                        }
                        if//第七集17分49秒
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                if(reader != null)
                {
                    reader.Close();
                }
            }
        }
    }
}

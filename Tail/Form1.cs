using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tail
{
    public partial class Form1 : Form
    {
        private string filePath = "";
        private int startPos = -1;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            filePath = SelectFile("", filePath);

            int b = GetFileLineNum(filePath);


            while (true)
            {
                var c = GetTail(filePath);

                outTail(c);

                System.Threading.Thread.Sleep(1000);
            }
        }

        private void outTail(List<string> lines)
        {
            foreach (var item in lines)
            {
                //System.Diagnostics.Debug.Print(item);

                this.textBox1.Text += item + Environment.NewLine; 
            }

            //カレット位置を末尾に移動
            this.textBox1.SelectionStart = this.textBox1.Text.Length;
            //テキストボックスにフォーカスを移動
            this.textBox1.Focus();
            //カレット位置までスクロール
            this.textBox1.ScrollToCaret();

            Application.DoEvents();
        }


        private string SelectFile(string defaultFileName = "", string defaultDir = @"C:\", string defaultFilter = "LOGファイル(*.log)|*.log|TXTファイル(*.txt)|*.txt|すべてのファイル(*.*)|*.*")
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.FileName = defaultFileName;
            ofd.InitialDirectory = defaultDir;
            ofd.Filter = defaultFilter;
            ofd.FilterIndex = 1;
            ofd.Title = "ファイルを選択してください";
            //ダイアログボックスを閉じる前に現在のディレクトリを復元するようにする
            ofd.RestoreDirectory = true;
            //存在しないファイルの名前が指定されたとき警告を表示する
            //デフォルトでTrueなので指定する必要はない
            ofd.CheckFileExists = true;
            //存在しないパスが指定されたとき警告を表示する
            //デフォルトでTrueなので指定する必要はない
            ofd.CheckPathExists = true;

            //ダイアログを表示する
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                return ofd.FileName;
            }

            return "";
        }


        private int GetFileLineNum(string fileName)
        {
            var lineCount = 0;

            using (System.IO.StreamReader sr = OpenFile(fileName))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    lineCount++;
                }
            }

            return lineCount;
        }


        private List<string> GetTail(string fileName)
        {
            var res = new List<string>();
            var lineCount = 0;
            var maxLine = GetFileLineNum(fileName);

            using (System.IO.StreamReader sr = OpenFile(fileName))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    if( this.startPos <= lineCount)
                    {
                        res.Add(line);
                    }

                    lineCount++;
                }

                this.startPos = lineCount;
            }

            return res;
        }

        private System.IO.StreamReader OpenFile(string fileName)
        {
            System.IO.FileStream fs = new System.IO.FileStream(
                fileName,
                System.IO.FileMode.Open,
                System.IO.FileAccess.Read,
                System.IO.FileShare.ReadWrite);

            return new System.IO.StreamReader(fs);
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            var sz = 16 + ((System.Windows.Forms.TrackBar)sender).Value;

            this.textBox1.Font = new System.Drawing.Font(this.textBox1.Font.FontFamily, sz, this.textBox1.Font.Style);
        }
    }
}

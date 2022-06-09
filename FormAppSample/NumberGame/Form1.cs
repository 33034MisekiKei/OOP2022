using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NumberGame 
{
    public partial class Form1 : Form 
    {
        private Random rand = new Random();

        //乱数
        private int randomNumder;

        public Form1() 
        {
            InitializeComponent();
        }
        //アプリケーション起動時に一度だけ呼ばれるイベントハンドラ
        private void Form1_Load(object sender, EventArgs e) 
        {
            //乱数取得
            randomNumder = rand.Next(1, (int)maxNum.Value);
            this.Text = randomNumder.ToString();
        }
        //入力ボタンイベントハンドラ
        private void Judge_Click(object sender, EventArgs e) 
        {
            //入力した値とあらかじめ取得した乱数を比較し結果を表示
            
        }
    }
}

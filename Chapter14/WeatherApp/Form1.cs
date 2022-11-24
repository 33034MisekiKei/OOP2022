using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WeatherApp {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }

        string[] codes ={"016000","020000","030000","040000","050000"
                        ,"060000","070000","080000","090000","100000"
                        ,"110000","120000","130000","140000","150000"
                        ,"160000","170000","180000","190000","200000"
                        ,"210000","220000","230000","240000","250000"
                        ,"260000","270000","280000","290000","300000"
                        ,"310000","320000","330000","340000","350000"
                        ,"360000","370000","380000","390000","400000"
                        ,"410000","420000","430000","440000","450000"
                        ,"460100","471000",};

        private void btWeatherGet_Click(object sender, EventArgs e) {

            var wc = new WebClient() 
            {
                Encoding = Encoding.UTF8
            };


            string url = "https://www.jma.go.jp/bosai/forecast/data/overview_forecast/" + codes[comboBox1.SelectedIndex] + ".json";

            var dString = wc.DownloadString(url);

            var json = JsonConvert.DeserializeObject<Rootobject>(dString);

            publishingOffice.Text = json.publishingOffice;
            name.Text = json.targetArea;
            
            tbWeatherInfo.Text = json.text;

            pbImage.ImageLocation = "https://www.jma.go.jp/bosai/forecast/img/101.png";

            //var String = wc.DownloadString("https://www.jma.go.jp/bosai/weather_map/data/png/20221116185030_0_Z__C_010000_20221116120000_MET_CHT_JCIfsas24_Rjp_JCP600x581_JRcolor_Tjmahp_image.png");
            
            var now = DateTimeOffset.Now;

            
        }

        private void Form1_Load(object sender, EventArgs e) {
            comboBox1.Items.Add("北海道");
            comboBox1.Items.Add("青森");
            comboBox1.Items.Add("岩手");
            comboBox1.Items.Add("宮城");
            comboBox1.Items.Add("秋田");
            comboBox1.Items.Add("山形");
            comboBox1.Items.Add("福島");
            comboBox1.Items.Add("茨城");
            comboBox1.Items.Add("栃木");
            comboBox1.Items.Add("群馬");
            comboBox1.Items.Add("埼玉");
            comboBox1.Items.Add("千葉");
            comboBox1.Items.Add("東京");
            comboBox1.Items.Add("神奈川");
            comboBox1.Items.Add("新潟");
            comboBox1.Items.Add("富山");
            comboBox1.Items.Add("石川");
            comboBox1.Items.Add("福井");
            comboBox1.Items.Add("山梨");
            comboBox1.Items.Add("長野");
            comboBox1.Items.Add("岐阜");
            comboBox1.Items.Add("静岡");
            comboBox1.Items.Add("愛知");
            comboBox1.Items.Add("三重");
            comboBox1.Items.Add("滋賀");
            comboBox1.Items.Add("京都");
            comboBox1.Items.Add("大阪");
            comboBox1.Items.Add("兵庫");
            comboBox1.Items.Add("奈良");
            comboBox1.Items.Add("和歌山");
            comboBox1.Items.Add("鳥取");
            comboBox1.Items.Add("島根");
            comboBox1.Items.Add("岡山");
            comboBox1.Items.Add("広島");
            comboBox1.Items.Add("山口");
            comboBox1.Items.Add("徳島");
            comboBox1.Items.Add("香川");
            comboBox1.Items.Add("愛媛");
            comboBox1.Items.Add("高知");
            comboBox1.Items.Add("福岡");
            comboBox1.Items.Add("佐賀");
            comboBox1.Items.Add("長崎");
            comboBox1.Items.Add("熊本");
            comboBox1.Items.Add("大分");
            comboBox1.Items.Add("長崎");
            comboBox1.Items.Add("鹿児島");
            comboBox1.Items.Add("沖縄");
        }
    }
}

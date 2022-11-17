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

        private void btWeatherGet_Click(object sender, EventArgs e) {

            var wc = new WebClient() 
            {
                Encoding = Encoding.UTF8
            };

            var dString = wc.DownloadString("https://www.jma.go.jp/bosai/forecast/data/forecast/130000.json");
            tbWeatherInfo.Text = wc.DownloadString("https://www.jma.go.jp/bosai/weather_map/data/png/20221116185030_0_Z__C_010000_20221116120000_MET_CHT_JCIfsas24_Rjp_JCP600x581_JRcolor_Tjmahp_image.png");

            var json = JsonConvert.DeserializeObject<Class1[]>(dString);

            name.Text = json[0].timeSeries[0].areas[1].area.name;
            weathers.Text = json[0].timeSeries[0].areas[0].weathers[0];
            winds.Text = json[0].timeSeries[0].areas[0].winds[0];
            waves.Text = json[0].timeSeries[0].areas[0].waves[0];
            
        }
    }
}

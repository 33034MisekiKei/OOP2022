﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Xml.Linq;

namespace RssReader 
{
    public partial class Form1 : Form 
    {
        IEnumerable<string> xTitle, xLink;

        public Form1() 
        {
            InitializeComponent();
        }

        private void btRssGet_Click(object sender, EventArgs e) 
        {
            using (var wc = new WebClient()) 
            {
                var stream = wc.OpenRead(cbRssUrl.Text);

                var xdoc = XDocument.Load(stream);
                xTitle = xdoc.Root.Descendants("item").Select(x => (string)x.Element("title"));
                xLink = xdoc.Root.Descendants("item").Select(x => (string)x.Element("link"));
                foreach (var data in xTitle) 
                {
                    lbRssTitle.Items.Add(data);
                }
            }
        }

        private void btBack_Click(object sender, EventArgs e) 
        {
           wvBrowser.GoBack();
        }

        private void btForward_Click(object sender, EventArgs e) 
        {
            wvBrowser.GoForward();
        }

        private void Form1_Load(object sender, EventArgs e) 
        {
            btBack.Enabled = wvBrowser.CanGoBack;
            btForward.Enabled = wvBrowser.CanGoForward;
        }

        private void wvBrowser_NavigationCompleted(object sender, Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT.WebViewControlNavigationCompletedEventArgs e) 
        {
            //進む・戻るボタンのマスク設定
            btBack.Enabled = wvBrowser.CanGoBack;
            btForward.Enabled = wvBrowser.CanGoForward;
        }

        private void lbRssTitle_SelectedIndexChanged(object sender, EventArgs e) 
        {
            int index = lbRssTitle.SelectedIndex; //選択した箇所のインデックスを取得（0～　）
            if (index == -1) return;

            var url = xLink.ElementAt(index);
            //wbBrowser.Url = new Uri(url);
            //wvBrowser.Url = new Uri(url);
            wvBrowser.Source = new Uri(url);
        }
    }
}

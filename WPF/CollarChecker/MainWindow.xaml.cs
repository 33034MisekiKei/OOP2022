﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CollarChecker {
    public partial class MainWindow : Window {

        public MainWindow() {
            InitializeComponent();
            DataContext = GetColorList(); //←追加          
        }

        List<MyColor> colorList = new List<MyColor>();
        MyColor mycolor = new MyColor();

        /// <summary>
        /// すべての色を取得するメソッド
        /// </summary>
        /// <returns></returns>
        private MyColor[] GetColorList() {
            return typeof(Colors).GetProperties(BindingFlags.Public | BindingFlags.Static)
                .Select(i => new MyColor() { Color = (Color)i.GetValue(null), Name = i.Name }).ToArray();
        }
        
        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) {
            setColor();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e) {
            setColor(); //起動時に初期状態の設定値 (R:0 G:0 B:0) から色を設定
        }

        //テキストボックスの値を元に色を設定
        private void setColor() {
            var r = byte.Parse(rValue.Text);
            var g = byte.Parse(gValue.Text);
            var b = byte.Parse(bValue.Text);

            Color color = Color.FromRgb(r,g,b);
            colorArea.Background = new SolidColorBrush(Color.FromRgb(r, g, b));
        }
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            var mycolor = (MyColor)((ComboBox)sender).SelectedItem;
            //var color = mycolor.Color;
            //var name = mycolor.Name;

            rSlider.Value = mycolor.Color.R;
            gSlider.Value = mycolor.Color.G;
            bSlider.Value = mycolor.Color.B;
            //setColor();
        }
        //ストックボタンクリック
        private void stockButton_Click(object sender, RoutedEventArgs e) {

            MyColor stColor = new MyColor();
            var r = byte.Parse(rValue.Text);
            var g = byte.Parse(gValue.Text);
            var b = byte.Parse(bValue.Text);
            stColor.Color = Color.FromRgb(r, g, b);

            //ここ分からん
            //テキストボックスのRGB値から色名称があるかチェック
            var colorName = ((IEnumerable<MyColor>)DataContext)
            .Where(c => c.Color.R == stColor.Color.R &&
                        c.Color.G == stColor.Color.G &&
                        c.Color.B == stColor.Color.B).FirstOrDefault();

            //stockList.Items.Add("R:" + rValue.Text + " G:" + gValue.Text + " B:" + bValue.Text);
            stockList.Items.Add(colorName?.Name ?? "R:" + rValue.Text + " G:" + gValue.Text + " B:" + bValue.Text);
            //AddをInsertにすると入れる場所を指定できる。Insert(0,colorName?　Insert(0,stColor);

            colorList.Add(stColor);
            //GetColorList.Add(stColor);
        }
        //デリートボタンクリック
        private void deleteButton_Click(object sender, RoutedEventArgs e) {

            //stockList.Items.RemoveAt(stockList.SelectedIndex);
            //colorList.Remove((MyColor)stockList.SelectedIndex);

            var dellndex = stockList.SelectedIndex;
            if (dellndex == -1) return;

            stockList.Items.RemoveAt(dellndex);
            colorList.RemoveAt(dellndex);
        }

        private void stockList_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (stockList.SelectedIndex == -1) return;

            rSlider.Value = colorList[stockList.SelectedIndex].Color.R;
            gSlider.Value = colorList[stockList.SelectedIndex].Color.G;
            bSlider.Value = colorList[stockList.SelectedIndex].Color.B;
            setColor();

        }
    }

    /// <summary>
    /// 色と色名を保持するクラス
    /// </summary>
    public class MyColor {
        public Color Color { get; set; }
        public string Name { get; set; }
    }
}

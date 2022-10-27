﻿using System;
using System.Collections.Generic;
using System.Linq;
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

namespace SampleApplication {
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
            seasonComboBox.SelectedIndex = 0;
        }

        private int getSeasonIndex() {
            var today = DateTime.Now;

            int ret = ((int)today.Month) /3 - 1;
            if (ret < 0)
                ret = 3;

            return ret;
        }

        private void checkBox_Checked(object sender, RoutedEventArgs e) {
            checlBoxTextBlock.Text = "チェック済み";
        }

        private void checkBox_Unchecked(object sender, RoutedEventArgs e) {
            checlBoxTextBlock.Text = "未チェック";
        }

        private void seasonComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            seasonTextBox.Text = (string)((ComboBoxItem)seasonComboBox.SelectedItem).Content;
        }

        private void redRadioButton_Checked_1(object sender, RoutedEventArgs e) {
            colorTextBox.Text = "赤";
        }

        private void yellowRadioButton_Checked_1(object sender, RoutedEventArgs e) {
            colorTextBox.Text = "黄";
        }

        private void blueRadioButton_Checked_1(object sender, RoutedEventArgs e) {
            colorTextBox.Text = "青";
        }
    }
}
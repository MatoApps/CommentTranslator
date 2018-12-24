using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Shapes;
using Core;
using MahApps.Metro.Controls;

namespace CommentTranslator.View
{
    /// <summary>
    /// AboutWindow.xaml 的交互逻辑
    /// </summary>
    public partial class AboutWindow : MetroWindow
    {
        private void Hyperlink_OnClick(object sender, RoutedEventArgs e)
        {
            Process.Start("https://github.com/MatoApps/CommentTranslator");
        }

        public AboutWindow()
        {
            InitializeComponent();
        }

        private void OpenFolder_OnClick(object sender, RoutedEventArgs e)
        {
            string outputsPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Output");
            Process.Start(outputsPath);
        }

    }
}

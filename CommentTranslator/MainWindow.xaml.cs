using System;
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
using CommentTranslator.View;
using Core;
using MahApps.Metro.Controls;

namespace CommentTranslator
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : MetroWindow
    {

        GetWordCore getWordCore;
        public MainWindow()
        {
            getWordCore = new GetWordCore();
            getWordCore.OnTextChanged += GetWordCore_OnTextChanged;
            InitializeComponent();
        }

        private void GetWordCore_OnTextChanged(object sender, string e)
        {
            MessageBox.Show(e);
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            getWordCore.Enable(true);

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            getWordCore.Load();
        }
    }
}

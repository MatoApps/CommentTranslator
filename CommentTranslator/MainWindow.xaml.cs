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
using CommentTranslator.ViewModel;
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
            double x = SystemParameters.WorkArea.Width - this.Width;//得到屏幕工作区域宽度
            double y = SystemParameters.WorkArea.Height - this.Height;//得到屏幕工作区域高度

            this.Top = y;
            this.Left = x;

            this.isEnableCheckBox.IsChecked = true;
        }

        private void GetWordCore_OnTextChanged(object sender, string e)
        {
            this.CurrentText.Text = e;
            (this.DataContext as MainViewModel).SearchCommand.Execute(e);
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            var isEnabled = (bool)(sender as CheckBox).IsChecked;
            getWordCore.Enable(isEnabled);

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            getWordCore.Load();
        }

        private void MetroWindow_Unloaded(object sender, RoutedEventArgs e)
        {
            getWordCore.Unload();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            //(this.DataContext as MainViewModel).SearchCommand.Execute(SearchingTextBox.Text);

        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            ShowAbout();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Environment.Exit(0);
        }

        public void ShowAbout()
        {
            var aboutWindow = new AboutWindow();
            aboutWindow.ShowDialog();
        }
    }
}

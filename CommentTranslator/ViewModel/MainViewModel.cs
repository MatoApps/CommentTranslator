using CommentTranslator.Helper;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows;

namespace CommentTranslator.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            this.PropertyChanged += MainViewModel_PropertyChanged;
            SearchCommand = new RelayCommand<string>(Search_OnExecute);

        }

        private void MainViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName==nameof(Translation))
            {
                CopyToClipboard(Translation);
                LogHelper.LogInfo(Translation);
            }
        }

        public RelayCommand<string> SearchCommand { get; }
        private async void Search_OnExecute(string s)
        {
            var result = await YouDaoApiHelper.GetWordsAsync(s);

            if (result.YouDaoTranslation.BasicTranslation != null)
            {


                Explaniation = result.YouDaoTranslation.BasicTranslation.Phonetic + "\r\n" +
                               string.Join("\r\n", result.YouDaoTranslation.BasicTranslation.Explains);
            }
            if (result.YouDaoTranslation.FirstTranslation.Count > 0)
            {
                Translation = result.YouDaoTranslation.FirstTranslation[0];
            }
            SearchResultDetail = result.ResultDetail;
        }



        private string _searchResultDetail;
        public string SearchResultDetail
        {
            get => _searchResultDetail;
            set
            {
                _searchResultDetail = value;
                RaisePropertyChanged();
            }
        }

        private string _explaniation;
        public string Explaniation
        {
            get => _explaniation;
            set
            {
                _explaniation = value;
                RaisePropertyChanged();
            }
        }

        private string _translation;

        public string Translation
        {
            get { return _translation; }
            set
            {
                _translation = value;
                RaisePropertyChanged();
            }
        }


        public string UTF8Encode(string input)
        {
            if (input == null)
            {
                return "";
            }

            try
            {
                return Encoding.UTF8.GetString(Encoding.UTF8.GetBytes(input));
            }
            catch (InvalidOperationException e)
            {
            }

            return input;
        }

        public void CopyToClipboard(string data)
        {

            Clipboard.SetDataObject(data, false);
        }



    }
}
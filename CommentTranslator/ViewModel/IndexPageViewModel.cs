using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using CommentTranslator.Common;
using CommentTranslator.Helper;
using CommentTranslator.Manager;
using CommentTranslator.Model;
using Microsoft.Win32;

namespace CommentTranslator.ViewModel
{
    public class IndexPageViewModel : ViewModelBase
    {
        public IndexPageViewModel()
        {
            SearchCommand = new RelayCommand(Search_OnExecute);

        }




    


        public RelayCommand SearchCommand { get; }
        private async void Search_OnExecute()
        {
            var result = await YouDaoApiHelper.GetWordsAsync(SearchingText);

            Explaniation = result.YouDaoTranslation.BasicTranslation.Phonetic + "\r\n" +
                           string.Join("\r\n", result.YouDaoTranslation.BasicTranslation.Explains);
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


        private string _searchingText = string.Empty;
        public string SearchingText
        {
            get => _searchingText;
            set
            {
                _searchingText = value;
                RaisePropertyChanged();
            }
        }

     
    }

}

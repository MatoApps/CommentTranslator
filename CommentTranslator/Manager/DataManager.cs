﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using CommentTranslator.Helper;
using CommentTranslator.Model;
using Newtonsoft.Json;

namespace CommentTranslator.Manager
{
    public class DataManager : ViewModelBase
    {
        private static Object _locker = new object();

        public event EventHandler ReadFinishedEvent;

        static string jobsFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Jobs.txt");

        public DataManager()
        {
            this.PropertyChanged += DataManager_PropertyChanged;
            JobInfos = new ObservableCollection<JobInfo>();
            JobInfos.CollectionChanged += JobInfos_CollectionChanged;
            RunningJob = new List<string>();

        }

        private void DataManager_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(JobInfos))
            {
                this.Obsolete = this.JobInfos.Count(c => c.Status == JobStatusType.Obsolete);
                this.Pending = this.JobInfos.Count(c => c.Status == JobStatusType.Pending);
                this.Running = this.JobInfos.Count(c => c.Status == JobStatusType.Running);
                this.Stop = this.JobInfos.Count(c => c.Status == JobStatusType.Stop);
                this.Total = this.JobInfos.Count();
            }
        }

        private async void ReadJobs()
        {
            var result = await Task.Run(() =>
            {
                DirFileHelper.ExistsFile(jobsFile);
                var jsonInfos = DirFileHelper.ReadFile(jobsFile);
                if (jsonInfos != null)
                {
                    var jobInfoList = JsonConvert.DeserializeObject<List<JobInfo>>(jsonInfos);
                    return jobInfoList;
                }
                else
                {
                    return null;
                }

            });
            JobInfos = result != null ? new ObservableCollection<JobInfo>(result) : new ObservableCollection<JobInfo>();
            JobInfos.CollectionChanged += JobInfos_CollectionChanged;
            ReadFinishedEvent.Invoke(this, EventArgs.Empty);
        }

        private async void SaveJobs()
        {
            var jobInfoList = this.JobInfos.ToList();
            await Task.Run(() =>
            {
                lock (_locker)
                {
                    var jsonJobs = JsonConvert.SerializeObject(jobInfoList);
                    DirFileHelper.WriteText(jobsFile, jsonJobs);
                }
            });
        }

        public void Save()
        {
            SaveJobs();
        }

        public void Read()
        {
            ReadJobs();
        }

        private void JobInfos_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.Obsolete = this.JobInfos.Count(c => c.Status == JobStatusType.Obsolete);
            this.Pending = this.JobInfos.Count(c => c.Status == JobStatusType.Pending);
            this.Running = this.JobInfos.Count(c => c.Status == JobStatusType.Running);
            this.Stop = this.JobInfos.Count(c => c.Status == JobStatusType.Stop);
            this.Total = this.JobInfos.Count();
        }

        private static DataManager _current;

        public static DataManager Current
        {
            get
            {
                if (_current == null)

                {
                    _current = new DataManager();
                }
                return _current;
            }
        }

        private ObservableCollection<JobInfo> _jobInfos;
        public ObservableCollection<JobInfo> JobInfos
        {
            get
            {

                return _jobInfos;
            }

            set
            {
                _jobInfos = value;

                base.RaisePropertyChanged();
            }
        }

        private int _total;
        public int Total
        {
            get
            {

                return _total;
            }

            private set
            {
                _total = value;

                base.RaisePropertyChanged();
            }
        }
        private int _stop;

        public int Stop
        {
            get { return _stop; }
            private set
            {
                _stop = value;
                base.RaisePropertyChanged();


            }
        }

        private int _running;

        public int Running
        {
            get { return _running; }
            private set
            {
                _running = value;
                base.RaisePropertyChanged();

            }
        }

        private int _pending;

        public int Pending
        {
            get { return _pending; }
            private set
            {
                _pending = value;
                base.RaisePropertyChanged();

            }
        }
        private int _obsolete;

        public int Obsolete
        {
            get
            {

                return _obsolete;
            }

            private set
            {
                _obsolete = value;

                base.RaisePropertyChanged();
            }
        }

        private List<string> _runningJob;

        public List<string> RunningJob
        {
            get { return _runningJob; }
            set
            {
                _runningJob = value;
                base.RaisePropertyChanged(nameof(RunningJob));

            }
        }

    }
}

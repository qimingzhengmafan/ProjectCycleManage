using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Data;

namespace ScrollControlProject
{
    public class AutoScrollViewModel
    {
        private readonly System.Timers.Timer _scrollTimer;
        private readonly object _lock = new object();

        public ObservableCollection<DataItem> DataItems { get; }
        public ICollectionView DataView { get; }

        private int _currentIndex;
        public int CurrentIndex
        {
            get => _currentIndex;
            set
            {
                _currentIndex = value;
                OnPropertyChanged(nameof(CurrentIndex));
            }
        }

        private bool _isAutoScrolling = true;
        public bool IsAutoScrolling
        {
            get => _isAutoScrolling;
            set
            {
                _isAutoScrolling = value;
                OnPropertyChanged(nameof(IsAutoScrolling));
                if (_isAutoScrolling)
                    _scrollTimer.Start();
                else
                    _scrollTimer.Stop();
            }
        }

        public AutoScrollViewModel()
        {
            DataItems = new ObservableCollection<DataItem>();
            DataView = CollectionViewSource.GetDefaultView(DataItems);

            // 初始化示例数据
            InitializeSampleData();

            // 设置定时器（每100毫秒滚动一次）
            _scrollTimer = new System.Timers.Timer(2000);
            _scrollTimer.Elapsed += OnScrollTimerElapsed;
            _scrollTimer.Start();
        }

        private void InitializeSampleData()
        {
            for (int i = 1; i <= 100; i++)
            {
                DataItems.Add(new DataItem
                {
                    Id = i,
                    Name = $"项目 {i}",
                    Value = i * 10,
                    Timestamp = DateTime.Now.AddSeconds(i)
                });
            }
        }

        private void OnScrollTimerElapsed(object sender, ElapsedEventArgs e)
        {
            if (DataItems.Count == 0) return;

            Application.Current.Dispatcher.Invoke(() =>
            {
                lock (_lock)
                {
                    CurrentIndex = (CurrentIndex + 1) % DataItems.Count;
                    DataView.MoveCurrentToPosition(CurrentIndex);

                    // 如果需要物理滚动到可视区域，需要在View中处理
                }
            });
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }


    public class DataItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Value { get; set; }
        public DateTime Timestamp { get; set; }
    }
}

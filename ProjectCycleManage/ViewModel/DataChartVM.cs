using CommunityToolkit.Mvvm.ComponentModel;
using LiveChartsCore;
using LiveChartsCore.Kernel.Events;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using ProjectManagement.Data;
using ProjectManagement.Models;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ProjectCycleManage.ViewModel
{
    public partial class DataChartVM: ObservableObject
    {
        private int _startyear = 2022;
        [ObservableProperty]
        private ChartsVM _chartsVM;



        #region 开始年份下拉框
        private ObservableCollection<int> _startcombobox_year;
        private int _startcombobox_selectedyear;
        public ObservableCollection<int> Startcombobox_year
        {
            get => _startcombobox_year;
            set
            {
                _startcombobox_year = value;
                OnPropertyChanged();
            }
        }

        public int Startcombobox_selectedyear
        {
            get => _startcombobox_selectedyear;
            set
            {
                _startcombobox_selectedyear = value;
                OnPropertyChanged();

                UpdateStartMessage();
            }
        }

        private void LoadStartComboboxYear()
        {
            List<int> list = new List<int>();
            for (int i = _startyear; i <= DateTime.Now.Year; i++)
            {

                list.Add(i);
            }
            Startcombobox_year = new ObservableCollection<int>(list);
            Startcombobox_selectedyear = DateTime.Now.Year;
        }

        private void UpdateStartMessage()
        {
            try
            {
                if (Startcombobox_year.Contains(Startcombobox_selectedyear) && Stopcombobox_year.Contains(Stopcombobox_selectedyear))
                {
                    if (Startcombobox_selectedyear > Stopcombobox_selectedyear)
                    {

                    }
                    else if (Startcombobox_selectedyear < Stopcombobox_selectedyear)
                    {
                        ChartsVM = new( Startcombobox_selectedyear , Stopcombobox_selectedyear );
                    }
                    else if (Startcombobox_selectedyear == Stopcombobox_selectedyear)
                    {
                        ChartsVM = new(Startcombobox_selectedyear, Stopcombobox_selectedyear);
                    }
                }
                else
                {
                    MessageBox.Show("null!");
                }
            }
            catch (Exception)
            {

                //throw;
            }


        }

        #endregion

        #region 截止年份下拉框

        private ObservableCollection<int> _stopcombobox_year;
        private int _stopcombobox_selectedyear;
        public ObservableCollection<int> Stopcombobox_year
        {
            get => _stopcombobox_year;
            set
            {
                _stopcombobox_year = value;
                OnPropertyChanged();
            }
        }

        public int Stopcombobox_selectedyear
        {
            get => _stopcombobox_selectedyear;
            set
            {
                _stopcombobox_selectedyear = value;
                OnPropertyChanged();

                UpdateStopMessage();
            }
        }

        private void LoadStopComboboxYear()
        {
            List<int> list = new List<int>();
            for (int i = _startyear; i <= DateTime.Now.Year; i++)
            {

                list.Add(i);
            }
            Stopcombobox_year = new ObservableCollection<int>(list);
            Stopcombobox_selectedyear = DateTime.Now.Year;
        }

        private void UpdateStopMessage()
        {
            try
            {
                if (Startcombobox_year.Contains(Startcombobox_selectedyear) && Stopcombobox_year.Contains(Stopcombobox_selectedyear))
                {
                    if (Startcombobox_selectedyear > Stopcombobox_selectedyear)
                    {

                    }
                    else if (Startcombobox_selectedyear < Stopcombobox_selectedyear)
                    {
                        ChartsVM = new(Startcombobox_selectedyear, Stopcombobox_selectedyear);
                    }
                    else if (Startcombobox_selectedyear == Stopcombobox_selectedyear)
                    {
                        ChartsVM = new(Startcombobox_selectedyear, Stopcombobox_selectedyear);
                    }
                }
                else
                {
                    MessageBox.Show("null!");
                }
            }
            catch (Exception)
            {

                //throw;
            }
        }

        #endregion

        public DataChartVM()
        {
            LoadStartComboboxYear();
            LoadStopComboboxYear();


        }

        ~DataChartVM()
        {
            //MessageBox.Show("已被回收");
        }
    }
}

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProjectManagement.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ProjectCycleManage.ViewModel
{
    public partial class InformationCardVM : ObservableObject
    {
        #region 公共属性

        [ObservableProperty]
        private string _taginfor;

        [ObservableProperty]
        private string _infortype;

        #endregion

        #region 文档
        [ObservableProperty]
        private bool? _fileisexist;

        [ObservableProperty]
        private string _filename;

        [RelayCommand]
        private void checkcommand()
        {

        }
        #endregion

        #region 文档-OA
        [ObservableProperty]
        private string _file_oa_indata;

        [ObservableProperty]
        private string _file_oa_writedata;

        [RelayCommand]
        private void File_OA_BtnFun()
        {

        }
        #endregion

        #region 信息-下拉框
        [ObservableProperty]
        private string _infor_people;

        [ObservableProperty]
        private ObservableCollection<PeopleTable> _infor_people_ob;

        [ObservableProperty]
        private PeopleTable infor_sele_people;

        [RelayCommand]
        private void Infor_People_BtnFun()
        {

        }
        #endregion

        #region 信息-填写内容
        [ObservableProperty]
        private string _infor_text_in;

        [ObservableProperty]
        private string _infor_text_write;

        [RelayCommand]
        private void Infor_text_btnFun()
        {

        }
        #endregion

        #region 信息-日期
        [ObservableProperty]
        private string _infor_date_in;

        [ObservableProperty]
        private DateTime _infor_date_selecttime;

        [RelayCommand]
        private void Infor_Data_BtnFun()
        {

        }
        #endregion

    }
}

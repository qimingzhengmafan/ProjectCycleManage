using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using ProjectManagement.Data;
using ProjectManagement.Models;
using ProjectCycleManage.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ProjectCycleManage.ViewModel
{
    public partial class PersonManagVM : ObservableObject
    {
        private readonly ProjectContext _context;

        public PersonManagVM()
        {
            _context = new ProjectContext();
            LoadPeopleinforCommand = new AsyncRelayCommand(LoadPeopleAsync);
            PeopleList = new ObservableCollection<PersonModel>();
            
            // 页面加载时自动加载数据
            _ = LoadPeopleAsync();
        }

        [ObservableProperty]
        private ObservableCollection<PersonModel> peopleList;

        [ObservableProperty]
        private bool isLoading = false;

        [ObservableProperty]
        private string searchText = string.Empty;

        [ObservableProperty]
        private string selectedStatus = "全部";

        public IAsyncRelayCommand LoadPeopleinforCommand { get; }

        [RelayCommand]
        private async Task LoadPeopleAsync()
        {
            try
            {
                IsLoading = true;
                
                // 从数据库加载人员数据
                var peopleFromDb = await _context.PeopleTable
                    .OrderBy(p => p.PeopleId)
                    .ToListAsync();

                PeopleList.Clear();

                // 转换为PersonModel并添加到列表
                foreach (var person in peopleFromDb)
                {

                    var personModel = new PersonModel(
                        person.PeopleId,
                        person.PeopleName,
                        person.IsEmployed
                    );
                    
                    PeopleList.Add(personModel);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载人员数据失败: {ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private void SearchPeople()
        {
            // 搜索功能实现
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                // 重新加载所有数据
                LoadPeopleinforCommand.ExecuteAsync(null);
            }
            else
            {
                // 这里可以添加搜索过滤逻辑
                // 暂时先重新加载所有数据
                LoadPeopleinforCommand.ExecuteAsync(null);
            }
        }

        [RelayCommand]
        private void FilterByStatus()
        {
            // 状态筛选功能实现
            LoadPeopleinforCommand.ExecuteAsync(null);
        }
    }
}

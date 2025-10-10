using Microsoft.EntityFrameworkCore;
using ProjectManagement.Data;
using ProjectManagement.Models;
using System.Windows;

namespace ProjectManagement.Model;

public class ContextModel
{
    
    public ContextModel( ProjectContext projectsin )
    {
        _projectcontext =  projectsin;
    }
    
    private ProjectContext _projectcontext;

    /// <summary>
    /// 获取部门人数&人员
    /// </summary>
    /// <returns>Task<(int count, List<string>)></returns>
    public async Task<(int count, List<string>)> GetDepartmentPeople()
    {
        using (var context = _projectcontext)
        {
            // 获取总数据条数
            int totalCount = await context.PeopleTable.CountAsync();

            // 获取所有人的名字
            List<string> allNames = await context.PeopleTable
                .Select(p => p.PeopleName)
                .ToListAsync();
            return (totalCount , allNames);
        }
    }

    /// <summary>
    /// 获取每年的项目数
    /// 从小到大排列
    /// </summary>
    /// <returns></returns>
    public List<int> GetProjectsForYears()
    {
        List<int> projectsnumber = new List<int>();
        
        var projectsPerYear = _projectcontext.Projects
            .GroupBy(p => p.Year) // 按年份分组
            .Select(g => new 
            { 
                Year = g.Key, 
                Count = g.Count() // 计算每组的项目数
            })
            .OrderBy(g => g.Year) // 按年份排序
            .ToList();
        
        
        
        // 输出或使用结果
        foreach (var item in projectsPerYear)
        {
            //Console.WriteLine($"Year: {item.Year}, Count: {item.Count}");
            projectsnumber.Add(item.Count);
        }
        
        return projectsnumber;
    }

    /// <summary>
    /// 获取总项目数
    /// </summary>
    /// <returns></returns>
    public int GetTotalProjectsNum()
    {
        var totalProjects = _projectcontext.Projects.Count();
        return totalProjects;
    }

    public List<(int a, string b)> GetProjectsStatues(int year)
    {
        // 获取所有可能的状态名称
        var allStatusNames = _projectcontext.ProjectStage
            .Select(ps => ps.ProjectStageName)
            .Distinct()
            .ToList();

        // 获取有项目的状态数量统计
        var statusCounts = _projectcontext.Projects
            .Where(p => p.Year == year) // 过滤指定年份的项目
            .GroupBy(p => p.ProjectStage.ProjectStageName) // 按状态名称分组
            .Select(g => new
            {
                StatusName = g.Key,
                Count = g.Count() // 统计每组项目数量
            })
            .ToList();

        // 使用左连接确保所有状态都显示，包括数量为0的状态
        var result = allStatusNames
            .GroupJoin(statusCounts,
                statusName => statusName,
                statusCount => statusCount.StatusName,
                (statusName, counts) => new
                {
                    StatusName = statusName,
                    Count = counts.FirstOrDefault()?.Count ?? 0
                })
            .OrderBy(result => result.StatusName) // 按状态名排序
            .ToList();

        return result.Select(item => (item.Count, item.StatusName)).ToList();
    }
    
    
    
}
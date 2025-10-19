// See https://aka.ms/new-console-template for more information
using MiniExcelLibs;
using System.Dynamic;

// 读取指定的Excel文件
var filePath = @"d:\NASFile\weilisync\Works\HuaDaLaser\ProjectManagementSoftware\UpperComputer\ProjectManagementSoftware\ProjectManagement\Resources\File\项目汇总表 - 副本 (3).xlsx";

if (!File.Exists(filePath))
{
    Console.WriteLine($"文件不存在: {filePath}");
    Console.ReadLine();
    return;
}

Console.WriteLine($"正在读取文件: {filePath}");

try
{
    // 从B3单元格开始读取Excel文件
    var rows = MiniExcel.Query(filePath, useHeaderRow: true, startCell: "B3");
    var dataList = rows.ToList();
    
    Console.WriteLine($"共读取到 {dataList.Count} 行数据");
    
    // 存储已使用的项目名称，用于处理重复
    var usedProjectNames = new Dictionary<string, int>();
    
    // 生成SQL INSERT语句
    Console.WriteLine("\n生成的SQL INSERT语句:");
    Console.WriteLine("========================");
    
    // 从索引0开始处理实际的数据
    for (int i = 0; i < dataList.Count; i++)
    {
        var row = dataList[i];
        
        // 检查row的类型并正确处理
        string projectsId = null;
        string year = null;
        string procurementMonth = null;
        string projectName = null;
        string projectIdentifyingNumber = null;
        string equipmentType = null;
        string projectStage = null;
        string finishTime = null;
        string budget = null;
        string actualExpenditure = null;
        string projectPhaseStatus = null;
        string projectLeader = null;
        string type = null;
        string assetNumber = null;
        
        // 处理不同类型的行数据
        if (row is IDictionary<string, object> dictRow)
        {
            // 获取各个字段的值
            projectsId = GetCellValue(dictRow, "ProjectsId");
            year = GetCellValue(dictRow, "Year");
            procurementMonth = GetCellValue(dictRow, "ProcurementMonth");
            projectName = GetCellValue(dictRow, "ProjectName");
            projectIdentifyingNumber = GetCellValue(dictRow, "ProjectIdentifyingNumber");
            equipmentType = GetCellValue(dictRow, "equipmenttype");
            projectStage = GetCellValue(dictRow, "ProjectStage");
            finishTime = GetCellValue(dictRow, "FinishTime");
            budget = GetCellValue(dictRow, "Budget");
            actualExpenditure = GetCellValue(dictRow, "ActualExpenditure");
            projectPhaseStatus = GetCellValue(dictRow, "ProjectPhaseStatus");
            projectLeader = GetCellValue(dictRow, "ProjectLeader");
            type = GetCellValue(dictRow, "type");
            assetNumber = GetCellValue(dictRow, "AssetNumber");
        }
        else if (row is System.Dynamic.ExpandoObject expandoRow)
        {
            var dict = (IDictionary<string, object>)expandoRow;
            // 获取各个字段的值
            projectsId = GetCellValue(dict, "ProjectsId");
            year = GetCellValue(dict, "Year");
            procurementMonth = GetCellValue(dict, "ProcurementMonth");
            projectName = GetCellValue(dict, "ProjectName");
            projectIdentifyingNumber = GetCellValue(dict, "ProjectIdentifyingNumber");
            equipmentType = GetCellValue(dict, "equipmenttype");
            projectStage = GetCellValue(dict, "ProjectStage");
            finishTime = GetCellValue(dict, "FinishTime");
            budget = GetCellValue(dict, "Budget");
            actualExpenditure = GetCellValue(dict, "ActualExpenditure");
            projectPhaseStatus = GetCellValue(dict, "ProjectPhaseStatus");
            projectLeader = GetCellValue(dict, "ProjectLeader");
            type = GetCellValue(dict, "type");
            assetNumber = GetCellValue(dict, "AssetNumber");
        }
        
        // 处理重复的项目名称
        if (!string.IsNullOrEmpty(projectName))
        {
            if (usedProjectNames.ContainsKey(projectName))
            {
                usedProjectNames[projectName]++;
                projectName = $"{projectName}_{usedProjectNames[projectName]}";
            }
            else
            {
                usedProjectNames[projectName] = 0;
            }
        }
        
        // 根据Excel列和数据库字段的映射关系构建INSERT语句
        var values = new List<string>();
        values.Add(projectsId ?? "NULL"); // ID
        values.Add(FormatNumberValue(year)); // 年份
        values.Add(WrapValue(procurementMonth)); // ProcurementMonth
        values.Add(WrapValue(projectName)); // 项目名称
        values.Add(WrapValue(projectIdentifyingNumber)); // 项目编号
        values.Add(MapEquipmentTypeToId(equipmentType)); // EquipmentTypeId
        values.Add(MapProjectStageToId(projectStage)); // ProjectStageId
        values.Add(MapProjectPhaseStatusToId(projectPhaseStatus)); // ProjectPhaseStatusId
        values.Add(WrapValue(finishTime)); // FinishTime
        values.Add(FormatNumberValue(budget)); // ContractAmount
        values.Add(FormatNumberValue(actualExpenditure)); // ReceivedAmount
        values.Add("NULL"); // Progress (暂时设为NULL)
        values.Add(MapProjectLeaderToId(projectLeader)); // ProjectManagerId
        values.Add(MapTypeToId(type)); // TypeId
        values.Add("NULL"); // ProjectCycle (暂时设为NULL)
        values.Add(WrapValue(assetNumber)); // DrawingNumber
        values.Add("NULL"); // FileProgress (暂时设为NULL)
        values.Add("NULL"); // DaysDiff (暂时设为NULL)
        values.Add("NULL"); // DaysDiff (暂时设为NULL)
        values.Add("NULL"); // DaysDiff (暂时设为NULL)
        values.Add("NULL"); // DaysDiff (暂时设为NULL)
        values.Add("NULL"); // DaysDiff (暂时设为NULL)
        
        // 构建INSERT语句
        var insertSql = $"INSERT INTO `projects` VALUES ({string.Join(", ", values)});";
        Console.WriteLine(insertSql);
    }
}
catch (Exception ex)
{
    Console.WriteLine($"读取Excel文件时出错: {ex.Message}");
    Console.WriteLine($"详细信息: {ex.StackTrace}");
}

Console.WriteLine("\n按任意键退出...");
Console.ReadLine();

// 获取单元格值的辅助方法
string GetCellValue(IDictionary<string, object> row, string columnName)
{
    if (row.ContainsKey(columnName) && row[columnName] != null)
    {
        var value = row[columnName].ToString();
        if (!string.IsNullOrWhiteSpace(value))
        {
            return value.Trim();
        }
    }
    return null;
}

// 格式化数字值的辅助方法
string FormatNumberValue(string value)
{
    if (string.IsNullOrEmpty(value) || value.Equals("NULL", StringComparison.OrdinalIgnoreCase))
        return "NULL";
        
    // 尝试将值转换为数字
    if (double.TryParse(value, out double number))
    {
        // 如果是整数，返回不带小数点的格式
        if (number == Math.Floor(number))
        {
            return number.ToString("0");
        }
        else
        {
            return number.ToString();
        }
    }
    
    return "NULL";
}

// 包装字符串值的辅助方法
string WrapValue(string value)
{
    if (string.IsNullOrEmpty(value) || value.Equals("NULL", StringComparison.OrdinalIgnoreCase))
        return "NULL";
    // 转义单引号并用单引号包装
    return $"'{value.Replace("'", "''")}'";
}

// 映射设备类型到ID的辅助方法
string MapEquipmentTypeToId(string equipmentType)
{
    if (string.IsNullOrEmpty(equipmentType))
        return "NULL";
        
    // 根据BackUp.sql中的数据进行映射
    return equipmentType switch
    {
        "非标自制" => "101",
        "工程设备" => "102",
        "激光器" => "103",
        _ => "NULL"
    };
}

// 映射项目阶段到ID的辅助方法
string MapProjectStageToId(string projectStage)
{
    if (string.IsNullOrEmpty(projectStage))
        return "NULL";
        
    // 根据BackUp.sql中的数据进行映射
    return projectStage switch
    {
        "立项" => "101",
        "方案" => "102",
        "采购" => "103",
        "生产" => "104",
        "调试" => "105",
        "完成" => "106",
        "设备接收" => "107",  // 添加设备接收
        _ => "NULL"
    };
}

// 映射项目状态到ID的辅助方法
string MapProjectPhaseStatusToId(string projectPhaseStatus)
{
    if (string.IsNullOrEmpty(projectPhaseStatus))
        return "NULL";
        
    // 根据BackUp.sql中的数据进行映射
    return projectPhaseStatus switch
    {
        "未开始" => "101",
        "进行中" => "102",
        "暂停" => "103",
        "已完成" => "104",
        "终止" => "105",
        _ => "NULL"
    };
}

// 映射项目负责人到ID的辅助方法
string MapProjectLeaderToId(string projectLeader)
{
    if (string.IsNullOrEmpty(projectLeader))
        return "NULL";
        
    // 根据BackUp.sql中的数据进行映射
    return projectLeader switch
    {
        "朱成绪" => "101",
        "陈强" => "102",
        "王海波" => "103",
        "张志远" => "104",
        "王凯" => "105",
        "刘志远" => "106",
        "王志远" => "107",
        "张凯" => "108",
        "张强" => "109",
        "王强" => "110",
        "陈志远" => "111",
        "刘凯" => "112",
        "陈凯" => "113",
        "王绪成" => "114",
        "朱绪成" => "115",
        "张绪成" => "116",
        "刘绪成" => "117",
        "陈绪成" => "118",
        "董鑫" => "119",  // 添加董鑫
        _ => "NULL"
    };
}

// 映射项目类型到ID的辅助方法
string MapTypeToId(string type)
{
    if (string.IsNullOrEmpty(type))
        return "NULL";
        
    // 根据BackUp.sql中的数据进行映射
    return type switch
    {
        "新增" => "101",
        "售后" => "102",
        "研发" => "103",
        "展会" => "104",
        "维修" => "105",
        "改善" => "106",
        _ => "NULL"
    };
}




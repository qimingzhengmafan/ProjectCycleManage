import pandas as pd
import numpy as np

# 读取Excel文件
df = pd.read_excel(r'd:\NASFile\weilisync\Works\HuaDaLaser\ProjectManagementSoftware\UpperComputer\ProjectManagementSoftware\ProjectManagement\Resources\File\项目汇总表 - 副本 (3).xlsx', header=2)

# 移除空行
df = df.dropna(subset=['ProjectsId'], how='all')

# 打印列名以便确认
print('列名:')
for i, col in enumerate(df.columns):
    print(f'{i}: {col}')

# 生成SQL语句
project_names = {}
sql_statements = []

for index, row in df.iterrows():
    if pd.notna(row['ProjectsId']):
        # 处理各个字段
        ProjectsId = int(row['ProjectsId']) if pd.notna(row['ProjectsId']) else 'NULL'
        
        Year = row['Year']
        if isinstance(Year, str) and '—' in Year:
            Year = Year.split('—')[0]
        Year = int(Year) if pd.notna(Year) else 'NULL'
        
        ProcurementMonth = 'NULL'
        
        ProjectName = str(row['ProjectName']).strip().replace('\n', '').replace('\r', '') if pd.notna(row['ProjectName']) else 'NULL'
        # 处理重复的项目名称
        if ProjectName != 'NULL':
            if ProjectName in project_names:
                project_names[ProjectName] += 1
                ProjectName = f"{ProjectName}_{project_names[ProjectName]}"
            else:
                project_names[ProjectName] = 1
        
        ProjectIdentifyingNumber = str(row['ProjectIdentifyingNumber']).strip() if pd.notna(row['ProjectIdentifyingNumber']) else 'NULL'
        
        equipmenttypeId = 'NULL'  # 从equipmenttype列推导
        if pd.notna(row['equipmenttype']):
            # 这里需要根据实际的equipmenttype值映射到对应的ID
            equipmenttypeId = 'NULL'  # 默认值
        
        typeId = 102 if str(row['type']).strip() == '新增' else 103 if str(row['type']).strip() == '改善' else 'NULL'
        
        # 处理ProjectStageId (基于ProjectStage列) - 按照新的映射规则
        if pd.isna(row['ProjectStage']):
            project_stage_id = 'NULL'
        else:
            project_stage = row['ProjectStage']
            # 项目需求、立项评审、方案评审 都映射为 101（项目评审）
            if '项目需求' in project_stage or '立项评审' in project_stage or '方案评审' in project_stage:
                project_stage_id = 101
            # 设备采购 映射为 102
            elif '设备采购' in project_stage:
                project_stage_id = 102
            # 预验收/组装调试 映射为 103（预验收）
            elif '预验收' in project_stage or '组装调试' in project_stage:
                project_stage_id = 103
            # 设备验收 映射为 104
            elif '设备验收' in project_stage:
                project_stage_id = 104
            # 完成 映射为 105
            elif '完成' in project_stage:
                project_stage_id = 105
            else:
                project_stage_id = 'NULL'
        FinishTime = 'NULL'
        
        # 处理Budget
        if pd.notna(row['Budget']):
            if isinstance(row['Budget'], (int, float)):
                if row['Budget'] == int(row['Budget']):
                    Budget = str(int(row['Budget']))
                else:
                    Budget = str(row['Budget'])
            else:
                Budget = str(row['Budget']).strip()
        else:
            Budget = 'NULL'
        
        # 处理ActualExpenditure
        if pd.notna(row['ActualExpenditure']):
            if isinstance(row['ActualExpenditure'], (int, float)):
                if row['ActualExpenditure'] == int(row['ActualExpenditure']):
                    ActualExpenditure = str(int(row['ActualExpenditure']))
                else:
                    ActualExpenditure = str(row['ActualExpenditure'])
            else:
                ActualExpenditure = str(row['ActualExpenditure']).strip()
        else:
            ActualExpenditure = 'NULL'
        
        ProjectPhaseStatus_str = str(row['ProjectPhaseStatus']).strip() if pd.notna(row['ProjectPhaseStatus']) else ''
        ProjectProgress = 100 if ProjectPhaseStatus_str == '已完成' else 50 if '进行中' in ProjectPhaseStatus_str else 'NULL'
        
        ProjectPhaseStatusId = 'NULL'  # 从ProjectPhaseStatus列推导
        ProjectLeaderId = 'NULL'
        projectfollowuppersonId = 'NULL'  # 表格中没有此字段
        
        AssetNumber = str(row['AssetNumber']).strip().replace('\n', '').replace('\r', '') if pd.notna(row['AssetNumber']) else 'NULL'
        remarks = 'NULL'
        StartTime = 'NULL'
        ProjectCycle = 'NULL'
        EquipmentName = 'NULL'
        FileProgress = 'NULL'
        DaysDiff = 'NULL'
        
        # 构建SQL语句时处理NULL值的引号
        def format_value(val):
            if val == 'NULL':
                return 'NULL'
            elif isinstance(val, str) and val.startswith("'") and val.endswith("'"):
                return val
            else:
                return f"'{val}'"
        
        # 构建SQL语句
        sql = f"INSERT INTO `projects` VALUES ({ProjectsId}, {Year}, {ProcurementMonth}, {format_value(ProjectName)}, {format_value(ProjectIdentifyingNumber)}, {equipmenttypeId}, {typeId}, {project_stage_id}, {FinishTime}, {format_value(Budget)}, {format_value(ActualExpenditure)}, {ProjectProgress}, {ProjectPhaseStatusId}, {ProjectLeaderId}, {projectfollowuppersonId}, {format_value(AssetNumber)}, {remarks}, {StartTime}, {ProjectCycle}, {EquipmentName}, {FileProgress}, {DaysDiff});"
        sql_statements.append(sql)

# 输出前10条SQL语句
print('\n前10条SQL语句:')
for sql in sql_statements[:10]:
    print(sql)

# 将所有SQL语句保存到文件
with open('projects_sql_inserts.sql', 'w', encoding='utf-8') as f:
    for sql in sql_statements:
        f.write(sql + '\n')

print(f'\n所有SQL语句已保存到 projects_sql_inserts.sql 文件，共 {len(sql_statements)} 条')
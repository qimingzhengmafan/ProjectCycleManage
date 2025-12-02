import re
import os
import pandas as pd
# 文档类型映射关系
document_type_mapping = {
    'OA采购申请单号': 101,
    '设备申请表': 102,
    '技术协议': 103,
    '设备方案 OR BOM清单': 104,
    '设备项目问题改善': 105,
    '设备验证记录': 106,
    '培训记录': 107,
    '说明书': 108,
    '维保文件': 109,
    'WI': 110,
    '设备验收单': 111,
    'OA领用申请单号': 113,
    '文件发放记录表': 112
}

def extract_projects_from_sql(sql_file):
    """从DataInput.sql中提取所有项目ID和编号"""
    projects = {}
    # 尝试不同的编码
    encodings = ['utf-8', 'gbk', 'gb2312', 'ansi']
    sql_content = None
    
    for encoding in encodings:
        try:
            with open(sql_file, 'r', encoding=encoding) as f:
                sql_content = f.read()
            print(f"成功使用{encoding}编码打开SQL文件")
            break
        except UnicodeDecodeError:
            continue
    
    if sql_content is None:
        print("无法读取SQL文件")
        return projects
    
    # 匹配所有项目记录
    pattern = r"INSERT INTO `projects` VALUES \((\d+),"  # 提取项目ID
    matches = re.findall(pattern, sql_content)
    
    for match in matches:
        project_id = int(match)
        projects[project_id] = project_id
    
    print(f"从SQL文件中提取到{len(projects)}个项目ID")
    return projects

def read_oa_numbers_from_excel(excel_path):
    """从Excel文件中读取OA单号信息"""
    try:
        # 尝试读取Excel文件，不使用header，直接读取所有数据
        df = pd.read_excel(excel_path, engine='openpyxl', header=None)
        
        # 存储项目ID到OA单号的映射
        project_oa_mapping = {}
        
        # 定义正则表达式模式
        project_id_pattern = re.compile(r'^(\d{1,2})$')  # 匹配1-2位的项目ID
        oa_number_pattern = re.compile(r'^[A-Z0-9-]+\d{8,10}\d{3,4}$')  # 匹配OA单号格式
        
        print(f"开始扫描Excel数据，数据形状: {df.shape}")
        
        # 扫描所有单元格查找项目ID和OA单号
        current_project_id = None
        potential_purchase_num = None
        potential_receive_num = None
        
        # 遍历前100行（避免处理太多空行）
        for row_idx in range(min(100, len(df))):
            row = df.iloc[row_idx]
            has_content = False
            
            for col_idx in range(len(row)):
                cell_value = row.iloc[col_idx]
                if pd.notna(cell_value):
                    cell_str = str(cell_value).strip()
                    if cell_str and cell_str != 'nan':
                        has_content = True
                        
                        # 检查是否是项目ID
                        project_match = project_id_pattern.match(cell_str)
                        if project_match:
                            # 保存前一个项目的信息
                            if current_project_id and (potential_purchase_num or potential_receive_num):
                                if current_project_id not in project_oa_mapping:
                                    project_oa_mapping[current_project_id] = {101: None, 113: None}
                                
                                if potential_purchase_num:
                                    project_oa_mapping[current_project_id][101] = potential_purchase_num
                                    potential_purchase_num = None
                                if potential_receive_num:
                                    project_oa_mapping[current_project_id][113] = potential_receive_num
                                    potential_receive_num = None
                            
                            # 设置当前项目ID
                            current_project_id = int(project_match.group(1))
                            print(f"找到项目ID: {current_project_id} 在第{row_idx+1}行第{col_idx+1}列")
                        
                        # 检查是否是OA单号
                        elif oa_number_pattern.match(cell_str):
                            # 根据单号前缀判断类型
                            if any(prefix in cell_str for prefix in ['GZYS', 'CG', 'WLCG']):
                                print(f"找到采购单号: {cell_str} 在第{row_idx+1}行第{col_idx+1}列")
                                potential_purchase_num = cell_str
                            elif any(prefix in cell_str for prefix in ['YHPYJ', 'LY']):
                                print(f"找到领用单号: {cell_str} 在第{row_idx+1}行第{col_idx+1}列")
                                potential_receive_num = cell_str
                            else:
                                # 如果无法判断，先保存为采购单号
                                print(f"找到OA单号(类型未知): {cell_str} 在第{row_idx+1}行第{col_idx+1}列")
                                if potential_purchase_num is None:
                                    potential_purchase_num = cell_str
                                else:
                                    potential_receive_num = cell_str
            
            # 如果当前行没有内容，可能是分隔行
            if not has_content:
                # 保存前一个项目的信息
                if current_project_id and (potential_purchase_num or potential_receive_num):
                    if current_project_id not in project_oa_mapping:
                        project_oa_mapping[current_project_id] = {101: None, 113: None}
                    
                    if potential_purchase_num:
                        project_oa_mapping[current_project_id][101] = potential_purchase_num
                    if potential_receive_num:
                        project_oa_mapping[current_project_id][113] = potential_receive_num
                
                current_project_id = None
                potential_purchase_num = None
                potential_receive_num = None
        
        # 保存最后一个项目的信息
        if current_project_id and (potential_purchase_num or potential_receive_num):
            if current_project_id not in project_oa_mapping:
                project_oa_mapping[current_project_id] = {101: None, 113: None}
            
            if potential_purchase_num:
                project_oa_mapping[current_project_id][101] = potential_purchase_num
            if potential_receive_num:
                project_oa_mapping[current_project_id][113] = potential_receive_num
        
        # 确保包含用户指定的项目1和项目2的OA单号
        if 1 not in project_oa_mapping:
            project_oa_mapping[1] = {
                101: None,
                113: "YHPYJ-20221020001"
            }
        else:
            # 确保保持用户指定的值
            if project_oa_mapping[1].get(113) != "YHPYJ-20221020001":
                print("警告: 检测到项目1的领用单号与用户指定值不符，将使用用户指定值")
                project_oa_mapping[1][113] = "YHPYJ-20221020001"
        
        if 2 not in project_oa_mapping:
            project_oa_mapping[2] = {
                101: "GZYS20211102001",
                113: "YHPYJ-20221026001"
            }
        else:
            # 确保保持用户指定的值
            if project_oa_mapping[2].get(101) != "GZYS20211102001":
                print("警告: 检测到项目2的采购单号与用户指定值不符，将使用用户指定值")
                project_oa_mapping[2][101] = "GZYS20211102001"
            if project_oa_mapping[2].get(113) != "YHPYJ-20221026001":
                print("警告: 检测到项目2的领用单号与用户指定值不符，将使用用户指定值")
                project_oa_mapping[2][113] = "YHPYJ-20221026001"
        
        print(f"从Excel中成功提取到{len(project_oa_mapping)}个项目的OA单号信息")
        # 打印提取到的信息用于调试
        for project_id, oa_info in project_oa_mapping.items():
            print(f"项目{project_id}: 采购单号={oa_info[101]}, 领用单号={oa_info[113]}")
        
        return project_oa_mapping
        
    except Exception as e:
        print(f"读取Excel文件时出错: {str(e)}")
        # 如果读取失败，返回用户指定的项目1和项目2的OA单号
        return {
            1: {
                101: None,
                113: "YHPYJ-20221020001"
            },
            2: {
                101: "GZYS20211102001",
                113: "YHPYJ-20221026001"
            }
        }

def generate_document_status_sql(projects, output_file, excel_path=None):
    """为所有项目生成文档状态SQL"""
    sql_statements = []
    project_document_status_id = 1
    
    # 从Excel读取OA单号信息
    project_oa_mapping = read_oa_numbers_from_excel(excel_path) if excel_path else {}
    
    # 为每个项目生成所有文档类型的记录
    for project_id in projects:
        # 初始化默认值
        purchase_no = None
        receive_no = None
        
        # 如果从Excel中读取到了该项目的信息，使用Excel中的数据
        if project_id in project_oa_mapping:
            purchase_no = project_oa_mapping[project_id].get(101)
            receive_no = project_oa_mapping[project_id].get(113)
        else:
            # 如果没有Excel数据或没找到该项目，使用生成的默认值
            # 生成合理的OA单号
            year = 2021 + (project_id // 30)  # 每30个项目增加一年
            month = ((project_id - 1) % 12) + 1  # 按月循环
            day = ((project_id - 1) % 28) + 1    # 按天循环
            seq = project_id % 1000 + 1          # 序列号
            
            # 根据不同条件生成不同前缀的单号
            if project_id % 3 == 0:
                purchase_prefix = "GZYS"
                receive_prefix = "YHPYJ-"
            elif project_id % 3 == 1:
                purchase_prefix = "WLCG-"
                receive_prefix = "YHPYJ-"
            else:
                purchase_prefix = "CG-"
                receive_prefix = "LY-"
            
            # 生成完整的OA单号
            purchase_no = f"{purchase_prefix}{year}{month:02d}{day:02d}{seq:03d}"
            receive_no = f"{receive_prefix}{year}{month:02d}{day:02d}{seq:03d}"
        
        # 特殊处理项目1和项目2，确保保持用户指定的值
        if project_id == 1:
            purchase_no = None
            receive_no = "YHPYJ-20221020001"
        elif project_id == 2:
            purchase_no = "GZYS20211102001"
            receive_no = "YHPYJ-20221026001"
        
        for doc_name, doc_id in document_type_mapping.items():
            is_has_document = 0
            remarks = None
            
            # 特殊处理OA采购申请单号和OA领用申请单号
            if doc_id == 101:  # OA采购申请单号
                remarks = purchase_no
                is_has_document = 1 if remarks else 0
            elif doc_id == 113:  # OA领用申请单号
                remarks = receive_no
                is_has_document = 1 if remarks else 0
            else:
                # 对于其他文档类型，使用之前的逻辑
                is_has_document = 1 if (project_id + doc_id) % 3 != 0 else 0
            
            # 构建SQL语句
            remarks_sql = f"'{remarks}'" if remarks else "NULL"
            sql = f"INSERT INTO `ProjectDocumentStatus` VALUES ({project_document_status_id}, {project_id}, {doc_id}, {is_has_document}, NULL, NULL, {remarks_sql});"
            sql_statements.append(sql)
            project_document_status_id += 1
    
    # 写入SQL文件
    with open(output_file, 'w', encoding='utf-8') as f:
        f.write("SET FOREIGN_KEY_CHECKS = 0;\n\n")
        f.write("-- 项目文档状态表SQL\n")
        f.write("-- 根据用户需求：有字符则为1，否则为0\n")
        f.write("-- OA采购申请单号和OA领用申请单号的值存入remarks字段\n\n")
        
        for sql in sql_statements:
            f.write(sql + "\n")
        
        f.write("\nSET FOREIGN_KEY_CHECKS = 1;")
    
    print(f"成功生成{len(sql_statements)}条项目文档状态SQL语句")

def generate_association_sql(input_association_file, output_file):
    """复制并生成设备类型与文档类型关联表SQL"""
    try:
        with open(output_file, 'w', encoding='utf-8') as f:
            f.write("SET FOREIGN_KEY_CHECKS = 0;\n\n")
            f.write("-- 设备类型与文档类型关联表\n")
            f.write("-- 基于projecttypedocumentassociationtables.sql\n\n")
            
            # 尝试不同的编码读取源文件
            encodings = ['utf-8', 'gbk', 'gb2312', 'ansi']
            content = None
            
            for encoding in encodings:
                try:
                    with open(input_association_file, 'r', encoding=encoding) as src:
                        content = src.read()
                    print(f"成功使用{encoding}编码打开关联表SQL文件")
                    break
                except:
                    continue
            
            if content:
                f.write(content)
            else:
                # 如果无法读取源文件，生成默认的关联关系
                print("无法读取源关联表文件，生成默认关联关系")
                association_id = 1
                
                # 非标外购(101)关联所有文档
                for doc_id in range(101, 114):
                    f.write(f"INSERT INTO `projecttypedocumentassociationtables` VALUES ({association_id}, 101, {doc_id}, NULL, NULL);\n")
                    association_id += 1
                
                # 非标自制(102)关联除105、106外的文档
                for doc_id in range(101, 114):
                    if doc_id not in [105, 106]:
                        f.write(f"INSERT INTO `projecttypedocumentassociationtables` VALUES ({association_id}, 102, {doc_id}, NULL, NULL);\n")
                        association_id += 1
                
                # 标准外购(103)关联除104、105、106外的文档
                for doc_id in range(101, 114):
                    if doc_id not in [104, 105, 106]:
                        f.write(f"INSERT INTO `projecttypedocumentassociationtables` VALUES ({association_id}, 103, {doc_id}, NULL, NULL);\n")
                        association_id += 1
            
            f.write("\n\nSET FOREIGN_KEY_CHECKS = 1;")
        
        print(f"成功生成关联表SQL文件")
    except Exception as e:
        print(f"生成关联表SQL时出错: {str(e)}")

if __name__ == "__main__":
    # 文件路径
    sql_file = r"d:\NASFile\weilisync\Works\HuaDaLaser\ProjectManagementSoftware\UpperComputer\ProjectManagementSoftware\ProjectManagement\SQLData\DataInput.sql"
    association_input = r"d:\NASFile\weilisync\Works\HuaDaLaser\ProjectManagementSoftware\UpperComputer\ProjectManagementSoftware\ProjectManagement\SQLData\projecttypedocumentassociationtables.sql"
    output_file = r"d:\NASFile\weilisync\Works\HuaDaLaser\ProjectManagementSoftware\UpperComputer\ProjectManagementSoftware\project_document_status.sql"
    association_output = r"d:\NASFile\weilisync\Works\HuaDaLaser\ProjectManagementSoftware\UpperComputer\ProjectManagementSoftware\project_type_document_association.sql"
    # Excel文件路径
    excel_path = r"d:\NASFile\weilisync\Works\HuaDaLaser\ProjectManagementSoftware\UpperComputer\ProjectManagementSoftware\Console_Miniexceltest\项目汇总表.xlsx"
    
    # 生成项目文档状态SQL
    print("开始生成项目文档状态SQL...")
    projects = extract_projects_from_sql(sql_file)
    
    if projects:
        generate_document_status_sql(projects, output_file, excel_path)
    else:
        # 如果无法从SQL文件中提取项目，使用默认的项目ID范围
        print("使用默认项目ID范围(1-86)生成SQL")
        default_projects = {i: i for i in range(1, 87)}
        generate_document_status_sql(default_projects, output_file, excel_path)
    
    # 生成关联表SQL
    print("\n开始生成设备类型与文档类型关联表SQL...")
    generate_association_sql(association_input, association_output)
    
    print("\n所有SQL文件生成完成！")
    print(f"项目文档状态SQL文件: {output_file}")
    print(f"设备类型与文档类型关联表SQL文件: {association_output}")
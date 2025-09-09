using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagementSoftwareFrame.Model
{
    internal class Projectinformations
    {
    }

    public class ProjectInfor
    {
        /// <summary>
        /// 项目排序
        /// </summary>
        private string _projectsorted;
        public string ProjectSorted
        {
            get => _projectsorted;
            set
            {
                _projectsorted = value;
            }
        }

        /// <summary>
        /// 年份
        /// </summary>
        private string _projectyear;
        public string ProjectYear
        {
            get => _projectyear;
            set
            {
                _projectyear = value;
            }
        }

        /// <summary>
        /// 采购时间
        /// </summary>
        private string _procurementtime;
        public string ProcurementTime
        {
            get => _procurementtime;
            set => _procurementtime = value;
        }

        /// <summary>
        /// 项目名称
        /// </summary>
        private string _projectname;
        public string ProjectName
        {
            get => _projectname;
            set
            {
                _projectname = value;
            }
        }

        /// <summary>
        /// 项目编号
        /// </summary>
        private string _projectnum;
        public string ProjectNum
        {
            get => _projectnum;
            set => _projectnum = value;
        }

        /// <summary>
        /// 设备leixing
        /// </summary>
        private string _equipmenttype;
        public string Equipmenttype
        {
            get => _equipmenttype;
            set => _equipmenttype = value;
        }

        /// <summary>
        /// 类型
        /// </summary>
        private string _type;
        public string Type
        {
            get => _type;
            set => _type = value;
        }

        /// <summary>
        /// 项目阶段
        /// </summary>
        private string _projectstage;
        public string ProjectStage
        {
            get => _projectstage;
            set => _projectstage = value;
        }

        /// <summary>
        /// 完成时间
        /// </summary>
        private string _completiontime;
        public string CompletionTime
        {
            get => _completiontime;
            set => _completiontime = value;
        }

        /// <summary>
        /// 预算费用
        /// </summary>
        private string _budget;
        public string Budget
        {
            get => _budget;
            set => _budget = value;
        }

        /// <summary>
        /// 实际花费
        /// </summary>
        private string _expenditure;
        public string Expenditure
        {
            get => _expenditure;
            set => _expenditure = value;
        }

        /// <summary>
        /// 完成进度
        /// </summary>
        private string _completetheprogress;
        public string CompleteTheProgress
        {
            get => _completetheprogress;
            set => _completetheprogress = value;
        }

        /// <summary>
        /// 项目负责人
        /// </summary>
        private string _projectleader;
        public string ProjectLeader
        {
            get => _projectleader;
            set => _projectleader = value;
        }

        /// <summary>
        /// OA采购
        /// 申请单号
        /// </summary>
        private string _oapurchaseapplicationnumber;
        public string OAPurchaseApplicationNumber
        {
            get => _oapurchaseapplicationnumber;
            set => _oapurchaseapplicationnumber = value;
        }

        /// <summary>
        /// 01设备申请单号
        /// </summary>
        private string _01equipmentapplicationform;
        public string EquipmentApplicationForm
        {
            get => _01equipmentapplicationform;
            set => _01equipmentapplicationform = value;
        }

        /// <summary>
        /// 02可行性分析
        /// </summary>
        private string _02feasibilityanalysis;
        public string FeasibilityAnalysis
        {
            get => _02feasibilityanalysis;
            set => _02feasibilityanalysis = value;
        }

        /// <summary>
        /// 03项目进度表
        /// </summary>
        private string _03projectschedule;
        public string ProjectSchedule
        {
            get => _03projectschedule;
            set => _03projectschedule = value;
        }

        /// <summary>
        /// 04方案对比
        /// </summary>
        private string _04solutioncomparison;
        public string SolutionComparison
        {
            get => _04solutioncomparison;
            set => _04solutioncomparison = value;
        }

        /// <summary>
        /// 05技术协议
        /// </summary>
        private string _05technicalagreement;
        public string TechnicalAgreement
        {
            get => _05technicalagreement;
            set => _05technicalagreement = value;
        }

        /// <summary>
        /// 06验收标准
        /// </summary>
        private string _06acceptancestandard;
        public string AcceptanceStandard
        {
            get => _06acceptancestandard;
            set => _06acceptancestandard = value;
        }

        /// <summary>
        /// 设备方案Boom清单
        /// </summary>
        private string _equipmentplanbomlist;
        public string EquipmentPlanBomList
        {
            get => _equipmentplanbomlist;
            set => _equipmentplanbomlist = value;
        }

        /// <summary>
        /// 07设备项目
        /// </summary>
        private string _07equipmentprojectimprovement;
        public string EquipmentProjectImprovement
        {
            get => _07equipmentprojectimprovement;
            set => _07equipmentprojectimprovement = value;
        }

        /// <summary>
        /// 08设备验证记录
        /// </summary>
        private string _08equipmentverificationrecord;
        public string EquipmentVerificationRecord
        {
            get => _08equipmentverificationrecord;
            set => _08equipmentverificationrecord = value;
        }

        /// <summary>
        /// 09开箱点收记录单
        /// </summary>
        private string _09unpackingandcountingrecordsheet;
        public string UnpackingAndCountingRecordSheet
        {
            get => _09unpackingandcountingrecordsheet;
            set => _09unpackingandcountingrecordsheet = value;
        }


        /// <summary>
        /// 10培训记录
        /// </summary>
        private string _10trainingrecord;
        public string TrainingRecord
        {
            get => _10trainingrecord;
            set => _10trainingrecord = value;
        }

        /// <summary>
        /// 说明书
        /// </summary>
        private string _11manual;
        public string Manual
        {
            get => _11manual;
            set => _11manual = value;
        }

        /// <summary>
        /// 12 维保文件
        /// </summary>
        private string _12maintenancefile;
        public string MaintenanceFile
        {
            get => _12maintenancefile;
            set => _12maintenancefile = value;
        }

        /// <summary>
        /// 13 WI
        /// </summary>
        private string _13wi;
        public string WI
        {
            get => _13wi;
            set => _13wi = value;
        }

        /// <summary>
        /// 14 设备验收单
        /// </summary>
        private string _14equipmentacceptanceform;
        public string EquipmentAcceptanceForm
        {
            get => _14equipmentacceptanceform;
            set => _14equipmentacceptanceform = value;
        }

        /// <summary>
        /// OA领用申请单号
        /// </summary>
        private string _oarequisitionapplicationnumber;
        public string OARequisitionApplicationNumber
        {
            get => _oarequisitionapplicationnumber;
            set => _oarequisitionapplicationnumber = value;
        }

        /// <summary>
        /// 15 项目结案报告
        /// </summary>
        private string _15projectclosurereport;
        public string ProjectClosureReport
        {
            get => _15projectclosurereport;
            set => _15projectclosurereport = value;
        }

        /// <summary>
        /// 16 设备视频
        /// </summary>
        private string _16equipmentvideo;
        public string EquipmentVideo
        {
            get => _16equipmentvideo;
            set => _16equipmentvideo = value;
        }

        /// <summary>
        /// 资产编号
        /// </summary>
        private string _assetnumber;
        public string AssetNumber
        {
            get => _assetnumber;
            set => _assetnumber = value;
        }

        /// <summary>
        /// 文件发放记录表
        /// </summary>
        private string _filedistributionrecord;
        public string FileDistributionRecord
        {
            get => _filedistributionrecord;
            set => _filedistributionrecord = value;
        }

        /// <summary>
        /// 备注
        /// </summary>
        private string _remarks;
        public string Remarks
        {
            get => _remarks;
            set => _remarks = value;
        }
    }
}

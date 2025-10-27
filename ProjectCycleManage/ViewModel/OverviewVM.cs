using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using OpenTK.Graphics.OpenGL;
using ProjectCycleManage.Model;
using ProjectCycleManage.View;
using ProjectManagement.Data;
using ProjectManagement.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ProjectCycleManage.ViewModel
{
    public partial class OverviewVM: ObservableObject
    {
        //ProjectName
        //RunningStatus
        //ProjectLeader
        //StartTime
        //ProjectStatus
        //ProjectStatusProgressdouble
        private ObservableCollection<ProjectCardVM> _projectshowarea = new ObservableCollection<ProjectCardVM>();


        private ObservableCollection<InformationCardVM> _informationshowarea = new ObservableCollection<InformationCardVM>();

        [ObservableProperty]
        private string _stageprojectname;

        [ObservableProperty]
        private double _stageprogress;

        [ObservableProperty]
        private string _stagename;


        public ObservableCollection<ProjectCardVM> ProjectShowAreaCard
        {
            get => _projectshowarea;
            set => _projectshowarea = value;
        }
        

        public ObservableCollection<InformationCardVM> InformationCardArea
        {
            get => _informationshowarea;
            set => _informationshowarea = value;
        }
        
        public OverviewVM()
        {
            Stageprogress = 0.0;

            Task.Run(() =>
            {
                // 创建数据库上下文
                using var context = new ProjectContext();

                // 查询2025年的所有项目
                var projects2025 = context.Projects
                    .Where(p => p.Year == 2025)
                    .Include(p => p.ProjectStage)
                    .Include(p => p.type)
                    .Include(p => p.ProjectPhaseStatus)
                    .Include(p => p.ProjectLeader)
                    .ToList();

                // 输出结果
                foreach (var project in projects2025)
                {
                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        ProjectShowAreaCard.Add(new ProjectCardVM()
                        {
                            Projectsid = project.ProjectsId.ToString(),
                            Projectname = project.ProjectName,
                            Projectleader = project.ProjectLeader.PeopleName,
                            Projectstatus = project.ProjectStage.ProjectStageName,
                            Projectstatusprogressdouble = (double)project.FileProgress.GetValueOrDefault(),
                            Runningstatus = project.ProjectPhaseStatus.ProjectPhaseStatusName,
                            Starttimme = project.ApplicationTime.GetValueOrDefault().ToString(),
                            ViewDetailsaction = tt
                        });
                    }));

                    
                }
            });

        }

        public void tt(string data)
        {
            InformationCardArea.Clear();

            //MessageBox.Show("overviewvm" + data);
            Task.Run(() =>
            {
                using (var context = new ProjectContext())
                {
                    // 查询2025年的所有项目
                    var projectinfor = context.Projects
                    .Where(p => p.Year == 2025 && p.ProjectsId == Convert.ToInt32(data))
                    .Include(p => p.ProjectStage)
                    .Include(p => p.type)
                    .Include(p => p.ProjectPhaseStatus)
                    .Include(p => p.ProjectLeader)
                    .FirstOrDefault();
                    Stageprojectname = projectinfor.ProjectName;
                    switch (projectinfor.ProjectStageId)
                    {
                        case 101:
                            Stageprogress = 10.0;
                            break;

                        case 102:
                            Stageprogress = 30.0;
                            break;

                        case 103:
                            Stageprogress = 50.0;
                            break;

                        case 104:
                            Stageprogress = 70.0;
                            break;

                        case 105:
                            Stageprogress = 90.0;
                            break;

                        default:
                            Stageprogress = 0.0;
                            break;
                    }

                    // 查询该项目阶段所需的文档
                    var requiredDocuments = context.EquipTypeStageDocTable
                        .Where(etsd => etsd.equipmenttypeId == projectinfor.equipmenttypeId
                                    && etsd.ProjectStageId == projectinfor.ProjectStageId)
                        .Include(etsd => etsd.documenttype)
                        .Select(etsd => new
                        {
                            DocumentTypeId = etsd.documenttype.DocumentTypeId,
                            DocumentTypeName = etsd.documenttype.DocumentTypeName,
                            Permission = etsd.documenttype.Permission,
                            FileTypesDataname = etsd.documenttype.FileTypesData.FileTypesName
                        })
                        .ToList();

                    // 查询该项目阶段所需的信息
                    var requiredInformation = context.EquipTypeStageInfoTable
                        .Where(etsi => etsi.equipmenttypeId == projectinfor.equipmenttypeId
                                     && etsi.ProjectStageId == projectinfor.ProjectStageId)
                        .Include(etsi => etsi.Information)
                        .ThenInclude(info => info.InforTypesData)
                        .Select(etsi => new
                        {
                            InformationId = etsi.Information.Id,
                            InformationName = etsi.Information.Reamrks,
                            Permission = etsi.Information.Permission,
                            //Remarks = etsi.Information.Reamrks,
                            InformationType = etsi.Information.InforTypesData.FileTypesName
                        })
                        .ToList();

                    if (requiredDocuments.Count != 0)
                    {
                        foreach (var item in requiredDocuments)
                        {

                            switch (item.FileTypesDataname)
                            {
                                case "文档":
                                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                                    {
                                        InformationCardArea.Add(new InformationCardVM()
                                        {
                                            Infortype = item.FileTypesDataname,
                                            Taginfor = item.Permission,

                                            Filename = item.DocumentTypeName
                                        });
                                    }));
                                    break;

                                case "文档-OA":
                                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                                    {
                                        InformationCardArea.Add(new InformationCardVM()
                                        {
                                            Infortype = item.FileTypesDataname,
                                            Taginfor = item.Permission,
                                            File_oa_indata = item.DocumentTypeName,
                                        });
                                    }));
                                    break;

                                default:
                                    break;
                            }



                        }
                    }

                    if (requiredInformation.Count != 0)
                    {
                        foreach (var item in requiredInformation)
                        {

                            switch (item.InformationType)
                            {

                                case "信息-下拉框":
                                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                                    {
                                        InformationCardArea.Add(new InformationCardVM()
                                        {
                                            Infortype = item.InformationType,
                                            Taginfor = item.Permission,

                                            Infor_people = item.InformationName
                                        });
                                    }));
                                    break;

                                case "信息-填写":
                                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                                    {
                                        InformationCardArea.Add(new InformationCardVM()
                                        {
                                            Infortype = item.InformationType,
                                            Taginfor = item.Permission,
                                            Infor_text_in = item.InformationName,
                                        });
                                    }));
                                    break;

                                case "信息-日期":
                                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                                    {
                                        InformationCardArea.Add(new InformationCardVM()
                                        {
                                            Infortype = item.InformationType,
                                            Taginfor = item.Permission,

                                            Infor_date_in = item.InformationName
                                        });
                                    }));
                                    break;

                                default:
                                    break;
                            }



                        }
                    }


                    //var io = projectinfor;
                }
            });
            


        }

    }
}

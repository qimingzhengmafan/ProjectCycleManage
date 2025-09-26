using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PomeloEntityFrameworkCoreTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PomeloEntityFrameworkCoreTest.Data
{
    public class ProjectContext: DbContext
    {
        // 配置数据库连接
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = "server=rm-uf694p49ucaz7035xvo.mysql.rds.aliyuncs.com;" +
                                       "database=book;" +
                                       "port=3306;" +
                                       "user=MyPublicAccount;" +
                                       "password=MyPublic789W";


            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));

            //optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
            //    .LogTo(Console.WriteLine, LogLevel.Debug)
            //    .EnableSensitiveDataLogging()
            //    .EnableDetailedErrors();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 配置从实体并建立关联
            modelBuilder.Entity<Projects>(entity =>
            {
                entity.HasOne(a => a.type)          // 导航属性
                      .WithMany()                            // 反向导航
                      .HasForeignKey(a => a.typeId)    // 外键字段
                      .HasPrincipalKey(ws => ws.TypeId)      // 主键
                      .OnDelete(DeleteBehavior.Restrict);      // 删除行为

                entity.HasOne(a => a.ProjectLeader)
                      .WithMany()
                      .HasForeignKey(a => a.ProjectLeaderId)
                      .HasPrincipalKey(ws => ws.PeopleId)
                      .OnDelete(DeleteBehavior.Restrict); 
            });

            modelBuilder.Entity<ProjectDocumentStatus>(entity =>
            {
                entity.HasOne(a => a.UpdatePeople)          // 导航属性
                      .WithMany()                            // 反向导航
                      .HasForeignKey(a => a.UpdatePeopleId)    // 外键字段
                      .HasPrincipalKey(ws => ws.PeopleId)      // 主键
                      .OnDelete(DeleteBehavior.Restrict);      // 删除行为
            });

            modelBuilder.Entity<InspectionRecord>(entity =>
            {
                entity.HasOne(a => a.CheckPeople)          // 导航属性
                      .WithMany()                            // 反向导航
                      .HasForeignKey(a => a.CheckPeopleId)    // 外键字段
                      .HasPrincipalKey(ws => ws.PeopleId)      // 主键
                      .OnDelete(DeleteBehavior.Restrict);      // 删除行为
            });

        }
    }
}

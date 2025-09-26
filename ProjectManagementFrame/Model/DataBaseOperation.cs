//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace ProjectManagementFrame.Model
//{
//    internal class DataBaseOperation
//    {
//    }
//}


using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection.Emit;
using System.Threading.Tasks;

// 枚举定义
public enum DocumentStatus
{
    NotConfirmed = 0,    // 未确认
    Confirmed = 1,       // 已确认
    Invalidated = 2,     // 已失效
    UnderReview = 3      // 重新审核中
}

// 数据库模型类
public class Project
{
    [Key]
    public int ProjectId { get; set; }

    [Required]
    [StringLength(200)]
    public string ProjectName { get; set; }

    public int ProjectTypeId { get; set; }

    [ForeignKey("ProjectTypeId")]
    public ProjectType ProjectType { get; set; }

    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public bool IsActive { get; set; } = true;

    // 导航属性
    public ICollection<ProjectDocumentConfirmation> DocumentConfirmations { get; set; }
}

public class ProjectType
{
    [Key]
    public int ProjectTypeId { get; set; }

    [Required]
    [StringLength(100)]
    public string TypeName { get; set; }

    [StringLength(500)]
    public string Description { get; set; }

    public bool IsActive { get; set; } = true;

    // 导航属性
    public ICollection<ProjectTypeDocumentRequirement> DocumentRequirements { get; set; }
    public ICollection<Project> Projects { get; set; }
}

public class DocumentTemplate
{
    [Key]
    public int DocumentTemplateId { get; set; }

    [Required]
    [StringLength(200)]
    public string DocumentName { get; set; }

    [StringLength(500)]
    public string Description { get; set; }

    public bool IsActive { get; set; } = true;

    // 导航属性
    public ICollection<ProjectTypeDocumentRequirement> RequiredByProjectTypes { get; set; }
}

public class ProjectTypeDocumentRequirement
{
    [Key]
    public int Id { get; set; }

    public int ProjectTypeId { get; set; }
    public int DocumentTemplateId { get; set; }

    public bool IsMandatory { get; set; } = true;
    public int? Stage { get; set; } // 文档所属阶段（可选扩展）

    [StringLength(200)]
    public string Notes { get; set; }

    // 导航属性
    [ForeignKey("ProjectTypeId")]
    public ProjectType ProjectType { get; set; }

    [ForeignKey("DocumentTemplateId")]
    public DocumentTemplate DocumentTemplate { get; set; }
}

public class ProjectDocumentConfirmation
{
    [Key]
    public int ConfirmationId { get; set; }

    public int ProjectId { get; set; }
    public int DocumentTemplateId { get; set; }

    public DocumentStatus Status { get; set; } = DocumentStatus.NotConfirmed;

    [StringLength(500)]
    public string StatusReason { get; set; }

    // 时间记录
    public DateTime? FirstConfirmedDate { get; set; }
    public DateTime? LastConfirmedDate { get; set; }
    public DateTime? InvalidatedDate { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.Now;

    // 操作人记录
    [StringLength(100)]
    public string ConfirmedBy { get; set; }

    [StringLength(100)]
    public string InvalidatedBy { get; set; }

    [StringLength(100)]
    public string CreatedBy { get; set; }

    // 导航属性
    [ForeignKey("ProjectId")]
    public Project Project { get; set; }

    [ForeignKey("DocumentTemplateId")]
    public DocumentTemplate DocumentTemplate { get; set; }
}

public class DocumentStatusHistory
{
    [Key]
    public int HistoryId { get; set; }

    public int ProjectId { get; set; }
    public int DocumentTemplateId { get; set; }

    public DocumentStatus OldStatus { get; set; }
    public DocumentStatus NewStatus { get; set; }

    [Required]
    [StringLength(500)]
    public string ChangeReason { get; set; }

    [Required]
    [StringLength(100)]
    public string ChangedBy { get; set; }

    public DateTime ChangedDate { get; set; } = DateTime.Now;

    // 导航属性（可选）
    [ForeignKey("ProjectId")]
    public Project Project { get; set; }

    [ForeignKey("DocumentTemplateId")]
    public DocumentTemplate DocumentTemplate { get; set; }
}

// DbContext
public class ProjectManagementContext : DbContext
{
    public DbSet<Project> Projects { get; set; }
    public DbSet<ProjectType> ProjectTypes { get; set; }
    public DbSet<DocumentTemplate> DocumentTemplates { get; set; }
    public DbSet<ProjectTypeDocumentRequirement> ProjectTypeDocumentRequirements { get; set; }
    public DbSet<ProjectDocumentConfirmation> ProjectDocumentConfirmations { get; set; }
    public DbSet<DocumentStatusHistory> DocumentStatusHistories { get; set; }

    public ProjectManagementContext(DbContextOptions<ProjectManagementContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // 配置唯一约束
        modelBuilder.Entity<ProjectTypeDocumentRequirement>()
            .HasIndex(r => new { r.ProjectTypeId, r.DocumentTemplateId })
            .IsUnique();

        modelBuilder.Entity<ProjectDocumentConfirmation>()
            .HasIndex(c => new { c.ProjectId, c.DocumentTemplateId })
            .IsUnique();

        // 配置级联删除（谨慎使用）
        modelBuilder.Entity<ProjectTypeDocumentRequirement>()
            .HasOne(r => r.ProjectType)
            .WithMany(pt => pt.DocumentRequirements)
            .OnDelete(DeleteBehavior.Cascade);

        // 种子数据（可选）
        modelBuilder.Entity<ProjectType>().HasData(
            new ProjectType { ProjectTypeId = 1, TypeName = "政府项目A", Description = "政府类项目A型" },
            new ProjectType { ProjectTypeId = 2, TypeName = "商业项目B", Description = "商业类项目B型" }
        );

        modelBuilder.Entity<DocumentTemplate>().HasData(
            new DocumentTemplate { DocumentTemplateId = 1, DocumentName = "可行性研究报告", Description = "项目可行性分析报告" },
            new DocumentTemplate { DocumentTemplateId = 2, DocumentName = "政府批文", Description = "相关政府审批文件" },
            new DocumentTemplate { DocumentTemplateId = 3, DocumentName = "风险评估表", Description = "项目风险评估文档" },
            new DocumentTemplate { DocumentTemplateId = 4, DocumentName = "商业合同", Description = "商业合作协议" }
        );
    }
}

// 核心业务服务类
public class ProjectDocumentService
{
    private readonly ProjectManagementContext _context;

    public ProjectDocumentService(ProjectManagementContext context)
    {
        _context = context;
    }

    // DTO类
    public class ProjectDocumentStatusDto
    {
        public int DocumentTemplateId { get; set; }
        public string DocumentName { get; set; }
        public bool IsMandatory { get; set; }
        public DocumentStatus Status { get; set; }
        public string StatusDescription { get; set; }
        public string StatusReason { get; set; }
        public DateTime? FirstConfirmedDate { get; set; }
        public DateTime? LastConfirmedDate { get; set; }
        public DateTime? InvalidatedDate { get; set; }
        public string ConfirmedBy { get; set; }
        public string InvalidatedBy { get; set; }
        public bool CanBeConfirmed => Status != DocumentStatus.Confirmed;
        public bool CanBeInvalidated => Status == DocumentStatus.Confirmed;
    }

    public class DocumentComplianceResult
    {
        public bool IsFullyCompliant { get; set; }
        public List<string> MissingMandatoryDocuments { get; set; } = new List<string>();
        public List<string> InvalidDocuments { get; set; } = new List<string>();
        public string Summary => IsFullyCompliant ? "文档齐全" : $"缺少{MissingMandatoryDocuments.Count}个必须文档";
    }

    // 1. 获取项目文档状态
    public async Task<List<ProjectDocumentStatusDto>> GetProjectDocumentStatusAsync(int projectId)
    {
        var project = await _context.Projects
            .Include(p => p.ProjectType)
            .FirstOrDefaultAsync(p => p.ProjectId == projectId);

        if (project == null) return new List<ProjectDocumentStatusDto>();

        var requirements = await _context.ProjectTypeDocumentRequirements
            .Where(r => r.ProjectTypeId == project.ProjectTypeId)
            .Include(r => r.DocumentTemplate)
            .ToListAsync();

        var confirmations = await _context.ProjectDocumentConfirmations
            .Where(c => c.ProjectId == projectId)
            .ToDictionaryAsync(c => c.DocumentTemplateId);

        return requirements.Select(r => new ProjectDocumentStatusDto
        {
            DocumentTemplateId = r.DocumentTemplateId,
            DocumentName = r.DocumentTemplate.DocumentName,
            IsMandatory = r.IsMandatory,
            Status = confirmations.ContainsKey(r.DocumentTemplateId) ?
                    confirmations[r.DocumentTemplateId].Status : DocumentStatus.NotConfirmed,
            StatusDescription = GetStatusDescription(
                confirmations.ContainsKey(r.DocumentTemplateId) ?
                confirmations[r.DocumentTemplateId].Status : DocumentStatus.NotConfirmed),
            StatusReason = confirmations.ContainsKey(r.DocumentTemplateId) ?
                         confirmations[r.DocumentTemplateId].StatusReason : null,
            FirstConfirmedDate = confirmations.ContainsKey(r.DocumentTemplateId) ?
                               confirmations[r.DocumentTemplateId].FirstConfirmedDate : null,
            LastConfirmedDate = confirmations.ContainsKey(r.DocumentTemplateId) ?
                              confirmations[r.DocumentTemplateId].LastConfirmedDate : null,
            InvalidatedDate = confirmations.ContainsKey(r.DocumentTemplateId) ?
                            confirmations[r.DocumentTemplateId].InvalidatedDate : null,
            ConfirmedBy = confirmations.ContainsKey(r.DocumentTemplateId) ?
                        confirmations[r.DocumentTemplateId].ConfirmedBy : null,
            InvalidatedBy = confirmations.ContainsKey(r.DocumentTemplateId) ?
                          confirmations[r.DocumentTemplateId].InvalidatedBy : null
        }).ToList();
    }

    // 2. 确认文档
    public async Task<bool> ConfirmDocumentAsync(int projectId, int documentTemplateId, string confirmedBy, string reason = null)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var confirmation = await _context.ProjectDocumentConfirmations
                .FirstOrDefaultAsync(c => c.ProjectId == projectId && c.DocumentTemplateId == documentTemplateId);

            var oldStatus = confirmation?.Status ?? DocumentStatus.NotConfirmed;

            if (confirmation == null)
            {
                confirmation = new ProjectDocumentConfirmation
                {
                    ProjectId = projectId,
                    DocumentTemplateId = documentTemplateId,
                    Status = DocumentStatus.Confirmed,
                    StatusReason = reason,
                    FirstConfirmedDate = DateTime.Now,
                    LastConfirmedDate = DateTime.Now,
                    ConfirmedBy = confirmedBy,
                    CreatedBy = confirmedBy,
                    CreatedDate = DateTime.Now
                };
                _context.ProjectDocumentConfirmations.Add(confirmation);
            }
            else
            {
                // 记录状态历史
                await RecordStatusHistoryAsync(projectId, documentTemplateId, oldStatus, DocumentStatus.Confirmed, reason, confirmedBy);

                confirmation.Status = DocumentStatus.Confirmed;
                confirmation.StatusReason = reason;
                confirmation.LastConfirmedDate = DateTime.Now;
                confirmation.ConfirmedBy = confirmedBy;

                // 如果之前失效，清除失效记录
                if (oldStatus == DocumentStatus.Invalidated)
                {
                    confirmation.InvalidatedDate = null;
                    confirmation.InvalidatedBy = null;
                }
            }

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
            return true;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            // 记录日志
            return false;
        }
    }

    // 3. 标记文档失效（如批文被收回）
    public async Task<bool> InvalidateDocumentAsync(int projectId, int documentTemplateId, string reason, string invalidatedBy)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var confirmation = await _context.ProjectDocumentConfirmations
                .FirstOrDefaultAsync(c => c.ProjectId == projectId && c.DocumentTemplateId == documentTemplateId);

            var oldStatus = confirmation?.Status ?? DocumentStatus.NotConfirmed;

            if (confirmation == null)
            {
                confirmation = new ProjectDocumentConfirmation
                {
                    ProjectId = projectId,
                    DocumentTemplateId = documentTemplateId,
                    Status = DocumentStatus.Invalidated,
                    StatusReason = reason,
                    InvalidatedDate = DateTime.Now,
                    InvalidatedBy = invalidatedBy,
                    CreatedBy = invalidatedBy,
                    CreatedDate = DateTime.Now
                };
                _context.ProjectDocumentConfirmations.Add(confirmation);
            }
            else
            {
                await RecordStatusHistoryAsync(projectId, documentTemplateId, oldStatus, DocumentStatus.Invalidated, reason, invalidatedBy);

                confirmation.Status = DocumentStatus.Invalidated;
                confirmation.StatusReason = reason;
                confirmation.InvalidatedDate = DateTime.Now;
                confirmation.InvalidatedBy = invalidatedBy;
            }

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
            return true;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return false;
        }
    }

    // 4. 检查文档完整性
    public async Task<DocumentComplianceResult> CheckDocumentComplianceAsync(int projectId)
    {
        var result = new DocumentComplianceResult();

        var project = await _context.Projects
            .Include(p => p.ProjectType)
            .FirstOrDefaultAsync(p => p.ProjectId == projectId);

        if (project == null) return result;

        // 获取必须的文档要求
        var mandatoryRequirements = await _context.ProjectTypeDocumentRequirements
            .Where(r => r.ProjectTypeId == project.ProjectTypeId && r.IsMandatory)
            .Include(r => r.DocumentTemplate)
            .ToListAsync();

        // 获取当前确认状态
        var confirmations = await _context.ProjectDocumentConfirmations
            .Where(c => c.ProjectId == projectId)
            .ToDictionaryAsync(c => c.DocumentTemplateId);

        foreach (var requirement in mandatoryRequirements)
        {
            if (!confirmations.ContainsKey(requirement.DocumentTemplateId) ||
                confirmations[requirement.DocumentTemplateId].Status != DocumentStatus.Confirmed)
            {
                result.MissingMandatoryDocuments.Add(requirement.DocumentTemplate.DocumentName);
            }
            else if (confirmations[requirement.DocumentTemplateId].Status == DocumentStatus.Invalidated)
            {
                result.InvalidDocuments.Add(requirement.DocumentTemplate.DocumentName);
            }
        }

        result.IsFullyCompliant = !result.MissingMandatoryDocuments.Any() && !result.InvalidDocuments.Any();
        return result;
    }

    // 5. 安全删除文档模板
    public async Task<DeleteResult> SafeDeleteDocumentTemplateAsync(int documentTemplateId, string deletedBy)
    {
        var result = new DeleteResult();

        // 检查是否被项目类型引用
        var requirements = await _context.ProjectTypeDocumentRequirements
            .Where(r => r.DocumentTemplateId == documentTemplateId)
            .Include(r => r.ProjectType)
            .ToListAsync();

        if (requirements.Any())
        {
            result.Success = false;
            result.Message = $"该文档模板被{requirements.Count}个项目类型引用，无法删除";
            result.ReferencingProjectTypes = requirements.Select(r => r.ProjectType.TypeName).ToList();
            return result;
        }

        // 检查是否有确认记录
        var confirmations = await _context.ProjectDocumentConfirmations
            .Where(c => c.DocumentTemplateId == documentTemplateId)
            .AnyAsync();

        if (confirmations)
        {
            result.Success = false;
            result.Message = "该文档模板已被项目使用，建议使用软删除";
            return result;
        }

        // 安全删除
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var template = await _context.DocumentTemplates.FindAsync(documentTemplateId);
            if (template != null)
            {
                _context.DocumentTemplates.Remove(template);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                result.Success = true;
                result.Message = "删除成功";
            }
            else
            {
                result.Success = false;
                result.Message = "文档模板不存在";
            }
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            result.Success = false;
            result.Message = $"删除失败: {ex.Message}";
        }

        return result;
    }

    // 6. 软删除文档模板
    public async Task<bool> SoftDeleteDocumentTemplateAsync(int documentTemplateId, string deletedBy)
    {
        var template = await _context.DocumentTemplates.FindAsync(documentTemplateId);
        if (template == null) return false;

        template.IsActive = false;
        await _context.SaveChangesAsync();
        return true;
    }

    // 辅助方法
    private string GetStatusDescription(DocumentStatus status)
    {
        return status switch
        {
            DocumentStatus.NotConfirmed => "未确认",
            DocumentStatus.Confirmed => "已确认",
            DocumentStatus.Invalidated => "已失效",
            DocumentStatus.UnderReview => "审核中",
            _ => "未知"
        };
    }

    private async Task RecordStatusHistoryAsync(int projectId, int documentTemplateId,
        DocumentStatus oldStatus, DocumentStatus newStatus, string reason, string changedBy)
    {
        var history = new DocumentStatusHistory
        {
            ProjectId = projectId,
            DocumentTemplateId = documentTemplateId,
            OldStatus = oldStatus,
            NewStatus = newStatus,
            ChangeReason = reason,
            ChangedBy = changedBy,
            ChangedDate = DateTime.Now
        };
        _context.DocumentStatusHistories.Add(history);
    }
}

// 辅助类
public class DeleteResult
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public List<string> ReferencingProjectTypes { get; set; } = new List<string>();
}

// 在Startup.cs或Program.cs中注册（.NET Core）
// services.AddDbContext<ProjectManagementContext>(options => 
//     options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
// services.AddScoped<ProjectDocumentService>();

protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
{
    if (!optionsBuilder.IsConfigured)
    {
        optionsBuilder.UseMySql("Server=localhost;Database=pm_db;User=root;Password=123456;", 
            new MySqlServerVersion(new Version(8, 0, 33)));
    }
}
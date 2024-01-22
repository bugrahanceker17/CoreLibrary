using System.ComponentModel.DataAnnotations.Schema;
using CoreLibrary.Models.Abstract;

namespace CoreLibrary.Models.Concrete.Entities;

[Table("AppPermissionCorrelations")]
public class AppPermissionCorrelation: BaseEntity<Guid>, IEntity
{
    public Guid RoleId { get; set; }
    public Guid PermissionId { get; set; }
}
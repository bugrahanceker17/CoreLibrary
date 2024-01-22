using System.ComponentModel.DataAnnotations.Schema;
using CoreLibrary.Models.Abstract;

namespace CoreLibrary.Models.Concrete.Entities;

[Table("AppUserRoles")]
public class AppUserRole : BaseEntity<Guid>, IEntity
{
    public Guid UserId { get; set; }
    public Guid RoleId { get; set; }
}
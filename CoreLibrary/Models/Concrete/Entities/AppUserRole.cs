using System.ComponentModel.DataAnnotations.Schema;
using CoreLibrary.Models.Abstract;
using CoreLibrary.Models.Concrete.Entities.Base;

namespace CoreLibrary.Models.Concrete.Entities;

[Table("AppUserRoles")]
public class AppUserRole : BaseEntity<Guid>, IEntity
{
    public Guid UserId { get; set; }
    public Guid RoleId { get; set; }
}
using System.ComponentModel.DataAnnotations.Schema;
using CoreLibrary.Models.Abstract;
using CoreLibrary.Models.Concrete.Entities.Base;

namespace CoreLibrary.Models.Concrete.Entities;

[Table("AppPermissions")]
public class AppPermission: BaseEntity<Guid>, IEntity
{
    public string Name { get; set; }
    public string Code { get; set; }
}
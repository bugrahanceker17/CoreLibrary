using System.ComponentModel.DataAnnotations.Schema;
using CoreLibrary.Models.Abstract;

namespace CoreLibrary.Models.Concrete.Entities;

[Table("AppPermissions")]
public class AppPermission: BaseEntity<Guid>, IEntity
{
    public string Name { get; set; }
    public string Code { get; set; }
}
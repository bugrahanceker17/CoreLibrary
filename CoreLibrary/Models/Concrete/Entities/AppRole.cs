using System.ComponentModel.DataAnnotations.Schema;
using CoreLibrary.Models.Abstract;

namespace CoreLibrary.Models.Concrete.Entities;

[Table("AppRoles")]
public class AppRole : BaseEntity<Guid>, IEntity
{
    public string Name { get; set; }
}
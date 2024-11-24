using System.ComponentModel.DataAnnotations.Schema;
using CoreLibrary.Models.Abstract;
using CoreLibrary.Models.Concrete.Entities.Base;

namespace CoreLibrary.Models.Concrete.Entities;

[Table("SharedSettings")]
public class AppSharedSetting : BaseEntity<Guid>, IEntity
{
    public int Type { get; set; }
    public string Name { get; set; }
    public string ExtraValue { get; set; }
    public string Description { get; set; }
}
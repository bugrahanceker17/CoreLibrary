using System.ComponentModel.DataAnnotations.Schema;
using CoreLibrary.Models.Abstract;
using CoreLibrary.Models.Concrete.Entities.Base;

namespace CoreLibrary.Models.Concrete.Entities;

[Table("AppLoginLogs")]
public class AppLoginLog : BaseEntity<Guid>, IEntity
{
    public string Description { get; set; }
}
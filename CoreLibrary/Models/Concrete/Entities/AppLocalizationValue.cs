using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CoreLibrary.Models.Abstract;
using CoreLibrary.Models.Concrete.Entities.Base;

namespace CoreLibrary.Models.Concrete.Entities;

[Table("AppLocalizationValues")]
public class AppLocalizationValue : BaseEntity<Guid>, IEntity
{
    [StringLength(200)] public string Key { get; set; }
    [StringLength(200)] public string ValueEN { get; set; }
    [StringLength(200)] public string ValueTR { get; set; }
    [StringLength(250)] public string? Description { get; set; }
}
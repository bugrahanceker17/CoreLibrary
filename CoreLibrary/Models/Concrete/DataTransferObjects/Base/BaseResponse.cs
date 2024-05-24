using System.Text.Json.Serialization;

namespace CoreLibrary.Models.Concrete.DataTransferObjects.Base;

public class BaseResponse
{
    public Guid Id { get; set; }
    [JsonIgnore ]public DateTime CreatedAt { get; set; }
    public string CreatedAtText => CreatedAt.ToString("dd.MM.yyyy HH:mm");
}
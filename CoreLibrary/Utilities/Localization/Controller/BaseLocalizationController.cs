using CoreLibrary.Utilities.Localization.Model;
using CoreLibrary.Utilities.Result;
using Microsoft.AspNetCore.Mvc;

namespace CoreLibrary.Utilities.Localization.Controller;

[Route("api/{culture}/[controller]")]
[ApiController]
public class BaseLocalizationController : Microsoft.AspNetCore.Mvc.Controller
{
   private readonly ITranslateHelper _translateHelper;

   public BaseLocalizationController(ITranslateHelper translateHelper)
   {
      _translateHelper = translateHelper;
   }

   [HttpGet("get-all")]
   public async Task<DataResult> GetAll(string key = null)
   {
      DataResult dataResult = new DataResult();
      dataResult.Data = await _translateHelper.GetAll(key);
      return dataResult;
   }
   
   [HttpGet("value")]
   public async Task<DataResult> GetValue(string key)
   {
      DataResult dataResult = new DataResult();
      dataResult.Data = await _translateHelper.GetValue(key);
      return dataResult;
   }

   [HttpPost("add")]
   public async Task<DataResult> Add(CreateLocalizationValueRequest request)
   {
      DataResult dataResult = new DataResult();
      dataResult.Data = await _translateHelper.Add(request);
      return dataResult;
   }
   
   [HttpPost("delete")]
   public async Task<DataResult> Delete(Guid id)
   {
      DataResult dataResult = new DataResult();
      dataResult.Data = await _translateHelper.Delete(id);
      return dataResult;
   }
}
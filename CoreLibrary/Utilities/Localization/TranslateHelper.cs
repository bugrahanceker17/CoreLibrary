using CoreLibrary.Models.Concrete.Entities;
using CoreLibrary.Utilities.DataAccess.Operation.Dapper.Abstract;
using CoreLibrary.Utilities.DataAccess.Operation.EntityFramework.Abstract;
using CoreLibrary.Utilities.Localization.Model;

namespace CoreLibrary.Utilities.Localization;

public interface ITranslateHelper
{
    Task<string> GetValue(string key);
    Task<List<GetAllLocalizationValueResponse>> GetAll(string key = null);
    Task<int> Add(CreateLocalizationValueRequest request);
    Task<int> Delete(Guid id);
}

public class TranslateHelper : ITranslateHelper
{
    private readonly IEfDynamicBaseQuery _dynamicBaseQuery;
    private readonly IEfDynamicBaseCommand _dynamicBaseCommand;
    private readonly ICultureHelper _cultureHelper;

    public TranslateHelper(IEfDynamicBaseQuery dynamicBaseQuery, IEfDynamicBaseCommand dynamicBaseCommand, ICultureHelper cultureHelper)
    {
        _dynamicBaseQuery = dynamicBaseQuery;
        _dynamicBaseCommand = dynamicBaseCommand;
        _cultureHelper = cultureHelper;
    }


    public async Task<string> GetValue(string key)
    {
        AppLocalizationValue? val = await _dynamicBaseQuery.GetByExpressionAsync<AppLocalizationValue>(c => c.Key.Equals(key));

        if (val is null)
            return key;

        string cultureValue = _cultureHelper.GetCurrentCulture();

        if (cultureValue == "tr-TR")
            return val.ValueTR;

        else if (cultureValue == "en-US")
            return val.ValueEN;

        else
            return val.ValueTR;
    }

    public async Task<List<GetAllLocalizationValueResponse>> GetAll(string key)
    {
        List<AppLocalizationValue> list = await _dynamicBaseQuery.GetAllByExpressionAsync<AppLocalizationValue>(c => !c.IsDeleted && c.IsStatus && (key == null || c.Key.Contains(key)));

        if (list.Any())
        {
            return list.Select(c => new GetAllLocalizationValueResponse
            {
                Id = c.Id,
                Key = c.Key,
                ValueEN = c.ValueEN,
                ValueTR = c.ValueTR,
                Description = c.Description
            }).ToList();
        }

        return null!;
    }

    public async Task<int> Add(CreateLocalizationValueRequest request)
    {
        List<AppLocalizationValue> keyExists = await _dynamicBaseQuery.GetAllByExpressionAsync<AppLocalizationValue>(c => c.Key.Equals(request.Key));

        if (keyExists.Any() && keyExists.Count > 0)
            return -1;

        (bool succeeded, Guid id) createRequest = await _dynamicBaseCommand.AddWithGuidIdentityAsync(new AppLocalizationValue
        {
            Key = request.Key,
            ValueEN = request.ValueEN,
            ValueTR = request.ValueTR,
            Description = request.Description ?? null
        });

        if (createRequest.succeeded)
            return 1;

        return -1;
    }

    public async Task<int> Delete(Guid id)
    {
        AppLocalizationValue? value = await _dynamicBaseQuery.GetByExpressionAsync<AppLocalizationValue>(c => c.Id == id);

        if (value is null)
            return -1;

        int passiveRequest = await _dynamicBaseCommand.Passive(value);

        if (passiveRequest > 0)
            return 1;

        return -1;
    }
}
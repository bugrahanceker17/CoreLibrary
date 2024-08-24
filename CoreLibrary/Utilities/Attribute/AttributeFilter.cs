using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using CoreLibrary.Extensions;
using CoreLibrary.Models.Concrete.Entities;
using CoreLibrary.Models.Setting;
using CoreLibrary.Utilities.DataAccess;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

namespace CoreLibrary.Utilities.Attribute;

public class AttributeFilter : IAuthorizationFilter
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ConfigurationValues _configurationValues;

    public AttributeFilter(IHttpContextAccessor httpContextAccessor, IOptions<ConfigurationValues> configurationValues)
    {
        _httpContextAccessor = httpContextAccessor;
        _configurationValues = configurationValues.Value;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (context.ActionDescriptor.EndpointMetadata.Any(em => em is AuthorizationControlAttribute))
        {
            AuthorizationControlAttribute? customAttribute = context.ActionDescriptor.EndpointMetadata.OfType<AuthorizationControlAttribute>().FirstOrDefault();
            bool? loginCheck = customAttribute?.MustLogin;

            if (loginCheck.HasValue && loginCheck.Value)
            {
                if (string.IsNullOrEmpty(_httpContextAccessor.AccessToken().accessToken))
                {
                    context.Result = new UnauthorizedResult();
                    return;
                }
            }

            HttpContext httpContext = _httpContextAccessor.HttpContext;
            HttpRequest request = httpContext?.Request;

            string accessToken = request?.Headers["Authorization"].FirstOrDefault();

            if (string.IsNullOrEmpty(accessToken))
                if (request.Query.TryGetValue("access_token", out StringValues token))
                    accessToken = $"Bearer {token}";

            List<string> roles = new();
            List<Claim> claims = null;
            string userId = string.Empty;

            if (!string.IsNullOrEmpty(accessToken))
            {
                if (accessToken.StartsWith("Bearer")) accessToken = accessToken.Replace("Bearer ", "").Replace("\"", "").Trim();

                JwtSecurityToken securityToken = new JwtSecurityToken(accessToken);

                if (securityToken.Claims.Any())
                {
                    userId = securityToken.Claims.FirstOrDefault(c => c.Type == "nameid")?.Value;
                    roles = securityToken.Claims.Where(c => c.Type == "sub").ToList().Select(c => c.Value).ToList();
                }
            }

            using var dbContext = new ApplicationDbContext(_configurationValues.Database.ConnectionString, _configurationValues.DbType.Name);
            
            if (!string.IsNullOrEmpty(userId))
            {
                Guid gUserId = new Guid(userId);
                    
                AppUser? userControl = dbContext.AppUsers.FirstOrDefault(c => c.Id == gUserId && c.IsDeleted == false && c.IsStatus == true);
                    
                if(userControl is null)
                    context.Result = new ObjectResult("Kullanıcı bulunamadı!") { StatusCode = 401 };
                    
            }

            List<string>? permissionParams = customAttribute?.Permissions?.ToList();

            if (permissionParams != null && permissionParams.Any())
            {
                if (roles.Any())
                {
                    List<Guid> guidRoles = new();
                    roles.ForEach(item => { guidRoles.Add(new Guid(item)); });
                        
                    List<AppPermissionCorrelation> rolePermissionRelation = dbContext.AppPermissionCorrelations.ToList()
                        .Where(c => guidRoles.Contains(c.RoleId) && c is { IsStatus: true, IsDeleted: false }).ToList();

                    List<string> allPermissionInRoles = rolePermissionRelation.Select(c => c.PermissionId.ToString()).ToList();

                    IEnumerable<string> permissions = dbContext.Permissions.ToList()
                        .Where(c => permissionParams.Contains(c.Code.ToString()))
                        .ToList().Select(c => c.Id.ToString());

                    if (permissions.Any(c => allPermissionInRoles.Contains(c)))
                        Console.WriteLine("Yetki kontrolü başarılı !");

                    else
                        context.Result = new ObjectResult("Yetkilendirme başarısız") { StatusCode = 403 };
                }
            }
        }
    }
}
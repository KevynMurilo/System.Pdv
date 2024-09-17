using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Pdv.Application.Interfaces.Authorization;

public class HasPermissionAttribute : Attribute, IAsyncAuthorizationFilter
{
    private readonly string _recurso;
    private readonly string _acao;

    public HasPermissionAttribute(string recurso, string acao)
    {
        _recurso = recurso;
        _acao = acao;
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var userId = Guid.Parse(context.HttpContext.User.FindFirst("id")?.Value);

        var authorizationUseCase = context.HttpContext.RequestServices.GetService<IAuthorizationUseCase>();
        if (authorizationUseCase == null)
        {
            context.Result = new ForbidResult();
            return;
        }

        var hasPermission = await authorizationUseCase.HasPermissionAsync(userId, _recurso, _acao);
        if (!hasPermission)
        {
            context.Result = new ForbidResult();
        }
    }
}

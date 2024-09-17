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
        var userIdClaim = context.HttpContext.User.FindFirst("id");

        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
        {
            context.HttpContext.Response.Headers.Add("WWW-Authenticate", "Bearer");

            context.HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Result = new EmptyResult();  // Não retorna corpo de resposta
            return;
        }

        var authorizationUseCase = context.HttpContext.RequestServices.GetService<IAuthorizationUseCase>();
        if (authorizationUseCase == null)
        {
            context.HttpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
            context.Result = new EmptyResult();  // Não retorna corpo de resposta
            return;
        }

        var hasPermission = await authorizationUseCase.HasPermissionAsync(userId, _recurso, _acao);
        if (!hasPermission)
        {
            context.HttpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
            context.Result = new EmptyResult();  // Não retorna corpo de resposta
        }
    }
}

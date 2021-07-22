using FDevsQuiz.Domain.Interface;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Security.Claims;

namespace FDevsQuiz.Application.Identity
{
    public class UsuarioContext: IUsuario
    {
        private readonly IHttpContextAccessor _accessor;

        public UsuarioContext(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public long? Codigo => _accessor.HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier)?.Select(c => string.IsNullOrEmpty(c.Value) ? null : (int?)Convert.ToInt32(c.Value)).FirstOrDefault();

        public string Nome => _accessor.HttpContext.User.Identity.Name;

    }
}

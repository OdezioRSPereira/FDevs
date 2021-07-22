using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;

namespace FDevsQuiz.Application.Controllers.V3.Base
{
    [ApiController]
    public abstract class BaseController: ControllerBase
    {
        protected long? CodigoUsuario()
        {
            return User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier)?.Select(c => string.IsNullOrEmpty(c.Value) ? null : (int?)Convert.ToInt32(c.Value)).FirstOrDefault();
        }
    }
}

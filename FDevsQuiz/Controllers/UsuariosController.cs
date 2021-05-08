using FDevsQuiz.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace FDevsQuiz.Controllers
{
    [Controller]
    [Route(("usuarios"))]
    public class UsuariosController : ControllerBase
    {

        private ICollection<T> CarregarDados<T>()
        {
            using var openStream = System.IO.File.OpenRead(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "usuarios.json"));
            return JsonSerializer.DeserializeAsync<ICollection<T>>(openStream, new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            }).Result;
        }

    }
}

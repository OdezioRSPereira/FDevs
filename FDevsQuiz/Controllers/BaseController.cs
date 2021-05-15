using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace FDevsQuiz.Controllers
{
    public abstract class BaseController : ControllerBase, IFileController
    {
        public abstract string Filename { get; }

        private string Fullname { get => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", Filename); }

        protected async Task<ICollection<T>> CarregarDadosAsync<T>()
        {
            using var openStream = System.IO.File.OpenRead(Fullname);
            return await JsonSerializer.DeserializeAsync<ICollection<T>>(openStream, new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
        }

        protected async Task SalvarDadosAsync<T>(ICollection<T> dados)
        {
            using FileStream createStream = System.IO.File.Create(Fullname);
            await JsonSerializer.SerializeAsync(createStream, dados, new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
        }

    }
}

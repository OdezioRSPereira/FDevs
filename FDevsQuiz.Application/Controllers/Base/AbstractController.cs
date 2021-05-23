using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace FDevsQuiz.Controllers.Base
{
    public abstract class AbstractController<ID, T> : ControllerBase
    {
        private readonly ICollection<T> _dados;

        protected AbstractController()
        {
            _dados = CarregarDadosAsync().Result;
        }

        protected abstract string Filename { get; }
        protected abstract T Item(ICollection<T> dados, ID id);

        protected abstract T GerarChave(ICollection<T> dados, T model);

        protected string Fullname { get => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", Filename); }

        private async Task<ICollection<T>> CarregarDadosAsync()
        {
            using var openStream = System.IO.File.OpenRead(Fullname);
            return await JsonSerializer.DeserializeAsync<ICollection<T>>(openStream, SerializerOptions.Options);
        }

        protected async Task SalvarDadosAsync()
        {
            using FileStream createStream = System.IO.File.Create(Fullname);
            await JsonSerializer.SerializeAsync(createStream, _dados, SerializerOptions.Options);
        }

        protected Task<ICollection<T>> Dados() {
            return Task.FromResult(_dados);
        }

        protected T FindById(ID id)
        {
            return Item(_dados, id);
        }

        protected async Task<T> Adicionar(T model)
        {
            model = GerarChave(_dados, model);

            _dados.Add(model);

            await SalvarDadosAsync();

            return model;
        }

        protected async Task RemoveById(ID id)
        {
            var item = FindById(id);

            if (item == null)
                return;

            _dados.Remove(item);

            await SalvarDadosAsync();
        }

    }
}

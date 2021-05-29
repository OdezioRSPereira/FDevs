using System.Text.Json;

namespace FDevsQuiz.Application.Controllers.V1.Base
{
    public static class SerializerOptions
    {
        public static JsonSerializerOptions Options => new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
    }
}

using Jogos.API.Application.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;

namespace Jogos.Test.App
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        public Mock<ICategoriaUseCase> CategoriaUseCaseMock { get; } = new();
        public Mock<IDesenvolvedoraUseCase> DesenvolvedoraUseCaseMock { get; } = new();
        public Mock<IPlataformaUseCase> PlataformaUseCaseMock { get; } = new();
        public Mock<IJogoUseCase> JogoUseCaseMock { get; } = new();

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.RemoveAll(typeof(ICategoriaUseCase));
                services.AddSingleton(CategoriaUseCaseMock.Object);

                services.RemoveAll(typeof(IDesenvolvedoraUseCase));
                services.AddSingleton(DesenvolvedoraUseCaseMock.Object);

                services.RemoveAll(typeof(IPlataformaUseCase));
                services.AddSingleton(PlataformaUseCaseMock.Object);

                services.RemoveAll(typeof(IJogoUseCase));
                services.AddSingleton(JogoUseCaseMock.Object);
            });
        }
    }
}

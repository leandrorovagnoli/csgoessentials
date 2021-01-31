using CsgoEssentials.Infra.Data;
using CsgoEssentials.WebAPI;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace CsgoEssentials.IntegrationTests.Config
{
    public class IntegrationTestConfig
    {
        protected readonly HttpClient Client;
        private readonly WebApplicationFactory<Startup> _appFactory;

        protected IntegrationTestConfig()
        {
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            _appFactory = new WebApplicationFactory<Startup>()
               .WithWebHostBuilder(builder =>
               {
                   builder.ConfigureServices(services =>
                   {
                       services.RemoveAll(typeof(DataContext));
                       services.RemoveAll(typeof(DbContextOptions));
                       services.RemoveAll(typeof(DbContextOptions<DataContext>));
                       services.AddDbContext<DataContext>(options =>
                       {
                           options.UseInMemoryDatabase("TestDb");
                           options.UseInternalServiceProvider(serviceProvider);
                       });
                   });
               });

            Client = _appFactory.CreateClient();
        }

        protected DataContext Context
        {
            get => _appFactory.Services.CreateScope().ServiceProvider.GetRequiredService<DataContext>();
        }

        protected async Task AuthenticateAsync()
        {
            var jsonModel = await GetJwtAsync();

            if (string.IsNullOrEmpty(jsonModel.Token))
                throw new HttpRequestException(jsonModel.Message);

            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", jsonModel.Token);
        }

        private async Task<JsonModel> GetJwtAsync()
        {
            var response = await Client.PostAsJsonAsync(ApiRoutes.Users.Authenticate, new
            {
                UserName = "leolandrooo",
                Password = "@123456*"
            });

            return await response.Content.ReadFromJsonAsync<JsonModel>();
        }
    }

    public class JsonModel
    {
        public string Message { get; set; }
        public string Token { get; set; }
    }
}

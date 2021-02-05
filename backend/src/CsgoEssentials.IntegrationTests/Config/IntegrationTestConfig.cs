using CsgoEssentials.Domain.Enum;
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
        protected readonly string _baseUrl = "https://localhost:5001/";

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

        /// <summary>
        /// Authenticate user to allow access routes protected
        /// Administrator = 1, Editor = 2, Member = 3,
        /// </summary>
        /// <param name="userRole">Role to login with</param>
        /// <returns></returns>
        protected async Task AuthenticateAsync(EUserRole role)
        {
            var jsonModel = await GetJwtAsync(role);

            if (string.IsNullOrEmpty(jsonModel.Token))
                throw new HttpRequestException(jsonModel.Message);

            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", jsonModel.Token);
        }

        private async Task<JsonModel> GetJwtAsync(EUserRole role)
        {
            var userName = string.Empty;
            var password = string.Empty;
            switch (role)
            {
                case EUserRole.Administrator:
                    userName = "leolandrooo";
                    password = "@123456*";
                    break;
                case EUserRole.Editor:
                    userName = "maria";
                    password = "@123456*";
                    break;
                case EUserRole.Member:
                    userName = "joao";
                    password = "@123456*";
                    break;
                default:
                    break;
            }

            var response = await Client.PostAsJsonAsync(_baseUrl + "v1/users/" + ApiRoutes.Users.Authenticate, new
            {
                UserName = userName,
                Password = password
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

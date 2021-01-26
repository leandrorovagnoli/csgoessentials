using CsgoEssentials.Domain.Entities;
using CsgoEssentials.Domain.Enum;
using CsgoEssentials.Infra.Utils;
using CsgoEssentials.IntegrationTests.Config;
using FluentAssertions;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace CsgoEssentials.IntegrationTests.UserTests
{
    public class UserTest : IntegrationTestConfig
    {
        private User _newMemberUser;

        public UserTest()
        {
            _newMemberUser = new User(
                "Joao da Silva Member User",
                "joaozinho@gmail.com",
                "joaomember",
                "@123456*",
                EUserRole.Member);
        }

        [Fact]
        public async Task Create_Deve_Criar_Usuario_Retornando_Usuario_Criado()
        {
            //Arrange
            await AuthenticateAsync();

            //Act
            var response = await Client.PostAsJsonAsync(ApiRoutes.Users.Create, _newMemberUser);
            var user = await response.Content.ReadAsAsync<User>();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            user.Id.Should().BeGreaterThan(0);
            user.Name.Should().Be(_newMemberUser.Name);
            user.Password.Should().Be(MD5Hash.CalculaHash(_newMemberUser.Password));
            user.Role.Should().Be(_newMemberUser.Role);
        }

        [Fact]
        public async Task GetById_Deve_Retornar_UM_Usuario_Do_Banco()
        {
            //Arrange
            await AuthenticateAsync();

            var responseAux = await Client.PostAsJsonAsync(ApiRoutes.Users.Create, _newMemberUser);
            var userAux = await responseAux.Content.ReadAsAsync<User>();

            responseAux.StatusCode.Should().Be(HttpStatusCode.OK);
            userAux.Should().NotBeNull();

            //Act
            var response = await Client.GetAsync(ApiRoutes.Users.GetById.Replace("{userId}", userAux.Id.ToString()));
            var user = await response.Content.ReadAsAsync<User>();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            user.Id.Should().BeGreaterThan(0);
            user.Name.Should().Be(_newMemberUser.Name);
            user.Password.Should().Be(MD5Hash.CalculaHash(_newMemberUser.Password));
            user.Role.Should().Be(_newMemberUser.Role);
        }

        [Fact]
        public async Task GetById_Deve_Retornar_Usuario_Nao_Encontrado_Quando_Id_Nao_Existir()
        {
            //Arrange
            await AuthenticateAsync();

            //Act
            var response = await Client.GetAsync(ApiRoutes.Users.GetById.Replace("{userId}", "9999"));
            var content = await response.Content.ReadAsStringAsync();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            content.Should().Contain(Messages.USUARIO_NAO_ENCONTRADO);
        }

        [Fact]
        public async Task Update_Deve_Atualizar_Usuario_Existente()
        {
            //Arrange
            await AuthenticateAsync();
            var responseAux = await Client.GetAsync(ApiRoutes.Users.GetById.Replace("{userId}", "2"));
            var userAux = await responseAux.Content.ReadAsAsync<User>();

            userAux.Name = "updatedUser";
            userAux.Role = EUserRole.Editor;

            //Act
            var response = await Client.PutAsJsonAsync(ApiRoutes.Users.Update.Replace("{userId}", userAux.Id.ToString()), userAux);
            var user = await response.Content.ReadAsAsync<User>();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            user.Id.Should().BeGreaterThan(0);
            user.Name.Should().Be("updatedUser");
            user.Password.Should().Be(MD5Hash.CalculaHash(_newMemberUser.Password));
            user.Role.Should().Be(EUserRole.Editor);
        }

        [Fact]
        public async Task Update_Nao_Deve_Permitir_Alterar_Nome_De_Usuario()
        {
            //Arrange
            await AuthenticateAsync();
            var responseAux = await Client.GetAsync(ApiRoutes.Users.GetById.Replace("{userId}", "2"));
            var userAux = await responseAux.Content.ReadAsAsync<User>();

            userAux.UserName = "novonomedeusuario";

            //Act
            var response = await Client.PutAsJsonAsync(ApiRoutes.Users.Update.Replace("{userId}", userAux.Id.ToString()), userAux);
            var jsonModel = await response.Content.ReadFromJsonAsync<JsonModel>();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            jsonModel.Message.Should().Be(Messages.NAO_E_PERMITIDO_ALTERAR_NOME_DE_USUARIO);
        }

        [Fact]
        public async Task Update_Nao_Deve_Atualizar_Usuario_Com_Id_Diferente_Do_Editado_Retornando_NotFound()
        {
            //Arrange
            await AuthenticateAsync();
            var responseAux = await Client.PostAsJsonAsync(ApiRoutes.Users.Create, _newMemberUser);
            var userAux = await responseAux.Content.ReadAsAsync<User>();

            responseAux.StatusCode.Should().Be(HttpStatusCode.OK);
            userAux.Should().NotBeNull();

            //Act
            userAux.Name = "updatedUser";
            userAux.Role = EUserRole.Editor;

            var response = await Client.PutAsJsonAsync(ApiRoutes.Users.Update.Replace("{userId}", "9999"), userAux);
            var content = await response.Content.ReadAsStringAsync();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            content.Should().Contain(Messages.USUARIO_NAO_ENCONTRADO);
        }

        [Fact]
        public async Task Update_Deve_Atualizar_Usuario_Existente_Com_Nova_Senha()
        {
            //Arrange
            await AuthenticateAsync();
            var responseAux = await Client.PostAsJsonAsync(ApiRoutes.Users.Create, _newMemberUser);
            var userAux = await responseAux.Content.ReadAsAsync<User>();

            responseAux.StatusCode.Should().Be(HttpStatusCode.OK);
            userAux.Should().NotBeNull();

            userAux.Password = "@ABC123";

            //Act
            var response = await Client.PutAsJsonAsync(ApiRoutes.Users.Update.Replace("{userId}", userAux.Id.ToString()), userAux);
            var user = await response.Content.ReadAsAsync<User>();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            user.Id.Should().BeGreaterThan(0);
            user.Name.Should().Be(userAux.Name);
            user.Password.Should().Be(MD5Hash.CalculaHash("@ABC123"));
            user.Role.Should().Be(userAux.Role);
        }

        [Fact]
        public async Task Delete_Deve_Apagar_Usuario_Existente_Retornando_Mensagem()
        {
            //Arrange
            await AuthenticateAsync();
            var responseAux = await Client.PostAsJsonAsync(ApiRoutes.Users.Create, _newMemberUser);
            var userAux = await responseAux.Content.ReadAsAsync<User>();

            responseAux.StatusCode.Should().Be(HttpStatusCode.OK);
            userAux.Should().NotBeNull();

            //Act
            var response = await Client.DeleteAsync(ApiRoutes.Users.Delete.Replace("{userId}", userAux.Id.ToString()));
            var content = await response.Content.ReadAsStringAsync();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            content.Should().Contain(Messages.USUARIO_REMOVIDO_COM_SUCESSO);
        }

        [Fact]
        public async Task Create_Deve_Invalidar_Nome_Vazio()
        {
            //Arrange
            await AuthenticateAsync();
            await Client.PostAsJsonAsync(ApiRoutes.Users.Create, _newMemberUser);

            _newMemberUser.Name = string.Empty;

            //Act
            var response = await Client.PostAsJsonAsync(ApiRoutes.Users.Create, _newMemberUser);
            var content = await response.Content.ReadAsStringAsync();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            content.Should().Contain(Messages.CAMPO_OBRIGATORIO.Replace("{0}", Messages.NOME));
        }

        [Fact]
        public async Task Create_Deve_Invalidar_Nome_De_Usuario_Vazio()
        {
            //Arrange
            await AuthenticateAsync();
            await Client.PostAsJsonAsync(ApiRoutes.Users.Create, _newMemberUser);

            _newMemberUser.UserName = string.Empty;

            //Act
            var response = await Client.PostAsJsonAsync(ApiRoutes.Users.Create, _newMemberUser);
            var content = await response.Content.ReadAsStringAsync();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            content.Should().Contain(Messages.CAMPO_OBRIGATORIO.Replace("{0}", Messages.NOME_DE_USUARIO));
        }

        [Fact]
        public async Task Create_Deve_Invalidar_Senha_Vazio()
        {
            //Arrange
            await AuthenticateAsync();
            await Client.PostAsJsonAsync(ApiRoutes.Users.Create, _newMemberUser);

            _newMemberUser.Password = string.Empty;

            //Act
            var response = await Client.PostAsJsonAsync(ApiRoutes.Users.Create, _newMemberUser);
            var content = await response.Content.ReadAsStringAsync();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            content.Should().Contain(string.Format(Messages.CAMPO_OBRIGATORIO, Messages.SENHA));
        }

        [Fact]
        public async Task Create_Deve_Invalidar_Nome_Menor_Do_Que_Permitido()
        {
            //Arrange
            await AuthenticateAsync();
            await Client.PostAsJsonAsync(ApiRoutes.Users.Create, _newMemberUser);

            _newMemberUser.Name = "Leo";

            //Act
            var response = await Client.PostAsJsonAsync(ApiRoutes.Users.Create, _newMemberUser);
            var content = await response.Content.ReadAsStringAsync();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            content.Should().Contain(string.Format(Messages.CAMPO_PRECISA_TER_ENTRE_X2_E_Y1_CARACTERES, Messages.NOME, "60", "4"));
        }

        [Fact]
        public async Task Create_Deve_Invalidar_Nome_De_Usuario_Maior_Do_Que_Permitido()
        {
            //Arrange
            await AuthenticateAsync();
            await Client.PostAsJsonAsync(ApiRoutes.Users.Create, _newMemberUser);

            _newMemberUser.UserName = "nomedeusuariomuitograndenomedeusuariomuitograndenomedeusuariomuitogrande";

            //Act
            var response = await Client.PostAsJsonAsync(ApiRoutes.Users.Create, _newMemberUser);
            var content = await response.Content.ReadAsStringAsync();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            content.Should().Contain(string.Format(Messages.CAMPO_PRECISA_TER_ENTRE_X2_E_Y1_CARACTERES, Messages.NOME_DE_USUARIO, "60", "4"));
        }

        [Fact]
        public async Task Create_Deve_Invalidar_Email_Fora_Do_Padrao()
        {
            //Arrange
            await AuthenticateAsync();
            await Client.PostAsJsonAsync(ApiRoutes.Users.Create, _newMemberUser);

            _newMemberUser.Email = "abc.com";

            //Act
            var response = await Client.PostAsJsonAsync(ApiRoutes.Users.Create, _newMemberUser);
            var content = await response.Content.ReadAsStringAsync();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            content.Should().Contain(string.Format(Messages.CAMPO_INVALIDO, Messages.EMAIL));
        }

        [Fact]
        public async Task Create_Deve_Invalidar_Senha_Menor_Do_Que_Permitido()
        {
            //Arrange
            await AuthenticateAsync();
            await Client.PostAsJsonAsync(ApiRoutes.Users.Create, _newMemberUser);

            _newMemberUser.Password = "12345";

            //Act
            var response = await Client.PostAsJsonAsync(ApiRoutes.Users.Create, _newMemberUser);
            var content = await response.Content.ReadAsStringAsync();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            content.Should().Contain(string.Format(Messages.CAMPO_PRECISA_TER_ENTRE_X2_E_Y1_CARACTERES, Messages.SENHA, "60", "6"));
        }

        [Fact]
        public async Task Create_Deve_Invalidar_Nome_De_Usuario_Com_Espacos()
        {
            //Arrange
            await AuthenticateAsync();
            await Client.PostAsJsonAsync(ApiRoutes.Users.Create, _newMemberUser);

            _newMemberUser.UserName = "meu nome de usuario";

            //Act
            var response = await Client.PostAsJsonAsync(ApiRoutes.Users.Create, _newMemberUser);
            var content = await response.Content.ReadAsStringAsync();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            content.Should().Contain(string.Format(Messages.CAMPO_INVALIDO, Messages.NOME_DE_USUARIO));
        }

        [Fact]
        public async Task Authenticate_Deve_Retornar_Token_De_Autenticacao_Para_Usuario_Logado()
        {
            //Act
            var response = await Client.PostAsJsonAsync(ApiRoutes.Users.Authenticate, new
            {
                UserName = "leolandrooo",
                Password = "@123456*"
            });

            var content = await response.Content.ReadFromJsonAsync<JsonModel>();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            content.Message.Should().BeNullOrEmpty();
            content.Token.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task Authenticate_Deve_Retornar_Mensagem_Usuario_Ou_Senha_Invalidos()
        {
            //Act
            var response = await Client.PostAsJsonAsync(ApiRoutes.Users.Authenticate, new
            {
                UserName = "leolandrooo",
                Password = "xpto"
            });

            var content = await response.Content.ReadFromJsonAsync<JsonModel>();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            content.Message.Should().Be(Messages.USUARIO_OU_SENHA_INVALIDOS);
            content.Token.Should().BeNullOrEmpty();
        }

        [Fact]
        public async Task Authenticate_Deve_Retornar_Status_Proibido_Quando_Tentar_Criar_Novo_Usuario_Com_Funcao_Membro()
        {
            //Arrange
            await AuthenticateAsync();
            await Client.PostAsJsonAsync(ApiRoutes.Users.Create, _newMemberUser); // Adding joaomember to DB.

            var loginResponse = await Client.PostAsJsonAsync(ApiRoutes.Users.Authenticate, new
            {
                UserName = "joaomember",
                Password = "@123456*"
            });

            var jsonModel = await loginResponse.Content.ReadFromJsonAsync<JsonModel>();
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", jsonModel.Token);

            //Act
            var response = await Client.PostAsJsonAsync(ApiRoutes.Users.Create,
                new User("newUser", "newUser@newUser.com", "newuser", "@123456*", EUserRole.Member));

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Create_Nao_Deve_Criar_Usuario_Com_Nome_De_Usuario_Ja_Existente()
        {
            //Arrange
            await AuthenticateAsync();
            await Client.PostAsJsonAsync(ApiRoutes.Users.Create, _newMemberUser);

            //Act
            var response = await Client.PostAsJsonAsync(ApiRoutes.Users.Create, _newMemberUser);
            var jsonModel = await response.Content.ReadFromJsonAsync<JsonModel>();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            jsonModel.Message.Should().Be(Messages.NOME_DE_USUARIO_JA_EXISTENTE);
        }
    }
}

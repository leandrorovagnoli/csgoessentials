using CsgoEssentials.Domain.Entities;
using CsgoEssentials.Domain.Enum;
using CsgoEssentials.Infra.Data;
using CsgoEssentials.Domain.Utils;
using CsgoEssentials.IntegrationTests.Config;
using FluentAssertions;
using System.Collections.Generic;
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
        private string _defaultRoute;

        public UserTest()
        {
            //getting route: ..localhost:5001/v1/users/
            _defaultRoute = _baseUrl + ApiRoutes.Users.Route + "/";

            _newMemberUser = new User(
                "Joao da Silva Member User",
                "joaozinho@gmail.com",
                "joaomember",
                "@123456*",
                EUserRole.Member);
        }

        #region Create

        [Fact]
        public async Task Create_Deve_Criar_Usuario_Retornando_Usuario_Criado()
        {
            //Arrange
            await AuthenticateAsync(EUserRole.Administrator);

            //Act
            var response = await Client.PostAsJsonAsync(_defaultRoute + ApiRoutes.Users.Create, _newMemberUser);
            var user = await response.Content.ReadAsAsync<User>();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            user.Id.Should().BeGreaterThan(0);
            user.Name.Should().Be(_newMemberUser.Name);
            user.Password.Should().Be(MD5Hash.CalculaHash(_newMemberUser.Password));
            user.Role.Should().Be(_newMemberUser.Role);
        }

        [Fact]
        public async Task Create_Deve_Invalidar_Nome_Vazio()
        {
            //Arrange
            await AuthenticateAsync(EUserRole.Administrator);
            await Client.PostAsJsonAsync(_defaultRoute + ApiRoutes.Users.Create, _newMemberUser);

            _newMemberUser.Name = string.Empty;

            //Act
            var response = await Client.PostAsJsonAsync(_defaultRoute + ApiRoutes.Users.Create, _newMemberUser);
            var content = await response.Content.ReadAsStringAsync();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            content.Should().Contain(Messages.CAMPO_OBRIGATORIO.Replace("{0}", Messages.NOME));
        }

        [Fact]
        public async Task Create_Deve_Invalidar_Nome_De_Usuario_Vazio()
        {
            //Arrange
            await AuthenticateAsync(EUserRole.Administrator);
            await Client.PostAsJsonAsync(_defaultRoute + ApiRoutes.Users.Create, _newMemberUser);

            _newMemberUser.UserName = string.Empty;

            //Act
            var response = await Client.PostAsJsonAsync(_defaultRoute + ApiRoutes.Users.Create, _newMemberUser);
            var content = await response.Content.ReadAsStringAsync();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            content.Should().Contain(Messages.CAMPO_OBRIGATORIO.Replace("{0}", Messages.NOME_DE_USUARIO));
        }

        [Fact]
        public async Task Create_Deve_Invalidar_Senha_Vazio()
        {
            //Arrange
            await AuthenticateAsync(EUserRole.Administrator);
            await Client.PostAsJsonAsync(_defaultRoute + ApiRoutes.Users.Create, _newMemberUser);

            _newMemberUser.Password = string.Empty;

            //Act
            var response = await Client.PostAsJsonAsync(_defaultRoute + ApiRoutes.Users.Create, _newMemberUser);
            var content = await response.Content.ReadAsStringAsync();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            content.Should().Contain(string.Format(Messages.CAMPO_OBRIGATORIO, Messages.SENHA));
        }

        [Fact]
        public async Task Create_Deve_Invalidar_Nome_Menor_Do_Que_Permitido()
        {
            //Arrange
            await AuthenticateAsync(EUserRole.Administrator);
            await Client.PostAsJsonAsync(_defaultRoute + ApiRoutes.Users.Create, _newMemberUser);

            _newMemberUser.Name = "Leo";

            //Act
            var response = await Client.PostAsJsonAsync(_defaultRoute + ApiRoutes.Users.Create, _newMemberUser);
            var content = await response.Content.ReadAsStringAsync();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            content.Should().Contain(string.Format(Messages.CAMPO_PRECISA_TER_ENTRE_X2_E_Y1_CARACTERES, Messages.NOME, "60", "4"));
        }

        [Fact]
        public async Task Create_Deve_Invalidar_Nome_De_Usuario_Maior_Do_Que_Permitido()
        {
            //Arrange
            await AuthenticateAsync(EUserRole.Administrator);
            await Client.PostAsJsonAsync(_defaultRoute + ApiRoutes.Users.Create, _newMemberUser);

            _newMemberUser.UserName = "nomedeusuariomuitograndenomedeusuariomuitograndenomedeusuariomuitogrande";

            //Act
            var response = await Client.PostAsJsonAsync(_defaultRoute + ApiRoutes.Users.Create, _newMemberUser);
            var content = await response.Content.ReadAsStringAsync();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            content.Should().Contain(string.Format(Messages.CAMPO_PRECISA_TER_ENTRE_X2_E_Y1_CARACTERES, Messages.NOME_DE_USUARIO, "60", "4"));
        }

        [Fact]
        public async Task Create_Deve_Invalidar_Email_Fora_Do_Padrao()
        {
            //Arrange
            await AuthenticateAsync(EUserRole.Administrator);
            await Client.PostAsJsonAsync(_defaultRoute + ApiRoutes.Users.Create, _newMemberUser);

            _newMemberUser.Email = "abc.com";

            //Act
            var response = await Client.PostAsJsonAsync(_defaultRoute + ApiRoutes.Users.Create, _newMemberUser);
            var content = await response.Content.ReadAsStringAsync();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            content.Should().Contain(string.Format(Messages.CAMPO_INVALIDO, Messages.EMAIL));
        }

        [Fact]
        public async Task Create_Deve_Invalidar_Senha_Menor_Do_Que_Permitido()
        {
            //Arrange
            await AuthenticateAsync(EUserRole.Administrator);
            await Client.PostAsJsonAsync(_defaultRoute + ApiRoutes.Users.Create, _newMemberUser);

            _newMemberUser.Password = "12345";

            //Act
            var response = await Client.PostAsJsonAsync(_defaultRoute + ApiRoutes.Users.Create, _newMemberUser);
            var content = await response.Content.ReadAsStringAsync();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            content.Should().Contain(string.Format(Messages.CAMPO_PRECISA_TER_ENTRE_X2_E_Y1_CARACTERES, Messages.SENHA, "60", "6"));
        }

        [Fact]
        public async Task Create_Deve_Invalidar_Nome_De_Usuario_Com_Espacos()
        {
            //Arrange
            await AuthenticateAsync(EUserRole.Administrator);
            await Client.PostAsJsonAsync(_defaultRoute + ApiRoutes.Users.Create, _newMemberUser);

            _newMemberUser.UserName = "meu nome de usuario";

            //Act
            var response = await Client.PostAsJsonAsync(_defaultRoute + ApiRoutes.Users.Create, _newMemberUser);
            var content = await response.Content.ReadAsStringAsync();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            content.Should().Contain(string.Format(Messages.CAMPO_INVALIDO, Messages.NOME_DE_USUARIO));
        }

        [Fact]
        public async Task Create_Nao_Deve_Criar_Usuario_Com_Nome_De_Usuario_Ja_Existente()
        {
            //Arrange
            await AuthenticateAsync(EUserRole.Administrator);
            await Client.PostAsJsonAsync(_defaultRoute + ApiRoutes.Users.Create, _newMemberUser);

            //Act
            var response = await Client.PostAsJsonAsync(_defaultRoute + ApiRoutes.Users.Create, _newMemberUser);
            var jsonModel = await response.Content.ReadFromJsonAsync<JsonModel>();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            jsonModel.Message.Should().Be(Messages.NOME_DE_USUARIO_JA_EXISTENTE);
        }

        #endregion

        #region Update

        [Fact]
        public async Task Update_Deve_Atualizar_Usuario_Existente()
        {
            //Arrange
            await AuthenticateAsync(EUserRole.Administrator);
            var responseAux = await Client.GetAsync(_defaultRoute + ApiRoutes.Users.GetById.Replace("{id:int}", "2"));
            var userAux = await responseAux.Content.ReadAsAsync<User>();

            userAux.Name = "updatedUser";
            userAux.Role = EUserRole.Editor;

            //Act
            var response = await Client.PutAsJsonAsync(_defaultRoute + ApiRoutes.Users.Update.Replace("{id:int}", userAux.Id.ToString()), userAux);
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
            await AuthenticateAsync(EUserRole.Administrator);
            var responseAux = await Client.GetAsync(_defaultRoute + ApiRoutes.Users.GetById.Replace("{id:int}", "2"));
            var userAux = await responseAux.Content.ReadAsAsync<User>();

            userAux.UserName = "novonomedeusuario";

            //Act
            var response = await Client.PutAsJsonAsync(_defaultRoute + ApiRoutes.Users.Update.Replace("{id:int}", userAux.Id.ToString()), userAux);
            var jsonModel = await response.Content.ReadFromJsonAsync<JsonModel>();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            jsonModel.Message.Should().Be(Messages.NAO_E_PERMITIDO_ALTERAR_NOME_DE_USUARIO);
        }

        [Fact]
        public async Task Update_Nao_Deve_Atualizar_Usuario_Com_Id_Diferente_Do_Editado_Retornando_NotFound()
        {
            //Arrange
            await AuthenticateAsync(EUserRole.Administrator);
            var responseAux = await Client.PostAsJsonAsync(_defaultRoute + ApiRoutes.Users.Create, _newMemberUser);
            var userAux = await responseAux.Content.ReadAsAsync<User>();

            responseAux.StatusCode.Should().Be(HttpStatusCode.OK);
            userAux.Should().NotBeNull();

            //Act
            userAux.Name = "updatedUser";
            userAux.Role = EUserRole.Editor;

            var response = await Client.PutAsJsonAsync(_defaultRoute + ApiRoutes.Users.Update.Replace("{id:int}", "9999"), userAux);
            var jsonModel = await response.Content.ReadFromJsonAsync<JsonModel>();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            jsonModel.Message.Should().Be(Messages.USUARIO_NAO_ENCONTRADO);
        }

        [Fact]
        public async Task Update_Deve_Atualizar_Usuario_Existente_Com_Nova_Senha()
        {
            //Arrange
            await AuthenticateAsync(EUserRole.Administrator);
            var responseAux = await Client.PostAsJsonAsync(_defaultRoute + ApiRoutes.Users.Create, _newMemberUser);
            var userAux = await responseAux.Content.ReadAsAsync<User>();

            responseAux.StatusCode.Should().Be(HttpStatusCode.OK);
            userAux.Should().NotBeNull();

            userAux.Password = "@ABC123";

            //Act
            var response = await Client.PutAsJsonAsync(_defaultRoute + ApiRoutes.Users.Update.Replace("{id:int}", userAux.Id.ToString()), userAux);
            var user = await response.Content.ReadAsAsync<User>();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            user.Id.Should().BeGreaterThan(0);
            user.Name.Should().Be(userAux.Name);
            user.Password.Should().Be(MD5Hash.CalculaHash("@ABC123"));
            user.Role.Should().Be(userAux.Role);
        }

        #endregion

        #region Delete

        [Fact]
        public async Task Delete_Deve_Apagar_Usuario_Existente_Retornando_Mensagem()
        {
            //Arrange
            await AuthenticateAsync(EUserRole.Administrator);
            var responseAux = await Client.PostAsJsonAsync(_defaultRoute + ApiRoutes.Users.Create, _newMemberUser);
            var userAux = await responseAux.Content.ReadAsAsync<User>();

            responseAux.StatusCode.Should().Be(HttpStatusCode.OK);
            userAux.Should().NotBeNull();

            //Act
            var response = await Client.DeleteAsync(_defaultRoute + ApiRoutes.Users.Delete.Replace("{id:int}", userAux.Id.ToString()));
            var jsonModel = await response.Content.ReadFromJsonAsync<JsonModel>();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            jsonModel.Message.Should().Be(Messages.USUARIO_REMOVIDO_COM_SUCESSO);
        }

        [Fact]
        public async Task Delete_Nao_Deve_Apagar_Usuario_Que_Possui_Relacionamentos()
        {
            //Arrange
            await AuthenticateAsync(EUserRole.Administrator);
            var responseAux = await Client.GetAsync(_defaultRoute + ApiRoutes.Users.GetByIdWithRelationship.Replace("{id:int}", "5"));
            var user = await responseAux.Content.ReadFromJsonAsync<User>();

            //Act
            var response = await Client.DeleteAsync(_defaultRoute + ApiRoutes.Users.Delete.Replace("{id:int}", user.Id.ToString()));
            var jsonModel = await response.Content.ReadFromJsonAsync<JsonModel>();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            jsonModel.Message.Should().Be(Messages.NAO_FOI_POSSIVEL_REMOVER_USUARIO_POSSUI_ARTIGOS_OU_VIDEOS_CADASTRADOS);
        }

        #endregion

        #region GetById

        [Fact]
        public async Task GetById_Deve_Retornar_UM_Usuario_Do_Banco()
        {
            //Arrange
            await AuthenticateAsync(EUserRole.Administrator);

            var responseAux = await Client.PostAsJsonAsync(_defaultRoute + ApiRoutes.Users.Create, _newMemberUser);
            var userAux = await responseAux.Content.ReadAsAsync<User>();

            responseAux.StatusCode.Should().Be(HttpStatusCode.OK);
            userAux.Should().NotBeNull();

            //Act
            var response = await Client.GetAsync(_defaultRoute + ApiRoutes.Users.GetById.Replace("{id:int}", userAux.Id.ToString()));
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
            await AuthenticateAsync(EUserRole.Administrator);

            //Act
            var response = await Client.GetAsync(_defaultRoute + ApiRoutes.Users.GetById.Replace("{id:int}", "9999"));
            var jsonModel = await response.Content.ReadFromJsonAsync<JsonModel>();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            jsonModel.Message.Should().Be(Messages.USUARIO_NAO_ENCONTRADO);
        }

        #endregion

        #region GetByIdWithRelationship

        [Fact]
        public async Task GetByIdWithRelationship_Deve_Retornar_Usuario_Com_Artigos_Relacionados()
        {
            //Arrange
            await AuthenticateAsync(EUserRole.Administrator);

            //Act
            var response = await Client.GetAsync(_defaultRoute + ApiRoutes.Users.GetByIdWithRelationship.Replace("{id:int}", "5"));
            var user = await response.Content.ReadFromJsonAsync<User>();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            user.Id.Should().Be((int)EDummyTestId.User5Maria);
            user.Articles.Should().HaveCount(2);
        }

        #endregion

        #region GetAll

        [Fact]
        public async Task GetAll_Deve_Retornar_Todos_Os_Usuarios()
        {
            //Arrange
            await AuthenticateAsync(EUserRole.Administrator);

            //Act
            var response = await Client.GetAsync(_defaultRoute + ApiRoutes.Users.GetAll);
            var users = await response.Content.ReadAsAsync<List<User>>();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            users.Should().HaveCount(5);
        }

        #endregion

        #region Authenticate

        [Fact]
        public async Task Authenticate_Deve_Retornar_Token_De_Autenticacao_Para_Usuario_Logado_Como_ADMINISTRADOR()
        {
            //Act
            await AuthenticateAsync(EUserRole.Administrator);

            //Assert
            Client.DefaultRequestHeaders.Authorization.Should().NotBeNull();
            Client.DefaultRequestHeaders.Authorization.Scheme.Should().Be("bearer");
            Client.DefaultRequestHeaders.Authorization.Parameter.Length.Should().BeGreaterThan(100);
        }

        [Fact]
        public async Task Authenticate_Deve_Retornar_Token_De_Autenticacao_Para_Usuario_Logado_Como_EDITOR()
        {
            //Act
            await AuthenticateAsync(EUserRole.Editor);

            //Assert
            Client.DefaultRequestHeaders.Authorization.Should().NotBeNull();
            Client.DefaultRequestHeaders.Authorization.Scheme.Should().Be("bearer");
            Client.DefaultRequestHeaders.Authorization.Parameter.Length.Should().BeGreaterThan(100);
        }

        [Fact]
        public async Task Authenticate_Deve_Retornar_Token_De_Autenticacao_Para_Usuario_Logado_Como_MEMBER()
        {
            //Act
            await AuthenticateAsync(EUserRole.Member);

            //Assert
            Client.DefaultRequestHeaders.Authorization.Should().NotBeNull();
            Client.DefaultRequestHeaders.Authorization.Scheme.Should().Be("bearer");
            Client.DefaultRequestHeaders.Authorization.Parameter.Length.Should().BeGreaterThan(100);
        }

        [Fact]
        public async Task Authenticate_Deve_Retornar_Mensagem_Usuario_Ou_Senha_Invalidos()
        {
            //Act
            var response = await Client.PostAsJsonAsync(_baseUrl + "v1/users/" + ApiRoutes.Users.Authenticate, new
            {
                UserName = "leolandrooo",
                Password = "xpto"
            });

            var jsonModel = await response.Content.ReadFromJsonAsync<JsonModel>();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            jsonModel.Message.Should().Be(Messages.USUARIO_OU_SENHA_INVALIDOS);
            jsonModel.Token.Should().BeNullOrEmpty();
        }

        [Fact]
        public async Task Authenticate_Deve_Retornar_Status_Proibido_Quando_Tentar_Criar_Novo_Usuario_Logado_Com_Funcao_Membro()
        {
            //Arrange
            await AuthenticateAsync(EUserRole.Member);

            //Act
            var response = await Client.PostAsJsonAsync(_defaultRoute + ApiRoutes.Users.Create, _newMemberUser);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        #endregion
    }
}

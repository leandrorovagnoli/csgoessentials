using CsgoEssentials.Domain.Entities;
using CsgoEssentials.Domain.Enum;
using CsgoEssentials.Infra.Utils;
using CsgoEssentials.IntegrationTests.Config;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace CsgoEssentials.IntegrationTests.ArticleTests
{
    public class ArticleTest : IntegrationTestConfig
    {
        private Article _newArticle;
        private List<Article> _newArticle2;
        private User _newAdminUser;


        public ArticleTest()
        {
            // Instancias para usar como teste
            _newArticle = new Article(
                "Outro",
                new DateTime(2000, 20, 2, 20, 45, 3, 500),
                "Não sei",
                _newAdminUser
                );


            _newArticle2 = new List<Article>
            {
                new Article("Novo",
                new DateTime(2000, 20, 2, 20, 45, 3, 500),
                "I DONT NOW",
                _newAdminUser),

                new Article("Smoke",
                new DateTime(1990, 12, 13, 20, 45, 3, 500),
                "Sei la amigo",
                _newAdminUser)
            };



            _newAdminUser = new User(
             "Leandro",
             "leo@leo.com",
             "leandro",
             "@123456*",
             EUserRole.Administrator);

        }

        [Fact]
        public async Task Create_Deve_retornar_O_Artigo_criado()
        {
            //Arrange
            await AuthenticateAsync();

            //Act
            var response = await Client.PostAsJsonAsync(ApiRoutes.Articles.Create, _newArticle);
            var article = await response.Content.ReadAsAsync<Article>();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK); // Statuscode deve ser Ok
            article.Id.Should().BeGreaterThan(0); // ID deve ser maior que 0
            article.Title.Should().Be(_newArticle.Title); // Article title deve ser 
            article.ReleaseDate.Should().Be(_newArticle.ReleaseDate);
            article.Description.Should().Be(_newArticle.Description);
            article.User.Should().Be(_newArticle.User);


        }

        [Fact]
        public async Task GetById_Deve_Retornar_Um_Unico_Artigo()
        {
            //Arrange
            await AuthenticateAsync();

            var responseAux = await Client.PostAsJsonAsync(ApiRoutes.Articles.Create, _newArticle);
            var oneArticle = await responseAux.Content.ReadAsAsync<Article>();

            responseAux.StatusCode.Should().Be(HttpStatusCode.OK);
            oneArticle.Should().NotBeNull().And.Equals(_newArticle);

            //Act
            var response = await Client.GetAsync(ApiRoutes.Articles.GetById.Replace("{articleId}", oneArticle.Id.ToString()));
            var article = await response.Content.ReadAsAsync<Article>();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            article.Id.Should().BeGreaterThan(0);
            article.Title.Should().Be(_newArticle.Title);
            article.ReleaseDate.CompareTo(_newArticle.ReleaseDate);
            article.Description.ToUpper().Should().NotBe(_newArticle.Description);
        }

        [Fact]

        public async Task GetAll_Deve_Exibir_Todos_Artigos()
        {
            //Arrange
            await AuthenticateAsync();

            var responseAux = await Client.PostAsJsonAsync(ApiRoutes.Articles.Create, _newArticle);
            var allArticle = await responseAux.Content.ReadAsAsync<Article>();

            responseAux.StatusCode.Should().Be(HttpStatusCode.OK);
            allArticle.Should().NotBeNull().And.Equals(_newArticle);

            //Act
            var response = await Client.GetAsync(ApiRoutes.Articles.GetAll);
            var article = await response.Content.ReadAsAsync<Article>();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            article.Should().Be(allArticle);


        }

        [Fact]
        public async Task GetById_Deve_Retornar_Um_Artigo_Nao_Encontrado_Se_Não_Criado()
        {
            //Arrange
            await AuthenticateAsync();

            //Act
            var response = await Client.GetAsync(ApiRoutes.Articles.GetById.Replace("{articleId}", "9999"));
            var content = await response.Content.ReadAsStringAsync();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            content.Should().Be(Messages.ARTIGO_NAO_ENCONTRADO);
            content.Should().NotBeEmpty();

        }

        [Fact]
        public async Task Update_Deve_Atualizar_O_Campo_Title_Do_Artigo_Existente_No_Banco()
        {
            //Arrange
            await AuthenticateAsync();
            var responseAux = await Client.PostAsJsonAsync(ApiRoutes.Articles.Create, _newArticle);
            var articleResp = await responseAux.Content.ReadAsAsync<User>();

            responseAux.StatusCode.Should().Be(HttpStatusCode.OK);
            articleResp.Should().NotBeNull();

            articleResp.Title = "Modified Title";

            //Act
            var response = await Client.PutAsJsonAsync(ApiRoutes.Articles.Update.Replace("{articleId}", articleResp.Id.ToString()), articleResp);
            var title = await response.Content.ReadAsAsync<Article>();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            title.Id.Should().BeGreaterThan(0);
            title.Title.Should().Be("Modified Title");
            title.Title.Replace('a', 'b').Should().NotBe(articleResp.Title);


        }

        [Fact]
        public async Task Update_Nao_Deve_Atualizar_Usuario_Com_Id_Diferente_Do_Editado_Retornando_NotFound()
        {
            //Arrange
            await AuthenticateAsync();
            var responseAux = await Client.PostAsJsonAsync(ApiRoutes.Articles.Create, _newArticle);
            var articleResp = await responseAux.Content.ReadAsAsync<User>();

            responseAux.StatusCode.Should().Be(HttpStatusCode.OK);
            articleResp.Should().NotBeNull();

            //Act
            articleResp.Title = "Titulo Modificado";

            var response = await Client.PutAsJsonAsync(ApiRoutes.Articles.Update.Replace("{articleId}", "9999"), articleResp);
            var content = await response.Content.ReadAsStringAsync();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            content.Should().Contain(Messages.ARTIGO_NAO_ENCONTRADO);
        }

        [Fact]
        public async Task Update_Deve_Atualizar_O_Artigo_Existente_Com_Todos_Campos_Sem_Alterar_User()
        {
            //Arrange
            await AuthenticateAsync();
            var responseAux = await Client.PostAsJsonAsync(ApiRoutes.Articles.Create, _newArticle);
            var articleResp = await responseAux.Content.ReadAsAsync<Article>();

            responseAux.StatusCode.Should().Be(HttpStatusCode.OK);
            articleResp.Should().NotBeNull();

            articleResp.Title = "Gato vesgo taca HE";
            articleResp.ReleaseDate = new DateTime(2000, 30, 10, 20, 5, 3, 500);
            articleResp.Description = "Nova descrição adicionada";


            //Act
            var response = await Client.PutAsJsonAsync(ApiRoutes.Articles.Update.Replace("{articleId}", articleResp.Id.ToString()), articleResp);
            var article = await response.Content.ReadAsAsync<Article>();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            article.Id.Should().BeGreaterThan(0).And.Equals(articleResp.Id);
            article.Title.Should().Be(articleResp.Title);
            article.ReleaseDate.Should().Be(articleResp.ReleaseDate);
            article.Description.Should().Be(articleResp.Description);
            article.User.Should().BeSameAs(_newAdminUser);

        }


        [Fact]
        public async Task Delete_Deve_Apagar_Usuario_Existente_Retornando_Mensagem()
        {
            //Arrange
            await AuthenticateAsync();
            var responseAux = await Client.PostAsJsonAsync(ApiRoutes.Articles.Create, _newArticle);
            var userAux = await responseAux.Content.ReadAsAsync<User>();

            responseAux.StatusCode.Should().Be(HttpStatusCode.OK);
            userAux.Should().NotBeNull();

            //Act
            var response = await Client.DeleteAsync(ApiRoutes.Articles.Delete.Replace("{userId}", userAux.Id.ToString()));
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
            await Client.PostAsJsonAsync(ApiRoutes.Articles.Create, _newArticle);

            _newArticle.Name = string.Empty;

            //Act
            var response = await Client.PostAsJsonAsync(ApiRoutes.Articles.Create, _newArticle);
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
            await Client.PostAsJsonAsync(ApiRoutes.Articles.Create, _newArticle);

            _newArticle.UserName = string.Empty;

            //Act
            var response = await Client.PostAsJsonAsync(ApiRoutes.Articles.Create, _newArticle);
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
            await Client.PostAsJsonAsync(ApiRoutes.Articles.Create, _newArticle);

            _newArticle.Password = string.Empty;

            //Act
            var response = await Client.PostAsJsonAsync(ApiRoutes.Articles.Create, _newArticle);
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
            await Client.PostAsJsonAsync(ApiRoutes.Articles.Create, _newArticle);

            _newArticle.Name = "Leo";

            //Act
            var response = await Client.PostAsJsonAsync(ApiRoutes.Articles.Create, _newArticle);
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
            await Client.PostAsJsonAsync(ApiRoutes.Articles.Create, _newArticle);

            _newArticle.UserName = "nomedeusuariomuitograndenomedeusuariomuitograndenomedeusuariomuitogrande";

            //Act
            var response = await Client.PostAsJsonAsync(ApiRoutes.Articles.Create, _newArticle);
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
            await Client.PostAsJsonAsync(ApiRoutes.Articles.Create, _newArticle);

            _newArticle.Email = "abc.com";

            //Act
            var response = await Client.PostAsJsonAsync(ApiRoutes.Articles.Create, _newArticle);
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
            await Client.PostAsJsonAsync(ApiRoutes.Articles.Create, _newArticle);

            _newArticle.Password = "12345";

            //Act
            var response = await Client.PostAsJsonAsync(ApiRoutes.Articles.Create, _newArticle);
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
            await Client.PostAsJsonAsync(ApiRoutes.Articles.Create, _newArticle);

            _newArticle.UserName = "meu nome de usuario";

            //Act
            var response = await Client.PostAsJsonAsync(ApiRoutes.Articles.Create, _newArticle);
            var content = await response.Content.ReadAsStringAsync();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            content.Should().Contain(string.Format(Messages.CAMPO_INVALIDO, Messages.NOME_DE_USUARIO));
        }
    }
}

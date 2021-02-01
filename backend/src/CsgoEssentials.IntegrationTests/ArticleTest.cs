using CsgoEssentials.Domain.Entities;
using CsgoEssentials.Infra.Utils;
using CsgoEssentials.IntegrationTests.Config;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace CsgoEssentials.IntegrationTests.ArticleTests
{
    public class ArticleTest : IntegrationTestConfig
    {
        private Article _newArticle;

        public ArticleTest()
        {
            _newArticle = new Article(
               "Bind que facilita jogar a granada - JumpThrow",
               new DateTime(2021, 01, 10),
               "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo " +
               "ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis " +
               "dis parturient montes, nascetur ridiculus mus. Donec quam felis, ultricies " +
               "nec, pellentesque eu, pretium quis, sem. Nulla consequat massa quis enim.",
               5
               );
        }

        [Fact]
        public async Task Create_Deve_Retornar_O_Artigo_Criado()
        {
            //Arrange
            await AuthenticateAsync();

            //Act
            var response = await Client.PostAsJsonAsync(ApiRoutes.Articles.Create, _newArticle);
            var article = await response.Content.ReadAsAsync<Article>();


            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            article.Id.Should().BeGreaterThan(0);
            article.Title.Should().Be(_newArticle.Title);
            article.ReleaseDate.Should().Be(_newArticle.ReleaseDate);
            article.Description.Should().Be(_newArticle.Description);

        }

        [Fact]
        public async Task Create_Deve_Retornar_O_Artigo_Criado_E_Outros_Que_Ja_Foram_Criados_Por_Aquele_Usuario()
        {
            //Arrange
            await AuthenticateAsync();

            //Act
            var response = await Client.PostAsJsonAsync(ApiRoutes.Articles.Create, _newArticle);
            var article = await response.Content.ReadAsAsync<Article>();

            var responseUser = await Client.GetAsync(ApiRoutes.Users.GetByIdWithRelationship.Replace("{userId}", "5"));
            var user = await responseUser.Content.ReadAsAsync<User>();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            article.Id.Should().BeGreaterThan(0);
            article.Title.Should().Be(_newArticle.Title);
            article.ReleaseDate.Should().Be(_newArticle.ReleaseDate);
            article.Description.Should().Be(_newArticle.Description);
            user.Articles.Should().HaveCount(3);
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

            await AuthenticateAsync();

            //act
            var response = await Client.GetAsync(ApiRoutes.Articles.GetAll);
            var articles = await response.Content.ReadAsAsync<List<Article>>();


            //assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            articles.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetById_Deve_Retornar_Um_Artigo_Nao_Encontrado_Se_Não_Criado()
        {
            //Arrange
            await AuthenticateAsync();

            //Act
            var response = await Client.GetAsync(ApiRoutes.Articles.GetById.Replace("{articleId}", "9999"));
            var jsonModel = await response.Content.ReadFromJsonAsync<JsonModel>();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            jsonModel.Message.Should().Be(Messages.ARTIGO_NAO_ENCONTRADO);

        }

        [Fact]
        public async Task Update_Deve_Atualizar_O_Campo_Title_Do_Artigo_Existente_No_Banco()
        {
            //Arrange
            await AuthenticateAsync();

            var responseAux = await Client.GetAsync(ApiRoutes.Articles.GetById.Replace("{articleId}", "1"));
            var article = await responseAux.Content.ReadAsAsync<Article>();

            article.Title = "Bomba gira mais que tudo";

            //Act
            var response = await Client.PutAsJsonAsync(ApiRoutes.Articles.Update.Replace("{articleId}", article.Id.ToString()), article);
            var title = await response.Content.ReadAsAsync<Article>();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            title.Id.Should().BeGreaterThan(0);
            title.Title.Should().Be("Bomba gira mais que tudo");

        }

        [Fact]
        public async Task Update_Nao_Deve_Atualizar_Os_Campos_Do_Artigo_Com_Id_Diferente()
        {
            //Arrange
            await AuthenticateAsync();
            var responseAux = await Client.PostAsJsonAsync(ApiRoutes.Articles.Create, _newArticle);
            var articleResp = await responseAux.Content.ReadAsAsync<Article>();

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
        public async Task Update_Deve_Atualizar_Todos_Campos_Do_Artigo()
        {
            //Arrange
            await AuthenticateAsync();


            var responseAux = await Client.GetAsync(ApiRoutes.Articles.GetById.Replace("{articleId}", "1"));
            var article = await responseAux.Content.ReadAsAsync<Article>();

            article.Title = "Bomba gira mais que tudo";
            article.ReleaseDate = new DateTime(1980, 05, 02);
            article.Description = "Nova Description agora";


            //Act
            var response = await Client.PutAsJsonAsync(ApiRoutes.Articles.Update.Replace("{articleId}", article.Id.ToString()), article);
            var articleResult = await response.Content.ReadAsAsync<Article>();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            articleResult.Id.Should().BeGreaterThan(0);
            articleResult.Title.Should().Be("Bomba gira mais que tudo");
            articleResult.ReleaseDate.Should().Be(article.ReleaseDate);
            articleResult.Description.Should().Be(article.Description);

        }

        [Fact]
        public async Task Delete_Deve_Apagar_Um_Artigo_Existente_Retornando_Mensagem()
        {
            //Arrange
            await AuthenticateAsync();
            var responseAux = await Client.PostAsJsonAsync(ApiRoutes.Articles.Create, _newArticle);
            var articleAux = await responseAux.Content.ReadAsAsync<Article>();

            responseAux.StatusCode.Should().Be(HttpStatusCode.OK);
            articleAux.Should().NotBeNull();

            //Act
            var response = await Client.DeleteAsync(ApiRoutes.Articles.Delete.Replace("{articleId}", articleAux.Id.ToString()));
            var content = await response.Content.ReadAsStringAsync();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            content.Should().Contain(Messages.ARTIGO_REMOVIDO_COM_SUCESSO);
        }

        [Fact]
        public async Task Create_Deve_Invalidar_O_Title_Vazio()
        {
            //Arrange
            await AuthenticateAsync();
            await Client.PostAsJsonAsync(ApiRoutes.Articles.Create, _newArticle);

            _newArticle.Title = string.Empty;

            //Act
            var response = await Client.PostAsJsonAsync(ApiRoutes.Articles.Create, _newArticle);
            var content = await response.Content.ReadAsStringAsync();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            content.Should().Contain(Messages.CAMPO_OBRIGATORIO.Replace("{0}", Messages.TITULO));
        }

        [Fact]
        public async Task Create_Deve_Validar_Se_UserId_Estiver_Setado_De_Forma_Correta()
        {

            //Arrange
            await AuthenticateAsync();

            //Act
            var response = await Client.PostAsJsonAsync(ApiRoutes.Articles.Create, _newArticle);
            var article = await response.Content.ReadAsAsync<Article>();


            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            article.UserId.Should().BeGreaterThan(0).And.Equals(6);

        }

        [Fact] // VOLTAR AQUI
        public async Task Create_Deve_Invalidar_Se_Houver_UserId_Invalido()
        {
            //Arrange
            await AuthenticateAsync();

            _newArticle.UserId = 999;

            //Act
            var response = await Client.PostAsJsonAsync(ApiRoutes.Articles.Create, _newArticle);
            var content = await response.Content.ReadAsStringAsync();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        }

        // Criar TEST ****
        // Atualiar deve invalidar um campo menor que o obrigatorio ou maior, testar isso também.

        [Fact]
        public async Task Create_Deve_Invalidar_O_Campo_Title_Menor_Que_O_Permitido()
        {
            //Arrange
            await AuthenticateAsync();
            await Client.PostAsJsonAsync(ApiRoutes.Users.Create, _newArticle);

            _newArticle.Title = "oi";

            //Act
            var response = await Client.PostAsJsonAsync(ApiRoutes.Articles.Create, _newArticle);
            var content = await response.Content.ReadAsStringAsync();



            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            content.Should().Contain(string.Format(Messages.CAMPO_PRECISA_TER_ENTRE_X2_E_Y1_CARACTERES, Messages.TITULO, "200", "10"));
        }

        [Fact]
        public async Task Create_Deve_Invalidar_Nome_De_Usuario_Maior_Do_Que_Permitido()
        {
            //Arrange
            await AuthenticateAsync();
            await Client.PostAsJsonAsync(ApiRoutes.Articles.Create, _newArticle);

            _newArticle.Title = "Lorem ipsum odio auctor lorem augue lacus leo curae viverra nostra," +
                " netus per lacus senectus fames porta habitant pharetra tempor," +
                " egestas lorem tortor platea himenaeos hendrerit cras enim aliquam, heaas";

            //Act
            var response = await Client.PostAsJsonAsync(ApiRoutes.Articles.Create, _newArticle);
            var content = await response.Content.ReadAsStringAsync();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            content.Should().Contain(string.Format(Messages.CAMPO_PRECISA_TER_ENTRE_X2_E_Y1_CARACTERES, Messages.TITULO, "200", "10"));
        }

        [Fact]
        public async Task Create_Deve_Invalidar_Se_Campo_Description_Nao_For_Preenchido()
        {
            //Arrange
            await AuthenticateAsync();
            await Client.PostAsJsonAsync(ApiRoutes.Articles.Create, _newArticle);

            _newArticle.Description = null;

            //Act
            var response = await Client.PostAsJsonAsync(ApiRoutes.Articles.Create, _newArticle);
            var content = await response.Content.ReadAsStringAsync();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            content.Should().Contain(string.Format(Messages.CAMPO_OBRIGATORIO, Messages.DESCRICAO));
        }

        [Fact]
        public async Task Create_Deve_Invalidar_Description_Se_For_Menor_Do_Que_O_Permitido()
        {
            //Arrange
            await AuthenticateAsync();
            await Client.PostAsJsonAsync(ApiRoutes.Articles.Create, _newArticle);

            _newArticle.Description = "Ola";

            //Act
            var response = await Client.PostAsJsonAsync(ApiRoutes.Articles.Create, _newArticle);
            var content = await response.Content.ReadAsStringAsync();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            content.Should().Contain(string.Format(Messages.CAMPO_PRECISA_TER_ENTRE_X2_E_Y1_CARACTERES, Messages.DESCRICAO, "2000", "20"));
        }

        [Fact]
        public async Task GetById_Deve_Retornar_Um_Artigo_Existente_Com_Seu_Usuario_Associado()
        {
            //Arrange
            await AuthenticateAsync();

            //Act
            var response = await Client.GetAsync(ApiRoutes.Articles.GetByIdWithArticles.Replace("{articleId}", "1"));
            var article = await response.Content.ReadAsAsync<Article>();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            article.Id.Should().BeGreaterThan(0);
            article.Title.Should().Be(article.Title);
            article.ReleaseDate.CompareTo(article.ReleaseDate);
            article.Description.ToUpper().Should().NotBe(article.Description);
            article.User.Should().NotBeNull();
            article.User.UserName.Should().Be("maria");
        }
    }
}


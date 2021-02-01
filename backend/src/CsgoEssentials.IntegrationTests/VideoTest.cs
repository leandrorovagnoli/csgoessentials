using CsgoEssentials.Domain.Entities;
using CsgoEssentials.Domain.Enum;
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


namespace CsgoEssentials.IntegrationTests.VideoTests
{
    public class VideoTest : IntegrationTestConfig
    {
        private Video _newVideo;

        public VideoTest()
        {
            _newVideo = new Video(
               "Smoke Fundo D2",
                new DateTime(2021, 01, 10),
                EGrenadeType.Smoke,
                ETick.Tick128,
                "Video demonstracao de uma smoke fundo na d2",
                2,
                1);

            _newVideo = new Video(
               "Flash Fundo Mirage",
                new DateTime(2021, 01, 10),
                EGrenadeType.Flash,
                ETick.Tick128,
                "Video demonstracao de uma Flash na Mirage",
                2,
                2);
        }

        [Fact]
        public async Task Create_Deve_Retornar_O_Video_Criado()
        {
            //Arrange
            await AuthenticateAsync();

            //Act
            var response = await Client.PostAsJsonAsync(ApiRoutes.Videos.Create, _newVideo);
            var video = await response.Content.ReadAsAsync<Video>();


            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            video.Id.Should().BeGreaterThan(0);
            video.Title.Should().Be(_newVideo.Title);
            video.ReleaseDate.Should().Be(_newVideo.ReleaseDate);
            video.Description.Should().Be(_newVideo.Description);
        }

        [Fact]
        public async Task GetById_Deve_Retornar_Um_Unico_Video()
        {
            //Arrange
            await AuthenticateAsync();

            var responseAux = await Client.PostAsJsonAsync(ApiRoutes.Videos.Create, _newVideo);
            var oneVideo = await responseAux.Content.ReadAsAsync<Video>();

            responseAux.StatusCode.Should().Be(HttpStatusCode.OK);
            oneVideo.Should().NotBeNull().And.Equals(_newVideo);

            //Act
            var response = await Client.GetAsync(ApiRoutes.Videos.GetById.Replace("{articleId}", oneVideo.Id.ToString()));
            var video = await response.Content.ReadAsAsync<Video>();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            video.Id.Should().BeGreaterThan(0);
            video.Title.Should().Be(_newVideo.Title);
            video.ReleaseDate.CompareTo(_newVideo.ReleaseDate);
            video.Description.ToUpper().Should().NotBe(_newVideo.Description);
        }

        [Fact]
        public async Task GetAll_Deve_Exibir_Todos_Videos()
        {

            await AuthenticateAsync();

            //act
            var response = await Client.GetAsync(ApiRoutes.Videos.GetAll);
            var articles = await response.Content.ReadAsAsync<List<Video>>();


            //assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            articles.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetById_Deve_Retornar_Um_Video_Nao_Encontrado_Se_Não_Criado()
        {
            //Arrange
            await AuthenticateAsync();

            //Act
            var response = await Client.GetAsync(ApiRoutes.Videos.GetById.Replace("{articleId}", "9999"));
            var jsonModel = await response.Content.ReadFromJsonAsync<JsonModel>();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            jsonModel.Message.Should().Be(Messages.ARTIGO_NAO_ENCONTRADO);

        }

        [Fact]
        public async Task Update_Deve_Atualizar_O_Campo_Title_Do_Video_Existente_No_Banco()
        {
            //Arrange
            await AuthenticateAsync();

            var responseAux = await Client.GetAsync(ApiRoutes.Videos.GetById.Replace("{articleId}", "1"));
            var video = await responseAux.Content.ReadAsAsync<Video>();

            video.Title = "Bomba gira mais que tudo";

            //Act
            var response = await Client.PutAsJsonAsync(ApiRoutes.Videos.Update.Replace("{articleId}", video.Id.ToString()), video);
            var title = await response.Content.ReadAsAsync<Video>();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            title.Id.Should().BeGreaterThan(0);
            title.Title.Should().Be("Bomba gira mais que tudo");

        }

        [Fact]
        public async Task Update_Nao_Deve_Atualizar_Os_Campos_Do_Video_Com_Id_Diferente()
        {
            //Arrange
            await AuthenticateAsync();
            var responseAux = await Client.PostAsJsonAsync(ApiRoutes.Videos.Create, _newVideo);
            var articleResp = await responseAux.Content.ReadAsAsync<Video>();

            responseAux.StatusCode.Should().Be(HttpStatusCode.OK);
            articleResp.Should().NotBeNull();

            //Act
            articleResp.Title = "Titulo Modificado";

            var response = await Client.PutAsJsonAsync(ApiRoutes.Videos.Update.Replace("{articleId}", "9999"), articleResp);
            var content = await response.Content.ReadAsStringAsync();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            content.Should().Contain(Messages.ARTIGO_NAO_ENCONTRADO);
        }

        [Fact]
        public async Task Update_Deve_Atualizar_Todos_Campos_Do_Video()
        {
            //Arrange
            await AuthenticateAsync();


            var responseAux = await Client.GetAsync(ApiRoutes.Videos.GetById.Replace("{articleId}", "1"));
            var video = await responseAux.Content.ReadAsAsync<Video>();

            video.Title = "Bomba gira mais que tudo";
            video.ReleaseDate = new DateTime(1980, 05, 02);
            video.Description = "Nova Description agora";


            //Act
            var response = await Client.PutAsJsonAsync(ApiRoutes.Videos.Update.Replace("{articleId}", video.Id.ToString()), video);
            var articleResult = await response.Content.ReadAsAsync<Video>();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            articleResult.Id.Should().BeGreaterThan(0);
            articleResult.Title.Should().Be("Bomba gira mais que tudo");
            articleResult.ReleaseDate.Should().Be(video.ReleaseDate);
            articleResult.Description.Should().Be(video.Description);

        }

        [Fact]
        public async Task Delete_Deve_Apagar_Um_Video_Existente_Retornando_Mensagem()
        {
            //Arrange
            await AuthenticateAsync();
            var responseAux = await Client.PostAsJsonAsync(ApiRoutes.Videos.Create, _newVideo);
            var articleAux = await responseAux.Content.ReadAsAsync<Video>();

            responseAux.StatusCode.Should().Be(HttpStatusCode.OK);
            articleAux.Should().NotBeNull();

            //Act
            var response = await Client.DeleteAsync(ApiRoutes.Videos.Delete.Replace("{articleId}", articleAux.Id.ToString()));
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
            await Client.PostAsJsonAsync(ApiRoutes.Videos.Create, _newVideo);

            _newVideo.Title = string.Empty;

            //Act
            var response = await Client.PostAsJsonAsync(ApiRoutes.Videos.Create, _newVideo);
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
            var response = await Client.PostAsJsonAsync(ApiRoutes.Videos.Create, _newVideo);
            var video = await response.Content.ReadAsAsync<Video>();


            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            video.UserId.Should().BeGreaterThan(0).And.Equals(6);

        }

        [Fact] // VOLTAR AQUI
        public async Task Create_Deve_Invalidar_Se_Houver_UserId_Invalido()
        {
            //Arrange
            await AuthenticateAsync();

            _newVideo.UserId = 999;

            //Act
            var response = await Client.PostAsJsonAsync(ApiRoutes.Videos.Create, _newVideo);
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
            await Client.PostAsJsonAsync(ApiRoutes.Users.Create, _newVideo);

            _newVideo.Title = "oi";

            //Act
            var response = await Client.PostAsJsonAsync(ApiRoutes.Videos.Create, _newVideo);
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
            await Client.PostAsJsonAsync(ApiRoutes.Videos.Create, _newVideo);

            _newVideo.Title = "Lorem ipsum odio auctor lorem augue lacus leo curae viverra nostra," +
                " netus per lacus senectus fames porta habitant pharetra tempor," +
                " egestas lorem tortor platea himenaeos hendrerit cras enim aliquam, heaas";

            //Act
            var response = await Client.PostAsJsonAsync(ApiRoutes.Videos.Create, _newVideo);
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
            await Client.PostAsJsonAsync(ApiRoutes.Videos.Create, _newVideo);

            _newVideo.Description = null;

            //Act
            var response = await Client.PostAsJsonAsync(ApiRoutes.Videos.Create, _newVideo);
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
            await Client.PostAsJsonAsync(ApiRoutes.Videos.Create, _newVideo);

            _newVideo.Description = "Ola";

            //Act
            var response = await Client.PostAsJsonAsync(ApiRoutes.Videos.Create, _newVideo);
            var content = await response.Content.ReadAsStringAsync();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            content.Should().Contain(string.Format(Messages.CAMPO_PRECISA_TER_ENTRE_X2_E_Y1_CARACTERES, Messages.DESCRICAO, "2000", "20"));
        }        

        [Fact]
        public async Task Teste_Deve_Retornar_Todos_Os_Videos_Associados_Com_Usuario_Especifico()
        {
            //Arrange
            await AuthenticateAsync();

            //Act
            var response = await Client.PostAsJsonAsync(ApiRoutes.Videos.Create, _newVideo);
            var video = await response.Content.ReadAsAsync<Video>();

            var responseUser = await Client.GetAsync(ApiRoutes.Users.GetByIdWithUserVideos.Replace("{userId}", "1"));
            var user = await responseUser.Content.ReadAsAsync<User>();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            user.Videos.Should().HaveCount(2);
        }

        [Fact]
        public async Task Teste_Deve_Retornar_Todos_Os_Videos_Associados_Com_Mapa_Especifico()
        {
            //Arrange
            await AuthenticateAsync();

            //Act
            var response = await Client.PostAsJsonAsync(ApiRoutes.Videos.Create, _newVideo);
            var video = await response.Content.ReadAsAsync<Video>();

            var responseMap = await Client.GetAsync(ApiRoutes.Maps.GetByIdWithVideos.Replace("{mapId}", "1"));
            var map = await responseMap.Content.ReadAsAsync<Map>();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            map.Videos.Should().HaveCount(2);
        }
    }
}


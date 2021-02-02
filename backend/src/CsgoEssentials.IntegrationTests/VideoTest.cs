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
               "Flash Fundo Mirage",
                new DateTime(2021, 01, 10),
                EGrenadeType.Flash,
                ETick.Tick128,
                "Video demonstracao de uma Flash na Mirage",
                "http://www.videotesteprj.com",
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
            oneVideo.Should().NotBeNull();

            //Act
            var response = await Client.GetAsync(ApiRoutes.Videos.GetById.Replace("{videoId}", oneVideo.Id.ToString()));
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
            var videos = await response.Content.ReadAsAsync<List<Video>>();

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            videos.Should().HaveCount(3);
        }

        [Fact]
        public async Task GetById_Deve_Retornar_Um_Video_Existente_Com_Seu_Usuario_Associado()
        {
            //Arrange
            await AuthenticateAsync();

            //Act
            var response = await Client.GetAsync(ApiRoutes.Videos.GetByIdWithRelationship.Replace("{videoId}", "1"));
            var video = await response.Content.ReadAsAsync<Video>();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            video.Id.Should().BeGreaterThan(0);
            video.User.Should().NotBeNull();
            video.User.UserName.Should().Be("leolandrooo");
        }

        [Fact]
        public async Task GetById_Deve_Retornar_Um_Erro_Nao_Encontrado_Se_O_Video_Nao_Existir()
        {
            //Arrange
            await AuthenticateAsync();

            //Act
            var response = await Client.GetAsync(ApiRoutes.Videos.GetById.Replace("{videoId}", "9999"));
            var jsonModel = await response.Content.ReadFromJsonAsync<JsonModel>();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            jsonModel.Message.Should().Be(Messages.VIDEO_NAO_ENCONTRADO);

        }

        [Fact]
        public async Task Update_Deve_Atualizar_O_Campo_Title_Do_Video_Existente_No_Banco()
        {
            //Arrange
            await AuthenticateAsync();

            var responseAux = await Client.GetAsync(ApiRoutes.Videos.GetById.Replace("{videoId}", "1"));
            var video = await responseAux.Content.ReadAsAsync<Video>();

            video.Title = "Video da bomba gira muito";

            //Act
            var response = await Client.PutAsJsonAsync(ApiRoutes.Videos.Update.Replace("{videoId}", video.Id.ToString()), video);
            var title = await response.Content.ReadAsAsync<Video>();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            title.Id.Should().BeGreaterThan(0);
            title.Title.Should().Be("Video da bomba gira muito");

        }

        [Fact]
        public async Task Update_Nao_Deve_Atualizar_Os_Campos_Do_Video_Com_Id_Diferente()
        {
            //Arrange
            await AuthenticateAsync();
            var responseAux = await Client.PostAsJsonAsync(ApiRoutes.Videos.Create, _newVideo);
            var videoResp = await responseAux.Content.ReadAsAsync<Video>();

            responseAux.StatusCode.Should().Be(HttpStatusCode.OK);
            videoResp.Should().NotBeNull();

            //Act
            videoResp.Title = "Titulo do video modificado";

            var response = await Client.PutAsJsonAsync(ApiRoutes.Videos.Update.Replace("{videoId}", "9999"), videoResp);
            var content = await response.Content.ReadAsStringAsync();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            content.Should().Contain(Messages.VIDEO_NAO_ENCONTRADO);
        }

        [Fact]
        public async Task Update_Deve_Atualizar_Todos_Campos_Do_Video()
        {
            //Arrange
            await AuthenticateAsync();

            var responseAux = await Client.GetAsync(ApiRoutes.Videos.GetById.Replace("{videoId}", "1"));
            var video = await responseAux.Content.ReadAsAsync<Video>();

            video.Title = "Titulo novo";
            video.ReleaseDate = new DateTime(1980, 05, 02);
            video.Description = "Nova Description agora";

            //Act
            var response = await Client.PutAsJsonAsync(ApiRoutes.Videos.Update.Replace("{videoId}", video.Id.ToString()), video);
            var articleResult = await response.Content.ReadAsAsync<Video>();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            articleResult.Id.Should().BeGreaterThan(0);
            articleResult.Title.Should().Be("Titulo novo");
            articleResult.ReleaseDate.Should().Be(video.ReleaseDate);
            articleResult.Description.Should().Be(video.Description);

        }

        [Fact]
        public async Task Delete_Deve_Apagar_Um_Video_Existente_Retornando_Mensagem()
        {
            //Arrange
            await AuthenticateAsync();
            var responseAux = await Client.PostAsJsonAsync(ApiRoutes.Videos.Create, _newVideo);
            var videoAux = await responseAux.Content.ReadAsAsync<Video>();

            responseAux.StatusCode.Should().Be(HttpStatusCode.OK);
            videoAux.Should().NotBeNull();

            //Act
            var response = await Client.DeleteAsync(ApiRoutes.Videos.Delete.Replace("{videoId}", videoAux.Id.ToString()));
            var content = await response.Content.ReadAsStringAsync();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            content.Should().Contain(Messages.VIDEO_REMOVIDO_COM_SUCESSO);
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
        public async Task Create_Deve_Validar_Se_UserId_Estiver_Setado()
        {
            //Arrange
            await AuthenticateAsync();

            //Act
            var response = await Client.PostAsJsonAsync(ApiRoutes.Videos.Create, _newVideo);
            var video = await response.Content.ReadAsAsync<Video>();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            video.UserId.Should().BeGreaterThan(0);
        }

        [Fact]
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
            content.Should().Contain(Messages.USUARIO_NAO_ENCONTRADO);
        }

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
            content.Should().Contain(string.Format(Messages.CAMPO_PRECISA_TER_ENTRE_X2_E_Y1_CARACTERES, Messages.TITULO, "60", "4"));
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
            content.Should().Contain(string.Format(Messages.CAMPO_PRECISA_TER_ENTRE_X2_E_Y1_CARACTERES, Messages.TITULO, "60", "4"));
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
            content.Should().Contain(string.Format(Messages.CAMPO_PRECISA_TER_ENTRE_X2_E_Y1_CARACTERES, Messages.DESCRICAO, "120", "20"));
        }

        [Fact]
        public async Task Teste_Deve_Retornar_Todos_Os_Videos_Associados_Com_Usuario_Especifico()
        {
            //Arrange
            await AuthenticateAsync();

            //Act
            var responseUser = await Client.GetAsync(ApiRoutes.Users.GetByIdWithRelationship.Replace("{userId}", "1"));
            var user = await responseUser.Content.ReadAsAsync<User>();

            //Assert
            responseUser.StatusCode.Should().Be(HttpStatusCode.OK);
            user.Videos.Should().HaveCount(3);
        }

        [Fact]
        public async Task Teste_Deve_Retornar_Todos_Os_Videos_Associados_Com_Mapa_Especifico()
        {
            //Arrange
            await AuthenticateAsync();

            //Act
            var responseMap = await Client.GetAsync(ApiRoutes.Maps.GetByIdWithRelationship.Replace("{mapId}", "1"));
            var map = await responseMap.Content.ReadAsAsync<Map>();

            //Assert
            responseMap.StatusCode.Should().Be(HttpStatusCode.OK);
            map.Videos.Should().HaveCount(3);
        }

        // FAZER A ESTRUTURA do INICIO.
        [Fact] 
        public async Task Deve_Retornar_Videos_De_Um_Mapa_Com_Uma_Granada_Especifica()
        {
            //Arrange
            await AuthenticateAsync();

        }

        [Fact]
        public async Task Deve_Retornar_Um_Video_De_Um_Mapa_Com_Um_Tipo_De_Granada_E_Um_TickRate_Especifico()
        {
            //Arrange
            await AuthenticateAsync();
        }

        [Fact]
        public async Task Deve_Retornar_Videos_Com_Tick64()
        {
            //Arrange
            await AuthenticateAsync();
        }

        [Fact]
        public async Task Deve_Retornar_Videos_Com_Tick128()
        {
            //Arrange
            await AuthenticateAsync();
        }

        [Fact]
        public async Task Deve_Retornar_Videos_Com_Um_Tipo_De_Granada_Especifico()
        {
            //Arrange
            await AuthenticateAsync();
        }

        [Fact]
        public async Task Deve_Retornar_Videos_Com_Um_Tipo_De_Granada_E_TickRate_Especifico()
        {
            //Arrange
            await AuthenticateAsync();
        }

        [Fact]
        public async Task Deve_Retornar_Todos_Videos_Com_TickRate_Especifico()
        {
            //Arrange
            await AuthenticateAsync();
        }

        [Fact]
        public async Task Não_Deve_Retornar_Um_Video_Se_O_Mapa_For_Diferente_Do_Informado()
        {
            //Arrange
            await AuthenticateAsync();
        }

        [Fact]
        public async Task Nao_Deve_Retornar_Um_Video_Se_O_Tick_Informado_For_Diferente()
        {
            //Arrange
            await AuthenticateAsync();
        }

        [Fact]
        public async Task Atualizar_Deve_Invalidar_Um_Campo_Menor_Que_O_Obrigatorio()
        {
            //Arrange
            await AuthenticateAsync();
        }

        [Fact]
        public async Task Atualizar_Deve_Invalidar_Um_Campo_Maior_Que_O_Obrigatorio()
        {
            //Arrange
            await AuthenticateAsync();
        }

        [Fact]
        public async Task Delete_Nao_Deve_Deletar_O_Map_Se_Usuario_For_Diferente_De_Admin()
        {
            //Arrange
            await AuthenticateAsync();
        }

        [Fact]
        public async Task Delete_Deve_Validar_Se_Usuario_For_Admin()
        {
            //Arrange
            await AuthenticateAsync();
        }

        [Fact]
        public async Task Criar_Deve_Invalidar_Se_Não_Existir_Um_Mapa_Associado_Ao_Video()
        {
            //Arrange
            await AuthenticateAsync();
        }       
        
        [Fact]
        public async Task Criar_Deve_Invalidar_Se_Não_Existir_Um_Usuario_Associado_Ao_Video()
        {
            //Arrange
            await AuthenticateAsync();
        }   
        
        [Fact]
        public async Task Criar_Deve_Invalidar_Se_Não_Existir_Um_Mapa_E_Usuario_Associado_Ao_Video()
        {
            //Arrange
            await AuthenticateAsync();
        }

    }
}


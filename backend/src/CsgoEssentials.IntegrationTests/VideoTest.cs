using CsgoEssentials.Domain.Entities;
using CsgoEssentials.Domain.Enum;
using CsgoEssentials.Infra.Data;
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
        private string _defaultRoute;

        public VideoTest()
        {
            //getting route: ..localhost:5001/v1/videos/
            _defaultRoute = _baseUrl + ApiRoutes.Videos.Route + "/";

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

        #region Create

        [Fact]
        public async Task Create_Deve_Retornar_O_Video_Criado()
        {
            //Arrange
            await AuthenticateAsync();

            //Act
            var response = await Client.PostAsJsonAsync(_defaultRoute + ApiRoutes.Videos.Create, _newVideo);
            var video = await response.Content.ReadAsAsync<Video>();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            video.Id.Should().BeGreaterThan(0);
            video.Title.Should().Be(_newVideo.Title);
            video.ReleaseDate.Should().Be(_newVideo.ReleaseDate);
            video.Description.Should().Be(_newVideo.Description);
        }

        [Fact]
        public async Task Create_Deve_Invalidar_O_Title_Vazio()
        {
            //Arrange
            await AuthenticateAsync();
            await Client.PostAsJsonAsync(_defaultRoute + ApiRoutes.Videos.Create, _newVideo);

            _newVideo.Title = string.Empty;

            //Act
            var response = await Client.PostAsJsonAsync(_defaultRoute + ApiRoutes.Videos.Create, _newVideo);
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
            var response = await Client.PostAsJsonAsync(_defaultRoute + ApiRoutes.Videos.Create, _newVideo);
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
            var response = await Client.PostAsJsonAsync(_defaultRoute + ApiRoutes.Videos.Create, _newVideo);
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
            await Client.PostAsJsonAsync(_defaultRoute + ApiRoutes.Videos.Create, _newVideo);

            _newVideo.Title = "oi";

            //Act
            var response = await Client.PostAsJsonAsync(_defaultRoute + ApiRoutes.Videos.Create, _newVideo);
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
            await Client.PostAsJsonAsync(_defaultRoute + ApiRoutes.Videos.Create, _newVideo);

            _newVideo.Title = "Lorem ipsum odio auctor lorem augue lacus leo curae viverra nostra," +
                " netus per lacus senectus fames porta habitant pharetra tempor," +
                " egestas lorem tortor platea himenaeos hendrerit cras enim aliquam, heaas";

            //Act
            var response = await Client.PostAsJsonAsync(_defaultRoute + ApiRoutes.Videos.Create, _newVideo);
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
            await Client.PostAsJsonAsync(_defaultRoute + ApiRoutes.Videos.Create, _newVideo);

            _newVideo.Description = null;

            //Act
            var response = await Client.PostAsJsonAsync(_defaultRoute + ApiRoutes.Videos.Create, _newVideo);
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
            await Client.PostAsJsonAsync(_defaultRoute + ApiRoutes.Videos.Create, _newVideo);

            _newVideo.Description = "Ola";

            //Act
            var response = await Client.PostAsJsonAsync(_defaultRoute + ApiRoutes.Videos.Create, _newVideo);
            var content = await response.Content.ReadAsStringAsync();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            content.Should().Contain(string.Format(Messages.CAMPO_PRECISA_TER_ENTRE_X2_E_Y1_CARACTERES, Messages.DESCRICAO, "120", "20"));
        }

        #endregion

        #region Update

        [Fact]
        public async Task Update_Deve_Atualizar_O_Campo_Title_Do_Video_Existente_No_Banco()
        {
            //Arrange
            await AuthenticateAsync();

            var responseAux = await Client.GetAsync(_defaultRoute + ApiRoutes.Videos.GetById.Replace("{id:int}", "1"));
            var video = await responseAux.Content.ReadAsAsync<Video>();

            video.Title = "Video da bomba gira muito";

            //Act
            var response = await Client.PutAsJsonAsync(_defaultRoute + ApiRoutes.Videos.Update.Replace("{id:int}", video.Id.ToString()), video);
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
            var responseAux = await Client.PostAsJsonAsync(_defaultRoute + ApiRoutes.Videos.Create, _newVideo);
            var videoResp = await responseAux.Content.ReadAsAsync<Video>();

            responseAux.StatusCode.Should().Be(HttpStatusCode.OK);
            videoResp.Should().NotBeNull();

            //Act
            videoResp.Title = "Titulo do video modificado";

            var response = await Client.PutAsJsonAsync(_defaultRoute + ApiRoutes.Videos.Update.Replace("{id:int}", "9999"), videoResp);
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

            var responseAux = await Client.GetAsync(_defaultRoute + ApiRoutes.Videos.GetById.Replace("{id:int}", "1"));
            var video = await responseAux.Content.ReadAsAsync<Video>();

            video.Title = "Titulo novo";
            video.ReleaseDate = new DateTime(1980, 05, 02);
            video.Description = "Nova Description agora";

            //Act
            var response = await Client.PutAsJsonAsync(_defaultRoute + ApiRoutes.Videos.Update.Replace("{id:int}", video.Id.ToString()), video);
            var articleResult = await response.Content.ReadAsAsync<Video>();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            articleResult.Id.Should().BeGreaterThan(0);
            articleResult.Title.Should().Be("Titulo novo");
            articleResult.ReleaseDate.Should().Be(video.ReleaseDate);
            articleResult.Description.Should().Be(video.Description);

        }

        #endregion

        #region Delete

        [Fact]
        public async Task Delete_Deve_Apagar_Um_Video_Existente_Retornando_Mensagem()
        {
            //Arrange
            await AuthenticateAsync();
            var responseAux = await Client.PostAsJsonAsync(_defaultRoute + ApiRoutes.Videos.Create, _newVideo);
            var videoAux = await responseAux.Content.ReadAsAsync<Video>();

            responseAux.StatusCode.Should().Be(HttpStatusCode.OK);
            videoAux.Should().NotBeNull();

            //Act
            var response = await Client.DeleteAsync(_defaultRoute + ApiRoutes.Videos.Delete.Replace("{id:int}", videoAux.Id.ToString()));
            var content = await response.Content.ReadAsStringAsync();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            content.Should().Contain(Messages.VIDEO_REMOVIDO_COM_SUCESSO);
        }

        #endregion

        #region GetById

        [Fact]
        public async Task GetById_Deve_Retornar_Um_Unico_Video()
        {
            //Act
            var response = await Client.GetAsync(_defaultRoute + ApiRoutes.Videos.GetById.Replace("{id:int}", "1"));
            var video = await response.Content.ReadAsAsync<Video>();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            video.Id.Should().Be((int)EDummyTestId.Video1Dust2UserLeolandroooSmokeTick128);
            video.Title.Should().Be(video.Title);
            video.ReleaseDate.CompareTo(video.ReleaseDate);
            video.Description.Should().Be(video.Description);
        }

        [Fact]
        public async Task GetById_Deve_Retornar_Um_Video_Existente_Com_Seu_Usuario_Associado()
        {
            //Act
            var response = await Client.GetAsync(_defaultRoute + ApiRoutes.Videos.GetByIdWithRelationship.Replace("{id:int}", "1"));
            var video = await response.Content.ReadAsAsync<Video>();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            video.Id.Should().Be((int)EDummyTestId.Video1Dust2UserLeolandroooSmokeTick128);
            video.User.Should().NotBeNull();
            video.User.UserName.Should().Be("leolandrooo");
        }

        [Fact]
        public async Task GetById_Deve_Retornar_Video_Nao_Encontrado_Se_O_Video_Nao_Existir()
        {
            //Act
            var response = await Client.GetAsync(_defaultRoute + ApiRoutes.Videos.GetById.Replace("{id:int}", "9999"));
            var jsonModel = await response.Content.ReadFromJsonAsync<JsonModel>();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            jsonModel.Message.Should().Be(Messages.VIDEO_NAO_ENCONTRADO);

        }

        #endregion

        #region GetAll

        [Fact]
        public async Task GetAll_Deve_Exibir_Todos_Videos()
        {
            //Act
            var response = await Client.GetAsync(_defaultRoute + ApiRoutes.Videos.GetAll);
            var videos = await response.Content.ReadAsAsync<List<Video>>();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            videos.Should().HaveCount(5);
        }

        #endregion

        #region Filter

        [Fact]
        public async Task Filter_Deve_Retornar_Videos_De_Um_Usuario_Especifico()
        {
            //Arrange
            var query = $"?userid={(int)EDummyTestId.User1Leolandrooo}";

            //Act
            var response = await Client.GetAsync(_defaultRoute + ApiRoutes.Videos.Filter + query);
            var videos = await response.Content.ReadAsAsync<List<Video>>();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            videos[0].UserId.Should().Be((int)EDummyTestId.User1Leolandrooo);
            videos[1].UserId.Should().Be((int)EDummyTestId.User1Leolandrooo);
            videos[2].UserId.Should().Be((int)EDummyTestId.User1Leolandrooo);
            videos[3].UserId.Should().Be((int)EDummyTestId.User1Leolandrooo);
            videos[4].UserId.Should().Be((int)EDummyTestId.User1Leolandrooo);
        }

        [Fact]
        public async Task Filter_Deve_Retornar_Videos_De_Um_Mapa_Especifico()
        {
            //Arrange
            var query = $"?mapid={(int)EDummyTestId.Map3Mirage}";

            //Act
            var response = await Client.GetAsync(_defaultRoute + ApiRoutes.Videos.Filter + query);
            var videos = await response.Content.ReadAsAsync<List<Video>>();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            videos.Should().HaveCount(2);
            videos[0].MapId.Should().Be((int)EDummyTestId.Map3Mirage);
            videos[1].MapId.Should().Be((int)EDummyTestId.Map3Mirage);
        }

        [Fact]
        public async Task Filter_Deve_Retornar_Videos_De_Um_Mapa_Com_Granada_E_TickRate_Especificos()
        {
            //Arrange
            var query = $"?mapid={(int)EDummyTestId.Map1Dust2}&grenadetype={(int)EGrenadeType.Smoke}&tickrate={(int)ETick.Tick128}";

            //Act
            var response = await Client.GetAsync(_defaultRoute + ApiRoutes.Videos.Filter + query);
            var videos = await response.Content.ReadAsAsync<List<Video>>();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            videos.Should().HaveCount(1);
            videos[0].MapId.Should().Be((int)EDummyTestId.Map1Dust2);
            videos[0].GrenadeType.Should().Be((int)EGrenadeType.Smoke);
            videos[0].TickRate.Should().Be((int)ETick.Tick128);
        }

        [Fact]
        public async Task Filter_Deve_Retornar_Videos_De_Um_Mapa_E_Granada_Especificos()
        {
            //Arrange
            var query = $"?mapid={(int)EDummyTestId.Map1Dust2}&grenadetype={(int)EGrenadeType.Smoke}";

            //Act
            var response = await Client.GetAsync(_defaultRoute + ApiRoutes.Videos.Filter + query);
            var videos = await response.Content.ReadAsAsync<List<Video>>();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            videos.Should().HaveCount(2);
            videos[0].MapId.Should().Be((int)EDummyTestId.Map1Dust2);
            videos[1].MapId.Should().Be((int)EDummyTestId.Map1Dust2);
            videos[0].GrenadeType.Should().Be((int)EGrenadeType.Smoke);
            videos[1].GrenadeType.Should().Be((int)EGrenadeType.Smoke);
        }

        [Fact]
        public async Task Filter_Deve_Retornar_Todos_Os_Videos_De_Smoke()
        {
            //Arrange
            var query = $"?grenadetype={(int)EGrenadeType.Smoke}";

            //Act
            var response = await Client.GetAsync(_defaultRoute + ApiRoutes.Videos.Filter + query);
            var videos = await response.Content.ReadAsAsync<List<Video>>();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            videos.Should().HaveCount(4);
            videos[0].GrenadeType.Should().Be((int)EGrenadeType.Smoke);
            videos[1].GrenadeType.Should().Be((int)EGrenadeType.Smoke);
            videos[2].GrenadeType.Should().Be((int)EGrenadeType.Smoke);
            videos[3].GrenadeType.Should().Be((int)EGrenadeType.Smoke);
        }
        [Fact]
        public async Task Filter_Deve_Retornar_Um_Video_De_Um_Mapa_Com_Um_Tipo_De_Granada_E_Um_TickRate_Especifico()
        {
            //Arrange
            var query = $"?mapid={(int)EDummyTestId.Map1Dust2}&grenadetype={(int)EGrenadeType.Smoke}&tickrate={(int)ETick.Tick128}";

            //Act
            var response = await Client.GetAsync(_defaultRoute + ApiRoutes.Videos.Filter + query);
            var videos = await response.Content.ReadAsAsync<List<Video>>();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            videos.Should().HaveCount(1);
            videos[0].MapId.Should().Be((int)EDummyTestId.Map1Dust2);
            videos[0].GrenadeType.Should().Be((int)EGrenadeType.Smoke);
            videos[0].TickRate.Should().Be((int)ETick.Tick128);
        }

        [Fact]
        public async Task Filter_Deve_Retornar_Videos_Com_Tick64()
        {
            //Arrange
            var query = $"?tickrate={(int)ETick.Tick64}";

            //Act
            var response = await Client.GetAsync(_defaultRoute + ApiRoutes.Videos.Filter + query);
            var videos = await response.Content.ReadAsAsync<List<Video>>();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            videos.Should().HaveCount(3);
            videos[0].TickRate.Should().Be((int)ETick.Tick64);
            videos[1].TickRate.Should().Be((int)ETick.Tick64);
            videos[2].TickRate.Should().Be((int)ETick.Tick64);
        }

        [Fact]
        public async Task Filter_Deve_Retornar_Videos_Com_Tick128()
        {
            //Arrange
            var query = $"?mapid={(int)EDummyTestId.Map1Dust2}&tickrate={(int)ETick.Tick128}";

            //Act
            var response = await Client.GetAsync(_defaultRoute + ApiRoutes.Videos.Filter + query);
            var videos = await response.Content.ReadAsAsync<List<Video>>();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            videos.Should().HaveCount(2);
            videos[0].MapId.Should().Be((int)EDummyTestId.Map1Dust2);
            videos[1].MapId.Should().Be((int)EDummyTestId.Map1Dust2);
            videos[0].TickRate.Should().Be((int)ETick.Tick128);
            videos[1].TickRate.Should().Be((int)ETick.Tick128);
        }

        [Fact]
        public async Task Filter_Deve_Retornar_Videos_Com_Um_Tipo_De_Granada_Especifico()
        {
            //Arrange
            var query = $"?mapid={(int)EDummyTestId.Map1Dust2}&grenadetype={(int)EGrenadeType.Molotov}";

            //Act
            var response = await Client.GetAsync(_defaultRoute + ApiRoutes.Videos.Filter + query);
            var videos = await response.Content.ReadAsAsync<List<Video>>();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            videos.Should().HaveCount(1);
            videos[0].MapId.Should().Be((int)EDummyTestId.Map1Dust2);
            videos[0].TickRate.Should().Be((int)EGrenadeType.Molotov);
        }

        [Fact]
        public async Task Filter_Deve_Retornar_Videos_Com_Um_Tipo_De_Granada_E_TickRate_Especifico()
        {
            //Arrange
            var query = $"?grenadetype={(int)EGrenadeType.Smoke}&tickrate={(int)ETick.Tick64}";

            //Act
            var response = await Client.GetAsync(_defaultRoute + ApiRoutes.Videos.Filter + query);
            var videos = await response.Content.ReadAsAsync<List<Video>>();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            videos.Should().HaveCount(3);
            videos[0].GrenadeType.Should().Be((int)EGrenadeType.Smoke);
            videos[0].TickRate.Should().Be((int)ETick.Tick64);
            videos[1].GrenadeType.Should().Be((int)EGrenadeType.Smoke);
            videos[1].TickRate.Should().Be((int)ETick.Tick64);
            videos[2].GrenadeType.Should().Be((int)EGrenadeType.Smoke);
            videos[2].TickRate.Should().Be((int)ETick.Tick64);
        }

        #endregion

        [Fact]
        public async Task Filter_Nao_Deve_Retornar_Um_Video_Se_O_Tick_Informado_For_Diferente()
        {
            //Arrange
            await AuthenticateAsync();
        }

        [Fact]
        public async Task Update_Deve_Invalidar_Um_Campo_Menor_Que_O_Obrigatorio()
        {
            //Arrange
            await AuthenticateAsync();
        }

        [Fact]
        public async Task Update_Deve_Invalidar_Um_Campo_Maior_Que_O_Obrigatorio()
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
        public async Task Create_Deve_Invalidar_Se_Não_Existir_Um_Mapa_Associado_Ao_Video()
        {
            //Arrange
            await AuthenticateAsync();
        }

        [Fact]
        public async Task Create_Deve_Invalidar_Se_Não_Existir_Um_Usuario_Associado_Ao_Video()
        {
            //Arrange
            await AuthenticateAsync();
        }

        [Fact]
        public async Task Create_Deve_Invalidar_Se_Não_Existir_Um_Mapa_E_Usuario_Associado_Ao_Video()
        {
            //Arrange
            await AuthenticateAsync();
        }

    }
}


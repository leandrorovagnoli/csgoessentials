using CsgoEssentials.Domain.Entities;
using CsgoEssentials.Domain.Enum;
using CsgoEssentials.Infra.Utils;
using CsgoEssentials.IntegrationTests.Config;
using FluentAssertions;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace CsgoEssentials.IntegrationTests.MapTests
{
    public class MapTest : IntegrationTestConfig
    {
        private Map _newMap;
        private string _defaultRoute;

        public MapTest()
        {
            //getting route: ..localhost:5001/v1/maps/
            _defaultRoute = _baseUrl + ApiRoutes.Maps.Route + "/";

            _newMap = new Map(
                "Dust_2TEST",
                "Antigo mapa de cs TESTE");
        }

        #region Create

        [Fact]
        public async Task Create_Deve_Criar_Mapa_Retornando_Mapa_Criado()
        {
            //Arrange
            await AuthenticateAsync(EUserRole.Administrator);

            //Act
            var response = await Client.PostAsJsonAsync(_defaultRoute + ApiRoutes.Maps.Create, _newMap);
            var map = await response.Content.ReadAsAsync<Map>();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            map.Id.Should().BeGreaterThan(0);
            map.Name.Should().Be(_newMap.Name);
        }

        [Fact]
        public async Task Create_Nao_Deve_Inserir_Mapa_Novo_Se_Houver_Map_Com_Mesmo_Nome()
        {
            //Arrange
            await AuthenticateAsync(EUserRole.Administrator);
            var responseAux = await Client.PostAsJsonAsync(_defaultRoute + ApiRoutes.Maps.Create, _newMap);
            var mapAux = await responseAux.Content.ReadAsAsync<Map>();

            responseAux.StatusCode.Should().Be(HttpStatusCode.OK);
            mapAux.Should().NotBeNull();

            //Act
            var response = await Client.PostAsJsonAsync(_defaultRoute + ApiRoutes.Maps.Create, _newMap);
            var map = await response.Content.ReadAsStringAsync();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            map.Should().Contain(Messages.MAPA_EXISTENTE);
        }

        [Fact]
        public async Task Create_Deve_Invalidar_MapName_Vazio()
        {
            //Arrange
            await AuthenticateAsync(EUserRole.Administrator);
            await Client.PostAsJsonAsync(_defaultRoute + ApiRoutes.Maps.Create, _newMap);

            _newMap.Name = string.Empty;

            //Act
            var response = await Client.PostAsJsonAsync(_defaultRoute + ApiRoutes.Maps.Create, _newMap);
            var content = await response.Content.ReadAsStringAsync();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            content.Should().Contain(Messages.CAMPO_OBRIGATORIO.Replace("{0}", Messages.MAPA));
        }

        [Fact]
        public async Task Create_Nao_Deve_CriarMap_Com_Descricao_De_Mapa_Vazio()
        {
            //Arrange
            await AuthenticateAsync(EUserRole.Administrator);
            await Client.PostAsJsonAsync(_defaultRoute + ApiRoutes.Maps.Create, _newMap);

            _newMap.Description = string.Empty;

            //Act
            var response = await Client.PostAsJsonAsync(_defaultRoute + ApiRoutes.Maps.Create, _newMap);
            var content = await response.Content.ReadAsStringAsync();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Create_Deve_Invalidar_Nome_Menor_Do_Que_Permitido()
        {
            //Arrange
            await AuthenticateAsync(EUserRole.Administrator);
            await Client.PostAsJsonAsync(_defaultRoute + ApiRoutes.Maps.Create, _newMap);

            _newMap.Name = "Le";

            //Act
            var response = await Client.PostAsJsonAsync(_defaultRoute + ApiRoutes.Maps.Create, _newMap);
            var content = await response.Content.ReadAsStringAsync();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            content.Should().Contain(string.Format(Messages.CAMPO_PRECISA_TER_ENTRE_X2_E_Y1_CARACTERES, Messages.MAPA, "60", "3"));
        }

        [Fact]
        public async Task Create_Deve_Invalidar_Nome_De_Mapa_Maior_Do_Que_Permitido()
        {
            //Arrange
            await AuthenticateAsync(EUserRole.Administrator);
            await Client.PostAsJsonAsync(_defaultRoute + ApiRoutes.Maps.Create, _newMap);

            _newMap.Name = "nomedeusuariomuitograndenomedeusuariomuitograndenomedeusuariomuitogrande";

            //Act
            var response = await Client.PostAsJsonAsync(_defaultRoute + ApiRoutes.Maps.Create, _newMap);
            var content = await response.Content.ReadAsStringAsync();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            content.Should().Contain(string.Format(Messages.CAMPO_PRECISA_TER_ENTRE_X2_E_Y1_CARACTERES, Messages.MAPA, "60", "3"));
        }

        [Fact]
        public async Task Create_Deve_Invalidar_Descricao_De_Mapa_Maior_Do_Que_Permitido()
        {
            //Arrange
            await AuthenticateAsync(EUserRole.Administrator);
            await Client.PostAsJsonAsync(_defaultRoute + ApiRoutes.Maps.Create, _newMap);

            _newMap.Description = "nomedeusuariomuitograndenomedeusuariomuitograndenomedeusuariomuitogrande";

            //Act
            var response = await Client.PostAsJsonAsync(_defaultRoute + ApiRoutes.Maps.Create, _newMap);
            var content = await response.Content.ReadAsStringAsync();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            content.Should().Contain(string.Format(Messages.CAMPO_PRECISA_TER_ENTRE_X2_E_Y1_CARACTERES, Messages.DESCRICAO, "60", "3"));
        }

        [Fact]
        public async Task Create_Deve_Invalidar_Descricao_De_Mapa_Menor_Do_Que_Permitido()
        {
            //Arrange
            await AuthenticateAsync(EUserRole.Administrator);
            await Client.PostAsJsonAsync(_defaultRoute + ApiRoutes.Maps.Create, _newMap);

            _newMap.Description = "OU";

            //Act
            var response = await Client.PostAsJsonAsync(_defaultRoute + ApiRoutes.Maps.Create, _newMap);
            var content = await response.Content.ReadAsStringAsync();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            content.Should().Contain(string.Format(Messages.CAMPO_PRECISA_TER_ENTRE_X2_E_Y1_CARACTERES, Messages.DESCRICAO, "60", "3"));
        }

        #endregion

        #region Update

        [Fact]
        public async Task Update_Deve_Atualizar_Mapa_Existente()
        {
            //Arrange
            await AuthenticateAsync(EUserRole.Administrator);
            var responseAux = await Client.GetAsync(_defaultRoute + ApiRoutes.Maps.GetById.Replace("{id:int}", "2"));
            var mapAux = await responseAux.Content.ReadAsAsync<Map>();

            mapAux.Name = "RANDOMTEST";

            //Act
            var response = await Client.PutAsJsonAsync(_defaultRoute + ApiRoutes.Maps.Update.Replace("{id:int}", mapAux.Id.ToString()), mapAux);
            var map = await response.Content.ReadAsAsync<Map>();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            map.Id.Should().BeGreaterThan(0);
            map.Name.Should().Be("RANDOMTEST");
        }

        [Fact]
        public async Task Update_Nao_Deve_Atualizar_Mapa_Com_Id_Diferente_Do_Editado_Retornando_NotFound()
        {
            //Arrangew
            await AuthenticateAsync(EUserRole.Administrator);
            var responseAux = await Client.GetAsync(_defaultRoute + ApiRoutes.Maps.GetById.Replace("{id:int}", "2"));
            var mapAux = await responseAux.Content.ReadAsAsync<Map>();

            //Act
            mapAux.Name = "updatedMap";

            var response = await Client.PutAsJsonAsync(_defaultRoute + ApiRoutes.Maps.Update.Replace("{id:int}", "9999"), mapAux);
            var content = await response.Content.ReadAsStringAsync();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            content.Should().Contain(Messages.MAPA_NAO_ENCONTRADO);
        }

        [Fact]
        public async Task Update_Deve_Atualizar_Mapa_Existente_Com_Nova_Descricao()
        {
            //Arrange
            await AuthenticateAsync(EUserRole.Administrator);
            var responseAux = await Client.GetAsync(_defaultRoute + ApiRoutes.Maps.GetById.Replace("{id:int}", "2"));
            var mapAux = await responseAux.Content.ReadAsAsync<Map>();

            mapAux.Description = "DESCRICAORANDOMTESTE";

            //Act
            var response = await Client.PutAsJsonAsync(_defaultRoute + ApiRoutes.Maps.Update.Replace("{id:int}", mapAux.Id.ToString()), mapAux);
            var map = await response.Content.ReadAsAsync<Map>();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            map.Id.Should().BeGreaterThan(0);
            map.Name.Should().Be(mapAux.Name);
            map.Description.Should().Be(mapAux.Description);
        }

        #endregion

        #region Delete

        [Fact]
        public async Task Delete_Deve_Apagar_Mapa_Existente_Retornando_Mensagem()
        {
            //Arrange
            await AuthenticateAsync(EUserRole.Administrator);
            var responseAux = await Client.PostAsJsonAsync(_defaultRoute + ApiRoutes.Maps.Create, _newMap);
            var userAux = await responseAux.Content.ReadAsAsync<Map>();

            responseAux.StatusCode.Should().Be(HttpStatusCode.OK);
            userAux.Should().NotBeNull();

            //Act
            var response = await Client.DeleteAsync(_defaultRoute + ApiRoutes.Maps.Delete.Replace("{id:int}", userAux.Id.ToString()));
            var content = await response.Content.ReadAsStringAsync();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            content.Should().Contain(Messages.MAPA_REMOVIDO_COM_SUCESSO);
        }

        [Fact]
        public async Task Delete_Nao_Deve_Apagar_Map_Que_Possui_Videos_Relacionados()
        {
            //Arrange
            await AuthenticateAsync(EUserRole.Administrator);
            var responseAux = await Client.GetAsync(_defaultRoute + ApiRoutes.Maps.GetByIdWithRelationship.Replace("{id:int}", "1"));
            var map = await responseAux.Content.ReadFromJsonAsync<Map>();

            //Act
            var response = await Client.DeleteAsync(_defaultRoute + ApiRoutes.Maps.Delete.Replace("{id:int}", map.Id.ToString()));
            var jsonModel = await response.Content.ReadFromJsonAsync<JsonModel>();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            jsonModel.Message.Should().Be(Messages.NAO_FOI_POSSIVEL_REMOVER_MAP_POSSUI_VIDEOS_CADASTRADOS);
        }

        #endregion

        #region GetById

        [Fact]
        public async Task GetById_Deve_Retornar_Um_Mapa_Do_Banco()
        {
            //Arrange
            await AuthenticateAsync(EUserRole.Administrator);

            var responseAux = await Client.PostAsJsonAsync(_defaultRoute + ApiRoutes.Maps.Create, _newMap);
            var mapAux = await responseAux.Content.ReadAsAsync<Map>();

            responseAux.StatusCode.Should().Be(HttpStatusCode.OK);
            mapAux.Should().NotBeNull();

            //Act
            var response = await Client.GetAsync(_defaultRoute + ApiRoutes.Maps.GetById.Replace("{id:int}", mapAux.Id.ToString()));
            var map = await response.Content.ReadAsAsync<Map>();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            map.Id.Should().BeGreaterThan(0);
            map.Name.Should().Be(_newMap.Name);
        }

        [Fact]
        public async Task GetById_Deve_Retornar_Uma_Descricao_Do_Banco()
        {
            //Arrange
            await AuthenticateAsync(EUserRole.Administrator);

            var responseAux = await Client.PostAsJsonAsync(_defaultRoute + ApiRoutes.Maps.Create, _newMap);
            var mapAux = await responseAux.Content.ReadAsAsync<Map>();

            responseAux.StatusCode.Should().Be(HttpStatusCode.OK);
            mapAux.Should().NotBeNull();

            //Act
            var response = await Client.GetAsync(_defaultRoute + ApiRoutes.Maps.GetById.Replace("{id:int}", mapAux.Id.ToString()));
            var map = await response.Content.ReadAsAsync<Map>();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            map.Id.Should().BeGreaterThan(0);
            map.Description.Should().Be(_newMap.Description);
        }

        [Fact]
        public async Task GetById_Deve_Retornar_Mapa_Nao_Encontrado_Quando_Id_Nao_Existir()
        {
            //Arrange
            await AuthenticateAsync(EUserRole.Administrator);

            //Act
            var response = await Client.GetAsync(_defaultRoute + ApiRoutes.Maps.GetById.Replace("{id:int}", "9999"));
            var content = await response.Content.ReadAsStringAsync();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            content.Should().Contain(Messages.MAPA_NAO_ENCONTRADO);
        }

        #endregion

        #region GetAll

        [Fact]
        public async Task GetByAll_Deve_Retornar_Todos_Os_Mapas()
        {
            //Arrange
            await AuthenticateAsync(EUserRole.Administrator);

            //Act
            var response = await Client.GetAsync(_defaultRoute + ApiRoutes.Maps.GetAll);
            var content = await response.Content.ReadAsAsync<List<Map>>();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            content.Should().HaveCount(4);
        }

        #endregion
    }
}

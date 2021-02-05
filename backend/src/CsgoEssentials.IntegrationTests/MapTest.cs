using CsgoEssentials.Domain.Entities;
using CsgoEssentials.Domain.Enum;
using CsgoEssentials.Infra.Utils;
using CsgoEssentials.IntegrationTests.Config;
using FluentAssertions;
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
        public MapTest()
        {
            _newMap = new Map(
                "Dust_2TEST",
                "Antigo mapa de cs TESTE");
        }

        [Fact]
        public async Task Create_Deve_Criar_Mapa_Retornando_Mapa_Criado()
        {
            //Arrange
            await AuthenticateAsync(EUserRole.Administrator);

            //Act
            var response = await Client.PostAsJsonAsync(ApiRoutes.Maps.Create, _newMap);
            var map = await response.Content.ReadAsAsync<Map>();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            map.Id.Should().BeGreaterThan(0);
            map.Name.Should().Be(_newMap.Name);
        }

        [Fact]
        public async Task GetById_Deve_Retornar_Um_Mapa_Do_Banco()
        {
            //Arrange
            await AuthenticateAsync(EUserRole.Administrator);

            var responseAux = await Client.PostAsJsonAsync(ApiRoutes.Maps.Create, _newMap);
            var mapAux = await responseAux.Content.ReadAsAsync<Map>();

            responseAux.StatusCode.Should().Be(HttpStatusCode.OK);
            mapAux.Should().NotBeNull();

            //Act
            var response = await Client.GetAsync(ApiRoutes.Maps.GetById.Replace("{mapId}", mapAux.Id.ToString()));
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

            var responseAux = await Client.PostAsJsonAsync(ApiRoutes.Maps.Create, _newMap);
            var mapAux = await responseAux.Content.ReadAsAsync<Map>();

            responseAux.StatusCode.Should().Be(HttpStatusCode.OK);
            mapAux.Should().NotBeNull();

            //Act
            var response = await Client.GetAsync(ApiRoutes.Maps.GetById.Replace("{mapId}", mapAux.Id.ToString()));
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
            var response = await Client.GetAsync(ApiRoutes.Maps.GetById.Replace("{mapId}", "9999"));
            var content = await response.Content.ReadAsStringAsync();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            content.Should().Contain(Messages.MAPA_NAO_ENCONTRADO);
        }

        [Fact]
        public async Task Update_Deve_Atualizar_Mapa_Existente()
        {
            //Arrange
            await AuthenticateAsync(EUserRole.Administrator);
            var responseAux = await Client.GetAsync(ApiRoutes.Maps.GetById.Replace("{mapId}", "2"));
            var mapAux = await responseAux.Content.ReadAsAsync<Map>();

            mapAux.Name = "RANDOMTEST";            

            //Act
            var response = await Client.PutAsJsonAsync(ApiRoutes.Maps.Update.Replace("{mapId}", mapAux.Id.ToString()), mapAux);
            var map = await response.Content.ReadAsAsync<Map>();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            map.Id.Should().BeGreaterThan(0);
            map.Name.Should().Be("RANDOMTEST");
        }

        [Fact]
        public async Task Create_Nao_Deve_Inserir_Mapa_Novo_Se_Houver_Map_Com_Mesmo_Nome()
        {
            //Arrange
            await AuthenticateAsync(EUserRole.Administrator);
            var responseAux = await Client.PostAsJsonAsync(ApiRoutes.Maps.Create, _newMap);
            var mapAux = await responseAux.Content.ReadAsAsync<Map>();

            responseAux.StatusCode.Should().Be(HttpStatusCode.OK);
            mapAux.Should().NotBeNull();

            //Act
            var response = await Client.PostAsJsonAsync(ApiRoutes.Maps.Create, _newMap);
            var map = await response.Content.ReadAsStringAsync();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            map.Should().Contain(Messages.MAPA_EXISTENTE);            
        }

        [Fact]
        public async Task Update_Nao_Deve_Atualizar_Mapa_Com_Id_Diferente_Do_Editado_Retornando_NotFound()
        {
            //Arrangew
            await AuthenticateAsync(EUserRole.Administrator);
            var responseAux = await Client.GetAsync(ApiRoutes.Maps.GetById.Replace("{mapId}", "2"));
            var mapAux = await responseAux.Content.ReadAsAsync<Map>();

            //Act
            mapAux.Name = "updatedMap";
            
            var response = await Client.PutAsJsonAsync(ApiRoutes.Maps.Update.Replace("{mapId}", "9999"), mapAux);
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
            var responseAux = await Client.GetAsync(ApiRoutes.Maps.GetById.Replace("{mapId}", "2"));
            var mapAux = await responseAux.Content.ReadAsAsync<Map>();

            mapAux.Description = "DESCRICAORANDOMTESTE";

            //Act
            var response = await Client.PutAsJsonAsync(ApiRoutes.Maps.Update.Replace("{mapId}", mapAux.Id.ToString()), mapAux);
            var map = await response.Content.ReadAsAsync<Map>();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            map.Id.Should().BeGreaterThan(0);
            map.Name.Should().Be(mapAux.Name);
            map.Description.Should().Be(mapAux.Description);
        }

        [Fact]
        public async Task Delete_Deve_Apagar_Mapa_Existente_Retornando_Mensagem()
        {
            //Arrange
            await AuthenticateAsync(EUserRole.Administrator);
            var responseAux = await Client.PostAsJsonAsync(ApiRoutes.Maps.Create, _newMap);
            var userAux = await responseAux.Content.ReadAsAsync<Map>();

            responseAux.StatusCode.Should().Be(HttpStatusCode.OK);
            userAux.Should().NotBeNull();

            //Act
            var response = await Client.DeleteAsync(ApiRoutes.Maps.Delete.Replace("{mapId}", userAux.Id.ToString()));
            var content = await response.Content.ReadAsStringAsync();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            content.Should().Contain(Messages.MAPA_REMOVIDO_COM_SUCESSO);
        }

        [Fact]
        public async Task Create_Deve_Invalidar_MapName_Vazio()
        {
            //Arrange
            await AuthenticateAsync(EUserRole.Administrator);
            await Client.PostAsJsonAsync(ApiRoutes.Maps.Create, _newMap);

            _newMap.Name = string.Empty;

            //Act
            var response = await Client.PostAsJsonAsync(ApiRoutes.Maps.Create, _newMap);
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
            await Client.PostAsJsonAsync(ApiRoutes.Maps.Create, _newMap);

            _newMap.Description = string.Empty;

            //Act
            var response = await Client.PostAsJsonAsync(ApiRoutes.Maps.Create, _newMap);
            var content = await response.Content.ReadAsStringAsync();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);            
        }

         [Fact]
        public async Task Create_Deve_Invalidar_Nome_Menor_Do_Que_Permitido()
        {
            //Arrange
            await AuthenticateAsync(EUserRole.Administrator);
            await Client.PostAsJsonAsync(ApiRoutes.Maps.Create, _newMap);

            _newMap.Name = "Le";

            //Act
            var response = await Client.PostAsJsonAsync(ApiRoutes.Maps.Create, _newMap);
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
            await Client.PostAsJsonAsync(ApiRoutes.Maps.Create, _newMap);

            _newMap.Name = "nomedeusuariomuitograndenomedeusuariomuitograndenomedeusuariomuitogrande";

            //Act
            var response = await Client.PostAsJsonAsync(ApiRoutes.Maps.Create, _newMap);
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
            await Client.PostAsJsonAsync(ApiRoutes.Maps.Create, _newMap);

            _newMap.Description = "nomedeusuariomuitograndenomedeusuariomuitograndenomedeusuariomuitogrande";

            //Act
            var response = await Client.PostAsJsonAsync(ApiRoutes.Maps.Create, _newMap);
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
            await Client.PostAsJsonAsync(ApiRoutes.Maps.Create, _newMap);

            _newMap.Description = "OU";

            //Act
            var response = await Client.PostAsJsonAsync(ApiRoutes.Maps.Create, _newMap);
            var content = await response.Content.ReadAsStringAsync();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            content.Should().Contain(string.Format(Messages.CAMPO_PRECISA_TER_ENTRE_X2_E_Y1_CARACTERES, Messages.DESCRICAO, "60", "3"));
        }

        [Fact]
        public async Task Delete_Nao_Deve_Apagar_Map_Que_Possui_Videos_Relacionados()
        {
            //Arrange
            await AuthenticateAsync(EUserRole.Administrator);
            var responseAux = await Client.GetAsync(ApiRoutes.Maps.GetByIdWithRelationship.Replace("{mapId}", "1"));
            var map = await responseAux.Content.ReadFromJsonAsync<Map>();

            //Act
            var response = await Client.DeleteAsync(ApiRoutes.Maps.Delete.Replace("{mapId}", map.Id.ToString()));
            var jsonModel = await response.Content.ReadFromJsonAsync<JsonModel>();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            jsonModel.Message.Should().Be(Messages.NAO_FOI_POSSIVEL_REMOVER_MAP_POSSUI_VIDEOS_CADASTRADOS);
        }
    }
}

namespace CsgoEssentials.Infra.Utils
{
    public static class Messages
    {
        #region Names

        public const string NOME = "Nome";
        public const string EMAIL = "Email";
        public const string VIDEO = "Video";
        public const string TITULO = "Título";
        public const string DESCRICAO = "Descrição";
        public const string NOME_DE_USUARIO = "Nome de Usuário";
        public const string SENHA = "Senha";
        public const string FUNCAO = "Função";
        public const string ARTIGOS = "Artigos";
        public const string USUARIO = "Usuário";
        public const string MAPA = "Mapa";
        public const string DATA_DE_PUBLICACAO = "Data de Publicação";
        public const string TIPO_DE_GRANADA = "Tipo de Granada";
        public const string DATA = "Data";
        public const string VIDEOURL = "URL do video";

        #endregion

        #region Generic

        public const string OCORREU_UM_ERRO_INESPERADO = "Ocorreu um erro inesperado.";
        public const string CAMPO_INVALIDO = "O campo {0} não é válido.";
        public const string CAMPO_OBRIGATORIO = "O campo {0} é obrigatório.";
        public const string CAMPO_PRECISA_TER_ENTRE_X2_E_Y1_CARACTERES = "O campo {0} precisa ter entre {2} e {1} caracteres.";
        public const string CAMPO_POSSUI_LIMITE_DE_X1_CARACTERES = "O campo {0} possui um limite de até {1} caracteres.";
        public const string DATA_COM_FORMATO_INVALIDO = "Data em formato inválido";
        #endregion

        #region User

        public const string USUARIO_NAO_ENCONTRADO = "Usuário não encontrado.";
        public const string USUARIO_REMOVIDO_COM_SUCESSO = "Usuário removido com sucesso.";
        public const string NAO_FOI_POSSIVEL_BUSCAR_OS_USUARIOS = "Não foi possível buscar os usuários.";
        public const string NAO_FOI_POSSIVEL_CRIAR_O_USUARIO = "Não foi possível criar o usuário.";
        public const string NAO_FOI_POSSIVEL_ATUALIZAR_O_USUARIO = "Não foi possível atualizar o usuário.";
        public const string USUARIO_OU_SENHA_INVALIDOS = "Usuário ou senha inválidos";
        public const string NAO_FOI_POSSIVEL_AUTENTICAR_O_USUARIO = "Não foi possível autenticar o usuário.";
        public const string NOME_DE_USUARIO_JA_EXISTENTE = "Nome de Usuário já existente.";
        public const string NAO_E_PERMITIDO_ALTERAR_NOME_DE_USUARIO = "Não é permitido alterar o Nome de Usuário.";
        public const string NAO_FOI_POSSIVEL_REMOVER_USUARIO_POSSUI_ARTIGOS_OU_VIDEOS_CADASTRADOS = "Não foi possível remover. Usuário possui Artigos ou Videos cadastrados.";

        #endregion

        #region Map

        public const string MAPA_NAO_ENCONTRADO = "Mapa não encontrado.";
        public const string MAPA_EXISTENTE = "Mapa existente, não é possível recria-lo.";
        public const string MAPA_REMOVIDO_COM_SUCESSO = "Mapa removido com sucesso.";
        public const string NAO_FOI_POSSIVEL_BUSCAR_OS_MAPAS = "Não foi possível buscar os mapas.";
        public const string NAO_FOI_POSSIVEL_CRIAR_O_MAPA = "Não foi possível criar o mapa.";
        public const string NAO_FOI_POSSIVEL_ATUALIZAR_O_MAPA = "Não foi possível atualizar o mapa.";
        public const string NAO_FOI_POSSIVEL_REMOVER_MAP_POSSUI_VIDEOS_CADASTRADOS = "Não foi possível remover. Mapa possui Videos cadastrados.";

        #endregion

        #region Article

        public const string ARTIGO_NAO_ENCONTRADO = "Artigo não encontrado.";
        public const string ARTIGO_REMOVIDO_COM_SUCESSO = "Artigo foi removido com sucesso.";
        public const string NAO_FOI_POSSIVEL_BUSCAR_OS_ARTIGOS = "Não foi possível buscar os Artigos.";
        public const string NAO_FOI_POSSIVEL_CRIAR_UM_ARTIGO = "Não foi possível criar o Artigo.";
        public const string NAO_FOI_POSSIVEL_ATUALIZAR_O_ARTIGO = "Não foi possível atualizar o Artigo.";
        #endregion

        #region Video

        public const string VIDEO_NAO_ENCONTRADO = "Video não encontrado.";
        public const string NENHUM_VIDEO_ENCONTRADO = "Nenhum video encontrado.";
        public const string VIDEO_REMOVIDO_COM_SUCESSO = "Video removido com sucesso.";
        public const string NAO_FOI_POSSIVEL_BUSCAR_OS_VIDEOS = "Não foi possível buscar os videos.";
        public const string NAO_FOI_POSSIVEL_CRIAR_O_VIDEO = "Não foi possível criar o video.";
        public const string NAO_FOI_POSSIVEL_ATUALIZAR_O_VIDEO = "Não foi possível atualizar o video.";

        #endregion
    }
}

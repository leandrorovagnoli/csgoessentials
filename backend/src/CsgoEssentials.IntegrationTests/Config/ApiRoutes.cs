namespace CsgoEssentials.IntegrationTests.Config
{
    public class ApiRoutes
    {
        private static readonly string _baseUrl = "https://localhost:5001/v1/";

        public static class Users
        {
            private static readonly string _usersControllerUrl = string.Concat(_baseUrl, "users");

            public static readonly string GetAll = _usersControllerUrl;

            public static readonly string Create = _usersControllerUrl;

            public static readonly string GetById = string.Concat(_usersControllerUrl, "/{userId}");

            public static readonly string Delete = string.Concat(_usersControllerUrl, "/{userId}");

            public static readonly string Update = string.Concat(_usersControllerUrl, "/{userId}");

            public static readonly string Authenticate = string.Concat(_usersControllerUrl, "/login");
        }

        public static class Maps
        {
            private static readonly string _mapsControllerUrl = string.Concat(_baseUrl, "maps");

            public static readonly string GetAll = _mapsControllerUrl;

            public static readonly string Create = _mapsControllerUrl;

            public static readonly string GetById = string.Concat(_mapsControllerUrl, "/{mapId}");

            public static readonly string Delete = string.Concat(_mapsControllerUrl, "/{mapId}");

            public static readonly string Update = string.Concat(_mapsControllerUrl, "/{mapId}");

            public static readonly string Authenticate = string.Concat(_mapsControllerUrl, "/login");
        }
    }
}

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
    }
}

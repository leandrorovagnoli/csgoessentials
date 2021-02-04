namespace CsgoEssentials.IntegrationTests.Config
{
    public class ApiRoutes
    {
        private static readonly string _baseUrl = "https://localhost:5001/";

        public static class Users
        {
            public const string Route = "v1/users";

            private static readonly string _usersControllerUrl = string.Concat(_baseUrl, Route);

            public static readonly string GetAll = _usersControllerUrl;

            public static readonly string Create = _usersControllerUrl;

            public static readonly string GetById = string.Concat(_usersControllerUrl, "/{userId}");

            public static readonly string GetByIdWithRelationship = string.Concat(_usersControllerUrl, "/{userId}/include");

            public static readonly string Delete = string.Concat(_usersControllerUrl, "/{userId}");

            public static readonly string Update = string.Concat(_usersControllerUrl, "/{userId}");

            public static readonly string Authenticate = string.Concat(_usersControllerUrl, "/login");
        }

        public static class Maps
        {
            public const string Route = "v1/maps";

            private static readonly string _mapsControllerUrl = string.Concat(_baseUrl, Route);

            public static readonly string GetAll = _mapsControllerUrl;

            public static readonly string Create = _mapsControllerUrl;

            public static readonly string GetById = string.Concat(_mapsControllerUrl, "/{mapId}");

            public static readonly string GetByIdWithRelationship = string.Concat(_mapsControllerUrl, "/{mapId}/include");

            public static readonly string Delete = string.Concat(_mapsControllerUrl, "/{mapId}");

            public static readonly string Update = string.Concat(_mapsControllerUrl, "/{mapId}");
        }

        public static class Articles
        {
            public const string Route = "v1/articles";

            private static readonly string _articlesControllerUrl = string.Concat(_baseUrl, Route);

            public static readonly string GetAll = _articlesControllerUrl;

            public static readonly string Create = _articlesControllerUrl;

            public static readonly string GetById = string.Concat(_articlesControllerUrl, "/{articleId}");

            public static readonly string GetByIdWithRelationship = string.Concat(_articlesControllerUrl, "/{articleId}/include");

            public static readonly string Delete = string.Concat(_articlesControllerUrl, "/{articleId}");

            public static readonly string Update = string.Concat(_articlesControllerUrl, "/{articleId}");
        }

        public static class Videos
        {
            public const string Route = "v1/videos";

            public const string GetAll = "";

            public const string Create = "";

            public const string GetById = "{id:int}";

            public const string GetByIdWithRelationship = "{id:int}/include";

            public const string Filter = "filter";

            public const string Delete = "{id:int}";

            public const string Update = "{id:int}";
        }
    }
}

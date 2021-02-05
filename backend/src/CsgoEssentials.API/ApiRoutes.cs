namespace CsgoEssentials.IntegrationTests.Config
{
    public class ApiRoutes
    {
        public static class Users
        {
            public const string Route = "v1/users";

            public const string GetAll = "";

            public const string Create = "";

            public const string GetById = "{id:int}";

            public const string GetByIdWithRelationship = "{id:int}/include";

            public const string Delete = "{id:int}";

            public const string Update = "{id:int}";

            public const string Authenticate = "login";
        }

        public static class Maps
        {
            public const string Route = "v1/maps";

            public const string GetAll = "";

            public const string Create = "";

            public const string GetById = "{id:int}";

            public const string GetByIdWithRelationship = "{id:int}/include";

            public const string Delete = "{id:int}";

            public const string Update = "{id:int}";
        }

        public static class Articles
        {
            public const string Route = "v1/articles";

            public const string GetAll = "";

            public const string Create = "";

            public const string GetById = "{id:int}";

            public const string GetByIdWithRelationship = "{id:int}/include";

            public const string Delete = "{id:int}";

            public const string Update = "{id:int}";
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

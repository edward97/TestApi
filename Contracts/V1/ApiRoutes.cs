using Microsoft.AspNetCore.Authorization;

namespace TestApi.Contracts.V1
{
    [Authorize]
    public static class ApiRoutes
    {
        public const string Root = "api";
        public const string Version = "v1";
        public const string Base = Root + "/" + Version;

        public static class Identity
        {
            public const string Login = Base + "/identity/login";
            public const string Register = Base + "/identity/register";
            public const string Expired = Base + "/identity/expired";
        }

        public static class Todos
        {
            public const string GetAll = Base + "/todos";
            public const string Get = Base + "/todos/{id}";
            public const string Create = Base + "/todos";
            public const string Update = Base + "/todos/{id}";
            public const string Delete = Base + "/todos/{id}";
        }
    }
}

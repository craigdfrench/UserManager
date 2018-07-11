using System;
using Funq;
using Raven.Client.Documents;
using Raven.Client.ServerWide;
using Raven.Client.ServerWide.Operations;
using ServiceStack;
using ServiceStack.Api.Swagger;
using ServiceStack.Auth;
using ServiceStack.Caching;
using ServiceStack.Razor;
using ServiceStack.Text;
using System.Linq;
using UserManager.ServiceInterface;

namespace UserManager
{
    //VS.NET Template Info: https://servicestack.net/vs-templates/EmptyAspNet
    public class AppHost : AppHostBase
    {
        /// <summary>
        /// Base constructor requires a Name and Assembly where web service implementation is located
        /// </summary>
        public AppHost()
            : base("UserManager", typeof(MyServices).Assembly) { }

        private string databaseName = "UserManager";

        /// <summary>
        /// Application specific configuration
        /// This method should initialize any IoC resources utilized by your web service classes.
        /// </summary>
        public override void Configure(Container container)
        {
            var myDocStore = new DocumentStore
            {
                Urls = new[] { "http://localhost:8080" },
                Database = databaseName,
            };
            myDocStore.Initialize();

            // Is UserManager database created?
            if (myDocStore.Maintenance.Server.Send(
                new GetDatabaseNamesOperation(0, 200))
                .Count(x => x.EqualsIgnoreCase(databaseName)) == 0)
            {
                // No, then let's create it. 
                myDocStore.Maintenance.Server.Send(
                    new CreateDatabaseOperation(
                        new DatabaseRecord("databaseName")
                ));
            }
            container.AddSingleton<IDocumentStore>(implementationInstance: myDocStore);

            this.Plugins.Add(new AuthFeature(
                sessionFactory: () => new AuthUserSession(),
                authProviders: new IAuthProvider[] { new UserDataCredentialsAuthProvider() }));

            this.Plugins.Add(new RegistrationFeature());
            this.Plugins.Add(new RequestLogsFeature());
            this.Plugins.Add(new SwaggerFeature());
            this.Plugins.Add(new RazorFormat());

            container.Register<ICacheClient>(new MemoryCacheClient());
            var userRep = new InMemoryAuthRepository();
            container.Register<IAuthRepository>(userRep);

            container.RegisterAs<LogAuthEvents, IAuthEvents>();

            JsConfig<DateTime>.SerializeFn = time => new DateTime(time.Ticks, DateTimeKind.Local).ToString("o");
            JsConfig<DateTime?>.SerializeFn =
                time => time != null ? new DateTime(time.Value.Ticks, DateTimeKind.Local).ToString("o") : null;
            JsConfig.DateHandler = DateHandler.ISO8601;
        }
    }
}
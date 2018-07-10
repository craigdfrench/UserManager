using NUnit.Framework;
using ServiceStack;
using ServiceStack.Testing;
using UserManager.ServiceInterface;
using UserManager.ServiceModel;

namespace UserManager.Tests
{
    public class UnitTest
    {
        private readonly ServiceStackHost appHost;

        public UnitTest()
        {
            appHost = new BasicAppHost().Init();
            appHost.Container.AddTransient<MyServices>();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown() => appHost.Dispose();

        [Test]
        public void Can_call_MyServices()
        {
            var service = appHost.Container.Resolve<MyServices>();

            service.Get(new PopulateCheckRequest());
        }
    }
}

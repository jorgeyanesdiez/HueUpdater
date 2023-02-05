using System.Threading.Tasks;
using Flurl.Http.Testing;
using Xunit;

namespace HueUpdater.Services
{

    [Trait("TestType", "Integration")]
    public class HueInvokerIntegrationTests
    {

        public static readonly string url = "http://localhost";


        [Fact]
        public async Task PutAsync_CallsExpected()
        {
            using var httpTest = new HttpTest();
            var sut = new HueInvoker(url);
            await sut.PutAsync(new object());
            httpTest.ShouldHaveCalled(url);
        }

    }

}

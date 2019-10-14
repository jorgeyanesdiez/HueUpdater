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
        public async Task PutAsync_Anything_CallsExpected()
        {
            using (var httpTest = new HttpTest())
            {
                var sut = new HueInvoker(url);
                var result = await sut.PutAsync(new object());
                httpTest.ShouldHaveCalled(url);
            }
        }

    }

}

using NUnit.Framework;
using System.IO;
using System.Web;

namespace Template.Tests.Helpers
{
    [TestFixture]
    public class HttpContextSetUp
    {
        [SetUp]
        public virtual void SetUp()
        {
            HttpContext.Current = new HttpContext(
                new HttpRequest(null, "http://localhost:19174/", null),
                new HttpResponse(new StringWriter()));
        }

        [TearDown]
        public virtual void TearDown()
        {
            HttpContext.Current = null;
        }
    }
}

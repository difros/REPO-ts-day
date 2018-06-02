using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;

namespace GQ.Html.Rest.UnitTest
{
    [TestClass]
    public class TestHtmRest
    {
        [TestMethod]
        public void GetString()
        {
            HttpRest rest = new HttpRest();

            var a =  rest.GetString("http://gq-test2.cloudapp.net/PlataformaIOTDev/" + "Login", new string[] { "5848598ce6fb843294011987", "1", "1" });

            while(!a.IsCompleted)
            {
                Thread.Sleep(1000);
            }

            if (string.IsNullOrWhiteSpace(a.Result))
                throw new System.Exception("GetString Faild");
        }
    }
}

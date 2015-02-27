using System;
using System.Threading.Tasks;
using Syncano4.Net;
using Xunit;

namespace Syncano4.Tests
{

    public class TestData
    {

      /*    "id": 128, 
    "email": "syncano4_unittest@outlook.com", 
    "first_name": "Unit", 
    "last_name": "Test", 
    "account_key": "a1546d926e32a940a57cc6dc68a22fc40a3ae7f6"*/


        public const string AccountEmail = "syncano4_unittest@outlook.com";
        public const string AccountKey = "a1546d926e32a940a57cc6dc68a22fc40a3ae7f6";
    }


    public class InstancesTests
    {
        [Fact]
        public async Task Get()
        {
            var i = new SyncanoInstances(TestData.AccountKey);
            var instances = await i.Get();
        }
    }
}

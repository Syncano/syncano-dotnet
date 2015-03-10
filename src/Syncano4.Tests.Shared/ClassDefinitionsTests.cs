using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Syncano4.Shared;
using Shouldly;
#if Unity3d
using Syncano4.Unity3d;
using Syncano4.Tests.Unity3d;
#endif
#if dotNET
    using Syncano4.Net;
#endif
using Xunit;

namespace Syncano4.Tests.Shared
{
    public class ClassDefinitionsTests
    {
        public ISyncanoHttpClient GetClient()
        {
            return new SyncanoHttpClient(TestData.AccountKey);
        }


        [Fact]
        public async Task Get()
        {
            //given
            var classDefintions = new ClassDefinitions("/v1/instances/testinstance2/classes/", GetClient());

            //when
            var classes = await classDefintions.GetAsync(); 
            
            //then
            classes.ShouldAllBe(c => c.Name != null);
            
        }

      
    }
}
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Should;
using Syncano.Net.Api;
using Xunit;
using Xunit.Extensions;
using Xunit.Sdk;

namespace Syncano.Net.Tests
{
    public class UserSyncanoClientTests
    {
        [Theory, PropertyData("UserSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task New_CreatesNewUserObject(UserSyncanoClient client)
        {
            //given
            var name = "newUserName";

            //when
            var user = await client.New(name);

            //then
            user.ShouldNotBeNull();
            user.Name.ShouldEqual(name);

            //cleanup
            await client.Delete(user.Id);
        }

        [Theory, PropertyData("UserSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task New_WithNick_CreatesNewUserObject(UserSyncanoClient client)
        {
            //given
            var name = "newUserName";
            var nick = "newUserNick";

            //when
            var user = await client.New(name, nick);

            //then
            user.ShouldNotBeNull();
            user.Name.ShouldEqual(name);
            user.Nick.ShouldEqual(nick);

            //cleanup
            await client.Delete(user.Id);
        }

        [Theory, PropertyData("UserSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task New_WithPassword_CreatesNewUserObject(UserSyncanoClient client)
        {
            //given
            var name = "newUserName";
            var password = "abcde123";

            //when
            var user = await client.New(name, password: password);

            //then
            user.ShouldNotBeNull();
            user.Name.ShouldEqual(name);

            //cleanup
            await client.Delete(user.Id);
        }

        [Fact(Skip = "Avatar to big for tcp")]
        //[Theory, PropertyData("UserSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task New_WithAvatar_CreatesNewUserObject(UserSyncanoClient client)
        {
            //given
            var name = "newUserName";
            var avatar = TestData.ImageToBase64("sampleImage.jpg");

            //when
            var user = await client.New(name, avatar: avatar);

            //then
            user.ShouldNotBeNull();
            user.Name.ShouldEqual(name);
            user.Avatar.ShouldNotBeNull();

            //cleanup
            await client.Delete(user.Id);
        }

        [Theory, PropertyData("UserSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Delete_ByUserId_CreatesNewUserObject(UserSyncanoClient client)
        {
            //given
            var name = "newUserName";
            var user = await client.New(name);

            //when
            var result = await client.Delete(user.Id);

            //then
            result.ShouldBeTrue();
        }

        [Theory, PropertyData("UserSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Delete_ByUserId_WithNick_CreatesNewUserObject(UserSyncanoClient client)
        {
            //given
            var name = "newUserName";
            var nick = "newUserNick";
            var user = await client.New(name, nick);

            //when
            var result = await client.Delete(user.Id);

            //then
            result.ShouldBeTrue();
        }

        [Theory, PropertyData("UserSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Delete_ByUserId_WithPassword_CreatesNewUserObject(UserSyncanoClient client)
        {
            //given
            var name = "newUserName";
            var password = "abcde123";
            var user = await client.New(name, password: password);

            //when
            var result = await client.Delete(user.Id);

            //then
            result.ShouldBeTrue();
        }

        [Fact(Skip = "Avatar to big for tcp")]
        //[Theory, PropertyData("UserSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Delete_ByUserId_WithAvatar_CreatesNewUserObject(UserSyncanoClient client)
        {
            //given
            var name = "newUserName";
            var avatar = TestData.ImageToBase64("sampleImage.jpg");
            var user = await client.New(name, avatar: avatar);

            //cleanup
            var result = await client.Delete(user.Id);

            //then
            result.ShouldBeTrue();
        }

        [Theory, PropertyData("UserSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Delete_ByUserName_CreatesNewUserObject(UserSyncanoClient client)
        {
            //given
            var name = "newUserName";
            var user = await client.New(name);

            //when
            var result = await client.Delete(userName: user.Name);

            //then
            result.ShouldBeTrue();
        }

        [Theory, PropertyData("UserSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Delete_ByUserName_WithNick_CreatesNewUserObject(UserSyncanoClient client)
        {
            //given
            var name = "newUserName";
            var nick = "newUserNick";
            var user = await client.New(name, nick);

            //when
            var result = await client.Delete(userName: user.Name);

            //then
            result.ShouldBeTrue();
        }

        [Theory, PropertyData("UserSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Delete_ByUserName_WithPassword_CreatesNewUserObject(UserSyncanoClient client)
        {
            //given
            var name = "newUserName";
            var password = "abcde123";
            var user = await client.New(name, password: password);

            //when
            var result = await client.Delete(userName: user.Name);

            //then
            result.ShouldBeTrue();
        }

        [Fact(Skip = "Avatar to big for tcp")]
        //[Theory, PropertyData("UserSyncanoClients", PropertyType = typeof(SyncanoClientsProvider))]
        public async Task Delete_ByUserName_WithAvatar_CreatesNewUserObject(UserSyncanoClient client)
        {
            //given
            var name = "newUserName";
            var avatar = TestData.ImageToBase64("sampleImage.jpg");
            var user = await client.New(name, avatar: avatar);

            //cleanup
            var result = await client.Delete(userName: user.Name);

            //then
            result.ShouldBeTrue();
        }

    }
}

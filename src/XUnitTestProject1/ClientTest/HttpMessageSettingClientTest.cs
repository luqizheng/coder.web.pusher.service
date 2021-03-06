﻿using Coder.WebPuseherService.Hosting;
using Coder.WebPusherClient;
using Coder.WebPusherService.Senders.HttpSender;
using Coder.WebPusherService.Senders.HttpSender.ViewModel;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using Xunit;

namespace XUnitTestProject1.ClientTest
{
    public class HttpMessageSettingClientTest : IClassFixture<WebApplicationFactory<Startup>>
    {
        public HttpMessageSettingClientTest(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        private readonly WebApplicationFactory<Startup> _factory;

        [Fact]
        public void Test()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = client.GetAsync("/pusher-service/manage/HttpNotifySetting/1").Result;

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
        }

        [Fact]
        public void TestList()
        {
            var client = new HttpMessageSettingClient(_factory.CreateClient());
            var data = client.List(out var total);
        }

        [Fact]
        public void TestSave()
        {
            var client = new HttpMessageSettingClient(_factory.CreateClient());
            var viewModel = new HttpNotifySettingDetailViewModel
            {
                MessageType = Guid.NewGuid().ToString("N"),
                Url = "http://localhost:5000/WeatherForecast",
                Method = HttpNotifyMessageMethod.POST
            };
            viewModel.SubmitDataTemplate.Add(new Variable
            {
                Name = "a1",
                Value = "固定值"
            });

            viewModel.SubmitDataTemplate.Add(new Variable
            {
                Name = "a2",
                Value = "被修正值"
            });
            var result = client.Save(viewModel);


            Assert.True(result.Success);
        }
    }
}

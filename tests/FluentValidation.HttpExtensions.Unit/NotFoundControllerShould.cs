using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using FluentValidation.HttpExtensions.TestInfrastructure;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace FluentValidation.HttpExtensions.Unit
{
    public class NotFoundControllerShould : BaseController
    {
        private readonly NotFoundEntity _testEntity;

        public NotFoundControllerShould()
        {
            _testEntity = new NotFoundEntity();
        }

        [Fact]
        public async Task Use_Multiple_Messages()
        {
            _testEntity.Name = "James Bond";
            _testEntity.Address = "London, Great Britain";

            var response = await Post();
            var problems = await response.DeserializeAs<ValidationProblemDetails>();

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            problems.Errors.Should()
                .OnlyContain(x => x.Key == "");
            var messages = problems.Errors.Single().Value;
            messages.Should().BeEquivalentTo(new []
                {
                    "Could not find Name James Bond",
                    "Could not find Address London, Great Britain",
                });
        }

        [Fact]
        public async Task Use_Single_Message_When_Another_Rule_Passed()
        {
            _testEntity.Name = "Existing";
            _testEntity.Address = "London, Great Britain";

            var response = await Post();
            var problems = await response.DeserializeAs<ValidationProblemDetails>();

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            problems.Errors.Should()
                .OnlyContain(x => x.Key == "" &&
                                  x.Value.Single() == "Could not find Address London, Great Britain");
        }

        [Fact]
        public async Task Return_200_When_All_Rules_Passed()
        {
            _testEntity.Name = "Existing";
            _testEntity.Address = "Existing";

            var response = await Post();

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        private Task<HttpResponseMessage> Post()
        {
            var uri = new Uri("notFound", UriKind.Relative);
            return _apiClient.PostAsync(uri, _testEntity.ToStringContent());
        }
    }
}

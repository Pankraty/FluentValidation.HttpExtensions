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
    public class TestControllerShould : BaseController
    {
        private readonly TestEntity _testEntity;

        public TestControllerShould()
        {
            _testEntity = new TestEntity();
        }

        [Fact]
        public async Task Return_403_When_All_Rules_Fail()
        {
            var response = await Post();
            var problems = await response.DeserializeAs<ValidationProblemDetails>();

            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
            problems.Errors.Should()
                .OnlyContain(x => x.Key == "" &&
                                  x.Value.Single() == "The specified condition was not met for 'Is Forbidden'.");
        }

        [Fact]
        public async Task Return_404_When_IsForbidden_Passed()
        {
            _testEntity.IsForbidden = true;
            var response = await Post();
            var problems = await response.DeserializeAs<ValidationProblemDetails>();

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            problems.Errors.Should()
                .OnlyContain(x => x.Key == "" &&
                                  x.Value.Single() == "The specified condition was not met for 'Is Not Found'.");
        }

        [Fact]
        public async Task Return_405_When_IsNotFound_Passed()
        {
            _testEntity.IsForbidden = true;
            _testEntity.IsNotFound = true;

            var response = await Post();
            var problems = await response.DeserializeAs<ValidationProblemDetails>();

            response.StatusCode.Should().Be(HttpStatusCode.MethodNotAllowed);
            problems.Errors.Should()
                .OnlyContain(x => x.Key == "" &&
                                  x.Value.Single() == "The specified condition was not met for 'Is Method Not Allowed'.");
        }

        [Fact]
        public async Task Return_406_When_IsMethodNotAllowed_Passed()
        {
            _testEntity.IsForbidden = true;
            _testEntity.IsNotFound = true;
            _testEntity.IsMethodNotAllowed = true;

            var response = await Post();
            var problems = await response.DeserializeAs<ValidationProblemDetails>();

            response.StatusCode.Should().Be(HttpStatusCode.NotAcceptable);
            problems.Errors.Should()
                .OnlyContain(x => x.Key == "" &&
                                  x.Value.Single() == "The specified condition was not met for 'Is Not Acceptable'.");
        }

        [Fact]
        public async Task Return_409_When_IsNotAcceptable_Passed()
        {
            _testEntity.IsForbidden = true;
            _testEntity.IsNotFound = true;
            _testEntity.IsMethodNotAllowed = true;
            _testEntity.IsNotAcceptable = true;

            var response = await Post();
            var problems = await response.DeserializeAs<ValidationProblemDetails>();

            response.StatusCode.Should().Be(HttpStatusCode.Conflict);
            problems.Errors.Should()
                .OnlyContain(x => x.Key == "" &&
                                  x.Value.Single() == "The specified condition was not met for 'Is Conflict'.");
        }

        [Fact]
        public async Task Return_410_When_IsConflict_Passed()
        {
            _testEntity.IsForbidden = true;
            _testEntity.IsNotFound = true;
            _testEntity.IsMethodNotAllowed = true;
            _testEntity.IsNotAcceptable = true;
            _testEntity.IsConflict = true;

            var response = await Post();
            var problems = await response.DeserializeAs<ValidationProblemDetails>();

            response.StatusCode.Should().Be(HttpStatusCode.Gone);
            problems.Errors.Should()
                .OnlyContain(x => x.Key == "" &&
                                  x.Value.Single() == "The specified condition was not met for 'Is Gone'.");
        }

        [Fact]
        public async Task Return_423_When_IsGone_Passed()
        {
            _testEntity.IsForbidden = true;
            _testEntity.IsNotFound = true;
            _testEntity.IsMethodNotAllowed = true;
            _testEntity.IsNotAcceptable = true;
            _testEntity.IsConflict = true;
            _testEntity.IsGone = true;

            var response = await Post();
            var problems = await response.DeserializeAs<ValidationProblemDetails>();

            response.StatusCode.Should().Be(HttpStatusCode.Locked);
            problems.Errors.Should()
                .OnlyContain(x => x.Key == "" &&
                                  x.Value.Single() == "The specified condition was not met for 'Is Locked'.");
        }

        [Fact]
        public async Task Return_400_When_IsLocked_Passed()
        {
            _testEntity.IsForbidden = true;
            _testEntity.IsNotFound = true;
            _testEntity.IsMethodNotAllowed = true;
            _testEntity.IsNotAcceptable = true;
            _testEntity.IsConflict = true;
            _testEntity.IsGone = true;
            _testEntity.IsLocked = true;

            var response = await Post();
            var problems = await response.DeserializeAs<ValidationProblemDetails>();

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            problems.Errors.Should()
                .OnlyContain(x => x.Key == "IsValid" &&
                                  x.Value.Single() == "The specified condition was not met for 'Is Valid'.");
        }

        [Fact]
        public async Task Return_200_When_All_Passed()
        {
            _testEntity.IsForbidden = true;
            _testEntity.IsNotFound = true;
            _testEntity.IsMethodNotAllowed = true;
            _testEntity.IsNotAcceptable = true;
            _testEntity.IsConflict = true;
            _testEntity.IsGone = true;
            _testEntity.IsLocked = true;
            _testEntity.IsValid = true;

            var response = await Post();

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        private Task<HttpResponseMessage> Post()
        {
            var uri = new Uri("test", UriKind.Relative);
            return _apiClient.PostAsync(uri, _testEntity.ToStringContent());
        }
    }
}

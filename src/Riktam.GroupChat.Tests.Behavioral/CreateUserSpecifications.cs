using Microsoft.Extensions.DependencyInjection;
using Moq;
using Riktam.GroupChat.Domain.Common;
using Riktam.GroupChat.Domain.Models;
using Riktam.GroupChat.Domain.Providers;
using Riktam.GroupChat.Domain.Services;
using Riktam.GroupChat.Tests.Common.Extensions;
using Riktam.GroupChat.Tests.Common.Infrastructure;
using Riktam.GroupChat.Tests.Common.Models;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http.Json;
using System.Security.Claims;
using Xunit;

namespace Riktam.GroupChat.Tests.Behavioral
{
    [ExcludeFromCodeCoverage]
    public class CreateUserSpecifications
    {
        private readonly WebApplicationFactory _webApplicationFactory;
        private readonly TestAuthenticationOptions _testAuthenticationOptions;
        private readonly IHashGenerator _hashGenerator;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly string _apiUrl = "v1/users";
        private CreateUserTestModel _request = new()
        {
            UserName = "testuser",
            Email = "testEmail@something.com",
            Password = "testpassword"
        };

        public CreateUserSpecifications()
        {
            _webApplicationFactory = WebApplicationFactory.CreateForBehavioralTests();

            _testAuthenticationOptions = new TestAuthenticationOptions
            {
                ShouldSucceed = true,
                Claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, "loggedInUser"),
                    new Claim("UserId", "0"),
                    new Claim(ClaimTypes.Email, "loggedInUser@something.com")
                }
            };
            _webApplicationFactory.AddTestAuthenticationHandler(_testAuthenticationOptions);
            _userRepositoryMock = _webApplicationFactory.Mock<IUserRepository>();
            _hashGenerator = _webApplicationFactory.Services.GetRequiredService<IHashGenerator>();
        }

        [Fact(DisplayName = "CreateUser should return 401 Unauthorized when using invalid auth token")]
        public async void Test01()
        {
            // given the authentication should fail
            _testAuthenticationOptions.ShouldSucceed = false;

            // Act
            var response = await CallApi(_request);

            // then the response should be Unauthorized
            await response.ShouldHaveStatusCode(HttpStatusCode.Unauthorized);

            // verify there should be no calls 
            _userRepositoryMock.VerifyNoOtherCalls();
        }

        [Fact(DisplayName = "CreateSchedule should return Bad Request when request is null")]
        public async void Test02()
        {
            // Act
            var response = await CallApi(null);

            // then the response should be Bad Request
            await response.ShouldBeTheModelStateErrorsAndBadRequest(new Dictionary<string, string[]> {
                {string.Empty, new[] { "A non-empty request body is required." } },
                {"model", new[] { "The model field is required." } }
            });

            // verify there should be no calls 
            _userRepositoryMock.VerifyNoOtherCalls();
        }

        [Fact(DisplayName = "CreateSchedule should return Bad Request when required parameters are missing")]
        public async void Test03()
        {
            // given I have request with missing required parameters
            _request = new();

            // Act
            var response = await CallApi(_request);

            // then the response should be Bad Request
            await response.ShouldBeTheModelStateErrorsAndBadRequest(new Dictionary<string, string[]> {
                { nameof(_request.UserName), new[] { "The UserName field is required." } },
                { nameof(_request.Email), new[] { "The Email field is required." } },
                { nameof(_request.Password), new[] { "The Password field is required." } },
                });

            // verify there should be no calls 
            _userRepositoryMock.VerifyNoOtherCalls();
        }

        [Fact(DisplayName = "CreateSchedule should return Bad Request when required parameters are empty")]
        public async void Test04()
        {
            // given I have request with missing required parameters
            _request = new() { Email = "", UserName = "", Password = "" };

            // Act
            var response = await CallApi(_request);

            // then the response should be Bad Request
            await response.ShouldBeTheModelStateErrorsAndBadRequest(new Dictionary<string, string[]> {
                { nameof(_request.UserName), new[] { "The UserName field is required." } },
                { nameof(_request.Email), new[] { "The Email field is required.", "The Email field is not a valid e-mail address." } },
                { nameof(_request.Password), new[] { "The Password field is required.", "The field Password must be a string or array type with a minimum length of '6'." } },
                });

            // verify there should be no calls 
            _userRepositoryMock.VerifyNoOtherCalls();
        }

        [Fact(DisplayName = "CreateSchedule should return Bad Request when user name has exceeded its max lenght")]
        public async void Test05()
        {
            // Arrange
            _request.UserName = "this is a test username exceeding its max length of 50 characters";

            // Act
            var response = await CallApi(_request);

            // then the response should be Bad Request
            await response.ShouldBeTheModelStateErrorsAndBadRequest(new Dictionary<string, string[]> {
                {nameof(_request.UserName), new[] { "The field UserName must be a string or array type with a maximum length of '50'." } },
            });

            // verify there should be no calls 
            _userRepositoryMock.VerifyNoOtherCalls();
        }

        [Fact(DisplayName = "CreateSchedule should return Bad Request when email has exceeded its max lenght")]
        public async void Test06()
        {
            // Arrange
            _request.Email = "thisisatestusernameexceedingitsmaxlengthof50characters@something.com";

            // Act
            var response = await CallApi(_request);

            // then the response should be Bad Request
            await response.ShouldBeTheModelStateErrorsAndBadRequest(new Dictionary<string, string[]> {
                {nameof(_request.Email), new[] { "The field Email must be a string or array type with a maximum length of '50'." } },
            });

            // verify there should be no calls 
            _userRepositoryMock.VerifyNoOtherCalls();
        }

        [Theory(DisplayName = "CreateSchedule should return Bad Request when email value is invalid")]
        [InlineData("invalidEmail")]
        [InlineData("12345")]
        [InlineData("my@email@com")]
        public async void Test07(string email)
        {
            // Arrange
            _request.Email = email;

            // Act
            var response = await CallApi(_request);

            // then the response should be Bad Request
            await response.ShouldBeTheModelStateErrorsAndBadRequest(new Dictionary<string, string[]> {
                {nameof(_request.Email), new[] { "The Email field is not a valid e-mail address." } },
            });

            // verify there should be no calls 
            _userRepositoryMock.VerifyNoOtherCalls();
        }

        [Fact(DisplayName = "CreateSchedule should return Bad Request when password has exceeded its max length")]
        public async void Test08()
        {
            // Arrange
            _request.Password = "this is a test user password exceeding its max length of 50 characters";

            // Act
            var response = await CallApi(_request);

            // then the response should be Bad Request
            await response.ShouldBeTheModelStateErrorsAndBadRequest(new Dictionary<string, string[]> {
                {nameof(_request.Password), new[] { "The field Password must be a string or array type with a maximum length of '50'." } },
            });

            // verify there should be no calls 
            _userRepositoryMock.VerifyNoOtherCalls();
        }

        [Fact(DisplayName = "CreateSchedule should return Bad Request when password has length less than its min length")]
        public async void Test09()
        {
            // Arrange
            _request.Password = "pass";

            // Act
            var response = await CallApi(_request);

            // then the response should be Bad Request
            await response.ShouldBeTheModelStateErrorsAndBadRequest(new Dictionary<string, string[]> {
                {nameof(_request.Password), new[] { "The field Password must be a string or array type with a minimum length of '6'." } },
            });

            // verify there should be no calls 
            _userRepositoryMock.VerifyNoOtherCalls();
        }

        [Fact(DisplayName = "CreateUser should 409 Conflict when UserName is already taken")]
        public async Task Test10()
        {
            // arrange
            CreateUserTestModel request = new()
            {
                UserName = "testuser",
                Email = "testEmail@something.com",
                Password = "testpassword"
            };
            UserRecord createdUserRecord = ConstructExpectedCreatedUserRecord(request);

            _userRepositoryMock
                .Setup(repo => repo.GetByUserNameAsync(It.IsAny<string>()))
                .ReturnsAsync(new UserRecord { UserName = request.UserName });

            // Act
            var response = await CallApi(request);

            // Assert
            await response.ShouldBeAConflictStatusResult(AppErrorCodes.UserNameAlreadyTaken);

            _userRepositoryMock.Verify(repo => repo.GetByUserNameAsync(
                It.Is<string>(userName => userName == request.UserName)),
                Times.Once);

            _userRepositoryMock.VerifyNoOtherCalls();
        }

        [Fact(DisplayName = "CreateUser should 409 Conflict when Email is already taken")]
        public async Task Test11()
        {
            // arrange
            CreateUserTestModel request = new()
            {
                UserName = "testuser",
                Email = "testEmail@something.com",
                Password = "testpassword"
            };

            UserRecord createdUserRecord = ConstructExpectedCreatedUserRecord(request);
            _userRepositoryMock
               .Setup(repo => repo.GetByUserNameAsync(It.IsAny<string>()))
               .ReturnsAsync((UserRecord?)null);

            _userRepositoryMock
                .Setup(repo => repo.GetByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(new UserRecord { Email = request.Email });

            // Act
            var response = await CallApi(request);

            // Assert
            await response.ShouldBeAConflictStatusResult(AppErrorCodes.EmailAlreadyTaken);

            _userRepositoryMock.Verify(repo => repo.GetByUserNameAsync(
                It.Is<string>(userName => userName == request.UserName)),
                Times.Once);

            _userRepositoryMock.Verify(repo => repo.GetByEmailAsync(
              It.Is<string>(userName => userName == request.Email)),
              Times.Once);

            _userRepositoryMock.VerifyNoOtherCalls();
        }

        [Fact(DisplayName = "CreateUser should create a user and return 201 Created status")]
        public async Task Test12()
        {
            // arrange
            CreateUserTestModel request = new()
            {
                UserName = "testuser",
                Email = "testEmail@something.com",
                Password = "testpassword"
            };
            UserRecord createdUserRecord = ConstructExpectedCreatedUserRecord(request);

            _userRepositoryMock
               .Setup(repo => repo.AddAsync(It.IsAny<UserRecord>()))
               .ReturnsAsync(createdUserRecord);

            _userRepositoryMock
                .Setup(repo => repo.GetByUserNameAsync(It.IsAny<string>()))
                .ReturnsAsync((UserRecord?)null);

            _userRepositoryMock
                .Setup(repo => repo.GetByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((UserRecord?)null);

            // Act
            var response = await CallApi(request);

            // Assert
            await response.ShouldContainTheResult(HttpStatusCode.Created,
                ConstructExpectedCreatedUserResponse(request));

            _userRepositoryMock.Verify(repo => repo.GetByUserNameAsync(
                It.Is<string>(userName => userName == request.UserName)),
                Times.Once);

            _userRepositoryMock.Verify(repo => repo.GetByEmailAsync(
                It.Is<string>(email => email == request.Email)),
                Times.Once);

            _userRepositoryMock.Verify(repo => repo.AddAsync(
                It.Is<UserRecord>(newUser =>
                    newUser.UserName == request.UserName
                    && newUser.Email == request.Email
                    && newUser.Password == _hashGenerator.GenerateHash(request.Password))),
                Times.Once);

            _userRepositoryMock.VerifyNoOtherCalls();
        }

        private async Task<HttpResponseMessage> CallApi(CreateUserTestModel? request)
        {
            var httpClient = _webApplicationFactory.CreateClient();

            return await httpClient.PostAsJsonAsync(_apiUrl, request);
        }
        private UserRecord ConstructExpectedCreatedUserRecord(CreateUserTestModel createUserTestModel)
        {
            return new()
            {
                Id = 1,
                UserName = createUserTestModel.UserName!,
                Email = createUserTestModel.Email!,
                Password = _hashGenerator.GenerateHash(createUserTestModel.Password!)
            };
        }
        private static UserResponseTestModel ConstructExpectedCreatedUserResponse(CreateUserTestModel createUserTestModel)
        {
            return new()
            {
                Id = 1,
                UserName = createUserTestModel.UserName!,
                Email = createUserTestModel.Email!,
            };
        }
    }
}
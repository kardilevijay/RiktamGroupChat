using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Riktam.GroupChat.Domain.Common;
using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace Riktam.GroupChat.Tests.Common.Extensions;

[ExcludeFromCodeCoverage]
public static class HttpResponseMessageExtensions
{
    public static async Task<T> ShouldContainTheResult<T>(
        this HttpResponseMessage httpResponseMessage,
        HttpStatusCode statusCode,
        T expectedResult) where T : new()
    {
        await httpResponseMessage.ShouldHaveStatusCode(statusCode);

        var contentString = await httpResponseMessage.Content.ReadAsStringAsync();
        var actualResult = JsonConvert.DeserializeObject<T>(contentString);

        actualResult.Should().BeEquivalentTo(expectedResult, o => o);

        return actualResult ?? new T();
    }

    public static async Task ShouldBeAClientException(this HttpResponseMessage httpResponseMessage,
        AppErrorCodes errorCode)
    {
        await httpResponseMessage.ShouldHaveStatusCode(HttpStatusCode.BadRequest);

        var contentString = await httpResponseMessage.Content.ReadAsStringAsync();
        var problemDetails = JsonConvert.DeserializeObject<ProblemDetails>(contentString);

        problemDetails.Should().BeEquivalentTo(new ProblemDetails
        {
            Type = errorCode.ToString(),
            Title = errorCode.GetDescription(),
            Status = StatusCodes.Status400BadRequest
        });
    }

    public static async Task ShouldBeAConflictStatusResult(this HttpResponseMessage httpResponseMessage,
      AppErrorCodes errorCode)
    {
        await httpResponseMessage.ShouldHaveStatusCode(HttpStatusCode.Conflict);

        var contentString = await httpResponseMessage.Content.ReadAsStringAsync();
        var problemDetails = JsonConvert.DeserializeObject<ProblemDetails>(contentString);

        problemDetails.Should().BeEquivalentTo(new ProblemDetails
        {
            Type = errorCode.ToString(),
            Title = errorCode.GetDescription(),
            Status = StatusCodes.Status409Conflict
        });
    }

    public static async Task ShouldHaveStatusCode(this HttpResponseMessage httpResponseMessage,
        HttpStatusCode expectedHttpStatusCode)
        => httpResponseMessage.StatusCode.Should().Be(expectedHttpStatusCode, await httpResponseMessage.Because());

    public static async Task ShouldBeTheModelStateErrorsAndBadRequest(
       this HttpResponseMessage httpResponseMessage,
       Dictionary<string, string[]> errors)
    {
        await httpResponseMessage.ShouldHaveStatusCode(HttpStatusCode.BadRequest);

        var contentString = await httpResponseMessage.Content.ReadAsStringAsync();
        var validationProblems = JsonConvert.DeserializeObject<ValidationProblemDetails>(contentString);

        validationProblems.Should().NotBeNull();
        validationProblems.Should().BeEquivalentTo(new ValidationProblemDetails
        {
            Type = "InvalidRequest",
            Title = "The request is invalid",
            Status = StatusCodes.Status400BadRequest
        }, o => o.Excluding(x => x.Errors));

        validationProblems!.Errors.Should().BeEquivalentTo(errors);
    }

    private static async Task<string> Because(this HttpResponseMessage httpResponseMessage)
    {
        return await httpResponseMessage.Content.ReadAsStringAsync();
    }
}
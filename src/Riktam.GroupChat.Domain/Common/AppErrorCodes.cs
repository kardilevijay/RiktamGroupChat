using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;

namespace Riktam.GroupChat.Domain.Common;

public enum AppErrorCodes
{
    [Description("The request is invalid")]
    InvalidRequest,

    [Description("The username is already taken")]
    UserNameAlreadyTaken,

    [Description("The email is already taken")]
    EmailAlreadyTaken
}

public static class AppErrorCodeExtensions
{
    private static readonly ReadOnlyDictionary<AppErrorCodes, string> ErrorDescriptions;

    static AppErrorCodeExtensions()
    {
        var lookup = Enum.GetValues(typeof(AppErrorCodes))
            .Cast<AppErrorCodes>()
            .ToDictionary(x => x,
                x =>
                {
                    var field = x.GetType().GetField(x.ToString())
                        ?? throw new InvalidOperationException($"{nameof(AppErrorCodes)}.{x} is not a field of {x.GetType()}");

                    var descriptionAttribute = field.GetCustomAttribute<DescriptionAttribute>()
                        ?? throw new InvalidOperationException($"{nameof(AppErrorCodes)}.{x} is missing a {nameof(DescriptionAttribute)}");

                    return descriptionAttribute.Description;
                });

        ErrorDescriptions = new ReadOnlyDictionary<AppErrorCodes, string>(lookup);
    }

    public static string GetDescription(this AppErrorCodes error)
    {
        return ErrorDescriptions[error];
    }
}

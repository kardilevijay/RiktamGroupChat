﻿namespace Riktam.GroupChat.Domain.Models;

public record UpdateUserRequest
{
    public string Email { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
}
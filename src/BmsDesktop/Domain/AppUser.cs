namespace BmsDesktop.Domain;

public sealed record AppUser(Guid Id, string Username, string DisplayName, UserRole Role);

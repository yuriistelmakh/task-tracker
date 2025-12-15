namespace TaskTracker.Domain.Enums;

public enum AuthErrorType
{
    None,
    UserNotFound,
    InvalidPassword,
    EmailTaken,
    TagTaken,


    Unknown
}


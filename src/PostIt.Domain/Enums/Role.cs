using System.Runtime.Serialization;

namespace PostIt.Domain.Enums;

public enum Role
{
    User = 0,
    Moderator = 1,
    Admin = 2,
}
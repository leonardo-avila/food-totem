using Identity.Domain.Models.Enums;
using Domain.Core;

namespace Identity.Domain.Models
{
    public class Customer : Entity
    {
        public string? Email { get; private set; }
        public string? CPF { get; private set; }
        public Guid? Protocol { get; private set; }
        public AuthenticationTypeEnum AuthenticationType { get; private set; }

        public Customer(AuthenticationTypeEnum authenticationType, string? identification)
        {
            AuthenticationType = authenticationType;
            if (authenticationType == AuthenticationTypeEnum.Anonymous) Protocol = Guid.NewGuid();
            else if (authenticationType == AuthenticationTypeEnum.Email) Email = identification;
            else if (authenticationType == AuthenticationTypeEnum.CPF) CPF = identification;
        }

        public override string ToString()
        {
            if (AuthenticationType == AuthenticationTypeEnum.Anonymous) return Protocol.ToString()!;
            else if (AuthenticationType == AuthenticationTypeEnum.Email) return Email!;
            else return CPF!;
        }
    }
}

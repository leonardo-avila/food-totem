using FoodTotem.Identity.Domain.Models.Enums;
using FoodTotem.Domain.Core;

namespace FoodTotem.Identity.Domain.Models
{
    public class Customer : Entity, IAggregateRoot
    {
        public string? Name { get; private set; }
        public string? Address { get; private set; }
        public string? Phone { get; private set; }
        public string? Email { get; private set; }
        public string? CPF { get; private set; }
        public Guid? Protocol { get; private set; }
        public AuthenticationTypeEnum AuthenticationType { get; private set; }

        public Customer(AuthenticationTypeEnum authenticationType, string? identification, string? name, string? address, string? phone, string? email)
        {
            AuthenticationType = authenticationType;
            
            if (authenticationType == AuthenticationTypeEnum.Anonymous) Protocol = Guid.NewGuid();
            else if (authenticationType == AuthenticationTypeEnum.Email) Email = identification;
            else if (authenticationType == AuthenticationTypeEnum.CPF) CPF = identification;
            
            Name = name;
            Address = address;
            Phone = phone;
            Email = email;
        }

        protected Customer() { } // EF constructor

        public override string ToString()
        {
            if (AuthenticationType == AuthenticationTypeEnum.Anonymous) return Protocol.ToString()!;
            else if (AuthenticationType == AuthenticationTypeEnum.Email) return Email!;
            else return CPF!;
        }
    }
}

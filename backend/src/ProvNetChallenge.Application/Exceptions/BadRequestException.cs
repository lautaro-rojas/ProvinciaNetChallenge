// ProvNetChallenge.Application/Exceptions/BadRequestException.cs
namespace ProvNetChallenge.Application.Exceptions
{
    public class BadRequestException : Exception
    {
        public BadRequestException(string message) : base(message)
        {
        }
    }
}
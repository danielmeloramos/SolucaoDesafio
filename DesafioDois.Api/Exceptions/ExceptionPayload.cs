using System.Diagnostics.CodeAnalysis;

namespace DesafioSegundo.Api.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class ExceptionPayload
    {
        public int ErrorCode { get; set; }

        public string ErrorMessage { get; set; }
    }
}
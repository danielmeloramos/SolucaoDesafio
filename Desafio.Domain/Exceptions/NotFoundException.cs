namespace Desafio.Domain.Exceptions
{
    /// <summary>
    /// Representa uma exceção de negócio: o recurso solicitado não pode ser encontrado
    /// </summary>
    public class NotFoundException : BusinessException
    {
        public NotFoundException() : base(ErrorCodes.NotFound, "Registro não encontrado")
        {
        }
    }
}
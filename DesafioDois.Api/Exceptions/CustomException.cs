using System;
using System.Collections.Generic;

namespace DesafioSegundo.Api.Exceptions
{
    /// <summary>
    /// CustomException
    /// </summary>
    public class CustomException
    {
        /// <summary>
        /// Mensagem de erro
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Mensagens de erros
        /// </summary>
        public IList<string> ErrorMessages { get; set; }

        /// <summary>
        /// Exceção a ser logada
        /// </summary>
        public Exception Exception { get; set; }

        /// <summary>
        /// Criar novo CustomException
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="exception"></param>
        /// <returns></returns>
        public static CustomException New<T>(T exception) where T : Exception
        {
            return new CustomException
            {
                ErrorMessage = exception.Message,
                Exception = exception
            };
        }
    }
}

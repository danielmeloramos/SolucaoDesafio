using System;

namespace Desafio.Domain
{
    /// <summary>
    /// Todas as classes que representam entidades de negócio, devem herdar de Entity.
    /// Entity é uma abstração de funcionalidades comuns à entidades de negócio.
    ///
    /// Por exemplo: Id, método de comparação (Equals) e validação (Validate)
    ///
    /// </summary>
    public abstract class Entity
    {
        protected Entity() => Id = Guid.NewGuid();

        /// <summary>
        /// Identificador único
        /// </summary>
        public virtual Guid Id { get; set; }
    }
}
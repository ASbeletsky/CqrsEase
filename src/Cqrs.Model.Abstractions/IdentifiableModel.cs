using System;
using System.Collections.Generic;
using System.Text;

namespace Cqrs.Model.Abstractions
{
    public abstract class IdentifiableModel<TKey> : Identifiable<TKey>
    {
        public TKey Id { get; set; }

        object Identifiable.Id => this.Id;
    }
}

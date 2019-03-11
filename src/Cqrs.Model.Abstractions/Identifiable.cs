using System;

namespace Cqrs.Model.Abstractions
{
    public interface Identifiable
    {
        object Id { get; }
    }

    public interface Identifiable<out TKey> : Identifiable
    {
        new TKey Id { get; }
    }
}

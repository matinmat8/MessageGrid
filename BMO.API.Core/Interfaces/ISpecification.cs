using System.Linq.Expressions;

namespace Core.Interfaces;

public interface ISpecification<T, TResult>
{
    // The filtering criteria (predicate)
    Expression<Func<T, bool>>? Criteria { get; }

    // A collection of navigation properties to include in the query
    List<Expression<Func<T, object>>> Includes { get; }

    // Sorting condition (Order By)
    Expression<Func<T, object>>? OrderBy { get; }

    // Projection (selector) to shape the result
    Expression<Func<T, TResult>>? Selector { get; } // Add a generic selector
}

public interface ISpecification<T> : ISpecification<T, T>{};
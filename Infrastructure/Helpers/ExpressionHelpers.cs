using System.Linq.Expressions;
using System.Reflection;

namespace Infrastructure.Helpers;

public static class ExpressionHelpers
{
    /// <summary>
    ///     Simple mapping with expression func for ef
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    /// <returns></returns>
    public static Expression<Func<TEntity, TResponse>> MapTo<TEntity, TResponse>()
        where TResponse : new()
    {
        Type typeOfEntity = typeof(TEntity);
        Type typeOfResponse = typeof(TResponse);
        ParameterExpression parameters = Expression.Parameter(typeOfEntity, nameof(TEntity));
        IEnumerable<MemberAssignment?> bindings = typeof(TResponse).GetProperties()
            .Where(p => p.CanWrite)
            .Select(p =>
            {
                PropertyInfo? entityProp = typeOfEntity.GetProperty(p.Name);
                if (entityProp != null && entityProp.PropertyType == p.PropertyType)
                {
                    return Expression.Bind(p, Expression.Property(parameters, entityProp));
                }

                return null;
            })
            .Where(x => x != null);
        var memberInit = Expression.MemberInit(Expression.New(typeOfResponse), bindings!);
        return Expression.Lambda<Func<TEntity, TResponse>>(memberInit, parameters);
    }
}
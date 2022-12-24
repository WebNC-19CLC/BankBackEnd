using System.Linq.Expressions;
using AsrTool.Dtos;
using AsrTool.Infrastructure.Domain.Enums;

namespace AsrTool.Infrastructure.Extensions
{
  public static class QueryableExtension
  {
    private const string INCREASE_FIRST_SORTING = nameof(Queryable.OrderBy);
    private const string DECREASE_FIRST_SORTING = nameof(Queryable.OrderByDescending);

    private const string INCREASE_SORTING = nameof(Queryable.ThenBy);
    private const string DECREASE_SORTING = nameof(Queryable.ThenByDescending);

    /// <summary>
    ///     Apply the sorting of queryable by using <see cref="SortField" />
    ///     CAUTIONS: This sorting only support 1 level, the depth level will not working
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="queryable"></param>
    /// <param name="properties"></param>
    /// <returns></returns>
    public static IQueryable<T> ApplySortFields<T>(this IQueryable<T> queryable, params SortFieldDto[] properties)
    {
      if (properties == null || !properties.Any())
      {
        return queryable;
      }

      var firstSorting = properties.First();
      var sorted = ApplySort(queryable, firstSorting, true);
      foreach (var sortField in properties.Skip(1))
      {
        sorted = ApplySort(sorted, sortField, false);
      }

      return sorted;
    }

    private static IQueryable<T> ApplySort<T>(IQueryable<T> queryable, SortFieldDto sortField, bool isFirstSorting)
    {
      var sortMethod = isFirstSorting
        ? sortField.Order == SortOrder.Asc ? INCREASE_FIRST_SORTING : DECREASE_FIRST_SORTING
        : sortField.Order == SortOrder.Asc
          ? INCREASE_SORTING
          : DECREASE_SORTING;

      var argumentExpression = Expression.Parameter(typeof(T), "src");
      var property = typeof(T).GetProperty(sortField.Field);
      var propertyExpression = Expression.Property(argumentExpression, property);
      var lambdaExpression = Expression.Lambda(typeof(Func<,>).MakeGenericType(typeof(T), property.PropertyType),
          propertyExpression, argumentExpression);

      var result = typeof(Queryable).GetMethods().Single(
          method => method.Name == sortMethod &&
                    method.IsGenericMethodDefinition &&
                    method.GetGenericArguments().Length == 2 &&
                    method.GetParameters().Length == 2)
        .MakeGenericMethod(typeof(T), property.PropertyType)
        .Invoke(null, new object[] { queryable, lambdaExpression });

      return (IOrderedQueryable<T>)result;
    }
  }
}

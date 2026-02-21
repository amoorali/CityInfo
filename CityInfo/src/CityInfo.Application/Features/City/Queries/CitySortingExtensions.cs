using System.Linq.Expressions;

namespace CityInfo.Application.Features.City.Queries
{
    public static class CitySortingExtensions
    {
        private static readonly Dictionary<string, Expression<Func<Domain.Entities.City, object>>> _orderByMap =
            new()
            {
                ["id"] = c => c.Id,
                ["name"] = c => c.Name,
                ["description"] = c => c.Description
            };

        public static IQueryable<Domain.Entities.City> ApplySorting(
            this IQueryable<Domain.Entities.City> query,
            string? orderBy)
        {
            if (string.IsNullOrWhiteSpace(orderBy))
                return query.OrderBy(c => c.Id);

            var parts = orderBy.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var propertyName = parts[0].ToLower();
            var descending = parts.Length > 1 && parts[1].Equals("desc", StringComparison.CurrentCultureIgnoreCase);

            if (_orderByMap.TryGetValue(propertyName, out var expression))
            {
                return descending
                    ? query.OrderByDescending(expression)
                    : query.OrderBy(expression);
            }

            return query.OrderBy(c => c.Id);
        }
    }
}

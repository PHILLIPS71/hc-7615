using HC_7572.DbContexts;
using HC_7572.Domain.Objects;
using Microsoft.EntityFrameworkCore;

namespace HC_7572.GraphQL;

[QueryType]
internal sealed class UserQueries
{
    [UsePaging]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<User> Users(ApplicationDbContext database) => database.Users.AsNoTracking();
}
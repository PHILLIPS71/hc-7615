using GreenDonut.Selectors;
using HC_7572.DbContexts;
using HC_7572.Domain.Objects;
using HotChocolate.Execution.Processing;
using HotChocolate.Pagination;
using HotChocolate.Types.Pagination;
using Microsoft.EntityFrameworkCore;
using Location = HC_7572.Domain.Objects.Location;

namespace HC_7572.GraphQL.Types.Objects;

[ObjectType<User>]
public static partial class UserType
{
    [UsePaging]
    internal static Task<Connection<Location>> GetLocationsAsync(
        [Parent] User user,
        PagingArguments paging,
        ISelection selection,
        ILocationsByUserIdDataLoader dataloader,
        CancellationToken cancellation = default)
    {
        return dataloader
            .WithPagingArguments(paging)
            .Select(selection)
            .LoadAsync(user.Id, cancellation)
            .ToConnectionAsync();
    }

    [DataLoader]
    internal static ValueTask<Dictionary<Guid, Page<Location>>> GetLocationsByUserIdAsync(
        IReadOnlyList<Guid> keys,
        PagingArguments paging,
        ISelectorBuilder selector,
        ApplicationDbContext database,
        CancellationToken cancellation = default)
    {
        return database
            .Users
            .Include(x => x.Locations)
            .AsNoTracking()
            .Where(x => keys.Contains(x.Id))
            .SelectMany(x => x.Locations.Select(l => new { l.Id, Location = l }))
            .OrderBy(x => x.Location.Name)
            // uncommenting this will result is type errors
            // .Select(x => x.Id, x => x.Location, selector)
            .ToBatchPageAsync(x => x.Id, x => x.Location, paging, cancellation);
    }
}
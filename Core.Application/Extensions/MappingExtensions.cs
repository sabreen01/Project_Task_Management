using System.Collections;
using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace Core.Application.Extensions;

public static class MappingExtensions
{
    public static IQueryable<TDestination> MapTo<TDestination>(this IQueryable source, IMapper mapper)
    {
        return source.ProjectTo<TDestination>(mapper.ConfigurationProvider);
    }

    public static IEnumerable<TDestination> MapTo<TDestination>(this IEnumerable source, IMapper mapper)
    {
        return mapper.Map<IEnumerable<TDestination>>(source);
    }
}

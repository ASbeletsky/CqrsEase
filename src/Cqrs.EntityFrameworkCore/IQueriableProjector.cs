using AutoMapper;
using AutoMapper.QueryableExtensions;
using System.Collections.Generic;
using System.Linq;

namespace Cqrs.EntityFrameworkCore
{
    internal class IQueriableProjector : IProjector
    {
        public IQueriableProjector()
        {
            MapperConfig = new MapperConfiguration(x => x.CreateMissingTypeMaps = true);           
            Mapper = MapperConfig.CreateMapper();
        }

        public MapperConfiguration MapperConfig { get; }
        public IMapper Mapper { get; private set; }

        public IQueryable<TDest> ProjectOnly<TSource, TDest>(IQueryable<TSource> queryable, IEnumerable<string> paths)
        {
            return queryable.ProjectTo<TDest>(MapperConfig, parameters: null, membersToExpand: paths.ToArray());
        }

        public IQueryable<T> ProjectOnly<T>(IQueryable<T> queryable, IEnumerable<string> paths)
        {
            var dynamicConfig = new MapperConfiguration(x => x.CreateMap<T, T>().ForAllMembers(m => m.ExplicitExpansion()));
            return queryable.ProjectTo<T>(dynamicConfig, parameters: null, membersToExpand: paths.ToArray());
        }

        public IQueryable<TDest> ProjectTo<TDest>(IQueryable queryable)
        {
            return queryable.ProjectTo<TDest>();
        }
    }
}

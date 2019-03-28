namespace Cqrs.EntityFrameworkCore
{
    #region Using
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using System.Collections.Generic;
    using System.Linq;
    #endregion

    internal class AutoMapperProjector : IProjector
    {
        private readonly IMapper _mapper;

        public AutoMapperProjector(IMapper mapper)
        {
            this._mapper = mapper;
        }

        public IQueryable<TDest> ProjectTo<TDest>(IQueryable queryable)
        {
            return queryable.ProjectTo<TDest>(_mapper.ConfigurationProvider);
        }

        public IQueryable<TDest> ProjectTo<TSource, TDest>(IQueryable<TSource> queryable, IEnumerable<string> paths)
        {
            return queryable.ProjectTo<TDest>(_mapper.ConfigurationProvider, parameters: null, membersToExpand: paths.ToArray());
        }
    }
}

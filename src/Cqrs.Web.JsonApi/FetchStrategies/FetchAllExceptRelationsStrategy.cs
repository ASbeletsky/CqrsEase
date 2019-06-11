namespace Cqrs.Web.JsonApi.FetchStrategies
{
    #region Using
    using Cqrs.Common.Queries.FetchStrategies;
    using Cqrs.JsonApi;
    using System.Linq;
    #endregion

    public class FetchAllExceptRelationsStrategy<TResource> : FetchAllExceptStrategy<TResource>
        where TResource : IResource
    {
        public FetchAllExceptRelationsStrategy() 
            : base(TResourceExtensions.GetRelationsProperties<TResource>().Select(p => p.Name).ToArray())
        {
        }
    }
}

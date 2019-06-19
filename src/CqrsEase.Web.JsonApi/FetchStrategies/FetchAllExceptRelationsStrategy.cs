namespace CqrsEase.Web.JsonApi.FetchStrategies
{
    #region Using
    using CqrsEase.Common.Queries.FetchStrategies;
    using CqrsEase.JsonApi;
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

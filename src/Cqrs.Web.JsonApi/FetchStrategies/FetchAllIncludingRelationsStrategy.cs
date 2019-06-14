namespace Cqrs.Web.JsonApi.FetchStrategies
{
    #region Using
    using Cqrs.Common.Queries;
    using Cqrs.Common.Queries.FetchStrategies;
    using Cqrs.JsonApi;
    using System.Linq;
    #endregion

    public class FetchAllIncludingRelationsStrategy<TResource> : FetchAllStrategy<TResource>
        where TResource : IResource
    {
        public FetchAllIncludingRelationsStrategy(params string[] relationTypes)

        {
            foreach (var resourceType in relationTypes)
            {
                var relationType = TResourceExtensions.FindRelationsProperties<TResource>(resourceType).Select(p => p.DeclaringType).FirstOrDefault();
                if (relationType != null)
                {
                    var relationFields = relationType.GetProperiesNames();
                    this.IncludeRange(relationFields);
                }
            }
        }
    }
}

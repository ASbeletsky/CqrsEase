namespace Cqrs.JsonApi.CommandHandlers
{
    #region Using
    using Cqrs.Common.Commands;
    using Cqrs.Common.Queries;
    using Cqrs.Common.Queries.FetchStrategies;
    using Cqrs.Common.Queries.Pagination;
    using Cqrs.Core.Abstractions;
    using Cqrs.JsonApi.Web;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    #endregion

    public class UpdateCommandHandler<TResource>
        : ICommandHandlerAsync<UpdateCommand<TResource>, IUpdateResult>
        where TResource : class, IResource, new()
    {
        public UpdateCommandHandler(string baseUrl, IQueryHandlerAsync<GetManyQuery<TResource>, IEnumerable<TResource>> getResourcesToUpdateQueryHandler)
        {
            ResourceEndpoint = new JsonApiClient(baseUrl).For<IJsonApiEndpoint<TResource>>();
            GetResourcesToUpdateQueryHandler = getResourcesToUpdateQueryHandler;
        }

        public IJsonApiEndpoint<TResource> ResourceEndpoint { get; private set; }
        public IQueryHandlerAsync<GetManyQuery<TResource>, IEnumerable<TResource>> GetResourcesToUpdateQueryHandler { get; }

        protected async Task<IEnumerable<string>> RequestResourcesIdToUpdate(UpdateCommand<TResource> command)
        {
            var fetchOnlyId = new FetchOnlyStrategy<TResource>(r => r.Id);
            var paging = command.UpdateFirstMatchOnly ? new Page(1, 1) : null;
            var getResourcesQuery = new GetManyQuery<TResource>(fetchOnlyId, command.ApplyTo, paging, sorting: null);
            var resources = await GetResourcesToUpdateQueryHandler.RequestAsync(getResourcesQuery);
            return resources.Select(r => r.Id).ToList();
        }

        public async Task<IUpdateResult> ApplyAsync(UpdateCommand<TResource> command)
        {
            var resourcesIds = await RequestResourcesIdToUpdate(command);
            int updatedCount = 0;
            foreach(var resourceId in resourcesIds)
            {
                await ResourceEndpoint.Patch(resourceId, command.Value);
                //TODO: add exception handling
                updatedCount++;
            }

            return new UpdateResult(updatedCount);
        }
    }
}

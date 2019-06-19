namespace CqrsEase.JsonApi.CommandHandlers
{
    #region Using
    using CqrsEase.Common.Commands;
    using CqrsEase.Common.Queries;
    using CqrsEase.Common.Queries.FetchStrategies;
    using CqrsEase.Common.Queries.Pagination;
    using CqrsEase.Core.Abstractions;
    using CqrsEase.JsonApi.Web;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    #endregion

    public class DeleteCommandHandler<TResource>
        : ICommandHandlerAsync<DeleteCommand<TResource>, IDeleteResult>
        where TResource : class, IResource, new()
    {
        public DeleteCommandHandler(string baseUrl, IQueryHandlerAsync<GetManyQuery<TResource>, IEnumerable<TResource>> getResourcesToUpdateQueryHandler)
        {
            ResourceEndpoint = new JsonApiClient(baseUrl).For<IJsonApiEndpoint<TResource>>();
            GetResourcesToUpdateQueryHandler = getResourcesToUpdateQueryHandler;
        }

        public IJsonApiEndpoint<TResource> ResourceEndpoint { get; private set; }
        public IQueryHandlerAsync<GetManyQuery<TResource>, IEnumerable<TResource>> GetResourcesToUpdateQueryHandler { get; }

        protected async Task<IEnumerable<string>> RequestResourcesIdToUpdate(DeleteCommand<TResource> command)
        {
            var fetchOnlyId = new FetchOnlyStrategy<TResource>(r => r.Id);
            var paging = command.DeleteFirstMatchOnly ? new Page(1, 1) : null;
            var getResourcesQuery = new GetManyQuery<TResource>(fetchOnlyId, command.ApplyTo, paging, sorting: null);
            var resources = await GetResourcesToUpdateQueryHandler.RequestAsync(getResourcesQuery);
            return resources.Select(r => r.Id).ToList();
        }

        public async Task<IDeleteResult> ApplyAsync(DeleteCommand<TResource> command)
        {
            var resourcesIds = await RequestResourcesIdToUpdate(command);
            int deletedCount = 0;
            foreach (var resourceId in resourcesIds)
            {
                await ResourceEndpoint.Delete(resourceId);
                //TODO: add exception handling
                deletedCount++;
            }

            return new DeleteResult(deletedCount);
        }
    }
}

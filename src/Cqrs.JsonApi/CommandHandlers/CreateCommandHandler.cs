using Cqrs.Common.Commands;
using Cqrs.Core.Abstractions;
using Cqrs.JsonApi.Web;
using System.Threading.Tasks;

namespace Cqrs.JsonApi.CommandHandlers
{
    public class CreateCommandHandler<TResource>
        : ICommandHandlerAsync<CreateCommand<TResource>, ICreateResult<TResource>>
        where TResource : class, IResource, new() 
    {
        public CreateCommandHandler(string baseUrl)
        {
            ResourceEndpoint = new JsonApiClient(baseUrl).For<IJsonApiEndpoint<TResource>>();
        }

        public IJsonApiEndpoint<TResource> ResourceEndpoint { get; private set; }

        public async Task<ICreateResult<TResource>> ApplyAsync(CreateCommand<TResource> command)
        {
            var response = await ResourceEndpoint.Post(command.Value); 
            return new CreateResult<TResource>(response?.Data);
        }
    }
}

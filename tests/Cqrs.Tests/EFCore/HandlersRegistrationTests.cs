namespace Cqrs.Tests.EFCore
{
    #region Using
    using Autofac.Extensions.DependencyInjection;
    using Cqrs.Common.Commands;
    using Cqrs.Common.Queries;
    using Cqrs.Common.Queries.Pagination;
    using Cqrs.Core.Abstractions;
    using Cqrs.EntityFrameworkCore;
    using Cqrs.EntityFrameworkCore.CommandHandlers;
    using Cqrs.EntityFrameworkCore.QueryHandlers;
    using Cqrs.Tests.Model;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using System.Collections.Generic;
    using Xunit;
    #endregion

    public class HandlersRegistrationTests
    {
        [Fact]
        public void RegistersGetFirstQueryHandler()
        {
            var services = new ServiceCollection();
            services.AddDbContext<BloggingContext>(o => o.UseInMemoryDatabase());

            services.UseCqrsEntityFramework<BloggingContext>();
            var defaultServiceProvider = services.BuildServiceProvider();
            var autofacServiceProvider = defaultServiceProvider.GetService<AutofacServiceProvider>();

            var blogHandler = autofacServiceProvider.GetService<IQueryHandler<GetFirstQuery<Blog>, Blog>>();
            var blogHandlerAsync = autofacServiceProvider.GetService<IQueryHandlerAsync<GetFirstQuery<Blog>, Blog>>();

            Assert.NotNull(blogHandler);
            Assert.NotNull(blogHandlerAsync);
            Assert.Equal(typeof(GetFirstQueryHandler<Blog>), blogHandler.GetType());
            Assert.Equal(typeof(GetFirstQueryHandler<Blog>), blogHandlerAsync.GetType());
            Assert.NotNull((blogHandler as GetFirstQueryHandler<Blog>).DataSource);
            Assert.NotNull((blogHandlerAsync as GetFirstQueryHandler<Blog>).DataSource);
        }     

        [Fact]
        public void RegistersGetManyQueryHandler()
        {
            var services = new ServiceCollection();
            services.AddDbContext<BloggingContext>(o => o.UseInMemoryDatabase());

            services.UseCqrsEntityFramework<BloggingContext>();
            var defaultServiceProvider = services.BuildServiceProvider();
            var autofacServiceProvider = defaultServiceProvider.GetService<AutofacServiceProvider>();

            var blogHandler = autofacServiceProvider.GetService<IQueryHandler<GetManyQuery<Blog>, IEnumerable<Blog>>>();
            var blogHandlerAsync = autofacServiceProvider.GetService<IQueryHandlerAsync<GetManyQuery<Blog>, IEnumerable<Blog>>> ();

            Assert.NotNull(blogHandler);
            Assert.NotNull(blogHandlerAsync);
            Assert.Equal(typeof(GetManyQueryHandler<Blog>), blogHandler.GetType());
            Assert.Equal(typeof(GetManyQueryHandler<Blog>), blogHandlerAsync.GetType());
            Assert.NotNull((blogHandler as GetManyQueryHandler<Blog>).DataSource);
            Assert.NotNull((blogHandlerAsync as GetManyQueryHandler<Blog>).DataSource);
        }

        [Fact]
        public void RegistersPagedGetManyQueryHandler()
        {
            var services = new ServiceCollection();
            services.AddDbContext<BloggingContext>(o => o.UseInMemoryDatabase());

            services.UseCqrsEntityFramework<BloggingContext>();
            var defaultServiceProvider = services.BuildServiceProvider();
            var autofacServiceProvider = defaultServiceProvider.GetService<AutofacServiceProvider>();

            var blogHandler = autofacServiceProvider.GetService<IQueryHandler<GetManyQuery<Blog>, ILimitedEnumerable<Blog>>>();
            var blogHandlerAsync = autofacServiceProvider.GetService<IQueryHandlerAsync<GetManyQuery<Blog>, ILimitedEnumerable<Blog>>>();

            Assert.NotNull(blogHandler);
            Assert.NotNull(blogHandlerAsync);
            Assert.Equal(typeof(GetManyQueryHandler<Blog>), blogHandler.GetType());
            Assert.Equal(typeof(GetManyQueryHandler<Blog>), blogHandlerAsync.GetType());
            Assert.NotNull((blogHandler as GetManyQueryHandler<Blog>).DataSource);
            Assert.NotNull((blogHandlerAsync as GetManyQueryHandler<Blog>).DataSource);
        }

        [Fact]
        public void RegistersExistsQueryHandler()
        {
            var services = new ServiceCollection();
            services.AddDbContext<BloggingContext>(o => o.UseInMemoryDatabase());

            services.UseCqrsEntityFramework<BloggingContext>();
            var defaultServiceProvider = services.BuildServiceProvider();
            var autofacServiceProvider = defaultServiceProvider.GetService<AutofacServiceProvider>();

            var blogHandler = autofacServiceProvider.GetService<IQueryHandler<ExistsQuery<Blog>, bool>>();
            var blogHandlerAsync = autofacServiceProvider.GetService<IQueryHandlerAsync<ExistsQuery<Blog>, bool>>();

            Assert.NotNull(blogHandler);
            Assert.NotNull(blogHandlerAsync);
            Assert.Equal(typeof(ExistsQueryHandler<Blog>), blogHandler.GetType());
            Assert.Equal(typeof(ExistsQueryHandler<Blog>), blogHandlerAsync.GetType());
            Assert.NotNull((blogHandler as ExistsQueryHandler<Blog>).DataSource);
            Assert.NotNull((blogHandlerAsync as ExistsQueryHandler<Blog>).DataSource);
        }

        [Fact]
        public void RegistersCountQueryHandler()
        {
            var services = new ServiceCollection();
            services.AddDbContext<BloggingContext>(o => o.UseInMemoryDatabase());

            services.UseCqrsEntityFramework<BloggingContext>();
            var defaultServiceProvider = services.BuildServiceProvider();
            var autofacServiceProvider = defaultServiceProvider.GetService<AutofacServiceProvider>();

            var blogHandler = autofacServiceProvider.GetService<IQueryHandler<CountQuery<Blog>, long>>();
            var blogHandlerAsync = autofacServiceProvider.GetService<IQueryHandlerAsync<CountQuery<Blog>, long>>();

            Assert.NotNull(blogHandler);
            Assert.NotNull(blogHandlerAsync);
            Assert.Equal(typeof(CountQueryHandler<Blog>), blogHandler.GetType());
            Assert.Equal(typeof(CountQueryHandler<Blog>), blogHandlerAsync.GetType());
            Assert.NotNull((blogHandler as CountQueryHandler<Blog>).DataSource);
            Assert.NotNull((blogHandlerAsync as CountQueryHandler<Blog>).DataSource);
        }

        [Fact]
        public void RegistersCreateCommandHandler()
        {
            var services = new ServiceCollection();
            services.AddDbContext<BloggingContext>(o => o.UseInMemoryDatabase());

            services.UseCqrsEntityFramework<BloggingContext>();
            var defaultServiceProvider = services.BuildServiceProvider();
            var autofacServiceProvider = defaultServiceProvider.GetService<AutofacServiceProvider>();
           
            var blogHandler = autofacServiceProvider.GetService<ICommandHandler<CreateCommand<Blog>, ICreateResult<Blog>>>();           
            var blogHandlerAsync = autofacServiceProvider.GetService<ICommandHandlerAsync<CreateCommand<Blog>, ICreateResult<Blog>>>();

            Assert.NotNull(blogHandler);
            Assert.NotNull(blogHandlerAsync);
            Assert.Equal(typeof(CreateCommandHandler<Blog>), blogHandler.GetType());
            Assert.Equal(typeof(CreateCommandHandler<Blog>), blogHandlerAsync.GetType());
            Assert.NotNull((blogHandler as CreateCommandHandler<Blog>).DataSource);
            Assert.NotNull((blogHandlerAsync as CreateCommandHandler<Blog>).DataSource);
        }

        [Fact]
        public void RegistersCreateManyCommandHandler()
        {
            var services = new ServiceCollection();
            services.AddDbContext<BloggingContext>(o => o.UseInMemoryDatabase());

            services.UseCqrsEntityFramework<BloggingContext>();
            var defaultServiceProvider = services.BuildServiceProvider();
            var autofacServiceProvider = defaultServiceProvider.GetService<AutofacServiceProvider>();

            var blogHandler = autofacServiceProvider.GetService<ICommandHandler<CreateManyCommand<Blog>>>();
            var blogHandlerAsync = autofacServiceProvider.GetService<ICommandHandlerAsync<CreateManyCommand<Blog>>>();

            Assert.NotNull(blogHandler);
            Assert.NotNull(blogHandlerAsync);
            Assert.Equal(typeof(CreateManyCommandHandler<Blog>), blogHandler.GetType());
            Assert.Equal(typeof(CreateManyCommandHandler<Blog>), blogHandlerAsync.GetType());
            Assert.NotNull((blogHandler as CreateManyCommandHandler<Blog>).DataSource);
            Assert.NotNull((blogHandlerAsync as CreateManyCommandHandler<Blog>).DataSource);
        }

        [Fact]
        public void RegistersUpdateCommandHandler()
        {
            var services = new ServiceCollection();
            services.AddDbContext<BloggingContext>(o => o.UseInMemoryDatabase());

            services.UseCqrsEntityFramework<BloggingContext>();
            var defaultServiceProvider = services.BuildServiceProvider();
            var autofacServiceProvider = defaultServiceProvider.GetService<AutofacServiceProvider>();

            var blogHandler = autofacServiceProvider.GetService<ICommandHandler<UpdateCommand<Blog>, IUpdateResult>>();
            var blogHandlerAsync = autofacServiceProvider.GetService<ICommandHandlerAsync<UpdateCommand<Blog>, IUpdateResult>>();
            var projectedBlogHandler = autofacServiceProvider.GetService<ICommandHandler<UpdateCommand<Blog, BlogDto>, IUpdateResult>>();
            var projectedBlogHandlerAsync = autofacServiceProvider.GetService<ICommandHandlerAsync<UpdateCommand<Blog, BlogDto>, IUpdateResult>>();

            Assert.NotNull(blogHandler);
            Assert.NotNull(blogHandlerAsync);
            Assert.NotNull(projectedBlogHandler);
            Assert.NotNull(projectedBlogHandlerAsync);
            Assert.Equal(typeof(UpdateCommandHandler<Blog>), blogHandler.GetType());
            Assert.Equal(typeof(UpdateCommandHandler<Blog>), blogHandlerAsync.GetType());
            Assert.Equal(typeof(UpdateCommandHandler<Blog, BlogDto>), projectedBlogHandler.GetType());
            Assert.Equal(typeof(UpdateCommandHandler<Blog, BlogDto>), projectedBlogHandlerAsync.GetType());
            Assert.NotNull((blogHandler as UpdateCommandHandler<Blog>).DataSource);
            Assert.NotNull((blogHandlerAsync as UpdateCommandHandler<Blog>).DataSource);
            Assert.NotNull((projectedBlogHandler as UpdateCommandHandler<Blog, BlogDto>).DataSource);
            Assert.NotNull((projectedBlogHandlerAsync as UpdateCommandHandler<Blog, BlogDto>).DataSource);
        }

        [Fact]
        public void RegistersDeleteCommandHandler()
        {
            var services = new ServiceCollection();
            services.AddDbContext<BloggingContext>(o => o.UseInMemoryDatabase());

            services.UseCqrsEntityFramework<BloggingContext>();
            var defaultServiceProvider = services.BuildServiceProvider();
            var autofacServiceProvider = defaultServiceProvider.GetService<AutofacServiceProvider>();

            var blogHandler = autofacServiceProvider.GetService<ICommandHandler<DeleteCommand<Blog>, IDeleteResult>>();
            var blogHandlerAsync = autofacServiceProvider.GetService<ICommandHandlerAsync<DeleteCommand<Blog>, IDeleteResult>>();
           
            Assert.NotNull(blogHandler);
            Assert.NotNull(blogHandlerAsync);
            Assert.Equal(typeof(DeleteCommandHandler<Blog>), blogHandler.GetType());
            Assert.Equal(typeof(DeleteCommandHandler<Blog>), blogHandlerAsync.GetType());
            Assert.NotNull((blogHandler as DeleteCommandHandler<Blog>).DataSource);
            Assert.NotNull((blogHandlerAsync as DeleteCommandHandler<Blog>).DataSource);
        }
    }
}

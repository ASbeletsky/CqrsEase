namespace Cqrs.Tests.EFCore
{
    #region Using
    using Autofac.Extensions.DependencyInjection;
    using Cqrs.Common.Commands;
    using Cqrs.Common.Queries;
    using Cqrs.Common.Queries.Pagination;
    using Cqrs.Core.Abstractions;
    using Cqrs.EntityFrameworkCore;
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
            var commentHandler = autofacServiceProvider.GetService<IQueryHandler<GetFirstQuery<Comment>, Comment>>();
            var commentHandlerAsync = autofacServiceProvider.GetService<IQueryHandlerAsync<GetFirstQuery<Comment>, Comment>>();

            Assert.NotNull(blogHandler);
            Assert.NotNull(blogHandlerAsync);
            Assert.NotNull(commentHandler);
            Assert.NotNull(commentHandlerAsync);
        }

        [Fact]
        public void RegistersProjectFirstQueryHandler()
        {
            var services = new ServiceCollection();
            services.AddDbContext<BloggingContext>(o => o.UseInMemoryDatabase());

            services.UseCqrsEntityFramework<BloggingContext>();
            var defaultServiceProvider = services.BuildServiceProvider();
            var autofacServiceProvider = defaultServiceProvider.GetService<AutofacServiceProvider>();

            var blogHandler = autofacServiceProvider.GetService<IQueryHandler<ProjectFirstQuery<Blog, Blog>, Blog>>();
            var blogHandlerAsync = autofacServiceProvider.GetService<IQueryHandlerAsync<ProjectFirstQuery<Blog, Blog>, Blog>>();
            var commentHandler = autofacServiceProvider.GetService<IQueryHandler<ProjectFirstQuery<Comment, Comment>, Comment>>();
            var commentHandlerAsync = autofacServiceProvider.GetService<IQueryHandlerAsync<ProjectFirstQuery<Comment, Comment>, Comment>>();

            Assert.NotNull(blogHandler);
            Assert.NotNull(blogHandlerAsync);
            Assert.NotNull(commentHandler);
            Assert.NotNull(commentHandlerAsync);
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
            var commentHandler = autofacServiceProvider.GetService<IQueryHandler<GetManyQuery<Comment>, IEnumerable<Comment>>>();
            var commentHandlerAsync = autofacServiceProvider.GetService<IQueryHandlerAsync<GetManyQuery<Comment>, IEnumerable<Comment>>>();
            Assert.NotNull(blogHandler);
            Assert.NotNull(blogHandlerAsync);
            Assert.NotNull(commentHandler);
            Assert.NotNull(commentHandlerAsync);
        }

        [Fact]
        public void RegistersPageGetManyQueryHandler()
        {
            var services = new ServiceCollection();
            services.AddDbContext<BloggingContext>(o => o.UseInMemoryDatabase());

            services.UseCqrsEntityFramework<BloggingContext>();
            var defaultServiceProvider = services.BuildServiceProvider();
            var autofacServiceProvider = defaultServiceProvider.GetService<AutofacServiceProvider>();

            var blogHandler = autofacServiceProvider.GetService<IQueryHandler<GetManyQuery<Blog>, ILimitedEnumerable<Blog>>>();
            var blogHandlerAsync = autofacServiceProvider.GetService<IQueryHandlerAsync<GetManyQuery<Blog>, ILimitedEnumerable<Blog>>>();
            var commentHandler = autofacServiceProvider.GetService<IQueryHandler<GetManyQuery<Comment>, ILimitedEnumerable<Comment>>>();
            var commentHandlerAsync = autofacServiceProvider.GetService<IQueryHandlerAsync<GetManyQuery<Comment>, ILimitedEnumerable<Comment>>>();
            Assert.NotNull(blogHandler);
            Assert.NotNull(blogHandlerAsync);
            Assert.NotNull(commentHandler);
            Assert.NotNull(commentHandlerAsync);
        }

        [Fact]
        public void RegistersProjectManyQueryHandler()
        {
            var services = new ServiceCollection();
            services.AddDbContext<BloggingContext>(o => o.UseInMemoryDatabase());

            services.UseCqrsEntityFramework<BloggingContext>();
            var defaultServiceProvider = services.BuildServiceProvider();
            var autofacServiceProvider = defaultServiceProvider.GetService<AutofacServiceProvider>();

            var blogHandler = autofacServiceProvider.GetService<IQueryHandler<ProjectManyQuery<Blog, Blog>, IEnumerable<Blog>>>();
            var blogHandlerAsync = autofacServiceProvider.GetService<IQueryHandlerAsync<ProjectManyQuery<Blog, Blog>, IEnumerable<Blog>>>();
            var commentHandler = autofacServiceProvider.GetService<IQueryHandler<ProjectManyQuery<Comment, Comment>, IEnumerable<Comment>>>();
            var commentHandlerAsync = autofacServiceProvider.GetService<IQueryHandlerAsync<ProjectManyQuery<Comment, Comment>, IEnumerable<Comment>>>();

            Assert.NotNull(blogHandler);
            Assert.NotNull(blogHandlerAsync);
            Assert.NotNull(commentHandler);
            Assert.NotNull(commentHandlerAsync);
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
            var commentHandler = autofacServiceProvider.GetService<IQueryHandler<ExistsQuery<Comment>, bool>>();
            var commentHandlerAsync = autofacServiceProvider.GetService<IQueryHandlerAsync<ExistsQuery<Comment>, bool>>();

            Assert.NotNull(blogHandler);
            Assert.NotNull(blogHandlerAsync);
            Assert.NotNull(commentHandler);
            Assert.NotNull(commentHandlerAsync);
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
            var commentHandler = autofacServiceProvider.GetService<IQueryHandler<CountQuery<Comment>, long>>();
            var commentHandlerAsync = autofacServiceProvider.GetService<IQueryHandlerAsync<CountQuery<Comment>, long>>();

            Assert.NotNull(blogHandler);
            Assert.NotNull(blogHandlerAsync);
            Assert.NotNull(commentHandler);
            Assert.NotNull(commentHandlerAsync);
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
            var projectedBlogHandler = autofacServiceProvider.GetService<ICommandHandler<UpdateCommand<Blog, BlogDto>, IUpdateResult>>();
            var blogHandlerAsync = autofacServiceProvider.GetService<ICommandHandlerAsync<UpdateCommand<Blog>, IUpdateResult>>();
            var projectedBlogHandlerAsync = autofacServiceProvider.GetService<ICommandHandlerAsync<UpdateCommand<Blog, BlogDto>, IUpdateResult>>();

            Assert.NotNull(blogHandler);
            Assert.NotNull(projectedBlogHandler);
            Assert.NotNull(blogHandlerAsync);
            Assert.NotNull(projectedBlogHandlerAsync);
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
        }
    }
}

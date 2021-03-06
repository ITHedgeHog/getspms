﻿using System;
using System.Linq;
using System.Threading;
using MediatR;
using Moq;
using Shouldly;
using SPMS.Application.Authoring.Command.PublishPost;
using SPMS.Application.Tests.Common;
using SPMS.Common;
using SPMS.Domain.Models;
using SPMS.Persistence.MSSQL;
using Xunit;

namespace SPMS.Application.Tests.Authoring.Command
{
    public class PublishPostCommandTests : IClassFixture<PostFixture>
    {
        private SpmsContext Context;
        public PublishPostCommandTests(PostFixture fixture)
        {
            Context = fixture.Context;
        }

        [Fact]
        public async void Should_Mark_As_Published()
        {
            var mediatorMock = new Mock<IMediator>();
            var sut = new PublishPostCommand.PublishPostHandler(Context);

            var result = await sut.Handle(new PublishPostCommand() {Id = 1}, CancellationToken.None);

            result.ShouldBeTrue();
            Context.EpisodeEntry.Count(x =>
                x.EpisodeEntryStatusId == Context.EpisodeEntryStatus.First(x => x.Name == StaticValues.Published).Id).ShouldBe(1);
        }

        [Fact]
        public async void Should_Not_Find_Post_And_return_false()
        {

            var mediatorMock = new Mock<IMediator>();
            var sut = new PublishPostCommand.PublishPostHandler(Context);

            var result = await sut.Handle(new PublishPostCommand() {Id = 120}, CancellationToken.None);

            result.ShouldBeFalse();
        }
    }

    public class PostFixture : IDisposable
    {
        public SpmsContext Context { get; set; }
        public PostFixture()
        {
            Context = TestSpmsContextFactory.Create();
            Context.EpisodeEntry.Add(new EpisodeEntry()
            {
                Title = "Post 1",
                Location = "Location",
                Timeline = "Timeline",
                Content = "A post!",
                Created = DateTime.UtcNow,
                CreatedBy = "Dan Taylor",
                LastModified = DateTime.UtcNow,
                LastModifiedBy = "Dan Taylor",
                EpisodeEntryStatusId = Context.EpisodeEntryStatus.First(x => x.Name == StaticValues.Draft).Id,
                EpisodeEntryTypeId = Context.EpisodeEntryType.First(x => x.Name == StaticValues.Post).Id
            });

            Context.SaveChanges();
        }

        public void Dispose()
        {
            Context?.Dispose();
        }
    }
}
// ---------------------------------------------------------------
// Copyright (c) Brian Parker & Hassan Habib All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using System.Linq;
using AutoFixture.Xunit2;
using Bunit;
using Bunit.TestDoubles;
using bVirtualization.Models.BVirutalizationComponents;
using bVirtualization.Views.Components;
using FluentAssertions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Xunit;

namespace bVirtualization.Tests.Unit.Views.Components.BVirtualizations
{
    public partial class BVirtualizationComponentTests : TestContext
    {
        [Fact]
        public void ShouldInitializeComponent()
        {
            // given
            BVirutalizationComponentState expectedState =
                BVirutalizationComponentState.Loading;

            // when
            var initialBVirtualizationComponent =
                new BVirtualizationComponent<object>();

            // then
            initialBVirtualizationComponent.State.Should().Be(expectedState);
            initialBVirtualizationComponent.DataSource.Should().BeNull();
            initialBVirtualizationComponent.ChildContent.Should().BeNull();
            initialBVirtualizationComponent.Label.Should().BeNull();
            initialBVirtualizationComponent.ErrorMessage.Should().BeNull();
        }

        [Theory, AutoData]
        public void ShouldSetParameters(object[] randomData)
        {
            // given
            IQueryable<object> inputDataSource = randomData.AsQueryable();

            IQueryable<object> expectedDataSource = inputDataSource;

            RenderFragment<object> inputChildContent = val => builder => { };

            RenderFragment<object> expectedChildContent = inputChildContent;

            // when
            var cut = RenderComponent<BVirtualizationComponent<object>>(parameters => parameters
                .Add(p => p.DataSource, inputDataSource)
                .Add(p => p.ChildContent, inputChildContent));

            // then
            cut.Instance.DataSource.Should().BeSameAs(expectedDataSource);
            cut.Instance.ChildContent.Should().Be(expectedChildContent);
        }

        [Theory, AutoData]
        public void ShouldRenderContent(object[] inputDataSource)
        {
            // when
            var cut = RenderComponent<BVirtualizationComponent<object>>(parameters => parameters
                .Add(p => p.DataSource, inputDataSource.AsQueryable())
                .Add(p => p.ChildContent, obj => $"<p>{obj}</p>"));

            // then
            cut.FindAll("p").Count.Should().Be(inputDataSource.Count());
        }

        [Theory, AutoData]
        public void ShouldPassDataToChildContentTemplate(string[] inputDataSource)
        {
            // when
            var cut = RenderComponent<BVirtualizationComponent<string>>(parameters => parameters
                .Add(p => p.DataSource, inputDataSource.AsQueryable())
                .Add<Tmpl<string>, string>(p => p.ChildContent, data => paraParams => paraParams.Add(p => p.Data, data)));

            // then
            cut.FindComponents<Tmpl<string>>()
                .Select(x => x.Instance.Data)
                .Should()
                .BeEquivalentTo(inputDataSource);
        }

        [Fact]
        public void ShouldRenderErrorWhenNullDataSource()
        {
            // given
            BVirutalizationComponentState expectedState =
                BVirutalizationComponentState.Error;

            string expectedErrorMessage =
                "Virtualization service error ocurred, contact support.";

            // when
            var cut = RenderComponent<BVirtualizationComponent<object>>(parameters => parameters
                .Add(p => p.DataSource, null));

            // then
            cut.Instance.State.Should().Be(expectedState);
            cut.Instance.ErrorMessage.Should().Be(expectedErrorMessage);
            cut.Instance.Label.Should().NotBeNull();
            cut.Instance.Label.Value.Should().Be(expectedErrorMessage);
        }

        [Theory, AutoData]
        public void ShouldPassCorrectParametersToVirtualize(string[] inputDataSource)
        {
            // given
            ComponentFactories.AddStub<Virtualize<string>>();
            RenderFragment<string> inputChildContent = val => builder => { };

            // when
            var cut = RenderComponent<BVirtualizationComponent<string>>(parameters => parameters
                .Add(p => p.DataSource, inputDataSource.AsQueryable())
                .Add(p => p.ChildContent, inputChildContent));

            // then
            var virtStub = cut.FindComponent<Stub<Virtualize<string>>>();
            virtStub.Instance.Parameters.Get(x => x.OverscanCount).Should().Be(3);
        }
    }
}

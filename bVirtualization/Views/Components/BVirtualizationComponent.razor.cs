// ---------------------------------------------------------------
// Copyright (c) Brian Parker & Hassan Habib All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using System;
using System.Linq;
using bVirtualization.Models.BVirutalizationComponents;
using bVirtualization.Services;
using bVirtualization.Views.Base;
using Microsoft.AspNetCore.Components;

namespace bVirtualization.Views.Components
{
    public partial class BVirtualizationComponent<T> : ComponentBase
    {
        [Parameter]
        public RenderFragment<T> ChildContent { get; set; }

        [Inject]
        public IVirtualizationService<T> VirtualizeService { get; set; }

        protected override void OnInitialized()
        {
            this.State = BVirutalizationComponentState.Content;
            base.OnInitialized();
        }

        public IQueryable<T> DataSource { get; set; }
        public BVirutalizationComponentState State { get; set; }
        public string ErrorMessage { get; set; }
        public LabelBase Label { get; set; }

        private (IQueryable<T> DataSource, int TotalCount) RetrieveData(
            int index, 
            int quantity)
        {
            var data = VirtualizeService.LoadFirstPage((uint)index, (uint)quantity);
            var totalCount = data.Count();

            return (data, totalCount);
        }
    }
}
// ---------------------------------------------------------------
// Copyright (c) Brian Parker & Hassan Habib All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace bVirtualization.Tests.Unit.Views.Components.BVirtualizations
{
    public class Tmpl<T> : ComponentBase
    {
        [Parameter] public T? Data { get; set; }
    }
}

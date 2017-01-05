﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Xunit;

namespace Microsoft.Data.Sqlite.Internal
{
    public class SqliteEngineTest
    {
        [Fact(Skip = "Fails on .NET Framework from command line")]
        public void UseWinSqlite3_throws_when_loaded()
        {
            var ex = Assert.Throws<InvalidOperationException>(() => SqliteEngine.UseWinSqlite3());

            Assert.Equal(Strings.AlreadyLoaded, ex.Message);
        }
    }
}

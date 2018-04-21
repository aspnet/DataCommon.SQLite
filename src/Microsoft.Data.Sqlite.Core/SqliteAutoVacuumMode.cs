// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Microsoft.Data.Sqlite
{
    /// <summary>
    /// Represents the auto vacuum mode of a SQLite database.
    /// </summary>
    public enum SqliteAutoVacuumMode
    {
        /// <summary>
        /// The auto vacuum mode is not set.
        /// </summary>
        Undefined = 0,

        /// <summary>
        /// Auto-vacuum is disabled.
        /// </summary>
        None,

        /// <summary>
        /// The freelist pages are moved to the end of the database file and the database file is truncated to remove the freelist pages at every transaction commit.
        /// </summary>
        Full,

        /// <summary>
        /// The additional information needed to do auto-vacuuming is stored in the database file but auto-vacuuming does not occur automatically at each commit as it does with auto_vacuum=full. In incremental mode, the separate incremental_vacuum pragma must be invoked to cause the auto-vacuum to occur.
        /// </summary>
        Incremental,
    }
}
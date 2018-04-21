// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Microsoft.Data.Sqlite
{
    /// <summary>
    /// Represents the synchronous setting of a SQLite database.
    /// </summary>
    public enum SqliteSynchronousMode
    {
        /// <summary>
        /// The synchronous setting is not set.
        /// </summary>
        Undefined = 0,

        /// <summary>
        /// SQLite continues without syncing as soon as it has handed data off to the operating system.
        /// </summary>
        Off,

        /// <summary>
        /// The SQLite database engine will still sync at the most critical moments, but less often than in Full mode.
        /// </summary>
        Normal,

        /// <summary>
        /// The SQLite database engine will use the xSync method of the VFS to ensure that all content is safely written to the disk surface prior to continuing.
        /// </summary>
        Full,

        /// <summary>
        /// Extra synchronous is like Full with the addition that the directory containing a rollback journal is synced after that journal is unlinked to commit a transaction in DELETE mode. 
        /// </summary>
        Extra,
    }
}
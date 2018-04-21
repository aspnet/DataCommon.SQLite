// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Microsoft.Data.Sqlite
{
    /// <summary>
    /// Represents the journal mode of a SQLite database.
    /// </summary>
    public enum SqliteJournalMode
    {
        /// <summary>
        /// The journaling mode is not set.
        /// </summary>
        Undefined = 0,

        /// <summary>
        /// The rollback journal is deleted at the conclusion of each transaction. This is normal behaviour.
        /// </summary>
        Delete,

        /// <summary>
        /// This journaling mode commits transactions by truncating the rollback journal to zero-length instead of deleting it.
        /// </summary>
        Truncate,

        /// <summary>
        /// This journaling mode prevents the rollback journal from being deleted at the end of each transaction. Instead, the header of the journal is overwritten with zeros.
        /// </summary>
        Persist,

        /// <summary>
        /// This journaling mode stores the rollback journal in volatile RAM.
        /// </summary>
        Memory,

        /// <summary>
        /// This journaling mode uses a write-ahead log instead of a rollback journal to implement transactions.
        /// </summary>
        WAL,

        /// <summary>
        /// This journaling mode disables the rollback journal completely. It disables the atomic commit and rollback capabilities of SQLite.
        /// </summary>
        Off
    }
}
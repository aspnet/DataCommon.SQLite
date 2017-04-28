// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Microsoft.Data.Sqlite
{
    using System;
    using SQLitePCL;

    internal class SqliteStatement
    {
        private readonly SqliteConnection _connection;

        public SqliteStatement(sqlite3_stmt stmt, SqliteConnection connection)
        {
            RawStatement = stmt;
            _connection = connection;
        }

        private SqliteStatement() => throw new NotSupportedException();

        public sqlite3_stmt RawStatement { get; }
        public bool BinaryGUID => _connection.ConnectionStringBuilder.BinaryGUID;

        public IntPtr Ptr => RawStatement.ptr;

        public static implicit operator sqlite3_stmt(SqliteStatement stmt)
        {
            return stmt.RawStatement;
        }

        internal void Dispose() => RawStatement.Dispose();
    }
}
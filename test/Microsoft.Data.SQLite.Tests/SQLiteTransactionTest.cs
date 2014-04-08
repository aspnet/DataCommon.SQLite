﻿// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Data;
using Microsoft.Data.SQLite.Utilities;
using Xunit;

namespace Microsoft.Data.SQLite
{
    public class SQLiteTransactionTest
    {
        [Fact]
        public void Ctor_sets_read_uncommitted()
        {
            using (var connection = new SQLiteConnection("Filename=:memory:"))
            {
                connection.Open();

                using (connection.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    Assert.Equal(1L, connection.ExecuteScalar<long>("PRAGMA read_uncommitted"));
                }
            }
        }

        [Fact]
        public void Ctor_unsets_read_uncommitted_when_serializable()
        {
            using (var connection = new SQLiteConnection("Filename=:memory:"))
            {
                connection.Open();

                using (connection.BeginTransaction(IsolationLevel.Serializable))
                {
                    Assert.Equal(0L, connection.ExecuteScalar<long>("PRAGMA read_uncommitted"));
                }
            }
        }

        [Fact]
        public void Ctor_throws_when_invalid_isolation_level()
        {
            using (var connection = new SQLiteConnection("Filename=:memory:"))
            {
                connection.Open();

                var ex = Assert.Throws<ArgumentException>(() => connection.BeginTransaction(0));

                Assert.Equal(Strings.FormatInvalidIsolationLevel(0), ex.Message);
            }
        }

        [Fact]
        public void IsolationLevel_throws_when_completed()
        {
            using (var connection = new SQLiteConnection("Filename=:memory:"))
            {
                connection.Open();

                var transaction = connection.BeginTransaction();
                transaction.Dispose();

                var ex = Assert.Throws<InvalidOperationException>(() => transaction.IsolationLevel);

                Assert.Equal(Strings.TransactionCompleted, ex.Message);
            }
        }

        [Fact]
        public void IsolationLevel_is_infered_when_unspecified()
        {
            using (var connection = new SQLiteConnection("Filename=:memory:"))
            {
                connection.Open();
                connection.ExecuteNonQuery("PRAGMA read_uncommitted = 1");

                using (var transaction = connection.BeginTransaction())
                    Assert.Equal(IsolationLevel.ReadUncommitted, transaction.IsolationLevel);
            }
        }

        [Fact]
        public void Commit_throws_when_completed()
        {
            using (var connection = new SQLiteConnection("Filename=:memory:"))
            {
                connection.Open();

                var transaction = connection.BeginTransaction();
                transaction.Dispose();

                var ex = Assert.Throws<InvalidOperationException>(() => transaction.Commit());

                Assert.Equal(Strings.TransactionCompleted, ex.Message);
            }
        }

        [Fact]
        public void Commit_throws_when_connection_closed()
        {
            using (var connection = new SQLiteConnection("Filename=:memory:"))
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    connection.Close();

                    var ex = Assert.Throws<InvalidOperationException>(() => transaction.Commit());

                    Assert.Equal(Strings.TransactionCompleted, ex.Message);
                }
            }
        }

        [Fact]
        public void Commit_works()
        {
            using (var connection = new SQLiteConnection("Filename=:memory:"))
            {
                CreateTestTable(connection);

                using (var transaction = connection.BeginTransaction())
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "INSERT INTO TestTable VALUES (1)";
                    command.ExecuteNonQuery();

                    transaction.Commit();

                    Assert.Null(connection.Transaction);
                    Assert.Null(transaction.Connection);
                }

                Assert.Equal(1L, connection.ExecuteScalar<long>("SELECT COUNT(*) FROM TestTable"));
            }
        }

        [Fact]
        public void Rollback_throws_when_completed()
        {
            using (var connection = new SQLiteConnection("Filename=:memory:"))
            {
                connection.Open();

                var transaction = connection.BeginTransaction();
                transaction.Dispose();

                var ex = Assert.Throws<InvalidOperationException>(() => transaction.Rollback());

                Assert.Equal(Strings.TransactionCompleted, ex.Message);
            }
        }

        [Fact]
        public void Rollback_works()
        {
            using (var connection = new SQLiteConnection("Filename=:memory:"))
            {
                CreateTestTable(connection);

                using (var transaction = connection.BeginTransaction())
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "INSERT INTO TestTable VALUES (1)";
                    command.ExecuteNonQuery();

                    transaction.Rollback();

                    Assert.Null(connection.Transaction);
                    Assert.Null(transaction.Connection);
                }

                Assert.Equal(0L, connection.ExecuteScalar<long>("SELECT COUNT(*) FROM TestTable"));
            }
        }

        [Fact]
        public void Dispose_works()
        {
            using (var connection = new SQLiteConnection("Filename=:memory:"))
            {
                CreateTestTable(connection);

                using (var transaction = connection.BeginTransaction())
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "INSERT INTO TestTable VALUES (1)";
                    command.ExecuteNonQuery();

                    transaction.Dispose();

                    Assert.Null(connection.Transaction);
                    Assert.Null(transaction.Connection);
                }

                Assert.Equal(0L, connection.ExecuteScalar<long>("SELECT COUNT(*) FROM TestTable"));
            }
        }

        [Fact]
        public void Dispose_can_be_called_more_than_once()
        {
            using (var connection = new SQLiteConnection("Filename=:memory:"))
            {
                CreateTestTable(connection);

                var transaction = connection.BeginTransaction();
                transaction.Dispose();
                transaction.Dispose();
            }
        }

        private static void CreateTestTable(SQLiteConnection connection)
        {
            connection.Open();
            connection.ExecuteNonQuery(@"
                CREATE TABLE TestTable (
                    TestColumn INTEGER
                )");
        }
    }
}

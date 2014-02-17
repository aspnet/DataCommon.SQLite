﻿// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using JetBrains.Annotations;
using Microsoft.Data.SQLite.Interop;
using Microsoft.Data.SQLite.Utilities;

namespace Microsoft.Data.SQLite
{
    public class SQLiteConnection : DbConnection
    {
        private string _connectionString;
        private ConnectionState _state;
        internal IntPtr _db = IntPtr.Zero;
        private bool _disposed;

        public SQLiteConnection()
        {
        }

        public SQLiteConnection([NotNull] string connectionString)
            : this()
        {
            Check.NotEmpty(connectionString, "connectionString");

            ConnectionString = connectionString;
        }

        public override string ConnectionString
        {
            get { return _connectionString; }
            [param: NotNull]
            set
            {
                Check.NotEmpty(value, "value");
                if (_state != ConnectionState.Closed)
                    throw new InvalidOperationException(Strings.ConnectionStringRequiresClosedConnection);

                // TODO: Parse and cache
                _connectionString = value;
            }
        }

        public override string Database
        {
            get { return _connectionString; }
        }

        public override string DataSource
        {
            get { return _connectionString; }
        }

        public override string ServerVersion
        {
            get { return NativeMethods.sqlite3_libversion(); }
        }

        public override ConnectionState State
        {
            get { return _state; }
        }

        private void SetState(ConnectionState value)
        {
            if (_state == value)
                return;

            var originalState = _state;
            _state = value;
            OnStateChange(new StateChangeEventArgs(originalState, value));
        }

        public override void Open()
        {
            if (_disposed)
                throw new ObjectDisposedException(null);
            if (_state == ConnectionState.Open)
                return;
            if (_connectionString == null)
                throw new InvalidOperationException(Strings.OpenRequiresSetConnectionString);

            Debug.Assert(_db == IntPtr.Zero, "_db is not Zero.");

            var rc = NativeMethods.sqlite3_open(_connectionString, out _db);
            MarshalEx.ThrowExceptionForRC(rc);

            SetState(ConnectionState.Open);
        }

        public override void Close()
        {
            ReleaseNativeObjects();
            SetState(ConnectionState.Closed);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                SetState(ConnectionState.Closed);

                _disposed = true;
            }

            ReleaseNativeObjects();

            base.Dispose(disposing);
        }

        private void ReleaseNativeObjects()
        {
            if (_db == IntPtr.Zero)
                return;

            var rc = NativeMethods.sqlite3_close_v2(_db);
            Debug.Assert(rc == NativeMethods.SQLITE_OK, "rc is not SQLITE_OK.");

            _db = IntPtr.Zero;
        }

        public new SQLiteCommand CreateCommand()
        {
            return new SQLiteCommand { Connection = this };
        }

        protected override DbCommand CreateDbCommand()
        {
            return CreateCommand();
        }

        public new SQLiteTransaction BeginTransaction()
        {
            return BeginTransaction(IsolationLevel.Serializable);
        }

        protected override DbTransaction BeginDbTransaction(IsolationLevel isolationLevel)
        {
            return BeginTransaction(isolationLevel);
        }

        public new SQLiteTransaction BeginTransaction(IsolationLevel isolationLevel)
        {
            if (isolationLevel != IsolationLevel.ReadUncommitted && isolationLevel != IsolationLevel.Serializable)
                throw new ArgumentException(Strings.InvalidIsolationLevel(isolationLevel));
            if (_state != ConnectionState.Open)
                throw new InvalidOperationException(Strings.CallRequiresOpenConnection("BeginTransaction"));

            return new SQLiteTransaction(this, isolationLevel);
        }

        public override void ChangeDatabase(string databaseName)
        {
            throw new NotSupportedException();
        }
    }
}

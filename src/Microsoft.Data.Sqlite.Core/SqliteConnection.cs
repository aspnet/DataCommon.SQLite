// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.IO;
using Microsoft.Data.Sqlite.Properties;
using Microsoft.Data.Sqlite.Utilities;
using SQLitePCL;

namespace Microsoft.Data.Sqlite
{
    /// <summary>
    ///     Represents a connection to a SQLite database.
    /// </summary>
    public partial class SqliteConnection : DbConnection
    {
        private const string MainDatabaseName = "main";

        private readonly IList<WeakReference<SqliteCommand>> _commands = new List<WeakReference<SqliteCommand>>();
        private readonly SqliteConfiguration _configuration = new SqliteConfiguration();

        private string _connectionString;
        private ConnectionState _state;
        private sqlite3 _db;

        static SqliteConnection()
            => BundleInitializer.Initialize();

        /// <summary>
        ///     Initializes a new instance of the <see cref="SqliteConnection" /> class.
        /// </summary>
        public SqliteConnection()
            => StateChange += (_, e) =>
            {
                if (e.CurrentState == ConnectionState.Open)
                {
                    SetForeignKeys();
                    SetRecursiveTriggers();
                    SetJournalMode();
                    SetSynchronous();
                    SetAutoVacuum();
                    SetAutomaticIndex();

                    foreach (var (name, collation, obj) in _configuration.Collations)
                    {
                        CreateCollationCore(name, collation, obj);
                    }

                    foreach (var (name, arity, func, flags, obj) in _configuration.Functions)
                    {
                        CreateFunctionCore(name, arity, func, flags, obj);
                    }

                    foreach (var (name, arity, flags, obj, func_step, func_final) in _configuration.Aggregates)
                    {
                        CreateAggregateCore(name, arity, flags, obj, func_step, func_final);
                    }
                }
            };

        /// <summary>
        ///     Initializes a new instance of the <see cref="SqliteConnection" /> class.
        /// </summary>
        /// <param name="connectionString">The string used to open the connection.</param>
        /// <seealso cref="SqliteConnectionStringBuilder" />
        public SqliteConnection(string connectionString) : this()
            => ConnectionString = connectionString;

        /// <summary>
        ///     Initializes a new instance of the <see cref="SqliteConnection" /> class.
        /// </summary>
        /// <param name="connectionString">The string used to open the connection.</param>
        /// <param name="sqliteConfiguration">The configuration of the connection.</param>
        /// <seealso cref="SqliteConnectionStringBuilder" />
        public SqliteConnection(string connectionString, SqliteConfiguration sqliteConfiguration)
            : this(connectionString)
            => _configuration = sqliteConfiguration.Clone();

        /// <summary>
        ///     Gets a handle to underlying database connection.
        /// </summary>
        /// <value>A handle to underlying database connection.</value>
        /// <seealso href="http://sqlite.org/c3ref/sqlite3.html">Database Connection Handle</seealso>
        public virtual sqlite3 Handle
            => _db;

        /// <summary>
        ///     Gets or sets a string used to open the connection.
        /// </summary>
        /// <value>A string used to open the connection.</value>
        /// <seealso cref="SqliteConnectionStringBuilder" />
        public override string ConnectionString
        {
            get => _connectionString;
            set
            {
                if (State != ConnectionState.Closed)
                {
                    throw new InvalidOperationException(Resources.ConnectionStringRequiresClosedConnection);
                }

                _connectionString = value;
                ConnectionStringBuilder = new SqliteConnectionStringBuilder(value);
            }
        }

        internal SqliteConnectionStringBuilder ConnectionStringBuilder { get; set; }

        /// <summary>
        ///     Gets the name of the current database. Always 'main'.
        /// </summary>
        /// <value>The name of the current database.</value>
        public override string Database
            => MainDatabaseName;

        /// <summary>
        ///     Gets the path to the database file. Will be absolute for open connections.
        /// </summary>
        /// <value>The path to the database file.</value>
        public override string DataSource
        {
            get
            {
                string dataSource = null;
                if (State == ConnectionState.Open)
                {
                    dataSource = raw.sqlite3_db_filename(_db, MainDatabaseName);
                }

                return dataSource ?? ConnectionStringBuilder.DataSource;
            }
        }

        /// <summary>
        ///     Gets or sets the default <see cref="SqliteCommand.CommandTimeout"/> value for commands created using
        ///     this connection. This is also used for internal commands in methods like
        ///     <see cref="BeginTransaction()"/>.
        /// </summary>
        /// <value>The default <see cref="SqliteCommand.CommandTimeout"/> value</value>
        public virtual int DefaultTimeout { get; set; } = 30;

        /// <summary>
        ///     Gets the version of SQLite used by the connection.
        /// </summary>
        /// <value>The version of SQLite used by the connection.</value>
        public override string ServerVersion
            => raw.sqlite3_libversion();

        /// <summary>
        ///     Gets the current state of the connection.
        /// </summary>
        /// <value>The current state of the connection.</value>
        public override ConnectionState State
            => _state;

        /// <summary>
        /// Gets or sets a value indicating whether foreign key constraints are enforced.
        /// </summary>
        /// <value>A value indicating whether foreign key constraints are enforced.</value>
        public bool? ForeignKeys
        {
            get => _configuration.ForeignKeys;
            set
            {
                if (_configuration.ForeignKeys != value)
                {
                    if (_state != ConnectionState.Closed)
                    {
                        throw new InvalidOperationException(Resources.SetterRequiresClosedConnection(nameof(ForeignKeys)));
                    }

                    _configuration.ForeignKeys = value;
                    SetForeignKeys();
                }
            }
        }

        private void SetForeignKeys()
        {
            if (_configuration.ForeignKeys.HasValue && _state == ConnectionState.Open)
            {
                this.ExecuteNonQuery($"PRAGMA foreign_keys = {(_configuration.ForeignKeys.Value ? "1" : "0")};");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether recursive triggers are enabled.
        /// </summary>
        /// <value>A value indicating whether recursive triggers are enabled.</value>
        public bool? RecursiveTriggers
        {
            get => _configuration.RecursiveTriggers;
            set
            {
                if (_configuration.RecursiveTriggers != value)
                {
                    if (_state != ConnectionState.Closed)
                    {
                        throw new InvalidOperationException(Resources.SetterRequiresClosedConnection(nameof(RecursiveTriggers)));
                    }

                    _configuration.RecursiveTriggers = value;
                    SetRecursiveTriggers();
                }
            }
        }

        private void SetRecursiveTriggers()
        {
            if (_configuration.RecursiveTriggers.HasValue && _state == ConnectionState.Open)
            {
                this.ExecuteNonQuery($"PRAGMA recursive_triggers = {(_configuration.RecursiveTriggers.Value ? "1" : "0")};");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether automatic indexing is enabled.
        /// </summary>
        /// <value>A value indicating whether automatic indexing is enabled.</value>
        public bool? AutomaticIndex
        {
            get => _configuration.AutomaticIndex;
            set
            {
                if (_configuration.AutomaticIndex != value)
                {
                    if (_state != ConnectionState.Closed)
                    {
                        throw new InvalidOperationException(Resources.SetterRequiresClosedConnection(nameof(RecursiveTriggers)));
                    }

                    _configuration.AutomaticIndex = value;
                    SetAutomaticIndex();
                }
            }
        }

        private void SetAutomaticIndex()
        {
            if (_configuration.AutomaticIndex.HasValue && _state == ConnectionState.Open)
            {
                this.ExecuteNonQuery($"PRAGMA automatic_index = {(_configuration.AutomaticIndex.Value ? "1" : "0")};");
            }
        }

        /// <summary>
        /// Gets or sets the journaling mode.
        /// </summary>
        /// <value>The journaling mode.</value>
        public SqliteJournalMode JournalMode
        {
            get => _configuration.JournalMode;
            set
            {
                if (_configuration.JournalMode != value)
                {
                    if (_state != ConnectionState.Closed)
                    {
                        throw new InvalidOperationException(Resources.SetterRequiresClosedConnection(nameof(JournalMode)));
                    }

                    _configuration.JournalMode = value;
                    SetJournalMode();
                }
            }
        }

        private void SetJournalMode()
        {
            if (_state != ConnectionState.Open)
            {
                return;
            }

            string mode = null;
            switch (_configuration.JournalMode)
            {
                case SqliteJournalMode.Undefined:
                    return;
                case SqliteJournalMode.Delete:
                    mode = "DELETE";
                    break;
                case SqliteJournalMode.Truncate:
                    mode = "TRUNCATE";
                    break;
                case SqliteJournalMode.Persist:
                    mode = "PERSIST";
                    break;
                case SqliteJournalMode.Memory:
                    mode = "MEMORY";
                    break;
                case SqliteJournalMode.WAL:
                    mode = "WAL";
                    break;
                case SqliteJournalMode.Off:
                    mode = "OFF";
                    break;
            }

            this.ExecuteNonQuery($"PRAGMA journal_mode = {mode};");
        }

        /// <summary>
        /// Gets or sets the synchronous setting.
        /// </summary>
        /// <value>The synchronous setting.</value>
        public SqliteSynchronousMode Synchronous
        {
            get => _configuration.Synchronous;
            set
            {
                if (_configuration.Synchronous != value)
                {
                    _configuration.Synchronous = value;
                    SetSynchronous();
                }
            }
        }

        private void SetSynchronous()
        {
            if (_state != ConnectionState.Open)
            {
                return;
            }

            string mode = null;
            switch (_configuration.Synchronous)
            {
                case SqliteSynchronousMode.Undefined:
                    return;
                case SqliteSynchronousMode.Extra:
                    mode = "EXTRA";
                    break;
                case SqliteSynchronousMode.Full:
                    mode = "FULL";
                    break;
                case SqliteSynchronousMode.Normal:
                    mode = "NORMAL ";
                    break;
                case SqliteSynchronousMode.Off:
                    mode = "OFF";
                    break;
            }

            this.ExecuteNonQuery($"PRAGMA synchronous = {mode};");
        }

        /// <summary>
        /// Gets or sets the auto vacuum mode.
        /// </summary>
        /// <value>The auto vacuum mode.</value>
        public SqliteAutoVacuumMode AutoVacuum
        {
            get => _configuration.AutoVacuum;
            set
            {
                if (_configuration.AutoVacuum != value)
                {
                    _configuration.AutoVacuum = value;
                    SetAutoVacuum();
                }
            }
        }

        private void SetAutoVacuum()
        {
            if (_state != ConnectionState.Open)
            {
                return;
            }

            string mode = null;
            switch (_configuration.AutoVacuum)
            {
                case SqliteAutoVacuumMode.Undefined:
                    return;
                case SqliteAutoVacuumMode.Incremental:
                    mode = "INCREMENTAL";
                    break;
                case SqliteAutoVacuumMode.Full:
                    mode = "FULL";
                    break;
                case SqliteAutoVacuumMode.None:
                    mode = "None";
                    break;
            }

            this.ExecuteNonQuery($"PRAGMA auto_vacuum = {mode};");
        }

        /// <summary>
        ///     Gets the <see cref="DbProviderFactory" /> for this connection.
        /// </summary>
        /// <value>The <see cref="DbProviderFactory" />.</value>
        protected override DbProviderFactory DbProviderFactory
            => SqliteFactory.Instance;

        /// <summary>
        ///     Gets or sets the transaction currently being used by the connection, or null if none.
        /// </summary>
        /// <value>The transaction currently being used by the connection.</value>
        protected internal virtual SqliteTransaction Transaction { get; set; }

        private void SetState(ConnectionState value)
        {
            var originalState = _state;
            if (originalState != value)
            {
                _state = value;
                OnStateChange(new StateChangeEventArgs(originalState, value));
            }
        }

        /// <summary>
        /// Gets the current configuration of the connection.
        /// </summary>
        /// <returns>The current configuration of the connection.</returns>
        public SqliteConfiguration CurrentConfiguration()
            => _configuration.Clone();

        /// <summary>
        ///     Opens a connection to the database using the value of <see cref="ConnectionString" />. If
        ///     <c>Mode=ReadWriteCreate</c> is used (the default) the file is created, if it doesn't already exist.
        /// </summary>
        /// <exception cref="SqliteException">A SQLite error occurs while opening the connection.</exception>
        public override void Open()
        {
            if (State == ConnectionState.Open)
            {
                return;
            }
            if (ConnectionString == null)
            {
                throw new InvalidOperationException(Resources.OpenRequiresSetConnectionString);
            }

            var filename = ConnectionStringBuilder.DataSource;
            var flags = 0;

            if (filename.StartsWith("file:", StringComparison.OrdinalIgnoreCase))
            {
                flags |= raw.SQLITE_OPEN_URI;
            }

            switch (ConnectionStringBuilder.Mode)
            {
                case SqliteOpenMode.ReadOnly:
                    flags |= raw.SQLITE_OPEN_READONLY;
                    break;

                case SqliteOpenMode.ReadWrite:
                    flags |= raw.SQLITE_OPEN_READWRITE;
                    break;

                case SqliteOpenMode.Memory:
                    flags |= raw.SQLITE_OPEN_READWRITE | raw.SQLITE_OPEN_CREATE | raw.SQLITE_OPEN_MEMORY;
                    if ((flags & raw.SQLITE_OPEN_URI) == 0)
                    {
                        flags |= raw.SQLITE_OPEN_URI;
                        filename = "file:" + filename;
                    }
                    break;

                default:
                    Debug.Assert(
                        ConnectionStringBuilder.Mode == SqliteOpenMode.ReadWriteCreate,
                        "ConnectionStringBuilder.Mode is not ReadWriteCreate");
                    flags |= raw.SQLITE_OPEN_READWRITE | raw.SQLITE_OPEN_CREATE;
                    break;
            }

            switch (ConnectionStringBuilder.Cache)
            {
                case SqliteCacheMode.Shared:
                    flags |= raw.SQLITE_OPEN_SHAREDCACHE;
                    break;

                case SqliteCacheMode.Private:
                    flags |= raw.SQLITE_OPEN_PRIVATECACHE;
                    break;

                default:
                    Debug.Assert(
                        ConnectionStringBuilder.Cache == SqliteCacheMode.Default,
                        "ConnectionStringBuilder.Cache is not Default.");
                    break;
            }

            var dataDirectory = AppDomain.CurrentDomain.GetData("DataDirectory") as string;
            if (!string.IsNullOrEmpty(dataDirectory)
                && (flags & raw.SQLITE_OPEN_URI) == 0
                && !filename.Equals(":memory:", StringComparison.OrdinalIgnoreCase)
                && !Path.IsPathRooted(filename))
            {
                filename = Path.Combine(dataDirectory, filename);
            }

            var rc = raw.sqlite3_open_v2(filename, out _db, flags, vfs: null);
            SqliteException.ThrowExceptionForRC(rc, _db);

            SetState(ConnectionState.Open);
        }

        /// <summary>
        ///     Closes the connection to the database. Open transactions are rolled back.
        /// </summary>
        public override void Close()
        {
            if (_db == null
                || _db.ptr == IntPtr.Zero)
            {
                return;
            }

            Transaction?.Dispose();

            foreach (var reference in _commands)
            {
                if (reference.TryGetTarget(out var command))
                {
                    command.Dispose();
                }
            }

            _commands.Clear();

            _db.Dispose2();
            _db = null;
            SetState(ConnectionState.Closed);
        }

        /// <summary>
        ///     Releases any resources used by the connection and closes it.
        /// </summary>
        /// <param name="disposing">
        ///     true to release managed and unmanaged resources; false to release only unmanaged resources.
        /// </param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Close();
            }

            base.Dispose(disposing);
        }

        /// <summary>
        ///     Creates a new command associated with the connection.
        /// </summary>
        /// <returns>The new command.</returns>
        /// <remarks>
        ///     The command's <seealso cref="SqliteCommand.Transaction" /> property will also be set to the current
        ///     transaction.
        /// </remarks>
        public new virtual SqliteCommand CreateCommand()
            => new SqliteCommand { Connection = this, CommandTimeout = DefaultTimeout, Transaction = Transaction };

        /// <summary>
        ///     Creates a new command associated with the connection.
        /// </summary>
        /// <returns>The new command.</returns>
        protected override DbCommand CreateDbCommand()
            => CreateCommand();

        internal void AddCommand(SqliteCommand command)
            => _commands.Add(new WeakReference<SqliteCommand>(command));

        internal void RemoveCommand(SqliteCommand command)
        {
            for (var i = _commands.Count - 1; i >= 0; i--)
            {
                if (!_commands[i].TryGetTarget(out var item)
                    || item == command)
                {
                    _commands.RemoveAt(i);
                }
            }
        }

        /// <summary>
        ///     Create custom collation.
        /// </summary>
        /// <param name="name">Name of the collation.</param>
        /// <param name="comparison">Method that compares two strings.</param>
        public virtual void CreateCollation(string name, Comparison<string> comparison)
            => CreateCollation(name, null, comparison != null ? (_, s1, s2) => comparison(s1, s2) : (Func<object, string, string, int>)null);

        /// <summary>
        ///     Create custom collation.
        /// </summary>
        /// <typeparam name="T">The type of the state object.</typeparam>
        /// <param name="name">Name of the collation.</param>
        /// <param name="state">State object passed to each invocation of the collation.</param>
        /// <param name="comparison">Method that compares two strings, using additional state.</param>
        public virtual void CreateCollation<T>(string name, T state, Func<T, string, string, int> comparison)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (State != ConnectionState.Open)
            {
                throw new InvalidOperationException(Resources.CallRequiresOpenConnection(nameof(CreateCollation)));
            }

            var (_, collation, obj) = _configuration.AddCollationCore(name, state, comparison);
            CreateCollationCore(name, collation, obj);
        }
        
        private void CreateCollationCore(string name, delegate_collation collation, object obj)
        {
            var rc = raw.sqlite3_create_collation(_db, name, obj, collation);
            SqliteException.ThrowExceptionForRC(rc, _db);
        }

        /// <summary>
        ///     Begins a transaction on the connection.
        /// </summary>
        /// <returns>The transaction.</returns>
        public new virtual SqliteTransaction BeginTransaction()
            => BeginTransaction(IsolationLevel.Unspecified);

        /// <summary>
        ///     Begins a transaction on the connection.
        /// </summary>
        /// <param name="isolationLevel">The isolation level of the transaction.</param>
        /// <returns>The transaction.</returns>
        protected override DbTransaction BeginDbTransaction(IsolationLevel isolationLevel)
            => BeginTransaction(isolationLevel);

        /// <summary>
        ///     Begins a transaction on the connection.
        /// </summary>
        /// <param name="isolationLevel">The isolation level of the transaction.</param>
        /// <returns>The transaction.</returns>
        public new virtual SqliteTransaction BeginTransaction(IsolationLevel isolationLevel)
        {
            if (State != ConnectionState.Open)
            {
                throw new InvalidOperationException(Resources.CallRequiresOpenConnection(nameof(BeginTransaction)));
            }
            if (Transaction != null)
            {
                throw new InvalidOperationException(Resources.ParallelTransactionsNotSupported);
            }

            return Transaction = new SqliteTransaction(this, isolationLevel);
        }

        /// <summary>
        ///     Changes the current database. Not supported.
        /// </summary>
        /// <param name="databaseName">The name of the database to use.</param>
        /// <exception cref="NotSupportedException">Always.</exception>
        public override void ChangeDatabase(string databaseName)
            => throw new NotSupportedException();

        /// <summary>
        ///     Enables extension loading on the connection.
        /// </summary>
        /// <param name="enable">true to enable; false to disable</param>
        /// <seealso href="http://sqlite.org/loadext.html">Run-Time Loadable Extensions</seealso>
        public virtual void EnableExtensions(bool enable = true)
        {
            if (_db == null
                || _db.ptr == IntPtr.Zero)
            {
                throw new InvalidOperationException(Resources.CallRequiresOpenConnection(nameof(EnableExtensions)));
            }

            var rc = raw.sqlite3_enable_load_extension(_db, enable ? 1 : 0);
            SqliteException.ThrowExceptionForRC(rc, _db);
        }

        /// <summary>
        ///     Backup of the connected database.
        /// </summary>
        /// <param name="destination">The destination of the backup.</param>
        public virtual void BackupDatabase(SqliteConnection destination)
            => BackupDatabase(destination, MainDatabaseName, MainDatabaseName);

        /// <summary>
        ///     Backup of the connected database.
        /// </summary>
        /// <param name="destination">The destination of the backup.</param>
        /// <param name="destinationName">The name of the destination database.</param>
        /// <param name="sourceName">The name of the source database.</param>
        public virtual void BackupDatabase(SqliteConnection destination, string destinationName, string sourceName)
        {
            if (_db == null
                || _db.ptr == IntPtr.Zero)
            {
                throw new InvalidOperationException(Resources.CallRequiresOpenConnection(nameof(BackupDatabase)));
            }
            if (destination == null)
            {
                throw new ArgumentNullException(nameof(destination));
            }

            var close = false;
            if (destination.State != ConnectionState.Open)
            {
                destination.Open();
                close = true;
            }
            try
            {
                using (var backup = raw.sqlite3_backup_init(destination._db, destinationName, _db, sourceName))
                {
                    int rc;
                    if (backup.ptr == IntPtr.Zero)
                    {
                        rc = raw.sqlite3_errcode(destination._db);
                        SqliteException.ThrowExceptionForRC(rc, destination._db);
                    }

                    rc = raw.sqlite3_backup_step(backup, -1);
                    SqliteException.ThrowExceptionForRC(rc, destination._db);
                }
            }
            finally
            {
                if (close)
                {
                    destination.Close();
                }
            }
        }

        private void CreateFunctionCore<TState, TResult>(
            string name,
            int arity,
            TState state,
            Func<TState, SqliteValueReader, TResult> function,
            bool isDeterministic)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (State != ConnectionState.Open)
            {
                throw new InvalidOperationException(Resources.CallRequiresOpenConnection(nameof(CreateFunction)));
            }

            var (_, _, func, flags, obj) = _configuration.AddFunctionCore(name, arity, state, function, isDeterministic);
            CreateFunctionCore(name, arity, func, flags, obj);
        }
        
        private void CreateFunctionCore(string name, int arity, delegate_function_scalar func, int flags, object obj)
        {
            var rc = raw.sqlite3_create_function(
                _db,
                name,
                arity,
                flags,
                obj,
                func);
            SqliteException.ThrowExceptionForRC(rc, _db);
        }

        private void CreateAggregateCore<TAccumulate, TResult>(
            string name,
            int arity,
            TAccumulate seed,
            Func<TAccumulate, SqliteValueReader, TAccumulate> func,
            Func<TAccumulate, TResult> resultSelector,
            bool isDeterministic)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (State != ConnectionState.Open)
            {
                throw new InvalidOperationException(Resources.CallRequiresOpenConnection(nameof(CreateAggregate)));
            }

            var (_, _, flags, obj, func_step, func_final) = _configuration.AddAggregateCore(
                name, arity, seed, func, resultSelector, isDeterministic);
            CreateAggregateCore(name, arity, flags, obj, func_step, func_final);
        }

        private void CreateAggregateCore(string name, int arity, int flags, object obj, delegate_function_aggregate_step func_step, delegate_function_aggregate_final func_final)
        {
            var rc = raw.sqlite3_create_function(
                _db,
                name,
                arity,
                flags,
                obj,
                func_step,
                func_final);
            SqliteException.ThrowExceptionForRC(rc, _db);
        }
    }
}

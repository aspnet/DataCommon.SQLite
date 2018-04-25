// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using SQLitePCL;

namespace Microsoft.Data.Sqlite
{
    /// <summary>
    ///     Represents the connection configuration of a SQLite database.
    /// </summary>
    public partial class SqliteConfiguration
    {
        private readonly List<(string name, delegate_collation collation, object obj)> _collations
            = new List<(string name, delegate_collation collation, object obj)>();

        private readonly List<(
            string name,
            int arity,
            delegate_function_scalar func,
            int flags,
            object obj)> _functions = new List<(string name, int arity, delegate_function_scalar func, int flags, object obj)>();

        private readonly List<(
            string name,
            int arity,
            int flags,
            object obj,
            delegate_function_aggregate_step func_step,
            delegate_function_aggregate_final func_final)> _aggregates = new List<(string name, int arity, int flags, object obj, delegate_function_aggregate_step func_step, delegate_function_aggregate_final func_final)>();

        /// <summary>
        /// Gets or sets a value indicating whether foreign key constraints are enforced.
        /// </summary>
        /// <value>A value indicating whether foreign key constraints are enforced.</value>
        public bool? ForeignKeys { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether recursive triggers are enabled.
        /// </summary>
        /// <value>A value indicating whether recursive triggers are enabled.</value>
        public bool? RecursiveTriggers { get; set; }

        /// <summary>
        /// Gets or sets the journaling mode.
        /// </summary>
        /// <value>The journaling mode.</value>
        public SqliteJournalMode JournalMode { get; set; } = SqliteJournalMode.Undefined;

        /// <summary>
        /// Gets or sets the synchronous setting.
        /// </summary>
        /// <value>The synchronous setting.</value>
        public SqliteSynchronousMode Synchronous { get; set; } = SqliteSynchronousMode.Undefined;

        /// <summary>
        /// Gets or sets the auto vacuum mode.
        /// </summary>
        /// <value>The auto vacuum mode.</value>
        public SqliteAutoVacuumMode AutoVacuum { get; set; } = SqliteAutoVacuumMode.Undefined;

        /// <summary>
        /// Gets or sets a value indicating whether automatic indexing is enabled.
        /// </summary>
        /// <value>A value indicating whether automatic indexing is enabled.</value>
        public bool? AutomaticIndex { get; set; }

        /// <summary>
        ///     Create custom collation.
        /// </summary>
        /// <param name="name">Name of the collation.</param>
        /// <param name="comparison">Method that compares two strings.</param>
        public virtual void AddCollation(string name, Comparison<string> comparison)
            => AddCollation(name, null, comparison != null ? (_, s1, s2) => comparison(s1, s2) : (Func<object, string, string, int>)null);

        /// <summary>
        ///     Create custom collation.
        /// </summary>
        /// <typeparam name="T">The type of the state object.</typeparam>
        /// <param name="name">Name of the collation.</param>
        /// <param name="state">State object passed to each invocation of the collation.</param>
        /// <param name="comparison">Method that compares two strings, using additional state.</param>
        public virtual void AddCollation<T>(string name, T state, Func<T, string, string, int> comparison)
            => AddCollationCore(name, state, comparison);

        internal IEnumerable<(string name, int arity, delegate_function_scalar func, int flags, object obj)> Functions => _functions;

        internal IEnumerable<(string name, int arity, int flags, object obj, delegate_function_aggregate_step func_step, delegate_function_aggregate_final func_final)> Aggregates => _aggregates;

        internal IEnumerable<(string name, delegate_collation collation, object obj)> Collations => _collations;

        /// <summary>
        /// Deep clone of this configuration.
        /// </summary>
        /// <returns>A deep clone of this configuration.</returns>
        internal SqliteConfiguration Clone()
        {
            var config = new SqliteConfiguration()
            {
                ForeignKeys = ForeignKeys,
                RecursiveTriggers = RecursiveTriggers,
                JournalMode = JournalMode,
                Synchronous = Synchronous,
                AutoVacuum = AutoVacuum,
                AutomaticIndex = AutomaticIndex,
            };

            config._collations.AddRange(_collations);
            config._functions.AddRange(_functions);
            config._aggregates.AddRange(_aggregates);
            return config;
        }

        internal static Func<TState, SqliteValueReader, TResult> IfNotNull<TState, TResult>(
            object x,
            Func<TState, SqliteValueReader, TResult> value)
            => x != null ? value : null;

        internal static object[] GetValues(SqliteValueReader reader)
        {
            var values = new object[reader.FieldCount];
            reader.GetValues(values);

            return values;
        }

        internal (string name, delegate_collation collation, object obj) AddCollationCore<T>(string name, T state, Func<T, string, string, int> comparison)
        {
            for (var i = _collations.Count - 1; i >= 0; i--)
            {
                var item = _collations[i];
                if (item.name == name)
                {
                    _collations.RemoveAt(i);
                }
            }

            var collation = comparison != null ? (v, s1, s2) => comparison((T)v, s1, s2) : (delegate_collation)null;
            var obj = (object)state;
            var newItem = (name, collation, obj);
            if (collation != null)
            {
                _collations.Add(newItem);
            }

            return newItem;
        }

        internal (string name, int arity, delegate_function_scalar func, int flags, object obj) AddFunctionCore<TState, TResult>(
            string name,
            int arity,
            TState state,
            Func<TState, SqliteValueReader, TResult> function,
            bool isDeterministic)
        {
            for (var i = _functions.Count - 1; i >= 0; i--)
            {
                var item = _functions[i];
                if ((item.name == name) && (item.arity == arity))
                {
                    _functions.RemoveAt(i);
                }
            }

            delegate_function_scalar func = null;
            if (function != null)
            {
                func = (ctx, user_data, args) =>
                {
                    // TODO: Avoid allocation when niladic
                    var values = new SqliteParameterReader(name, args);

                    try
                    {
                        // TODO: Avoid closure by passing function via user_data
                        var result = function((TState)user_data, values);

                        new SqliteResultBinder(ctx, result).Bind();
                    }
                    catch (Exception ex)
                    {
                        raw.sqlite3_result_error(ctx, ex.Message);

                        if (ex is SqliteException sqlEx)
                        {
                            // NB: This must be called after sqlite3_result_error()
                            raw.sqlite3_result_error_code(ctx, sqlEx.SqliteErrorCode);
                        }
                    }
                };
            }

            var flags = isDeterministic ? raw.SQLITE_DETERMINISTIC : 0;
            var obj = (object)state;

            var newItem = (name, arity, func, flags, obj);
            if (func != null)
            {
                _functions.Add(newItem);
            }

            return newItem;
        }

        internal (string name, int arity, int flags, object obj, delegate_function_aggregate_step func_step, delegate_function_aggregate_final func_final) AddAggregateCore<TAccumulate, TResult>(
            string name,
            int arity,
            TAccumulate seed,
            Func<TAccumulate, SqliteValueReader, TAccumulate> func,
            Func<TAccumulate, TResult> resultSelector,
            bool isDeterministic)
        {
            for (var i = _aggregates.Count - 1; i >= 0; i--)
            {
                var item = _aggregates[i];
                if ((item.name == name) && (item.arity == arity))
                {
                    _aggregates.RemoveAt(i);
                }
            }

            delegate_function_aggregate_step func_step = null;
            if (func != null)
            {
                func_step = (ctx, user_data, args) =>
                {
                    var context = (AggregateContext<TAccumulate>)user_data;
                    if (context.Exception != null)
                    {
                        return;
                    }

                    // TODO: Avoid allocation when niladic
                    var reader = new SqliteParameterReader(name, args);

                    try
                    {
                        // TODO: Avoid closure by passing func via user_data
                        // NB: No need to set ctx.state since we just mutate the instance
                        context.Accumulate = func(context.Accumulate, reader);
                    }
                    catch (Exception ex)
                    {
                        context.Exception = ex;
                    }
                };
            }

            delegate_function_aggregate_final func_final = null;
            if (resultSelector != null)
            {
                func_final = (ctx, user_data) =>
                {
                    var context = (AggregateContext<TAccumulate>)user_data;

                    if (context.Exception == null)
                    {
                        try
                        {
                            // TODO: Avoid closure by passing resultSelector via user_data
                            var result = resultSelector(context.Accumulate);

                            new SqliteResultBinder(ctx, result).Bind();
                        }
                        catch (Exception ex)
                        {
                            context.Exception = ex;
                        }
                    }

                    if (context.Exception != null)
                    {
                        raw.sqlite3_result_error(ctx, context.Exception.Message);

                        if (context.Exception is SqliteException sqlEx)
                        {
                            // NB: This must be called after sqlite3_result_error()
                            raw.sqlite3_result_error_code(ctx, sqlEx.SqliteErrorCode);
                        }
                    }
                };
            }

            var flags = isDeterministic ? raw.SQLITE_DETERMINISTIC : 0;
            object obj = new AggregateContext<TAccumulate>(seed);

            var newItem = (name, arity, flags, obj, func_step, func_final);
            if (func_step != null)
            {
                _aggregates.Add(newItem);
            }

            return newItem;
        }

        private class AggregateContext<T>
        {
            public AggregateContext(T seed)
                => Accumulate = seed;

            public T Accumulate { get; set; }
            public Exception Exception { get; set; }
        }
    }
}

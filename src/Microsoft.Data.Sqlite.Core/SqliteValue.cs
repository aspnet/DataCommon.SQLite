// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Microsoft.Data.Sqlite
{
    using System;
    using System.Diagnostics;
    using SQLitePCL;

    /// <summary>
    /// Represents a value used by user defined functions.
    /// </summary>
    public class SqliteValue
    {
        private readonly int _sqliteType;

        /// <summary>
        /// Initializes a new instance of the <see cref="SqliteValue"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public SqliteValue(long value) : this(value, raw.SQLITE_INTEGER) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SqliteValue"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public SqliteValue(double value) : this(value, raw.SQLITE_FLOAT) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SqliteValue"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public SqliteValue(byte[] value) : this(value, raw.SQLITE_BLOB) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="SqliteValue"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public SqliteValue(string value) : this(value, raw.SQLITE_TEXT) { }

        private SqliteValue(object value, int sqliteType)
        {
            _sqliteType = sqliteType;
            Value = value;
        }

        /// <summary>
        /// Gets the current value.
        /// </summary>
        /// <value>
        /// The current value.
        /// </value>
        public object Value { get; }

        internal static SqliteValue[] ConvertToSqliteValueArray(sqlite3_value[] values)
        {
            if (values == null)
            {
                return null;
            }

            SqliteValue[] array = new SqliteValue[values.Length];
            for (int i = 0; i < array.Length; i++)
            {
                var value = values[i];
                array[i] = GetValue(value);
            }

            return array;
        }

        internal static void SetResult(SqliteValue result, sqlite3_context ctx)
        {
            switch (result._sqliteType)
            {
                case raw.SQLITE_INTEGER:
                    raw.sqlite3_result_int64(ctx, (long)result.Value);
                    break;
                case raw.SQLITE_FLOAT:
                    raw.sqlite3_result_double(ctx, (double)result.Value);
                    break;
                case raw.SQLITE_TEXT:
                    raw.sqlite3_result_text(ctx, (string)result.Value);
                    break;
                case raw.SQLITE_BLOB:
                    raw.sqlite3_result_blob(ctx, (byte[])result.Value);
                    break;
            }
        }

        private static SqliteValue GetValue(sqlite3_value value)
        {
            var sqliteType = raw.sqlite3_value_type(value);
            switch (sqliteType)
            {
                case raw.SQLITE_INTEGER:
                    return new SqliteValue(raw.sqlite3_value_int64(value), raw.SQLITE_INTEGER);
                case raw.SQLITE_FLOAT:
                    return new SqliteValue(raw.sqlite3_value_double(value), raw.SQLITE_FLOAT);
                case raw.SQLITE_TEXT:
                    return new SqliteValue(raw.sqlite3_value_text(value), raw.SQLITE_TEXT);
                case raw.SQLITE_BLOB:
                    return new SqliteValue(raw.sqlite3_value_blob(value), raw.SQLITE_BLOB);
                default:
                    Debug.Assert(false, "Unexpected value type: " + sqliteType);
                    return new SqliteValue(raw.sqlite3_value_int64(value), raw.SQLITE_INTEGER);
            }
        }
    }
}
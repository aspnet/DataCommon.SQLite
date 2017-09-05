// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Data.Common;
using Microsoft.Data.Sqlite.Properties;
using Xunit;

namespace Microsoft.Data.Sqlite
{
    public class SqliteDataReaderTest
    {
        [Fact]
        public void Depth_returns_zero()
        {
            using (var connection = new SqliteConnection("Data Source=:memory:"))
            {
                connection.Open();

                using (var reader = connection.ExecuteReader("SELECT 1;"))
                {
                    Assert.Equal(0, reader.Depth);
                }
            }
        }

        [Fact]
        public void FieldCount_works()
        {
            using (var connection = new SqliteConnection("Data Source=:memory:"))
            {
                connection.Open();

                using (var reader = connection.ExecuteReader("SELECT 1;"))
                {
                    Assert.Equal(1, reader.FieldCount);
                }
            }
        }

        [Fact]
        public void FieldCount_throws_when_closed()
            => X_throws_when_closed(r => { var x = r.FieldCount; }, "FieldCount");

        [Fact]
        public void GetBoolean_works()
            => GetX_works(
                "SELECT 1;",
                r => r.GetBoolean(0),
                true);

        [Fact]
        public void GetByte_works()
            => GetX_works(
                "SELECT 1;",
                r => r.GetByte(0),
                (byte)1);

        [Fact]
        public void GetBytes_not_supported()
        {
            using (var connection = new SqliteConnection("Data Source=:memory:"))
            {
                connection.Open();

                using (var reader = connection.ExecuteReader("SELECT x'7E57';"))
                {
                    var hasData = reader.Read();
                    Assert.True(hasData);

                    var buffer = new byte[2];
                    Assert.Throws<NotSupportedException>(() => reader.GetBytes(0, 0, buffer, 0, buffer.Length));
                }
            }
        }

        [Fact]
        public void GetChar_works()
            => GetX_works(
                "SELECT 1;",
                r => r.GetChar(0),
                (char)1);

        [Fact]
        public void GetChars_not_supported()
        {
            using (var connection = new SqliteConnection("Data Source=:memory:"))
            {
                connection.Open();

                using (var reader = connection.ExecuteReader("SELECT 'test';"))
                {
                    var hasData = reader.Read();
                    Assert.True(hasData);

                    var buffer = new char[4];
                    Assert.Throws<NotSupportedException>(() => reader.GetChars(0, 0, buffer, 0, buffer.Length));
                }
            }
        }

        [Fact]
        public void GetDateTime_works_with_text()
            => GetX_works(
                "SELECT '2014-04-15 10:47:16';",
                r => r.GetDateTime(0),
                new DateTime(2014, 4, 15, 10, 47, 16));

        [Fact]
        public void GetDateTime_works_with_real()
            => GetX_works(
                "SELECT julianday('2013-10-07 08:23:19.120');",
                r => r.GetDateTime(0),
                new DateTime(2013, 10, 7, 8, 23, 19, 120));

        [Fact]
        public void GetDateTime_works_with_integer()
            => GetX_works(
                "SELECT CAST(julianday('2013-10-07 12:00') AS INTEGER);",
                r => r.GetDateTime(0),
                new DateTime(2013, 10, 7, 12, 0, 0));

        [Fact]
        public void GetDateTime_throws_when_null()
            => GetX_throws_when_null(r => r.GetDateTime(0));

        [Fact]
        public void GetDateTimeOffset_works_with_text()
            => GetX_works(
                "SELECT '2014-04-15 10:47:16';",
                r => ((SqliteDataReader)r).GetDateTimeOffset(0),
                new DateTimeOffset(new DateTime(2014, 4, 15, 10, 47, 16)));

        [Fact]
        public void GetDateTimeOffset_works_with_real()
            => GetX_works(
                "SELECT julianday('2013-10-07 08:23:19.120');",
                r => ((SqliteDataReader)r).GetDateTimeOffset(0),
                new DateTimeOffset(new DateTime(2013, 10, 7, 8, 23, 19, 120)));

        [Fact]
        public void GetDateTimeOffset_works_with_integer()
            => GetX_works(
                "SELECT CAST(julianday('2013-10-07 12:00') AS INTEGER);",
                r => ((SqliteDataReader)r).GetDateTimeOffset(0),
                new DateTimeOffset(new DateTime(2013, 10, 7, 12, 0, 0)));

        [Fact]
        public void GetDateTimeOffset_throws_when_null()
            => GetX_throws_when_null(r => ((SqliteDataReader)r).GetDateTimeOffset(0));

        [Theory]
        [InlineData("SELECT 1;", "INTEGER")]
        [InlineData("SELECT 3.14;", "REAL")]
        [InlineData("SELECT 'test';", "TEXT")]
        [InlineData("SELECT X'7E57';", "BLOB")]
        [InlineData("SELECT NULL;", "INTEGER")]
        public void GetDataTypeName_works(string sql, string expected)
        {
            using (var connection = new SqliteConnection("Data Source=:memory:"))
            {
                connection.Open();

                using (var reader = connection.ExecuteReader(sql))
                {
                    Assert.Equal(expected, reader.GetDataTypeName(0));
                }
            }
        }

        [Fact]
        public void GetDataTypeName_works_when_column()
        {
            using (var connection = new SqliteConnection("Data Source=:memory:"))
            {
                connection.Open();
                connection.ExecuteNonQuery("CREATE TABLE Person ( Name nvarchar(4000) );");

                using (var reader = connection.ExecuteReader("SELECT Name FROM Person;"))
                {
                    Assert.Equal("nvarchar", reader.GetDataTypeName(0));
                }
            }
        }

        [Fact]
        public void GetDataTypeName_throws_when_ordinal_out_of_range()
        {
            using (var connection = new SqliteConnection("Data Source=:memory:"))
            {
                connection.Open();

                using (var reader = connection.ExecuteReader("SELECT 1;"))
                {
                    var ex = Assert.Throws<ArgumentOutOfRangeException>(() => reader.GetDataTypeName(1));

                    Assert.Equal("ordinal", ex.ParamName);
                    Assert.Equal(1, ex.ActualValue);
                }
            }
        }

        [Fact]
        public void GetDataTypeName_throws_when_closed()
            => X_throws_when_closed(r => r.GetDataTypeName(0), "GetDataTypeName");

        [Theory]
        [InlineData("3.14", 3.14)]
        [InlineData("1.0e-2", 0.01)]
        public void GetDecimal_works(string input, decimal expected)
            => GetX_works(
                "SELECT '" + input + "';",
                r => r.GetDecimal(0),
                expected);

        [Fact]
        public void GetDecimal_throws_when_null()
            => GetX_throws_when_null(r => r.GetDecimal(0));

        [Fact]
        public void GetDouble_throws_when_null()
            => GetX_throws_when_null(
                r => r.GetDouble(0));

        [Fact]
        public void GetEnumerator_works()
        {
            using (var connection = new SqliteConnection("Data Source=:memory:"))
            {
                connection.Open();

                using (var reader = connection.ExecuteReader("SELECT 1;"))
                {
                    var hasData = reader.Read();
                    Assert.True(hasData);

                    Assert.NotNull(reader.GetEnumerator());
                }
            }
        }

        [Theory]
        [InlineData("SELECT 1;", true)]
        [InlineData("SELECT 1;", (byte)1)]
        [InlineData("SELECT 1;", (char)1)]
        [InlineData("SELECT 3.14;", 3.14)]
        [InlineData("SELECT 3;", 3f)]
        [InlineData("SELECT 1;", 1)]
        [InlineData("SELECT 1;", 1L)]
        [InlineData("SELECT 1;", (sbyte)1)]
        [InlineData("SELECT 1;", (short)1)]
        [InlineData("SELECT 'test';", "test")]
        [InlineData("SELECT 1;", 1u)]
        [InlineData("SELECT 1;", 1ul)]
        [InlineData("SELECT 1;", (ushort)1)]
        public void GetFieldValue_works<T>(string sql, T expected)
        {
            using (var connection = new SqliteConnection("Data Source=:memory:"))
            {
                connection.Open();

                using (var reader = connection.ExecuteReader(sql))
                {
                    var hasData = reader.Read();

                    Assert.True(hasData);
                    Assert.Equal(expected, reader.GetFieldValue<T>(0));
                }
            }
        }

        [Fact]
        public void GetFieldValue_of_byteArray_works()
            => GetFieldValue_works(
                "SELECT X'7E57';",
                new byte[] { 0x7e, 0x57 });

        [Fact]
        public void GetFieldValue_of_byteArray_empty()
            => GetFieldValue_works(
                "SELECT X'';",
                new byte[0]);

        [Fact]
        public void GetFieldValue_of_byteArray_throws_when_null()
            => GetX_throws_when_null(
                r => r.GetFieldValue<byte[]>(0));

        [Fact]
        public void GetFieldValue_of_DateTime_works()
            => GetFieldValue_works(
                "SELECT '2014-04-15 11:58:13';",
                new DateTime(2014, 4, 15, 11, 58, 13));

        [Fact]
        public void GetFieldValue_of_DateTimeOffset_works()
            => GetFieldValue_works(
                "SELECT '2014-04-15 11:58:13-08:00';",
                new DateTimeOffset(2014, 4, 15, 11, 58, 13, new TimeSpan(-8, 0, 0)));

        [Fact]
        public void GetFieldValue_of_DBNull_works()
            => GetFieldValue_works(
                "SELECT NULL;",
                DBNull.Value);

        [Fact]
        public void GetFieldValue_of_DBNull_throws_when_not_null()
        {
            using (var connection = new SqliteConnection("Data Source=:memory:"))
            {
                connection.Open();

                using (var reader = connection.ExecuteReader("SELECT 1;"))
                {
                    var hasData = reader.Read();

                    Assert.True(hasData);
                    Assert.Throws<InvalidCastException>(() => reader.GetFieldValue<DBNull>(0));
                }
            }
        }

        [Fact]
        public void GetFieldValue_of_decimal_works()
            => GetFieldValue_works(
                "SELECT '3.14';",
                3.14m);

        [Fact]
        public void GetFieldValue_of_Enum_works()
            => GetFieldValue_works(
                "SELECT 1;",
                MyEnum.One);

        [Fact]
        public void GetFieldValue_of_Guid_works()
            => GetFieldValue_works(
                "SELECT X'0E7E0DDC5D364849AB9B8CA8056BF93A';",
                new Guid("dc0d7e0e-365d-4948-ab9b-8ca8056bf93a"));

        [Fact]
        public void GetFieldValue_of_Nullable_works()
            => GetFieldValue_works(
                "SELECT 1;",
                (int?)1);

        [Fact]
        public void GetFieldValue_of_TimeSpan_works()
            => GetFieldValue_works(
                "SELECT '12:06:29';",
                new TimeSpan(12, 6, 29));

        [Fact]
        public void GetFieldValue_of_TimeSpan_throws_when_null()
            => GetX_throws_when_null(r => r.GetFieldValue<TimeSpan>(0));

        [Fact]
        public void GetFieldValue_throws_before_read()
            => X_throws_before_read(r => r.GetFieldValue<DBNull>(0));

        [Fact]
        public void GetFieldValue_throws_when_done()
            => X_throws_when_done(r => r.GetFieldValue<DBNull>(0));

        [Theory]
        [InlineData("SELECT 1;", typeof(long))]
        [InlineData("SELECT 3.14;", typeof(double))]
        [InlineData("SELECT 'test';", typeof(string))]
        [InlineData("SELECT X'7E57';", typeof(byte[]))]
        [InlineData("SELECT NULL;", typeof(int))]
        public void GetFieldType_works(string sql, Type expected)
        {
            using (var connection = new SqliteConnection("Data Source=:memory:"))
            {
                connection.Open();

                using (var reader = connection.ExecuteReader(sql))
                {
                    Assert.Equal(expected, reader.GetFieldType(0));
                }
            }
        }

        [Fact]
        public void GetFieldType_throws_when_ordinal_out_of_range()
        {
            using (var connection = new SqliteConnection("Data Source=:memory:"))
            {
                connection.Open();

                using (var reader = connection.ExecuteReader("SELECT 1;"))
                {
                    var ex = Assert.Throws<ArgumentOutOfRangeException>(() => reader.GetFieldType(1));

                    Assert.Equal("ordinal", ex.ParamName);
                    Assert.Equal(1, ex.ActualValue);
                }
            }
        }

        [Fact]
        public void GetFieldType_throws_when_closed()
            => X_throws_when_closed(r => r.GetFieldType(0), "GetFieldType");

        [Theory]
        [InlineData("3", 3f)]
        [InlineData("9e999", float.PositiveInfinity)]
        [InlineData("-9e999", float.NegativeInfinity)]
        public void GetFloat_works(string val, float result)
            => GetX_works(
                "SELECT " + val,
                r => r.GetFloat(0),
                result);

        [Theory]
        [InlineData("2.0", 2.0)]
        [InlineData("9e999", double.PositiveInfinity)]
        [InlineData("-9e999", double.NegativeInfinity)]
        [InlineData("'3.14'", 3.14)]
        [InlineData("'1.2e-03'", 0.0012)]
        public void GetDouble_works(string val, double result)
            => GetX_works(
                "SELECT " + val,
                r => r.GetDouble(0),
                result);

        [Fact]
        public void GetGuid_works_when_blob()
            => GetX_works(
                "SELECT X'0E7E0DDC5D364849AB9B8CA8056BF93A';",
                r => r.GetGuid(0),
                new Guid("dc0d7e0e-365d-4948-ab9b-8ca8056bf93a"));

        [Fact]
        public void GetGuid_works_when_text_blob()
            => GetX_works(
                "SELECT CAST('dc0d7e0e-365d-4948-ab9b-8ca8056bf93a' AS BLOB);",
                r => r.GetGuid(0),
                new Guid("dc0d7e0e-365d-4948-ab9b-8ca8056bf93a"));

        [Fact]
        public void GetGuid_works_when_text()
            => GetX_works(
                "SELECT 'dc0d7e0e-365d-4948-ab9b-8ca8056bf93a';",
                r => r.GetGuid(0),
                new Guid("dc0d7e0e-365d-4948-ab9b-8ca8056bf93a"));

        [Fact]
        public void GetGuid_throws_when_null()
            => GetX_throws_when_null(r => r.GetGuid(0));

        [Fact]
        public void GetInt16_works()
            => GetX_works(
                "SELECT 1;",
                r => r.GetInt16(0),
                (short)1);

        [Fact]
        public void GetInt32_works()
            => GetX_works(
                "SELECT 1;",
                r => r.GetInt32(0),
                1);

        [Fact]
        public void GetInt64_works()
            => GetX_works(
                "SELECT 1;",
                r => r.GetInt64(0),
                1L);

        [Fact]
        public void GetInt64_throws_when_null()
            => GetX_throws_when_null(
                r => r.GetInt64(0));

        [Fact]
        public void GetName_works()
        {
            using (var connection = new SqliteConnection("Data Source=:memory:"))
            {
                connection.Open();

                using (var reader = connection.ExecuteReader("SELECT 1 AS Id;"))
                {
                    Assert.Equal("Id", reader.GetName(0));
                }
            }
        }

        [Fact]
        public void GetName_throws_when_ordinal_out_of_range()
        {
            using (var connection = new SqliteConnection("Data Source=:memory:"))
            {
                connection.Open();

                using (var reader = connection.ExecuteReader("SELECT 1;"))
                {
                    var ex = Assert.Throws<ArgumentOutOfRangeException>(() => reader.GetName(1));

                    Assert.Equal("ordinal", ex.ParamName);
                    Assert.Equal(1, ex.ActualValue);
                }
            }
        }

        [Fact]
        public void GetName_throws_when_closed()
            => X_throws_when_closed(r => r.GetName(0), "GetName");

        [Fact]
        public void GetOrdinal_works()
        {
            using (var connection = new SqliteConnection("Data Source=:memory:"))
            {
                connection.Open();

                using (var reader = connection.ExecuteReader("SELECT 1 AS Id;"))
                {
                    Assert.Equal(0, reader.GetOrdinal("Id"));
                }
            }
        }

        [Fact]
        public void GetOrdinal_throws_when_out_of_range()
        {
            using (var connection = new SqliteConnection("Data Source=:memory:"))
            {
                connection.Open();

                using (var reader = connection.ExecuteReader("SELECT 1;"))
                {
                    var ex = Assert.Throws<ArgumentOutOfRangeException>(() => reader.GetOrdinal("Name"));
                    Assert.NotNull(ex.Message);
                    Assert.Equal("name", ex.ParamName);
                    Assert.Equal("Name", ex.ActualValue);
                }
            }
        }

        [Fact]
        public void GetString_works_utf8()
            => GetX_works(
                "SELECT '测试测试测试';",
                r => r.GetString(0),
                "测试测试测试");

        [Fact]
        public void GetFieldValue_works_utf8()
            => GetX_works(
                "SELECT '测试测试测试';",
                r => r.GetFieldValue<string>(0),
                "测试测试测试");

        [Fact]
        public void GetValue_to_string_works_utf8()
            => GetX_works(
                "SELECT '测试测试测试';",
                r => r.GetValue(0) as string,
                "测试测试测试");


        [Fact]
        public void GetString_works()
            => GetX_works(
                "SELECT 'test';",
                r => r.GetString(0),
                "test");

        [Fact]
        public void GetString_throws_when_null()
            => GetX_throws_when_null(
                r => r.GetString(0));

        [Theory]
        [InlineData("SELECT 1;", 1L)]
        [InlineData("SELECT 3.14;", 3.14)]
        [InlineData("SELECT 'test';", "test")]
        public void GetValue_works(string sql, object expected)
        {
            using (var connection = new SqliteConnection("Data Source=:memory:"))
            {
                connection.Open();

                using (var reader = connection.ExecuteReader(sql))
                {
                    var hasData = reader.Read();

                    Assert.True(hasData);
                    Assert.Equal(expected, reader.GetValue(0));
                }
            }
        }

        [Fact]
        public void GetValue_works_when_blob()
            => GetValue_works(
                "SELECT X'7E57';",
                new byte[] { 0x7e, 0x57 });

        [Fact]
        public void GetValue_works_when_null()
            => GetValue_works(
                "SELECT NULL;",
                DBNull.Value);

        [Fact]
        public void GetValue_throws_before_read()
            => X_throws_before_read(r => r.GetValue(0));

        [Fact]
        public void GetValue_throws_when_done()
            => X_throws_when_done(r => r.GetValue(0));

        [Fact]
        public void GetValue_throws_when_closed()
            => X_throws_when_closed(r => r.GetValue(0), "GetValue");

        [Fact]
        public void GetValues_works()
        {
            using (var connection = new SqliteConnection("Data Source=:memory:"))
            {
                connection.Open();

                using (var reader = connection.ExecuteReader("SELECT 1;"))
                {
                    var hasData = reader.Read();
                    Assert.True(hasData);

                    // Array may be wider than row
                    var values = new object[2];
                    var result = reader.GetValues(values);

                    Assert.Equal(1, result);
                    Assert.Equal(1L, values[0]);
                }
            }
        }

        [Fact]
        public void GetValues_throws_when_too_narrow()
        {
            using (var connection = new SqliteConnection("Data Source=:memory:"))
            {
                connection.Open();

                using (var reader = connection.ExecuteReader("SELECT 1;"))
                {
                    var hasData = reader.Read();
                    Assert.True(hasData);

                    var values = new object[0];
                    Assert.Throws<IndexOutOfRangeException>(() => reader.GetValues(values));
                }
            }
        }

        [Fact]
        public void HasRows_returns_true_when_rows()
        {
            using (var connection = new SqliteConnection("Data Source=:memory:"))
            {
                connection.Open();

                using (var reader = connection.ExecuteReader("SELECT 1;"))
                {
                    Assert.True(reader.HasRows);
                }
            }
        }

        [Fact]
        public void HasRows_returns_false_when_no_rows()
        {
            using (var connection = new SqliteConnection("Data Source=:memory:"))
            {
                connection.Open();

                using (var reader = connection.ExecuteReader("SELECT 1 WHERE 0 = 1;"))
                {
                    Assert.False(reader.HasRows);
                }
            }
        }

        [Fact]
        public void HasRows_works_when_batching()
        {
            using (var connection = new SqliteConnection("Data Source=:memory:"))
            {
                connection.Open();

                using (var reader = connection.ExecuteReader("SELECT 1 WHERE 0 = 1; SELECT 1;"))
                {
                    Assert.False(reader.HasRows);

                    reader.NextResult();

                    Assert.True(reader.HasRows);
                }
            }
        }

        [Fact]
        public void IsClosed_returns_false_when_active()
        {
            using (var connection = new SqliteConnection("Data Source=:memory:"))
            {
                connection.Open();

                using (var reader = connection.ExecuteReader("SELECT 1;"))
                {
                    Assert.False(reader.IsClosed);
                }
            }
        }

        [Fact]
        public void IsClosed_returns_true_when_closed()
        {
            using (var connection = new SqliteConnection("Data Source=:memory:"))
            {
                connection.Open();

                var reader = connection.ExecuteReader("SELECT 1;");
                reader.Close();

                Assert.True(reader.IsClosed);
            }
        }

        [Fact]
        public void IsDBNull_works()
        {
            using (var connection = new SqliteConnection("Data Source=:memory:"))
            {
                connection.Open();

                using (var reader = connection.ExecuteReader("SELECT NULL;"))
                {
                    var hasData = reader.Read();

                    Assert.True(hasData);
                    Assert.True(reader.IsDBNull(0));
                }
            }
        }

        [Fact]
        public void IsDBNull_throws_before_read()
            => X_throws_before_read(r => r.IsDBNull(0));

        [Fact]
        public void IsDBNull_throws_when_done()
            => X_throws_when_done(r => r.IsDBNull(0));

        [Fact]
        public void IsDBNull_throws_when_closed()
            => X_throws_when_closed(r => r.IsDBNull(0), "IsDBNull");

        [Fact]
        public void Item_by_ordinal_works()
        {
            using (var connection = new SqliteConnection("Data Source=:memory:"))
            {
                connection.Open();

                using (var reader = connection.ExecuteReader("SELECT 1;"))
                {
                    var hasData = reader.Read();
                    Assert.True(hasData);

                    Assert.Equal(1L, reader[0]);
                }
            }
        }

        [Fact]
        public void Item_by_name_works()
        {
            using (var connection = new SqliteConnection("Data Source=:memory:"))
            {
                connection.Open();

                using (var reader = connection.ExecuteReader("SELECT 1 AS Id;"))
                {
                    var hasData = reader.Read();
                    Assert.True(hasData);

                    Assert.Equal(1L, reader["Id"]);
                }
            }
        }

        [Fact]
        public void NextResult_works()
        {
            using (var connection = new SqliteConnection("Data Source=:memory:"))
            {
                connection.Open();

                using (var reader = connection.ExecuteReader("SELECT 1; SELECT 2;"))
                {
                    var hasData = reader.Read();
                    Assert.True(hasData);
                    Assert.Equal(1L, reader.GetInt64(0));

                    var hasResults = reader.NextResult();
                    Assert.True(hasResults);

                    hasData = reader.Read();
                    Assert.True(hasData);
                    Assert.Equal(2L, reader.GetInt64(0));

                    hasResults = reader.NextResult();
                    Assert.False(hasResults);
                }
            }
        }

        [Fact]
        public void NextResult_can_be_called_more_than_once()
        {
            using (var connection = new SqliteConnection("Data Source=:memory:"))
            {
                connection.Open();

                using (var reader = connection.ExecuteReader("SELECT 1;"))
                {
                    var hasResults = reader.NextResult();
                    Assert.False(hasResults);

                    hasResults = reader.NextResult();
                    Assert.False(hasResults);
                }
            }
        }

        [Fact]
        public void NextResult_skips_DML_statements()
        {
            using (var connection = new SqliteConnection("Data Source=:memory:"))
            {
                connection.Open();
                connection.ExecuteNonQuery("CREATE TABLE Test(Value);");

                var sql = @"
                    SELECT 1;
                    INSERT INTO Test VALUES(1);
                    SELECT 2;";
                using (var reader = connection.ExecuteReader(sql))
                {
                    var hasResults = reader.NextResult();
                    Assert.True(hasResults);

                    var hasData = reader.Read();
                    Assert.True(hasData);

                    Assert.Equal(2L, reader.GetInt64(0));
                }
            }
        }

        [Fact]
        public void Read_works()
        {
            using (var connection = new SqliteConnection("Data Source=:memory:"))
            {
                connection.Open();

                using (var reader = connection.ExecuteReader("SELECT 1 UNION SELECT 2;"))
                {
                    var hasData = reader.Read();
                    Assert.True(hasData);
                    Assert.Equal(1L, reader.GetInt64(0));

                    hasData = reader.Read();
                    Assert.True(hasData);
                    Assert.Equal(2L, reader.GetInt64(0));

                    hasData = reader.Read();
                    Assert.False(hasData);
                }
            }
        }

        [Fact]
        public void Read_throws_when_closed()
            => X_throws_when_closed(r => r.Read(), "Read");

        [Fact]
        public void RecordsAffected_works()
        {
            using (var connection = new SqliteConnection("Data Source=:memory:"))
            {
                connection.Open();
                connection.ExecuteNonQuery("CREATE TABLE Test(Value);");

                var reader = connection.ExecuteReader("INSERT INTO Test VALUES(1);");
                ((IDisposable)reader).Dispose();

                Assert.Equal(1, reader.RecordsAffected);
            }
        }

        [Fact]
        public void RecordsAffected_works_when_no_DDL()
        {
            using (var connection = new SqliteConnection("Data Source=:memory:"))
            {
                connection.Open();

                var reader = connection.ExecuteReader("SELECT 1;");
                ((IDisposable)reader).Dispose();

                Assert.Equal(-1, reader.RecordsAffected);
            }
        }

        [Fact]
        public void GetSchemaTable_works()
        {
            using (var connection = new SqliteConnection("Data Source=:memory:"))
            {
                connection.Open();
                connection.ExecuteNonQuery(
                    "CREATE TABLE Person (ID INTEGER PRIMARY KEY, FirstName TEXT, LastName TEXT NOT NULL, Code INT UNIQUE);");
                connection.ExecuteNonQuery("INSERT INTO Person VALUES(101, 'John', 'Dee', 123);");
                connection.ExecuteNonQuery("INSERT INTO Person VALUES(105, 'Jane', 'Doe', 456);");

                using (var reader = connection.ExecuteReader("SELECT LastName, ID, Code, ID+1 AS IncID FROM Person;"))
                {
                    var schema = reader.GetSchemaTable();
                    Assert.True(schema.Columns.Contains("ColumnName"));
                    Assert.True(schema.Columns.Contains("ColumnOrdinal"));
                    Assert.True(schema.Columns.Contains("ColumnSize"));
                    Assert.True(schema.Columns.Contains("NumericPrecision"));
                    Assert.True(schema.Columns.Contains("NumericScale"));
                    Assert.True(schema.Columns.Contains("IsUnique"));
                    Assert.True(schema.Columns.Contains("IsKey"));
                    Assert.True(schema.Columns.Contains("BaseServerName"));
                    Assert.True(schema.Columns.Contains("BaseCatalogName"));
                    Assert.True(schema.Columns.Contains("BaseColumnName"));
                    Assert.True(schema.Columns.Contains("BaseSchemaName"));
                    Assert.True(schema.Columns.Contains("BaseTableName"));
                    Assert.True(schema.Columns.Contains("DataType"));
                    Assert.True(schema.Columns.Contains("DataTypeName"));
                    Assert.True(schema.Columns.Contains("AllowDBNull"));
                    Assert.True(schema.Columns.Contains("IsAliased"));
                    Assert.True(schema.Columns.Contains("IsExpression"));
                    Assert.True(schema.Columns.Contains("IsAutoIncrement"));
                    Assert.True(schema.Columns.Contains("IsLong"));

                    Assert.Equal(4, schema.Rows.Count);

                    Assert.Equal("LastName", schema.Rows[0]["ColumnName"]);
                    Assert.Equal(0, schema.Rows[0]["ColumnOrdinal"]);
                    Assert.Equal(DBNull.Value, schema.Rows[0]["ColumnSize"]);
                    Assert.Equal(DBNull.Value, schema.Rows[0]["NumericPrecision"]);
                    Assert.Equal(DBNull.Value, schema.Rows[0]["NumericScale"]);
                    Assert.False((bool)schema.Rows[0]["IsUnique"]);
                    Assert.False((bool)schema.Rows[0]["IsKey"]);
                    Assert.Equal("", schema.Rows[0]["BaseServerName"]);
                    Assert.Equal("main", schema.Rows[0]["BaseCatalogName"]);
                    Assert.Equal("LastName", schema.Rows[0]["BaseColumnName"]);
                    Assert.Equal(DBNull.Value, schema.Rows[0]["BaseSchemaName"]);
                    Assert.Equal("Person", schema.Rows[0]["BaseTableName"]);
                    Assert.Equal(typeof(String), schema.Rows[0]["DataType"]);
                    Assert.Equal("TEXT", schema.Rows[0]["DataTypeName"]);
                    Assert.False((bool)schema.Rows[0]["AllowDBNull"]);
                    Assert.False((bool)schema.Rows[0]["IsAliased"]);
                    Assert.False((bool)schema.Rows[0]["IsExpression"]);
                    Assert.False((bool)schema.Rows[0]["IsAutoIncrement"]);
                    Assert.Equal(DBNull.Value, schema.Rows[0]["IsLong"]);

                    Assert.Equal("ID", schema.Rows[1]["ColumnName"]);
                    Assert.Equal(1, schema.Rows[1]["ColumnOrdinal"]);
                    Assert.Equal(DBNull.Value, schema.Rows[1]["ColumnSize"]);
                    Assert.Equal(DBNull.Value, schema.Rows[1]["NumericPrecision"]);
                    Assert.Equal(DBNull.Value, schema.Rows[1]["NumericScale"]);
                    Assert.False((bool)schema.Rows[1]["IsUnique"]);
                    Assert.True((bool)schema.Rows[1]["IsKey"]);
                    Assert.Equal("", schema.Rows[1]["BaseServerName"]);
                    Assert.Equal("main", schema.Rows[1]["BaseCatalogName"]);
                    Assert.Equal("ID", schema.Rows[1]["BaseColumnName"]);
                    Assert.Equal(DBNull.Value, schema.Rows[1]["BaseSchemaName"]);
                    Assert.Equal("Person", schema.Rows[1]["BaseTableName"]);
                    Assert.Equal(typeof(Int64), schema.Rows[1]["DataType"]);
                    Assert.Equal("INTEGER", schema.Rows[1]["DataTypeName"]);
                    Assert.True((bool)schema.Rows[1]["AllowDBNull"]);
                    Assert.False((bool)schema.Rows[1]["IsAliased"]);
                    Assert.False((bool)schema.Rows[1]["IsExpression"]);
                    Assert.False((bool)schema.Rows[1]["IsAutoIncrement"]);
                    Assert.Equal(DBNull.Value, schema.Rows[1]["IsLong"]);

                    Assert.Equal("Code", schema.Rows[2]["ColumnName"]);
                    Assert.Equal(2, schema.Rows[2]["ColumnOrdinal"]);
                    Assert.Equal(DBNull.Value, schema.Rows[2]["ColumnSize"]);
                    Assert.Equal(DBNull.Value, schema.Rows[2]["NumericPrecision"]);
                    Assert.Equal(DBNull.Value, schema.Rows[2]["NumericScale"]);
                    Assert.True((bool)schema.Rows[2]["IsUnique"]);
                    Assert.False((bool)schema.Rows[2]["IsKey"]);
                    Assert.Equal("", schema.Rows[2]["BaseServerName"]);
                    Assert.Equal("main", schema.Rows[2]["BaseCatalogName"]);
                    Assert.Equal("Code", schema.Rows[2]["BaseColumnName"]);
                    Assert.Equal(DBNull.Value, schema.Rows[2]["BaseSchemaName"]);
                    Assert.Equal("Person", schema.Rows[2]["BaseTableName"]);
                    Assert.Equal(typeof(Int64), schema.Rows[2]["DataType"]);
                    Assert.Equal("INT", schema.Rows[2]["DataTypeName"]);
                    Assert.True((bool)schema.Rows[2]["AllowDBNull"]);
                    Assert.False((bool)schema.Rows[2]["IsAliased"]);
                    Assert.False((bool)schema.Rows[2]["IsExpression"]);
                    Assert.False((bool)schema.Rows[2]["IsAutoIncrement"]);
                    Assert.Equal(DBNull.Value, schema.Rows[2]["IsLong"]);

                    Assert.Equal("IncID", schema.Rows[3]["ColumnName"]);
                    Assert.Equal(3, schema.Rows[3]["ColumnOrdinal"]);
                    Assert.Equal(DBNull.Value, schema.Rows[3]["ColumnSize"]);
                    Assert.Equal(DBNull.Value, schema.Rows[3]["NumericPrecision"]);
                    Assert.Equal(DBNull.Value, schema.Rows[3]["NumericScale"]);
                    Assert.Equal(DBNull.Value, schema.Rows[3]["IsUnique"]);
                    Assert.Equal(DBNull.Value, schema.Rows[3]["IsKey"]);
                    Assert.Equal("", schema.Rows[3]["BaseServerName"]);
                    Assert.Equal(DBNull.Value, schema.Rows[3]["BaseCatalogName"]);
                    Assert.Equal(DBNull.Value, schema.Rows[3]["BaseColumnName"]);
                    Assert.Equal(DBNull.Value, schema.Rows[3]["BaseSchemaName"]);
                    Assert.Equal(DBNull.Value, schema.Rows[3]["BaseTableName"]);
                    Assert.Equal(typeof(Int64), schema.Rows[3]["DataType"]);
                    Assert.Equal("INTEGER", schema.Rows[3]["DataTypeName"]);
                    Assert.Equal(DBNull.Value, schema.Rows[3]["AllowDBNull"]);
                    Assert.True((bool)schema.Rows[3]["IsAliased"]);
                    Assert.True((bool)schema.Rows[3]["IsExpression"]);
                    Assert.Equal(DBNull.Value, schema.Rows[3]["IsAutoIncrement"]);
                    Assert.Equal(DBNull.Value, schema.Rows[3]["IsLong"]);
                }
            }
        }

        private static void GetX_works<T>(string sql, Func<DbDataReader, T> action, T expected)
        {
            using (var connection = new SqliteConnection("Data Source=:memory:"))
            {
                connection.Open();

                using (var reader = connection.ExecuteReader(sql))
                {
                    var hasData = reader.Read();

                    Assert.True(hasData);
                    Assert.Equal(expected, action(reader));
                }
            }
        }

        private static void GetX_throws_when_null(Action<DbDataReader> action)
        {
            using (var connection = new SqliteConnection("Data Source=:memory:"))
            {
                connection.Open();

                using (var reader = connection.ExecuteReader("SELECT NULL;"))
                {
                    var hasData = reader.Read();

                    Assert.True(hasData);
                    Assert.Throws<InvalidCastException>(() => action(reader));
                }
            }
        }

        private static void X_throws_before_read(Action<DbDataReader> action)
        {
            using (var connection = new SqliteConnection("Data Source=:memory:"))
            {
                connection.Open();

                using (var reader = connection.ExecuteReader("SELECT NULL;"))
                {
                    var ex = Assert.Throws<InvalidOperationException>(() => action(reader));

                    Assert.Equal(Resources.NoData, ex.Message);
                }
            }
        }

        private static void X_throws_when_done(Action<DbDataReader> action)
        {
            using (var connection = new SqliteConnection("Data Source=:memory:"))
            {
                connection.Open();

                using (var reader = connection.ExecuteReader("SELECT NULL;"))
                {
                    var hasData = reader.Read();
                    Assert.True(hasData);

                    hasData = reader.Read();
                    Assert.False(hasData);

                    var ex = Assert.Throws<InvalidOperationException>(() => action(reader));
                    Assert.Equal(Resources.NoData, ex.Message);
                }
            }
        }

        private static void X_throws_when_closed(Action<DbDataReader> action, string operation)
        {
            using (var connection = new SqliteConnection("Data Source=:memory:"))
            {
                connection.Open();

                var reader = connection.ExecuteReader("SELECT 1;");
                ((IDisposable)reader).Dispose();

                var ex = Assert.Throws<InvalidOperationException>(() => action(reader));
                Assert.Equal(Resources.DataReaderClosed(operation), ex.Message);
            }
        }

        private enum MyEnum
        {
            One = 1
        }
    }
}

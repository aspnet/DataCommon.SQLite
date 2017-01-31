using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Data.Sqlite
{
    public class SqliteConvert
    {
        static IDictionary<string, DbType> typeNameToDbTypeMapping = new Dictionary<string, DbType>(StringComparer.OrdinalIgnoreCase)
        {
            {"BIGINT",            DbType.Int64 },
            {"BIGUINT",           DbType.UInt64 },
            {"BINARY",            DbType.Binary },
            {"BIT",               DbType.Boolean},
            {"BLOB",              DbType.Binary},
            {"BOOL",              DbType.Boolean},
            {"BOOLEAN",           DbType.Boolean},
            {"CHAR",              DbType.AnsiStringFixedLength},
            {"CLOB",              DbType.String},
            {"COUNTER",           DbType.Int64},
            {"CURRENCY",          DbType.Decimal},
            {"DATE",              DbType.DateTime},
            {"DATETIME",          DbType.DateTime},
            {"DECIMAL",           DbType.Decimal},
            {"DOUBLE",            DbType.Double},
            {"FLOAT",             DbType.Double},
            {"GENERAL",           DbType.Binary},
            {"GUID",              DbType.Guid},
            {"IDENTITY",          DbType.Int64},
            {"IMAGE",             DbType.Binary},
            {"INT",               DbType.Int32},
            {"INT8",              DbType.SByte},
            {"INT16",             DbType.Int16},
            {"INT32",             DbType.Int32},
            {"INT64",             DbType.Int64},
            {"INTEGER",           DbType.Int64},
            {"INTEGER8",          DbType.SByte},
            {"INTEGER16",         DbType.Int16},
            {"INTEGER32",         DbType.Int32},
            {"INTEGER64",         DbType.Int64},
            {"LOGICAL",           DbType.Boolean},
            {"LONG",              DbType.Int64},
            {"LONGCHAR",          DbType.String},
            {"LONGTEXT",          DbType.String},
            {"LONGVARCHAR",       DbType.String},
            {"MEMO",              DbType.String},
            {"MONEY",             DbType.Decimal},
            {"NCHAR",             DbType.StringFixedLength},
            {"NOTE",              DbType.String},
            {"NTEXT",             DbType.String},
            {"NUMBER",            DbType.Decimal},
            {"NUMERIC",           DbType.Decimal},
            {"NVARCHAR",          DbType.String},
            {"OLEOBJECT",         DbType.Binary},
            {"RAW",               DbType.Binary},
            {"REAL",              DbType.Double},
            {"SINGLE",            DbType.Single},
            {"SMALLDATE",         DbType.DateTime},
            {"SMALLINT",          DbType.Int16},
            {"SMALLUINT",         DbType.UInt16},
            {"STRING",            DbType.String},
            {"TEXT",              DbType.String},
            {"TIME",              DbType.DateTime},
            {"TIMESTAMP",         DbType.DateTime},
            {"TINYINT",           DbType.Byte},
            {"TINYSINT",          DbType.SByte},
            {"UINT",              DbType.UInt32},
            {"UINT8",             DbType.Byte},
            {"UINT16",            DbType.UInt16},
            {"UINT32",            DbType.UInt32},
            {"UINT64",            DbType.UInt64},
            {"ULONG",             DbType.UInt64},
            {"UNIQUEIDENTIFIER",  DbType.Guid},
            {"UNSIGNEDINTEGER",   DbType.UInt64},
            {"UNSIGNEDINTEGER8",  DbType.Byte},
            {"UNSIGNEDINTEGER16", DbType.UInt16},
            {"UNSIGNEDINTEGER32", DbType.UInt32},
            {"UNSIGNEDINTEGER64", DbType.UInt64},
            {"VARBINARY",         DbType.Binary},
            {"VARCHAR",           DbType.AnsiString},
            {"VARCHAR2",          DbType.AnsiString},
            {"YESNO",             DbType.Boolean},
        };

        static IDictionary<DbType, Type> dbTypeToTypeMapping = new Dictionary<DbType, Type>
        {
            {DbType.AnsiString, typeof(string) },
            {DbType.Binary,     typeof(byte[]) },
            {DbType.Byte,       typeof(byte) },
            {DbType.Boolean,    typeof(bool) },
            //{DbType.Currency
            //{DbType.Date
            {DbType.DateTime,   typeof(DateTime) },
            {DbType.Decimal,    typeof(decimal) },
            {DbType.Double,     typeof(double) },
            {DbType.Guid,       typeof(Guid) },
            {DbType.Int16,      typeof(short) },
            {DbType.Int32,      typeof(int) },
            {DbType.Int64,      typeof(long) },
            //{DbType.Object
            {DbType.SByte,      typeof(sbyte) },
            {DbType.Single,     typeof(Single) },
            {DbType.String,     typeof(string) },
            //{DbType.Time
            {DbType.UInt16,     typeof(ushort) },
            {DbType.UInt32,     typeof(uint) },
            {DbType.UInt64,     typeof(ulong) },
            //{DbType.VarNumeric
            {DbType.AnsiStringFixedLength, typeof(string) },
            {DbType.StringFixedLength,     typeof(string) },
            //{DbType.Xml
            {DbType.DateTime2,      typeof(DateTime) },
            {DbType.DateTimeOffset, typeof(DateTimeOffset) },
        };

        public static Type TypeNameToType(string typeName)
        {
            DbType dbType;
            if (!typeNameToDbTypeMapping.TryGetValue(typeName, out dbType))
            {
                var index = typeName.IndexOf('(');
                if (index > 0)
                {
                    var newTypeName = typeName.Substring(0, index);
                    if (!typeNameToDbTypeMapping.TryGetValue(newTypeName, out dbType))
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            Type returnType;
            if (!dbTypeToTypeMapping.TryGetValue(dbType, out returnType))
            {
                return null;
            }
            return returnType;
        }
    }
}

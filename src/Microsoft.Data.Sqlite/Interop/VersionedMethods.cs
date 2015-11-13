// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Microsoft.Data.Sqlite.Interop
{
    internal class VersionedMethods
    {
        public static string SqliteErrorMessage(int rc, Sqlite3Handle db)
        {
            var message = db == null || db.IsInvalid
                ? _strategy.ErrorString(rc)
                : NativeMethods.sqlite3_errmsg(db);

            return Strings.FormatSqliteNativeError(rc, message);
        }

        public static int SqliteClose(IntPtr handle)
           => _strategy.Close(handle);

        private static readonly StrategyBase _strategy = GetStrategy(new Version(NativeMethods.sqlite3_libversion()));

        private static StrategyBase GetStrategy(Version current)
        {
            if (current >= new Version("3.7.15"))
            {
                return new Strategy3_7_15();
            }
            if (current >= new Version("3.7.14"))
            {
                return new Strategy3_7_14();
            }
            return new StrategyBase();
        }

        private class Strategy3_7_15 : Strategy3_7_14
        {
            public override string ErrorString(int rc)
                => NativeMethods.sqlite3_errstr(rc) + " " + base.ErrorString(rc);
        }

        private class Strategy3_7_14 : StrategyBase
        {
            public override int Close(IntPtr handle)
                => NativeMethods.sqlite3_close_v2(handle);
        }

        private class StrategyBase
        {
            public virtual string ErrorString(int rc)
                => Strings.DefaultNativeError;
                
            public virtual int Close(IntPtr handle)
                => NativeMethods.sqlite3_close(handle);
        }
    }
}
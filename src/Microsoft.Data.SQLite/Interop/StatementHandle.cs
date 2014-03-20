﻿// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Runtime.InteropServices;

namespace Microsoft.Data.SQLite.Interop
{
    internal class StatementHandle : SafeHandle
    {
        private StatementHandle()
            : base(IntPtr.Zero, true)
        {
        }

        public override bool IsInvalid
        {
            get { return handle == IntPtr.Zero; }
        }

        protected override bool ReleaseHandle()
        {
            var rc = NativeMethods.sqlite3_finalize(handle);
            handle = IntPtr.Zero;

            return rc == Constants.SQLITE_OK;
        }
    }
}

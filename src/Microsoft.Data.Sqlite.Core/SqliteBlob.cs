// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using SQLitePCL;

namespace Microsoft.Data.Sqlite
{
    /// <summary>
    ///     Provides methods to access the contents of a BLOB.
    /// </summary>
    public class SqliteBlob : IDisposable
    {
        private readonly sqlite3_blob _blob;
        private readonly sqlite3 _db;
        private readonly int _blobLength;
        private bool _disposed = false;

        /// <summary>
        /// Gets the length of the BLOB in bytes.
        /// </summary>
        /// <value>Length of the BLOB in bytes.</value>
        public int Length => _blobLength;

        internal SqliteBlob(sqlite3_blob blob, sqlite3 db)
        {
            _blob = blob;
            _db = db;
            _blobLength = raw.sqlite3_blob_bytes(blob);
        }

        private SqliteBlob() => throw new NotSupportedException();

        /// <summary>
        /// Copies bytes from the BLOB into the buffer.
        /// </summary>
        /// <param name="buffer">Specified buffer.</param>
        /// <param name="count">Number of bytes to be read.</param>
        /// <param name="dataOffset">Offset in the BLOB.</param>
        public void ReadBytes(byte[] buffer, int count, int dataOffset)
        {
            var rc = raw.sqlite3_blob_read(_blob, buffer, count, dataOffset);
            SqliteException.ThrowExceptionForRC(rc, _db);
        }

        /// <summary>
        /// Copies bytes from the buffer into the BLOB.
        /// </summary>
        /// <param name="buffer">Specified buffer.</param>
        /// <param name="count">Number of bytes to be read.</param>
        /// <param name="dataOffset">Offset in the BLOB.</param>
        public void WriteBytes(byte[] buffer, int count, int dataOffset)
        {
            var rc = raw.sqlite3_blob_write(_blob, buffer, count, dataOffset);
            SqliteException.ThrowExceptionForRC(rc, _db);
        }

        /// <summary>
        ///     Releases any resources used by the BLOB and closes it.
        /// </summary>
        /// <param name="disposing">
        ///     true to release managed and unmanaged resources; false to release only unmanaged resources.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                raw.sqlite3_blob_close(_blob);
                _disposed = true;
            }
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="SqliteBlob"/> class.
        /// </summary>
        ~SqliteBlob()
        {
            Dispose(false);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
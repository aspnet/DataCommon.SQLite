Connection strings
------------------

> Tip: you can use [`Microsoft.Data.Sqlite.SqliteConnectionStringBuilder`](../src/Microsoft.Data.Sqlite/SqliteConnectionStringBuilder.cs) to explore available connection string options.

The following options are available on connection strings.

 - **`"Data Source"`, `"DataSource"`, `"Filename"`** (aliases)
 
   The filepath to the SQLite database or the SQLite URI.

   When a relative path is used, the base for the relative path is determined from:

    1. The ADO.NET data directory
        - .NET Framework: `System.AppDomain.CurrentDomain.GetData("DataDirectory")`
        - All other runtimes: `System.Environment.GetVariable("ADONET_DATA_DIR")`
    2. If data directory is not specified, then use the application directory instead:
        - .NET Framework: `System.AppDomain.CurrentDomain.BaseDirectory`
        - Universal Windows Apps: `Windows.Storage.ApplicationData.Current.LocalFolder.Path`
        - All other runtimes: `System.AppContext.BaseDirectory`

    When a SQLite URI format is specified, the connection does not attempt to adjust relative file paths.

    Examples:

    - `"Filename=./my_database.db"` Relative file path.
    - `"Data Source=C:\data\test.sqlite3"` Absolute file path. The extension is arbitrary.
    - `"Data Source=file:/home/fred/data.db?mode=ro&cache=private"` See <https://www.sqlite.org/uri.html> for documentation on file URI formats.
    - `"Data Source=:memory:"` An in-memory SQLite database that deletes when the connection closes.

 - [**`"Mode"`**](../src/Microsoft.Data.Sqlite/SqliteOpenMode.cs)

    Determines the connection mode. Available values:

    - `ReadWriteCreate` (default). Opens the database for reading and writing, and creates it if it doesn't exist.
    - `ReadWrite` Opens the database for reading and writing.
    - `ReadOnly` Opens the database in read-only mode.
    - `Memory` Opens an in-memory database.

    Examples:

    - `"Filename=./cache.db; Mode=ReadOnly"` A read-only connection to a file.
    - `"Data Source=InMemoryDbName; Mode=Memory" An named, in-memory database

- [**`"Cache"`**](../src/Microsoft.Data.Sqlite/SqliteCacheMode.cs)

    Determines the cache mode used by the connection. Available values:

    - `Default` (default). Uses the default cache mode. (Depends on how [SQLite was compiled]().)
    - `Private` Each conneciton uses a private cache.
    - `Shared` Connections share a cache. This mode can change the behavior of transaction and table locking.

    See <https://www.sqlite.org/sharedcache.html> more more information.

    Examples:

    - `"Data Source=people; Mode=Memory; Cache=Shared"` A named, in-memory database with shared cache. This allows sharing in-memory tables between multiple connections.
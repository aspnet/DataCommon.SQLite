Pragma Settings
---------------

SQLite supports an extensive list of "PRAGMA" options that configure how SQLite operates. (see <https://www.sqlite.org/pragma.html>). 
Microsoft.Data.Sqlite does not automatically issue PRAGMA statements. These can be set manually by issuing a SqliteCommand.

Example:

```c#
var command = connection.CreateCommand();
command.CommandText = "PRAGMA automatic_index = false";
command.ExecuteNonQuery();
```
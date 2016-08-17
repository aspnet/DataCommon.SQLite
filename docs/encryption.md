Encryption
----------

Microsoft.Data.Sqlite does not support encryption by default. 

To use encryption, substitute in you own native library, such as SQLite SEE (<https://www.sqlite.org/see>) or SQL Cipher (<https://github.com/sqlcipher/sqlcipher>).

Most encryption extensions support setting the encryption key by a [pragma setting](./pragma-settings.md). 

See <http://www.bricelam.net/2016/06/13/sqlite-encryption.html> for an example of how to do this.
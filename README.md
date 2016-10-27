# SQL-Blob-Inserter
Development utility for inserting blobs into SQL database.

You can download this application from the [Releases](https://github.com/cxminer/SQL-Blob-Inserter/releases).

## Usage
Enter connection information into SQL Server field, click Connect, write SQL query, define name of BLOB parameter - must be prefixed with `@`, choose a file to insert and click execute. Application will inform you how many records were affected.

## CLI
Application can be prepopulated with values by specifying them as arguments. For example:
```
BlobInserter.exe "example.com" "admin" "pass123" "sampledb"
```
This will open the application and insert "example.com" into host field, "admin" into username field, "pass123" into password field and "sampledb" into database field.

You can also prepopualte all other field by adding them to the end of the previous line, like so:
```
BlobInserter.exe "example.com" "admin" "pass123" "sampledb" "UPDATE users SET profile_pic = @BLOB WHERE id = 1;" "@BLOB" "profile_pic.png"
```
I believe the added arguments are self-explanatory.

## License
See `LICENSE`.

This example allows you to load any parseable content into a dataset and sync to database using OleDB, ODBC or SQL connection.

To test this, please do the following:

1. Create a database named "TestDatabase" in SQL Server instance ('.\SQLEXPRESS')
2. Make sure the TestDatabase is permissioned for your logged in user account to INSERT, UPDATE
3. Run the DatabaseWriterSample project
4. Load the data file (sample\CSVFileWithHeader.txt) with "CSVWithHeader" profile.
5. At this point, you should see 5 records in the data grid.
6. Click on "Sync to Database"
7. The DatabasePropertiesForm opens up with 4 inputs.  

	The Database Connection Type allows "Sql", "ODBC" and "OleDB".  

	Connection String ===> 
		Database Connection Type "SQL"
			Server=.\SQLEXPRESS;Database=TestDataBase;Trusted_Connection=True; (Replace .\SQLEXPRESSS with your server name and TestDatabase with your database name)
		Database Connection Type "OleDB"
			Provider=sqloledb;Data Source=.\SQLEXPRESS;Initial Catalog=TestDataBase;Integrated Security=SSPI; (Replace .\SQLEXPRESSS with your server name and TestDatabase with your database name)
		Database Connection Type "ODBC"
			Driver={SQL Server};Server=.\SQLEXPRESS;Database=TestDataBase;Trusted_Connection=Yes; (Replace .\SQLEXPRESSS with your server name and TestDatabase with your database name)

	Insert Command ===> 
		INSERT INTO [dbo].[CSVContents] ([Name],[DateOfBirth],[Occupation],[Heading]) VALUES ('{1}','{2}','{3}','{4}')
			or 
		INSERT INTO [dbo].[CSVContents] ([Name],[DateOfBirth],[Occupation],[Heading]) VALUES ('{Name}','{DateOfBirth}','{Occupation}','{Heading}')
			or
		INSERT INTO [dbo].[CSVContents] ([Name],[DateOfBirth],[Occupation],[Heading]) VALUES (@Name,@DateOfBirth,@Occupation,@Heading)  <==== This works only in database Connection type SQL...

	Update Command ===>
		UPDATE [dbo].[CSVContents] SET [Occupation] = '{3}', [Heading] = '{4}' WHERE [Name] = '{1}' and [DateOfBirth] = '{2}'
			or
		UPDATE [dbo].[CSVContents] SET [Occupation] = '{Occupation}', [Heading] = '{Heading}' WHERE [Name] = '{Name}' and [DateOfBirth] = '{DateOfBirth}'
			or
		UPDATE [dbo].[CSVContents] SET [Occupation] = @Occupation,[Heading] = @Heading WHERE [Name] = @Name and [DateOfBirth] = @DateOfBirth <==== This works only in database Connection type SQL...



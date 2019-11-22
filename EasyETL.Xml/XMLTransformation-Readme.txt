The following are the available Transformation commands
1. ADD
2. HIDE/REMOVE
3. RENAME
4. FILTER
5. SORT/SORTBY/ORDER/ORDERBY
6. GO/EXECUTE -- This command would execute the previous set of commands and the resulting XML would be used for subsequent commands
7. To rename a table, use the syntax [<TableOldName>=><TableNewName>].  In this, the <TableOldName> shall be replaced with the existing tablename and the <TableNewName> is the desired table name.

For a list of all supported functions in the transformations, see https://www.w3.org/TR/1999/REC-xpath-19991116/
In addition The following easy functions are available.  All easy functions need to be prefixed with easy:.  For example,
    ADD <NewColumnName> easy:Upper(<ColumnName>) would create a new column <NewColumnName> with the Upper Case value of <ColumnName>

1. Upper(inputStr) returns upper case of the input string
2. Lower(inputStr) returns lower case of the input string
3. Proper(inputStr) returns title case of the input string
4. Length(inputStr) returns the length of the input string
5. Left(inputStr, numChars) returns the first numChars from the input string
6. Right(inputStr, numChars) returns the last numChars from the input string
7. Contains(inputStr, searchStr) returns true if the searchStr is present in the inputStr.  This is not case sensitive.
8. IsEmpty(inputStr) returns true is the input string is empty
9. IsNumber(inputStr) returns true is the input string is a valid Number (including Decimals)
10. IsDate(inputStr) returns true is the input string is a valid date
11. Trim(inputStr) returns a string with spaces removed from beginning and end
12. Replace(inputStr, searchStr, replaceStr) returns a string that replaced the occurrence of search string with replace string in the input string
13. Deidentify(inputStr, inputType) returns deidentified data based on the inputType.  Valid inputType values are:
    a. Blank or empty would be replaced with "detect"
	b. detect -- the functions auto detects and deidentifies accordingly
	c. name
    d. phone
    e. email
    f. zip/zipcode
    g. address
    h. ssn
    i. creditcard
    j. date/datetime
    k. ipaddress
    l. uri/url


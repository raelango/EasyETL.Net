# EasyETL.Net
Set of .Net Libraries written in C# to create Listeners, Extractors, Writers and possibly more. These libraries allow you to (a) listen for events, (b) load data into dataset and (c) Write dataset to Excel, Html and more destinations.

This is a lightweight library that contains three different components:

* [Listeners](#listeners)
* [Extractors](#extractors) and
* [Writers](#writers)

The `EasyETLJob` class allows you to create a job that can listener on zero or more listeners, load data from one or more sources and write the output to zero or more destinations.  Please browser through sample codes for usage.

## Listeners
This library has 5 types of listeners and all are derived from a base class `JobListener`.
### Timer (TimerListener)
Triggers an event at specific times of the day or on a defined interval (in seconds).  In addition, you can specify the StartTime, EndTime, WeekDaysToRun, DaysToRun, MonthsToRun and TimesToRun.
### MSMQ (MSMQListener)
Monitors a MSMQ and triggers an event when a new message is ready to be processed.
### EventLog (EventLogListener)
Monitors an EventLog on any machine for events raised by a specific source (or all sources) and triggers when an eventlog is ready to be processed.
### Folder (FolderListener)
Monitors a folder for change events and triggers an event when such change event occurs.
### File (FileListener)
Monitors a specific file and triggers an event when the file is modified.  Very useful while monitoring log file for text appended to it.

## Extractors 
This library uses Regex for parsing data into dataset and has 6 different extractors.  All these can be invoked from the class `Extractor`.  The extractor pulls data from the source into a dataset for further operations.
### Regex (RegexDataSet)
AutoDetects and parses any delimited file.  Also, has ability to specify columns (using `AddColumn`) and allows to specify column level filters.
### Database (DatabaseDataSet)
Extracts records from any `ODBC`, `OleDB` or `SQL` data source by specifying the `DatabaseType` and `ConnectionString`.
### Excel (ExcelDataSet)
Extracts records from excel 2007 (`.xls`) or later (`.xlsx` etc) format files.  This loads all sheets as tables with the sheetname as the TableName in the `DataSet`. 
### Html (HtmlDataSet)
Extracts all `<TABLE>` elements from a HTML file to dataset.  If the table specifies ID or NAME value, it would be used for TableName in the `DataSet`.
### Json (JsonDataSet)
Extracts data from a `JSON` file to dataset.  Expects the JSON file to contain data which can be loaded as a `DataSet` with no complex sub elements.
### Xml (XmlDataSet)
Extracts data from a `XML` file to dataset.  Expects the XML file to contain data which can be loaded as a `DataSet` with no complex sub elements.

## Writers
This library allows user to write any dataset to 4 different targets.
### Delimited (DelimitedDatasetWriter)
Writes the dataset contents to a delimited file.
### Excel (ExcelDatasetWriter)
Writes the dataset to an excel file in .xml format.  __This does not require any additional libraries (interop) to create excel file__.  Each `DataTable` would be exported as a sheet.
### Html (HtmlDatasetWriter)
Writes the dataset to a HTML file.  Each `DataTable` would be exported as a `<TABLE>`.
### Json (JsonDatasetWriter)
Writes the dataset to a JSON file.

# EasyETL.Net
Set of .Net Libraries written in C# to create Listeners, Extractors, Writers and possibly more. These libraries allow you to (a) listen for events, (b) load data into dataset and (c) Write dataset to Excel, Html and more destinations.

This is a lightweight library that contains three different components:

* [Listeners](#listeners)
* [Extractors](#extractors) and
* [Writers](#writers)

The `EasyETLJob` class allows you to create a job that can listener on zero or more listeners, load data from one or more sources and write the output to zero or more destinations.  Please browser through sample codes for usage.

## Listeners

Each Listener runs in the background waiting for the specific event to occur and makes a call to `OnTriggered` event with `ListenerTriggeredEventArgs`.  The ListenerTriggeredEventArgs would contain additional data in the `Data` dictionary within the event.

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

This library uses Regex for parsing data into dataset.  Extractors raise the event `LineReadAndProcessed` for each line that is processed.  

This library has 8 different extractors.   All these extractors are derived from an abstract class `EasyDataSet`. All these extractors can be invoked from the class `Extractor`.  The extractor pulls data from the source into a dataset for further operations.
### Regex (RegexDataSet)
AutoDetects and parses any delimited or fixed length file.  Also, has ability to specify columns (using `AddColumn`) with separator or fixed length.  AddColumn also allows to specify column level filters.
### Database (DatabaseDataSet)
Extracts records from any `ODBC`, `OleDB` or `SQL` data source by specifying the `DatabaseType` and `ConnectionString`.
### Excel (ExcelDataSet)
Extracts records from excel 2007 (`.xls`) or later (`.xlsx` etc) format files.  This loads all sheets as tables with the sheetname as the TableName in the `DataSet`. 
### Html (HtmlDataSet)
Extracts all `<TABLE>` elements from a HTML file to dataset.  If the table specifies ID or NAME value, it would be used for TableName in the `DataSet`.
### Json (JsonDataSet)
Extracts data from a `JSON` file to dataset.  Expects the JSON file to contain data which can be loaded as a `DataSet` with no complex sub elements.
### MsMq (MsMqDataSet)
Pulls data from `MsMq` to dataset.  
### EventLog (EventDataSet)
Pulls data from `MsMq` to dataset.  
### Xml (XmlDataSet)
Extracts data from a `XML` file to dataset.  Expects the XML file to contain data which can be loaded as a `DataSet` with no complex sub elements.

## Writers

This library allows user to write any dataset to different targets.  Writers raise the event `RowWritten` for each line that is processed.  

The following are the support writers:
### Delimited (DelimitedDatasetWriter)
Writes the dataset contents to a delimited file.
### Database (DatabaseDatasetWriter)
Writes the dataset to a database.  Writes records to any `ODBC`, `OleDB` or `SQL` targets by specifying the `DatabaseType` and `ConnectionString`.  Use `InsertCommand` and `UpdateCommand` strings to Insert/Update record.  Insert would happen when Update did not find any match.  If you set the `UpdateCommand` to empty, all records would be inserted.  Please see the `DatabaseWriterSample` project and the `Readme.txt` file in the project for more details.  

For each row `Insert`, `Update` or `Error` an associated event `RowInserted`, `RowUpdated` or `RowErrored` is raised.
### Excel (ExcelDatasetWriter)
Writes the dataset to an excel file in .xml format.  __This does not require any additional libraries (interop) to create excel file__.  Each `DataTable` would be exported as a sheet.
### Html (HtmlDatasetWriter)
Writes the dataset to a HTML file.  Each `DataTable` would be exported as a `<TABLE>`.
### Json (JsonDatasetWriter)
Writes the dataset to a JSON file. Each `DataTable` would be exported.

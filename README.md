# CompaniesSearchEngine
A search engine that will utilise Azure Functions, Table Storage and Service Bus

A bit hacky, but shows how one could save some sites into table storage, then use a service bus/functions to crawl those sites in parallel.

This project is just playing about - it doesn't crawl the sites.

## Set up:

1. 
a. Create the service bus using the `devops/enviro-setup.ps1` script (this will also give you a connection string). I run it from VS Code, opening the project at the top level. Highlight a few lines, then F8 to run them. You could just run the lot.

OR

b. Manually create a service bus with a queue called 'webcrawl', generate a shared access policy with 'Listen' rights and copy the connection string

2. Put the connection string in the functions project `local.settings.json`


3. Have a local Azure storage emulator running. It will use local table storage for now.



## TO RUN

1. `AdminCompaniesImport` project imports data from a CSV into table storage for a starting point.
(we could have a table trigger, but I wanted to use a service bus)

2. Run the `SearchFunctions` project.

3. Using a browser, call `WebCrawlStarter` (url should be shown in the console). This will take information out of table storage and put it onto the service bus. `WebCrawlCompany` will then pick up the information for an imaginary processing.
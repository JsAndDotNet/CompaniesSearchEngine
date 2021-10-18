# CompaniesSearchEngine
A search engine that will utilise Azure Functions, Table Storage and Service Bus

A bit hacky, but shows how one could save some sites into table storage, then use a service bus/functions to crawl those sites in parallel.

This project is just playing about - it doesn't crawl the sites.

To set up:

1. Manually create a service bus with a queue called 'webcrawl', then put the connection string into the Functions config. 
2. Have a local Azure storage emulator running. It will use local table storage.



A template has been created, but haven't yet created the powershell to run it.
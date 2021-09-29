# CompaniesSearchEngine
A search engine that will utilise Azure Functions, Table Storage and Service Bus

A bit hacky, but shows how one could save some sites into table storage, then use a service bus/functions to crawl those sites in parallel.

This project is just playing about - it doesn't crawl the sites.



Currently need to create a service bus with a queue called 'webcrawl', then put the connection string into the Functions config.

A template has been created, but haven't yet created the powershell to run it.
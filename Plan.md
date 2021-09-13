
*** PLAN

Search Engine Sites to Scan

- Using a console, Import a list of websites from excel into Azure Table Storage.


Search Engine Site Scan

- Create a azure function on a timer (each month on the 2nd?). It will:
    - Read the list of websites to scan
    - Put details for each one on the service bus ('url_to_scan') 
      each with a short lived token (saved to table storage - stops people spamming the service with urls).
    
- Storage Bus Queue Listener DURABLE Function (Search)
    - Listen to 'url_to_scan' event and kick off. (may have an email address in data for notification)
    - Check short lived token - mark it as used.
    - Scan website (keep a short timeout to avoid az function costs spiralling!)
    - Save pages text to azure table Storage (cosmos too expensive!)
    - Send 'website_data_ready' message to service bus (may need to send on email address to this).
    - Save total execution time to table storage (for cost analytics)

- Storage Bus Queue Listener DURABLE Function (Aggregate)
    - Listen for 'website_data_ready'
    - Read data out of Table Storage and aggregate data
    - Save data to Azure Table Storage
    - Notify via email if required 
        (just smpt it or something, unless sendgrid have a super quick template - don't put smtp creds in code!).
    - Save total execution time to table storage (for cost analytics)


Search Engine UI

Just a console or v simple VANILLA javascript, so it'll still work when left for ages!
    - Query via Az Function
    - If has it, returns top level data as json (e.g. Vodafone 2, Boohoo 5 ...)
    - If no data, returns a short lived token
    - UI asks to enter email address for notification when done. 
      If email address added, creates a 'url_to_scan' event



DevOps
    - Powershell script to create:
        - Azure Table Storage
        - Service Bus
        - Functions Container.
        - Application Insights


Anticipated costs (approx)
        - Azure Table Storage - £7/month - high number of transactions ups the cost
        - Service Bus         - £0.076
        - *Functions          - Free
        - Application Insights - Free

        - Total Cost/Month:
            Expected            - £8
            Worst Case          - £58

        *Could be free, BUT searching a webpages takes time and GB/s cost could factor here. 
        Get first 400,000 GB/s of execution and 1,000,000 executions are free. Expect to be under that
        (about 800K executions), but need to work out GB/s averages! Get it wrong, it could be £50/month!





Investigations
- Ideally we'd use service bus transactions, but using Basic tier to keep costs down, so transactions aren't available.
- going with serverless to keep costs to minimum.
- could use azure blob storage with cognitive search, but it would cost too much.
- could use cosmosDB instead of table storage, but would cost roughly double on most basic plans.
- Azure Monitor would provide more detailed metrics for logging, but going to use application insights for 
  now as it's free.



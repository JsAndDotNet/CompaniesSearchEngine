# TODO: This should be idempotent, but it's a run once!
cls

# Write verbose statements out to console.
$VerbosePreference = "Continue"

. '.\devops\resourcegroup\rg-functions.ps1'
. '.\devops\servicebus\sb-functions.ps1'


# Connect to Azure
Connect-AzAccount

$rgName = 'jj-sbexample-delme'
$rgLocation = 'westeurope'

$sbNamespace = 'jjtestiotest'
$sbSku = 'Basic'
$sbQName = 'webcrawl'
$sbIsQueueSessionRequired = $false # NOTE: Later, other queues might be session enabled
                                   #       Sessions enable FIFO for messages marked with the same SessionID
$sbAuthznRuleName = "$sbQName-listen"
$sbAuthznRuleRight_s = 'listen' # others are 'send' and 'manage'. Can have more than one (comma separated?)

$rg = CreateOrGetResourceGroup $rgName  $rgLocation

# Create...
# Namespace
# Queue
# Authorization Rule Policy
# Connection String for said policy


$sbNs = CreateOrGetSBNamespace $rgName $rgLocation $sbNamespace $sbSku

$sbq = CreateOrGetSBQueue $rgName $sbNamespace $sbQName $sbIsQueueSessionRequired


$sbListenRule = CreateOrGetSBAuthznRule $rgName $sbNamespace $sbQName $sbAuthznRuleName $sbAuthznRuleRight_s

$sbListenKeyObj = CreateOrGetSBKeyConnectionObj $rgName $sbNamespace $sbQName $sbAuthznRuleName

$sbListenConnStr = $sbListenKeyObj.PrimaryConnectionString
$sbListenConnStrForFunction = $sbListenConnStr.Replace("EntityPath=webcrawl", "")

Write-Verbose "The key you need to put into your functions 'local.settings.json' file "
Write-Verbose "WARNING: You may need to delete 'EntityPath=webcrawl'"
Write-Verbose "Az-ServiceBus-Connection:"
$sbListenConnStrForFunction
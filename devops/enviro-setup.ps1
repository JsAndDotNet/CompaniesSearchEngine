# TODO: This should be idempotent, but it's a run once!
cls

'./devops/sb-functions.ps1'


# Connect to Azure
Connect-AzAccount

$rgName = 'jj-sbexample-delme'
$rgLocation = 'westeurope'

New-AzResourceGroup -Name $rgName -Location $rgLocation -Force

# New/Get-AzServiceBusNamespace
# New/Get-AzServiceBusQueue

# SEND
# New/Get-AzServiceBusAuthorizationRule
# New/Get-AzServiceBusKey

# LISTEN
# New/Get-AzServiceBusAuthorizationRule
# New/Get-AzServiceBusKey

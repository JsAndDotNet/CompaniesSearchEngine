# New/Get-AzServiceBusNamespace
function CreateOrGetSBNamespace([string] $rgName, [string] $location, 
[string] $sbNamespace, [string] $sbSku) {

    $ns = Get-AzServiceBusNamespace -ResourceGroupName $rgName -Name $sbNamespace -ErrorAction Ignore
    
    if(!$ns){
        Write-Verbose "Creating service bus $sbNamespace"
        $ns = New-AzServiceBusNamespace -ResourceGroupName $rgName -Location $location `
        -Name $sbNamespace -SkuName $sbSku
    } else{
        Write-Verbose "Service bus exists $sbNamespace"
    }
    
    return $ns
}



# New/Get-AzServiceBusQueue
function  CreateOrGetSBQueue([string]$rgName, [string]$sbNamespace, [string]$sbQName, [bool]$sbIsQueueSessionRequired)
{
    $sbq = Get-AzServiceBusQueue -ResourceGroupName $rgName -NamespaceName  $sbNamespace -QueueName $sbQName -ErrorAction Ignore

    if(!$sbq)
    {
        Write-Verbose "Creating Queue $sbQName, Session=$sbIsQueueSessionRequired"
        $sbq = New-AzServiceBusQueue -ResourceGroup $rgName ` -NamespaceName  $sbNamespace `
                        -QueueName $sbQName ` -RequiresSession $sbIsQueueSessionRequired
    } else {
        Write-Verbose "Queue Exists: $sbQName"
    }

    return $sbq
}



function CreateOrGetSBAuthznRule([string] $rgName, [string] $sbNamespace, [string] $sbQName, [string] $sbAuthznRuleName, [string] $sbAuthznRuleRight_s){
    # Type is 'send' or 'listen'
    $sbRule = Get-AzServiceBusAuthorizationRule -ResourceGroupName $rgName -NamespaceName  $sbNamespace -QueueName $sbQname -Name $sbAuthznRuleName -ErrorAction Ignore

    if(!$sbRule)
    {
        Write-Verbose "Creating Queue $sbQName rule $sbAuthznRuleName (type '$sbAuthznRuleRight_s')"

        $sbRule = New-AzServiceBusAuthorizationRule  -ResourceGroupName $rgName `
                        -NamespaceName  $sbNamespace -QueueName $sbQName `
                        -Name $sbAuthznRuleName -Rights $sbAuthznRuleRight_s 

    } else {
        Write-Verbose "Queue rule exists: $sbAuthznRuleName"
    }

    return $sbRule
}


# Note: The key is the connection string
function CreateOrGetSBKeyConnectionObj([string] $rgName, [string] $sbNamespace, [string] $sbQName, [string] $sbAuthznRuleName)
{
    $sbKey = Get-AzServiceBusKey -ResourceGroupName $rgName -NamespaceName `
                                        $sbNamespace -QueueName $sbQName -Name $sbAuthznRuleName

    if(!$sbKey)
    {
        Write-Verbose "Creating key for $sbAuthznRuleName"

        $sbKey = New-AzServiceBusKey  -ResourceGroupName $rgName -NamespaceName  $sbNamespace `
                                            -QueueName $sbQName -Name $sbAuthznRuleName 
    } else {
        Write-Verbose "Key exists for $sbAuthznRuleName"
    }
    
    return $sbKey
}


# SEND
# New/Get-AzServiceBusAuthorizationRule
# New/Get-AzServiceBusKey

# LISTEN
# New/Get-AzServiceBusAuthorizationRule
# New/Get-AzServiceBusKey
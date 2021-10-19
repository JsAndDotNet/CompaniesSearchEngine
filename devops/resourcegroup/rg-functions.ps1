function CreateOrGetResourceGroup([string]$rgName, [string]$rgLocation)
{
    $rg = Get-AzResourceGroup -Name $rgName -ErrorAction Ignore

    if(!$rg) {
        Write-Verbose "Create Resource Group $rgName"
        New-AzResourceGroup -Name $rgName -Location $rgLocation -Force
    }
    else {
        Write-Verbose "Resource Group exists: $rgName"
    }

    return $rg
}
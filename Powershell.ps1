Clear-Host

$pwaURL = "qa-internal-dmg-schedule/projectserver"
###update the uri with the correct PWA URL for your farm

$svcPSProxy = New-WebServiceProxy -uri ($pwaURL + "/_vti_bin/PSI/Project.asmx?wsdl") -useDefaultCredential
$svcWSSProxy = New-WebServiceProxy -uri ($pwaURL + "/_vti_bin/PSI/WssInterop.asmx?wsdl") -useDefaultCredential
$svcQSProxy = New-WebServiceProxy -uri ($pwaURL + "/_vti_bin/PSI/QueueSystem.asmx?wsdl") -useDefaultCredential
$svcLUTProxy = New-WebServiceProxy -uri ($pwaURL + "/_vti_bin/PSI/LookupTable.asmx?wsdl") -useDefaultCredential
$svcCFProxy = New-WebServiceProxy -uri ($pwaURL + "/_vti_bin/PSI/CustomFields.asmx?wsdl") -useDefaultCredential

$webTemplateLcid = '1033'
$xmlFilter = [system.string]::Empty
$sessionGuid = [system.guid]::NewGuid()

Write-Host "Retrieving custom fields and lookup tables..."
$customFieldList = $svcCFProxy.ReadCustomFields($xmlFilter, $false)
$lookupTableList = $svcLUTProxy.ReadLookupTables($xmlFilter, $false, $webTemplateLcid)

Write-Host "Looking for Ent_Status custom field..."
$entTagText = "Ent_Tag"
$entTagGuid = [system.guid]::empty
$entTagPreorderCode = "Digital Distribution; Digital Partners; Preorder Date"
$entTagPreorderCodeGuid = [system.guid]::empty
$entTagSaleStartCode = "Digital Distribution; Digital Partners; Sale Start Date"
$entTagSaleStartCodeGuid = [system.guid]::empty

foreach ($customField in $customFieldList.CustomFields)
{
    if ($customField.MD_PROP_NAME -eq $entTagText)
    {
        $entTagGuid = $customField.MD_PROP_UID
        Write-Host "Found!"
        break
    }
}

$customFieldRow = $customFieldList.CustomFields.FindByMD_PROP_UID($entTagGuid)
$lookupTableRow = $lookupTableList.LookupTables.FindByLT_UID($customFieldRow.MD_LOOKUP_TABLE_UID)

foreach ($lookupTableTree in $lookupTableList.LookupTableTrees)
{
    if (($lookupTableTree.LT_UID -eq $lookupTableRow.LT_UID) -and ($lookupTableTree.LT_VALUE_TEXT -eq $entTagPreorderCode))
    {
        $entTagPreorderCodeGuid = $lookupTableTree.LT_STRUCT_UID
        #Write-Host $lookupTableTree.LT_VALUE_TEXT
        Write-Host "Preorder Code Guid Found"
        break
    }
}

foreach ($lookupTableTree in $lookupTableList.LookupTableTrees)
{
    if (($lookupTableTree.LT_UID -eq $lookupTableRow.LT_UID) -and ($lookupTableTree.LT_VALUE_TEXT -eq $entTagSaleStartCode))
    {
        $entTagSaleStartCodeGuid = $lookupTableTree.LT_STRUCT_UID
        #Write-Host $lookupTableTree.LT_VALUE_TEXT
        Write-Host "Sale Start Code Guid Found"
        break
    }
}

$dsProject = 'CA9989C4-45ED-E511-80F2-069C18A09A76',
'133C8976-2CED-E511-80F2-069C18A09A76',
'86CCCDB3-F0EC-E511-80F2-069C18A09A76',
'32B9F127-05ED-E511-80F2-069C18A09A76',
'A48D5201-E1EC-E511-80F2-069C18A09A76',
'D192DC9F-AFEC-E511-80F2-069C18A09A76',
'598EE792-FBEC-E511-80F2-069C18A09A76',
'F61751E6-57ED-E511-80F2-069C18A09A76',
'6588AAC7-CFEC-E511-80F2-069C18A09A76',
'E9527B1C-D0EC-E511-80F2-069C18A09A76',
'74EA3F6B-EEEC-E511-80F2-069C18A09A76',
'378B2984-3BED-E511-80F2-069C18A09A76',
'BFA05E96-3BED-E511-80F2-069C18A09A76',
'AECAF5E7-1EED-E511-80F2-069C18A09A76',
'2B36FB01-1FED-E511-80F2-069C18A09A76',
'02BA3D1D-1FED-E511-80F2-069C18A09A76',
'9164A55A-45ED-E511-80F2-069C18A09A76',
'4F24D6CD-1EED-E511-80F2-069C18A09A76',
'86932DD7-ADEC-E511-80F2-069C18A09A76',
'F05C95FB-ADEC-E511-80F2-069C18A09A76',
'10C30A3E-48ED-E511-80F2-069C18A09A76',
'E409A15E-48ED-E511-80F2-069C18A09A76',
'5D74B45B-5AED-E511-80F2-069C18A09A76',
'2C00D32C-20ED-E511-80F2-069C18A09A76',
'D8833448-20ED-E511-80F2-069C18A09A76',
'B620423A-39ED-E511-80F2-069C18A09A76',
'00CC2438-D5EC-E511-80F2-069C18A09A76',
'8D8D978D-D5EC-E511-80F2-069C18A09A76',
'4846BCAC-D5EC-E511-80F2-069C18A09A76',
'21927B68-1FED-E511-80F2-069C18A09A76',
'15FC74CF-59ED-E511-80F2-069C18A09A76',
'E17817F5-15ED-E511-80F2-069C18A09A76',
'EF780A48-ACEC-E511-80F2-069C18A09A76',
'F9294F08-2AED-E511-80F2-069C18A09A76',
'22061AD0-F2EC-E511-80F2-069C18A09A76',
'8A822214-F3EC-E511-80F2-069C18A09A76',
'9FA1FD37-F3EC-E511-80F2-069C18A09A76',
'2E5F6CD8-46ED-E511-80F2-069C18A09A76'

Write-Host "   updating projects: "


foreach ($projectGuid in $dsProject)
{
    $dsSingleProject = $svcPSProxy.ReadProject([system.guid]::Parse($projectGuid), 1)
    Write-Host $dsSingleProject.Project[0].PROJ_NAME

    foreach ($taskCustomField in $dsSingleProject.TaskCustomFields)
    {
        if ($taskCustomField.CODE_VALUE -eq $entTagPreorderCodeGuid)
        {            
            $projectGuid = $taskCustomField.PROJ_UID
            $taskCustomField.CODE_VALUE = $entTagSaleStartCodeGuid

            foreach ($task in $dsSingleProject.Task)
            {
                if ($task.TASK_UID -eq $taskCustomField.TASK_UID)
                {
                    Write-Host "--" $task.TASK_NAME
                    $task.TASK_NAME = $task.TASK_NAME.Replace("Preorder","Sale Start")
                    break;
                }
            }

            $svcPSProxy.CheckOutProject($projectGuid, $sessionGuid, "check out project for sale start date")
            $svcPSProxy.QueueUpdateProject([system.guid]::NewGuid(), $sessionGuid, $dsSingleProject, $false)
            $publish = $svcPSProxy.QueuePublish([system.guid]::NewGuid(), $projectGuid, $true, $xmlFilter)
            $svcPSProxy.QueueCheckInProject([system.guid]::NewGuid(),$projectGuid, $false, $sessionGuid, "check in")
        }
    }
}
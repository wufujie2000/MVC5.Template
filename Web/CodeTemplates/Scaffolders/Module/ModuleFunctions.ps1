Function Delete-ProjectItem([String]$Project, [String]$Path)
{
    $ProjectItem = Get-ProjectItem -Project $Project -Path $Path
    If ($ProjectItem)
    {
        $ProjectItem.Delete()
        Write-Host "Deleted $Project\$Path"
    }
    Else
    {
        Write-Host "$Project\$Path is missing! Skipping..." -BackgroundColor Yellow;
    }
}

Function Delete-IfEmpty([String]$Project, [String]$Dir)
{
    $ProjectItem = Get-ProjectItem -Project $Project -Path $Dir
    If ($ProjectItem -and !(Get-ChildItem $ProjectItem.FileNames(0) -force))
    {
        $ProjectItem.Delete()
        Write-Host "Deleted $Project\$Dir"
    }
}

[T4Scaffolding.Scaffolder(Description = "Creates default module controller, service, model, view model and views")][CmdletBinding()]
param(
    [parameter(Mandatory = $true)][string]$ModelName,
    [parameter(Mandatory = $true)][string]$ControllerName,
    [parameter(Mandatory = $false)][string]$AreaName,
    [string[]]$TemplateFolders,
    [switch]$Delete = $false
)

$TestsProject = "Tests"
$RazorViewProject = "Web"
$ObjectsProject = "Objects"
$ServiceProject = "Services"
$ControllerProject = "Controllers"

$ControllerNamespace = "Template.Controllers"
If ($AreaName) { $ControllerNamespace = "Template.Controllers.$AreaName" }
$ControllerTestsNamespace = "Template.Tests.Unit.Controllers"
If ($AreaName) { $ControllerTestsNamespace = "Template.Tests.Unit.Controllers.$AreaName" }

$Model = $ModelName
$View = $ModelName + "View"
$Service = $ControllerName + "Service"
$Controller = $ControllerName + "Controller"
$IService = "I" + $ControllerName + "Service"
$ServiceTests = $ControllerName + "Service" + "Tests"
$ControllerTests = $ControllerName + "Controller" + "Tests"

If ($AreaName) { $ElementPath = "$AreaName\$ControllerName" }
Else { $ElementPath = $ControllerName }

If ($AreaName)
{
    $ViewAreaDir = "Views\$AreaName"
    $ControllerAreaDir = "$AreaName"
    $ModelAreaDir = "Models\$AreaName"
    $RazorViewAreaDir = "Views\$AreaName"
    $ServicesAreaDir = "$AreaName"
    $ControllerTestsAreaDir = "Unit\Controllers\$AreaName"
    $ServiceTestsAreaDir = "Unit\Services\$AreaName"
    
    $ViewControllerDir = "Views\$AreaName\$ControllerName"
    $ControllerControllerDir = "$AreaName\$ControllerName"
    $ModelControllerDir = "Models\$AreaName\$ControllerName"
    $RazorViewControllerDir = "Views\$AreaName\$ControllerName"
    $ServicesControllerDir = "$AreaName\$ControllerName"
    $ControllerTestsControllerDir = "Unit\Controllers\$AreaName\$ControllerName"
    $ServiceTestsControllerDir = "Unit\Services\$AreaName\$ControllerName"
}
Else
{
    $ViewAreaDir = "Views\$ControllerName"
    $ControllerAreaDir = "$ControllerName"
    $ModelAreaDir = "Models\$ControllerName"
    $RazorViewAreaDir = "Views\$ControllerName"
    $ServicesAreaDir = "$ControllerName"
    $ControllerTestsAreaDir = "Unit\Controllers\$ControllerName"
    $ServiceTestsAreaDir = "Unit\Services\$ControllerName"

    $ViewControllerDir = "Views\$ControllerName"
    $ControllerControllerDir = "$ControllerName"
    $ModelControllerDir = "Models\$ControllerName"
    $RazorViewControllerDir = "Views\$ControllerName"
    $ServicesControllerDir = "$ControllerName"
    $ControllerTestsControllerDir = "Unit\Controllers\$ControllerName"
    $ServiceTestsControllerDir = "Unit\Services\$ControllerName"
}

$ViewPath = "Views\$ElementPath\$View"
$EditViewPath = "Views\$ElementPath\Edit"
$IndexViewPath = "Views\$ElementPath\Index"
$CreateViewPath = "Views\$ElementPath\Create"
$DeleteViewPath = "Views\$ElementPath\Delete"
$DetailsViewPath = "Views\$ElementPath\Details"
$ControllerPath = "$ElementPath\$Controller"
$ModelPath = "Models\$ElementPath\$ModelName"
$ServicePath = "$ElementPath\$Service"
$IServicePath = "$ElementPath\$IService"
$ControllerTestsPath = "Unit\Controllers\$ElementPath\$ControllerTests"
$ServiceTestsPath = "Unit\Services\$ElementPath\$ServiceTests"

$ScriptDir = Split-Path -Path $MyInvocation.MyCommand.Definition -Parent
. $ScriptDir\ModuleFunctions.ps1

If ($Delete)
{
	Delete-ProjectItem $TestsProject "$ControllerTestsPath.cs"
	Delete-ProjectItem $TestsProject "$ServiceTestsPath.cs"
	Delete-ProjectItem $RazorViewProject "$DeleteViewPath.cshtml"
	Delete-ProjectItem $RazorViewProject "$EditViewPath.cshtml"
	Delete-ProjectItem $RazorViewProject "$DetailsViewPath.cshtml"
	Delete-ProjectItem $RazorViewProject "$CreateViewPath.cshtml"
	Delete-ProjectItem $RazorViewProject "$IndexViewPath.cshtml"
	Delete-ProjectItem $ControllerProject "$ControllerPath.cs"
	Delete-ProjectItem $ServiceProject "$ServicePath.cs"
	Delete-ProjectItem $ServiceProject "$IServicePath.cs"
	Delete-ProjectItem $ObjectsProject "$ViewPath.cs"
	Delete-ProjectItem $ObjectsProject "$ModelPath.cs"

    Delete-IfEmpty $TestsProject $ControllerTestsControllerDir
    Delete-IfEmpty $TestsProject $ControllerTestsAreaDir

    Delete-IfEmpty $TestsProject $ServiceTestsControllerDir
    Delete-IfEmpty $TestsProject $ServiceTestsAreaDir

    Delete-IfEmpty $RazorViewProject $RazorViewControllerDir
    Delete-IfEmpty $RazorViewProject $RazorViewAreaDir

    Delete-IfEmpty $ControllerProject $ControllerControllerDir
    Delete-IfEmpty $ControllerProject $ControllerAreaDir

    Delete-IfEmpty $ServiceProject $ServicesControllerDir
    Delete-IfEmpty $ServiceProject $ServicesAreaDir

    Delete-IfEmpty $ObjectsProject $ViewControllerDir
    Delete-IfEmpty $ObjectsProject $ViewAreaDir

    Delete-IfEmpty $ObjectsProject $ModelControllerDir
    Delete-IfEmpty $ObjectsProject $ModelAreaDir
}
Else
{
    Add-ProjectItemViaTemplate $ModelPath `
        -Template "Controls\Model" `
        -Model @{ `
		    Name = $ModelName `
	    } `
        -SuccessMessage "Added $ObjectsProject\{0}" `
        -TemplateFolders $TemplateFolders `
	    -CodeLanguage $CodeLanguage `
	    -Project $ObjectsProject

    Add-ProjectItemViaTemplate $ViewPath `
        -Template "Controls\View" `
        -Model @{ `
	      Name = $View `
	    } `
        -SuccessMessage "Added $ObjectsProject\{0}" `
        -TemplateFolders $TemplateFolders `
	    -CodeLanguage $CodeLanguage `
	    -Project $ObjectsProject

    Add-ProjectItemViaTemplate $IServicePath `
        -Template "Controls\IService" `
        -Model @{ `
		    Name = $IService; `
		    View = $View `
	    } `
        -SuccessMessage "Added $ServiceProject\{0}" `
        -TemplateFolders $TemplateFolders `
	    -CodeLanguage $CodeLanguage `
	    -Project $ServiceProject

    Add-ProjectItemViaTemplate $ServicePath `
        -Template "Controls\Service" `
        -Model @{ `
		    Interface = $IService; `
		    Model = $ModelName; `
		    Name = $Service; `
		    View = $View `
	    } `
        -SuccessMessage "Added $ServiceProject\{0}" `
        -TemplateFolders $TemplateFolders `
	    -CodeLanguage $CodeLanguage `
	    -Project $ServiceProject

    Add-ProjectItemViaTemplate $ControllerPath `
        -Template "Controls\Controller" `
        -Model @{ `
            ModelName = $ModelName.SubString(0, 1).ToLower() + $ModelName.SubString(1); `
	        Namespace = $ControllerNamespace; `
		    Service = $IService; `
		    Name = $Controller; `
		    View = $View `
	    } `
        -SuccessMessage "Added $ControllerProject\{0}" `
        -TemplateFolders $TemplateFolders `
	    -CodeLanguage $CodeLanguage `
	    -Project $ControllerProject


    Add-ProjectItemViaTemplate $IndexViewPath `
        -Template "Views\Index" `
        -Model @{ `
	        View = $View `
	    } `
        -SuccessMessage "Added $RazorViewProject\{0}" `
        -TemplateFolders $TemplateFolders `
	    -CodeLanguage $CodeLanguage `
	    -Project $RazorViewProject

    Add-ProjectItemViaTemplate $CreateViewPath `
        -Template "Views\Create" `
        -Model @{ `
	        View = $View `
	    } `
        -SuccessMessage "Added $RazorViewProject\{0}" `
        -TemplateFolders $TemplateFolders `
	    -CodeLanguage $CodeLanguage `
	    -Project $RazorViewProject

    Add-ProjectItemViaTemplate $DetailsViewPath `
        -Template "Views\Details" `
        -Model @{ `
	        View = $View `
	    } `
        -SuccessMessage "Added $RazorViewProject\{0}" `
        -TemplateFolders $TemplateFolders `
	    -CodeLanguage $CodeLanguage `
	    -Project $RazorViewProject

    Add-ProjectItemViaTemplate $EditViewPath `
        -Template "Views\Edit" `
        -Model @{ `
	        View = $View `
	    } `
        -SuccessMessage "Added $RazorViewProject\{0}" `
        -TemplateFolders $TemplateFolders `
	    -CodeLanguage $CodeLanguage `
	    -Project $RazorViewProject

    Add-ProjectItemViaTemplate $DeleteViewPath `
        -Template "Views\Delete" `
        -Model @{ `
	        View = $View `
	    } `
        -SuccessMessage "Added $RazorViewProject\{0}" `
        -TemplateFolders $TemplateFolders `
	    -CodeLanguage $CodeLanguage `
	    -Project $RazorViewProject


    Add-ProjectItemViaTemplate $ServiceTestsPath `
        -Template "Tests\ServiceTests" `
        -Model @{ `
		    Name = $ServiceTests; `
		    Service = $Service `
	    } `
        -SuccessMessage "Added $TestsProject\{0}" `
        -TemplateFolders $TemplateFolders `
	    -CodeLanguage $CodeLanguage `
	    -Project $TestsProject

    Add-ProjectItemViaTemplate $ControllerTestsPath `
        -Template "Tests\ControllerTests" `
        -Model @{ `
            ViewName = $ModelName.SubString(0, 1).ToLower() + $ModelName.SubString(1); `
		    ControllerNamespace = $ControllerNamespace; `
	        Namespace = $ControllerTestsNamespace; `
		    ServiceInterface = $IService; `
		    Controller = $Controller; `
		    Name = $ControllerTests; `
		    View = $View `
	    } `
        -SuccessMessage "Added $TestsProject\{0}" `
        -TemplateFolders $TemplateFolders `
	    -CodeLanguage $CodeLanguage `
	    -Project $TestsProject
}

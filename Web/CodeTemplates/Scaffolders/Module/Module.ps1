[T4Scaffolding.Scaffolder(Description = "Creates default module controller, service, model, view model and views")][CmdletBinding()]
param(
    [parameter(Mandatory = $true)][string]$ModelName,
    [parameter(Mandatory = $true)][string]$ControllerName,
    [parameter(Mandatory = $false)][string]$AreaName,
    [string[]]$TemplateFolders,
    [switch]$Force = $false
)

# Variables

$TestsProject = "Tests"
$RazorViewProject = "Web"
$ObjectsProject = "Objects"
$ServiceProject = "Components"
$ControllerProject = "Controllers"

$ViewNamespace = "Template.Objects"
$ModelNamespace = "Template.Objects"
$ServiceNamespace = "Template.Components.Services"
$ControllerNamespace = "Template.Controllers.$AreaName"
$ServiceTestsNamespace = "Template.Tests.Unit.Components.Services"
$ControllerTestsNamespace = "Template.Tests.Unit.Controllers.$AreaName"

$Model = $ModelName
$View = $ModelName + "View"
$Service = $ControllerName + "Service"
$Controller = $ControllerName + "Controller"
$ServiceInterface = "I" + $ControllerName + "Service"
$ServiceTests = $ControllerName + "Service" + "Tests"
$ControllerTests = $ControllerName + "Controller" + "Tests"

If ($AreaName) { $ElementPath = "$AreaName\$ControllerName"; }
Else { $ElementPath = $ControllerName; }

$ViewPath = "Views\$ElementPath\$View"
$RazorViewPath = "Views\$ElementPath\"
$ControllerPath = "$ElementPath\$Controller"
$ModelPath = "Models\$ElementPath\$ModelName"
$ServicePath = "Services\$ElementPath\$Service"
$ServiceInterfacePath = "Services\$ElementPath\$ServiceInterface"
$ControllerTestsPath = "Unit\Controllers\$ElementPath\$ControllerTests"
$ServiceTestsPath = "Unit\Components\Services\$ElementPath\$ServiceTests"

# Controls

Add-ProjectItemViaTemplate $ModelPath `
    -Template "Controls\Model" `
    -Model @{ `
	    Namespace = $ModelNamespace; `
		Name = $ModelName `
	} `
    -SuccessMessage "Added model at $ObjectsProject\{0}" `
    -TemplateFolders $TemplateFolders `
	-CodeLanguage $CodeLanguage `
	-Project $ObjectsProject `
	-Force:$Force

Add-ProjectItemViaTemplate $ViewPath `
    -Template "Controls\View" `
    -Model @{ `
	  Namespace = $ViewNamespace; `
	  Name = $View `
	} `
    -SuccessMessage "Added view model at $ObjectsProject\{0}" `
    -TemplateFolders $TemplateFolders `
	-CodeLanguage $CodeLanguage `
	-Project $ObjectsProject `
	-Force:$Force

Add-ProjectItemViaTemplate $ServiceInterfacePath `
    -Template "Controls\IService" `
    -Model @{ `
	    Namespace = $ServiceNamespace; `
		Name = $ServiceInterface; `
		View = $View `
	} `
    -SuccessMessage "Added service interface at $ServiceProject\{0}" `
    -TemplateFolders $TemplateFolders `
	-CodeLanguage $CodeLanguage `
	-Project $ServiceProject `
	-Force:$Force

Add-ProjectItemViaTemplate $ServicePath `
    -Template "Controls\Service" `
    -Model @{ `
	    Namespace = $ServiceNamespace; `
		Interface = $ServiceInterface; `
		Model = $ModelName; `
		Name = $Service; `
		View = $View `
	} `
    -SuccessMessage "Added service implementation at $ServiceProject\{0}" `
    -TemplateFolders $TemplateFolders `
	-CodeLanguage $CodeLanguage `
	-Project $ServiceProject `
	-Force:$Force

Add-ProjectItemViaTemplate $ControllerPath `
    -Template "Controls\Controller" `
    -Model @{ `
	    Namespace = $ControllerNamespace; `
		Service = $ServiceInterface; `
		Name = $Controller; `
		View = $View `
	} `
    -SuccessMessage "Added controller at $ControllerProject\{0}" `
    -TemplateFolders $TemplateFolders `
	-CodeLanguage $CodeLanguage `
	-Project $ControllerProject `
	-Force:$Force

# Views

Add-ProjectItemViaTemplate ($RazorViewPath + "Index") `
    -Template "Views\Index" `
    -Model @{ `
	    View = $View `
	} `
    -SuccessMessage "Added index view at $RazorViewProject\{0}" `
    -TemplateFolders $TemplateFolders `
	-CodeLanguage $CodeLanguage `
	-Project $RazorViewProject `
	-Force:$Force

Add-ProjectItemViaTemplate ($RazorViewPath + "Create") `
    -Template "Views\Create" `
    -Model @{ `
	    View = $View `
	} `
    -SuccessMessage "Added create view at $RazorViewProject\{0}" `
    -TemplateFolders $TemplateFolders `
	-CodeLanguage $CodeLanguage `
	-Project $RazorViewProject `
	-Force:$Force

Add-ProjectItemViaTemplate ($RazorViewPath + "Details") `
    -Template "Views\Details" `
    -Model @{ `
	    View = $View `
	} `
    -SuccessMessage "Added details view at $RazorViewProject\{0}" `
    -TemplateFolders $TemplateFolders `
	-CodeLanguage $CodeLanguage `
	-Project $RazorViewProject `
	-Force:$Force

Add-ProjectItemViaTemplate ($RazorViewPath + "Edit") `
    -Template "Views\Edit" `
    -Model @{ `
	    View = $View `
	} `
    -SuccessMessage "Added edit view at $RazorViewProject\{0}" `
    -TemplateFolders $TemplateFolders `
	-CodeLanguage $CodeLanguage `
	-Project $RazorViewProject `
	-Force:$Force

Add-ProjectItemViaTemplate ($RazorViewPath + "Delete") `
    -Template "Views\Delete" `
    -Model @{ `
	    View = $View `
	} `
    -SuccessMessage "Added delete view at $RazorViewProject\{0}" `
    -TemplateFolders $TemplateFolders `
	-CodeLanguage $CodeLanguage `
	-Project $RazorViewProject `
	-Force:$Force

# Tests

Add-ProjectItemViaTemplate $ServiceTestsPath `
    -Template "Tests\ServiceTests" `
    -Model @{ `
	    Namespace = $ServiceTestsNamespace; `
		Name = $ServiceTests; `
		Service = $Service; `
		View = $View `
	} `
    -SuccessMessage "Added tests for service at $TestsProject\{0}" `
    -TemplateFolders $TemplateFolders `
	-CodeLanguage $CodeLanguage `
	-Project $TestsProject `
	-Force:$Force

Add-ProjectItemViaTemplate $ControllerTestsPath `
    -Template "Tests\ControllerTests" `
    -Model @{ `
		ControllerNamespace = $ControllerNamespace; `
	    Namespace = $ControllerTestsNamespace; `
		ServiceInterface = $ServiceInterface; `
		Controller = $Controller; `
		Name = $ControllerTests; `
		View = $View `
	} `
    -SuccessMessage "Added tests for controller at $ControllerTests\{0}" `
    -TemplateFolders $TemplateFolders `
	-CodeLanguage $CodeLanguage `
	-Project $TestsProject `
	-Force:$Force

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
$ValidatorProject = "Validators"
$ControllerProject = "Controllers"

$ControllerNamespace = "MvcTemplate.Controllers"
If ($AreaName) { $ControllerNamespace = "MvcTemplate.Controllers.$AreaName" }
$ControllerTestsNamespace = "MvcTemplate.Tests.Unit.Controllers"
If ($AreaName) { $ControllerTestsNamespace = "MvcTemplate.Tests.Unit.Controllers.$AreaName" }

$Model = $ModelName
$View = $ModelName + "View"
$Service = $ModelName + "Service"
$Validator = $ModelName + "Validator"
$IService = "I" + $ModelName + "Service"
$Controller = $ControllerName + "Controller"
$IValidator = "I" + $ModelName + "Validator"
$ValidatorTests = $ModelName + "Validator" + "Tests"
$ServiceTests = $ControllerName + "Service" + "Tests"
$ControllerTests = $ControllerName + "Controller" + "Tests"

If ($AreaName) { $ElementPath = "$AreaName\$ControllerName" }
Else { $ElementPath = $ControllerName }

If ($AreaName)
{
    $ServicesAreaDir = "$AreaName"
    $ViewAreaDir = "Views\$AreaName"
    $ControllerAreaDir = "$AreaName"
    $ValidatorsAreaDir = "$AreaName"
    $ModelAreaDir = "Models\$AreaName"
    $RazorViewAreaDir = "Views\$AreaName"
    $ServiceTestsAreaDir = "Unit\Services\$AreaName"
    $ValidatorTestsAreaDir = "Unit\Validators\$AreaName"
    $ControllerTestsAreaDir = "Unit\Controllers\$AreaName"

    $ServicesControllerDir = "$AreaName\$ControllerName"
    $ViewControllerDir = "Views\$AreaName\$ControllerName"
    $ValidatorsControllerDir = "$AreaName\$ControllerName"
    $ControllerControllerDir = "$AreaName\$ControllerName"
    $ModelControllerDir = "Models\$AreaName\$ControllerName"
    $RazorViewControllerDir = "Views\$AreaName\$ControllerName"
    $ServiceTestsControllerDir = "Unit\Services\$AreaName\$ControllerName"
    $ValidatorTestsControllerDir = "Unit\Validators\$AreaName\$ControllerName"
    $ControllerTestsControllerDir = "Unit\Controllers\$AreaName\$ControllerName"
}
Else
{
    $ServicesAreaDir = "$ControllerName"
    $ViewAreaDir = "Views\$ControllerName"
    $ControllerAreaDir = "$ControllerName"
    $ValidatorsAreaDir = "$ControllerName"
    $ModelAreaDir = "Models\$ControllerName"
    $RazorViewAreaDir = "Views\$ControllerName"
    $ServiceTestsAreaDir = "Unit\Services\$ControllerName"
    $ValidatorTestsAreaDir = "Unit\Validators\$ControllerName"
    $ControllerTestsAreaDir = "Unit\Controllers\$ControllerName"

    $ServicesControllerDir = "$ControllerName"
    $ViewControllerDir = "Views\$ControllerName"
    $ControllerControllerDir = "$ControllerName"
    $ValidatorsControllerDir = "$ControllerName"
    $ModelControllerDir = "Models\$ControllerName"
    $RazorViewControllerDir = "Views\$ControllerName"
    $ServiceTestsControllerDir = "Unit\Services\$ControllerName"
    $ValidatorTestsControllerDir = "Unit\Validators\$ControllerName"
    $ControllerTestsControllerDir = "Unit\Controllers\$ControllerName"
}

$ViewPath = "Views\$ElementPath\$View"
$EditViewPath = "Views\$ElementPath\Edit"
$IndexViewPath = "Views\$ElementPath\Index"
$CreateViewPath = "Views\$ElementPath\Create"
$DeleteViewPath = "Views\$ElementPath\Delete"
$DetailsViewPath = "Views\$ElementPath\Details"
$ControllerPath = "$ElementPath\$Controller"
$ModelPath = "Models\$ElementPath\$Model"
$ServicePath = "$ElementPath\$Service"
$IServicePath = "$ElementPath\$IService"
$ValidatorPath = "$ElementPath\$Validator"
$IValidatorPath = "$ElementPath\$IValidator"
$ControllerTestsPath = "Unit\Controllers\$ElementPath\$ControllerTests"
$ValidatorTestsPath = "Unit\Validators\$ElementPath\$ValidatorTests"
$ServiceTestsPath = "Unit\Services\$ElementPath\$ServiceTests"

$ScriptDir = Split-Path -Path $MyInvocation.MyCommand.Definition -Parent
. $ScriptDir\ModuleFunctions.ps1

If ($Delete)
{
	Delete-ProjectItem $RazorViewProject "$DetailsViewPath.cshtml"
	Delete-ProjectItem $RazorViewProject "$DeleteViewPath.cshtml"
	Delete-ProjectItem $RazorViewProject "$CreateViewPath.cshtml"
	Delete-ProjectItem $RazorViewProject "$IndexViewPath.cshtml"
	Delete-ProjectItem $RazorViewProject "$EditViewPath.cshtml"
	Delete-ProjectItem $TestsProject "$ControllerTestsPath.cs"
	Delete-ProjectItem $TestsProject "$ValidatorTestsPath.cs"
	Delete-ProjectItem $TestsProject "$ServiceTestsPath.cs"
	Delete-ProjectItem $ControllerProject "$ControllerPath.cs"
	Delete-ProjectItem $ValidatorProject "$IValidatorPath.cs"
	Delete-ProjectItem $ValidatorProject "$ValidatorPath.cs"
	Delete-ProjectItem $ServiceProject "$IServicePath.cs"
	Delete-ProjectItem $ServiceProject "$ServicePath.cs"
	Delete-ProjectItem $ObjectsProject "$ModelPath.cs"
	Delete-ProjectItem $ObjectsProject "$ViewPath.cs"

    Delete-IfEmpty $TestsProject $ValidatorTestsControllerDir
    Delete-IfEmpty $TestsProject $ValidatorTestsAreaDir

    Delete-IfEmpty $TestsProject $ControllerTestsControllerDir
    Delete-IfEmpty $TestsProject $ControllerTestsAreaDir

    Delete-IfEmpty $TestsProject $ServiceTestsControllerDir
    Delete-IfEmpty $TestsProject $ServiceTestsAreaDir

    Delete-IfEmpty $RazorViewProject $RazorViewControllerDir
    Delete-IfEmpty $RazorViewProject $RazorViewAreaDir

    Delete-IfEmpty $ControllerProject $ControllerControllerDir
    Delete-IfEmpty $ControllerProject $ControllerAreaDir

    Delete-IfEmpty $ValidatorProject $ValidatorsControllerDir
    Delete-IfEmpty $ValidatorProject $ValidatorsAreaDir

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
		    Name = $Model `
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
		    Name = $Service; `
		    Model = $Model; `
		    View = $View `
	    } `
        -SuccessMessage "Added $ServiceProject\{0}" `
        -TemplateFolders $TemplateFolders `
	    -CodeLanguage $CodeLanguage `
	    -Project $ServiceProject

    Add-ProjectItemViaTemplate $IValidatorPath `
        -Template "Controls\IValidator" `
        -Model @{ `
		    Name = $IValidator; `
		    View = $View `
	    } `
        -SuccessMessage "Added $ValidatorProject\{0}" `
        -TemplateFolders $TemplateFolders `
	    -CodeLanguage $CodeLanguage `
	    -Project $ValidatorProject

    Add-ProjectItemViaTemplate $ValidatorPath `
        -Template "Controls\Validator" `
        -Model @{ `
		    Interface = $IValidator; `
		    Name = $Validator; `
		    View = $View `
	    } `
        -SuccessMessage "Added $ValidatorProject\{0}" `
        -TemplateFolders $TemplateFolders `
	    -CodeLanguage $CodeLanguage `
	    -Project $ValidatorProject

    Add-ProjectItemViaTemplate $ControllerPath `
        -Template "Controls\Controller" `
        -Model @{ `
            ModelName = $Model.SubString(0, 1).ToLower() + $Model.SubString(1); `
	        Namespace = $ControllerNamespace; `
		    IValidator = $IValidator; `
		    IService = $IService; `
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
            ModelName = $Model.SubString(0, 1).ToLower() + $Model.SubString(1); `
		    Name = $ServiceTests; `
		    Service = $Service; `
            Model = $Model; `
            View = $View `
	    } `
        -SuccessMessage "Added $TestsProject\{0}" `
        -TemplateFolders $TemplateFolders `
	    -CodeLanguage $CodeLanguage `
	    -Project $TestsProject

    Add-ProjectItemViaTemplate $ValidatorTestsPath `
        -Template "Tests\ValidatorTests" `
        -Model @{ `
		    Name = $ValidatorTests; `
		    Validator = $Validator; `
            Model = $Model; `
            View = $View `
	    } `
        -SuccessMessage "Added $TestsProject\{0}" `
        -TemplateFolders $TemplateFolders `
	    -CodeLanguage $CodeLanguage `
	    -Project $TestsProject

    Add-ProjectItemViaTemplate $ControllerTestsPath `
        -Template "Tests\ControllerTests" `
        -Model @{ `
            ModelName = $Model.SubString(0, 1).ToLower() + $Model.SubString(1); `
		    ControllerNamespace = $ControllerNamespace; `
	        Namespace = $ControllerTestsNamespace; `
		    ValidatorInterface = $IValidator; `
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

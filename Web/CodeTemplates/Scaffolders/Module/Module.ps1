[T4Scaffolding.Scaffolder(Description = "Creates default module controller, service, model, view model and views")][CmdletBinding()]
param(
    [parameter(Mandatory = $true)][string]$AreaName,
    [parameter(Mandatory = $true)][string]$ControllerName,
    [parameter(Mandatory = $true)][string]$ModelName,
    [string[]]$TemplateFolders,
    [switch]$Force = $false
)

$ModelProject = "Objects"
$ModelNamespace = "Template.Objects"
$ModelPath = "Models\" + $AreaName + "\" + $ControllerName + "\" + $ModelName

Add-ProjectItemViaTemplate $ModelPath -Template "Controls\Model" `
    -Model @{ Namespace = $ModelNamespace; Name = $ModelName } `
    -SuccessMessage "Added model at {0}" `
    -TemplateFolders $TemplateFolders -Project $ModelProject -CodeLanguage $CodeLanguage -Force:$Force

$ViewProject = "Objects"
$View = $ModelName + "View"
$ViewNamespace = "Template.Objects"
$ViewPath = "Views\" + $AreaName + "\" + $ControllerName + "\" + $View

Add-ProjectItemViaTemplate $ViewPath -Template "Controls\View" `
    -Model @{ Namespace = $ViewNamespace; Name = $View } `
    -SuccessMessage "Added view model at {0}" `
    -TemplateFolders $TemplateFolders -Project $ViewProject -CodeLanguage $CodeLanguage -Force:$Force

$ServiceProject = "Components"
$Service = $ControllerName + "Service"
$ServiceNamespace = "Template.Components.Services"
$ServiceInterface = "I" + $ControllerName + $Service
$ServicePath = "Services\" + $AreaName + "\" + $ControllerName + "\" + $Service
$ServiceInterfacePath = "Services\" + $AreaName + "\" + $ControllerName + "\" + $ServiceInterface

Add-ProjectItemViaTemplate $ServiceInterfacePath -Template "Controls\IService" `
    -Model @{ Namespace = $ServiceNamespace; Name = $ServiceInterface; View = $View } `
    -SuccessMessage "Added service interface at {0}" `
    -TemplateFolders $TemplateFolders -Project $ServiceProject -CodeLanguage $CodeLanguage -Force:$Force

Add-ProjectItemViaTemplate $ServicePath -Template "Controls\Service" `
    -Model @{ Namespace = $ServiceNamespace; Name = $Service; Model = $ModelName; View = $View; Interface = $ServiceInterface } `
    -SuccessMessage "Added service implementation at {0}" `
    -TemplateFolders $TemplateFolders -Project $ServiceProject -CodeLanguage $CodeLanguage -Force:$Force

$ControllerProject = "Controllers"
$Controller = $ControllerName + "Controller"
$ControllerNamespace = "Template.Controllers." + $AreaName
$ControllerPath = $AreaName + "\" + $ControllerName + "\" + $Controller

Add-ProjectItemViaTemplate $ControllerPath -Template "Controls\Controller" `
    -Model @{ Namespace = $ControllerNamespace; Name = $Controller; Service = $ServiceInterface; View = $View } `
    -SuccessMessage "Added controller at {0}" `
    -TemplateFolders $TemplateFolders -Project $ControllerProject -CodeLanguage $CodeLanguage -Force:$Force
    
$ViewProject = "Web"
$ViewPath = "Views\" + $AreaName + "\" + $ControllerName + "\"

Add-ProjectItemViaTemplate ($ViewPath + "Index") -Template "Views\Index" `
    -Model @{ View = $View } `
    -SuccessMessage "Added index view at {0}" `
    -TemplateFolders $TemplateFolders -Project $ViewProject -CodeLanguage $CodeLanguage -Force:$Force

Add-ProjectItemViaTemplate ($ViewPath + "Create") -Template "Views\Create" `
    -Model @{ View = $View } `
    -SuccessMessage "Added create view at {0}" `
    -TemplateFolders $TemplateFolders -Project $ViewProject -CodeLanguage $CodeLanguage -Force:$Force

Add-ProjectItemViaTemplate ($ViewPath + "Details") -Template "Views\Details" `
    -Model @{ View = $View } `
    -SuccessMessage "Added details view at {0}" `
    -TemplateFolders $TemplateFolders -Project $ViewProject -CodeLanguage $CodeLanguage -Force:$Force

Add-ProjectItemViaTemplate ($ViewPath + "Edit") -Template "Views\Edit" `
    -Model @{ View = $View } `
    -SuccessMessage "Added edit view at {0}" `
    -TemplateFolders $TemplateFolders -Project $ViewProject -CodeLanguage $CodeLanguage -Force:$Force

Add-ProjectItemViaTemplate ($ViewPath + "Delete") -Template "Views\Delete" `
    -Model @{ View = $View } `
    -SuccessMessage "Added delete view at {0}" `
    -TemplateFolders $TemplateFolders -Project $ViewProject -CodeLanguage $CodeLanguage -Force:$Force

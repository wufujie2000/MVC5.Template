[T4Scaffolding.Scaffolder(Description = "Creates default module controller, service, model, view model and views")][CmdletBinding()]
param(
	[parameter(Mandatory = $true)][string]$Area,
    [parameter(Mandatory = $true)][string]$Controller,
    [parameter(Mandatory = $true)][string]$Model,
	[string[]]$TemplateFolders,
	[switch]$Force = $false
)

$ModelProject = "Objects"
$ModelNamespace = "Template.Objects"
$ModelPath = "Models\" + $Area + "\" + $Controller + "\" + $Model

Add-ProjectItemViaTemplate $ModelPath -Template Model `
	-Model @{ Namespace = $ModelNamespace; Name = $Model } `
	-SuccessMessage "Added model at {0}" `
	-TemplateFolders $TemplateFolders -Project $ModelProject -CodeLanguage $CodeLanguage -Force:$Force

$ViewModel = $Model + "View"
$ViewModelProject = "Objects"
$ViewModelNamespace = "Template.Objects"
$ViewModelPath = "Views\" + $Area + "\" + $Controller + "\" + $Model

Add-ProjectItemViaTemplate $ViewModelPath -Template View `
	-Model @{ Namespace = $ViewModelNamespace; Name = $ViewModel } `
	-SuccessMessage "Added view model at {0}" `
	-TemplateFolders $TemplateFolders -Project $ViewModelProject -CodeLanguage $CodeLanguage -Force:$Force
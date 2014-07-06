Function Scaffold-CsTemplate([String]$Template, [String]$Project, [String]$OutputPath)
{
    if ($Delete)
    {
	    Delete-ProjectItem $Project "$OutputPath.cs"
        Return;
    }

    $ControllerNamespace = "MvcTemplate.Controllers"
    $ControllerTestsNamespace = "MvcTemplate.Tests.Unit.Controllers"
    If ($AreaName) { $ControllerNamespace = "MvcTemplate.Controllers.$Area" }
    If ($AreaName) { $ControllerTestsNamespace = "MvcTemplate.Tests.Unit.Controllers.$Area" }

    Add-ProjectItemViaTemplate `
        -OutputPath $OutputPath `
        -Template $Template `
        -Model @{ `
            ModelName = $Model.SubString(0, 1).ToLower() + $Model.SubString(1); `
		    ControllerTests = $Controller + "ControllerTests"; `
		    ValidatorTests = $Model + "ValidatorTests"; `
		    ServiceTests = $Model + "ServiceTests"; `

	        ControllerTestNamespace = $ControllerTestsNamespace; `
            ControllerNamespace = $ControllerNamespace; `

		    Controller = $Controller + "Controller"; `
		    IValidator = "I" + $Model + "Validator"; `
		    IService = "I" + $Model + "Service"; `
		    Validator = $Model + "Validator"; `
		    Service = $Model + "Service"; `
            View = $Model + "View"; `
            Model = $Model; `
	    } `
        -SuccessMessage "Added $Project\{0}." `
        -TemplateFolders $TemplateFolders `
	    -Project $Project
}
Function Scaffold-CshtmlTemplate([String]$Template, [String]$Project, [String]$OutputPath)
{
    if ($Delete)
    {
	    Delete-ProjectItem $Project "$OutputPath.cshtml"
        Return;
    }

    Add-ProjectItemViaTemplate `
        -OutputPath $OutputPath `
        -Template $Template `
        -Model @{ `
            View = $Model + "View"; `
	    } `
        -SuccessMessage "Added $Project\{0}." `
        -TemplateFolders $TemplateFolders `
	    -Project $Project
}

Function Scaffold-DbSet()
{
    if (!$Delete)
    {
        $TestingContext = Get-ProjectType -Project $TestsProject -Type "TestingContext"
	    $Context = Get-ProjectType -Project $DataProject -Type "Context"
        $Models = Get-PluralizedWord $Model

        Add-ClassMemberViaTemplate `
            -SuccessMessage "Added DbSet<$Model> member to testing context." `
            -TemplateFolders $TemplateFolders `
            -CodeClass $TestingContext `
            -Template "Members\DbSet" `
            -Model @{ `
                Area = $ElementArea; `
                Models = $Models; `
                Model = $Model; `
            }

        Add-ClassMemberViaTemplate `
            -SuccessMessage "Added DbSet<$Model> member to context." `
            -TemplateFolders $TemplateFolders `
            -Template "Members\DbSet" `
            -CodeClass $Context `
            -Model @{ `
                Area = $ElementArea; `
                Models = $Models; `
                Model = $Model; `
            }
    }
}

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

Function Delete-EmptyDir([String]$Project, [String]$Dir)
{
    $ProjectItem = Get-ProjectItem -Project $Project -Path $Dir
    If ($ProjectItem -and !(Get-ChildItem $ProjectItem.FileNames(0) -force))
    {
        $ProjectItem.Delete()
        Write-Host "Deleted $Project\$Dir"
    }
}

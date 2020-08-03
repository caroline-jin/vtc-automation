$path = $PSScriptRoot
$app = "\reset.bat"
$app_path = $path + $app
$A = New-ScheduledTaskAction -Execute $app_path
$T = New-ScheduledTaskTrigger -Once -At (Get-Date) -RepetitionInterval (New-TimeSpan -Minutes 5) 
$S = New-ScheduledTaskSettingsSet
$D = New-ScheduledTask -Action $A -Trigger $T -Settings $S
Register-ScheduledTask -TaskName "Reset" -InputObject $D



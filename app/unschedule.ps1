$Path = "C:\Windows\System32\Tasks"

Unregister-ScheduledTask -TaskName "Reset" -confirm:$false 
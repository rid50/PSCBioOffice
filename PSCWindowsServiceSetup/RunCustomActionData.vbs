Set WshShell = CreateObject("WScript.Shell")
WshShell.Run Property("CustomActionData"),7,False
'WshShell.Run "%COMSPEC% /k ipconfig"
'WshShell.Run "%COMSPEC% cd ."
'WshShell.Run "cd C:\psc\PSCBioOffice\bin\PSCWindowsServiceSetup",7,False
'WshShell.Run "installutil PSCWindowsService.exe",7,False
'WshShell.Run "C:\PSCInstallationFolder\PSCWindowsServiceSetup\installutil C:\PSCInstallationFolder\PSCWindowsServiceSetup\PSCWindowsService.exe",7,False

Set WshShell = Nothing

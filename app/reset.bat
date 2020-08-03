QPROCESS "ConsoleApp1.exe">NUL
IF %ERRORLEVEL% EQU 0 (taskkill /im "ConsoleApp1.exe" /fi "STATUS eq NOT RESPONDING") else (start "ConsoleApp1.exe" "%~dp0\ConsoleApp1\ConsoleApp1\bin\Debug\ConsoleApp1.exe") 

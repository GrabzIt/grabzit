cd "%~dp0"

IF EXIST %SystemRoot%\Microsoft.NET\Framework64\v4.0.30319\regasm.exe (
	%SystemRoot%\Microsoft.NET\Framework64\v4.0.30319\regasm.exe /tlb GrabzIt.dll /codebase || goto :Paused
	set /p temp="Successfully registered! Hit enter to exit"
	goto :eof
) 

IF EXIST %SystemRoot%\Microsoft.NET\Framework\v4.0.30319\regasm.exe (
	%SystemRoot%\Microsoft.NET\Framework\v4.0.30319\regasm.exe /tlb GrabzIt.dll /codebase || goto :Paused
	set /p temp="Successfully registered! Hit enter to exit"
	goto :eof
) 

IF EXIST %SystemRoot%\Microsoft.NET\Framework64\v2.0.50727\regasm.exe (
	%SystemRoot%\Microsoft.NET\Framework64\v2.0.50727\regasm.exe /tlb GrabzIt.dll /codebase || goto :Paused
	set /p temp="Successfully registered! Hit enter to exit"
	goto :eof
) 

IF EXIST %SystemRoot%\Microsoft.NET\Framework\v2.0.50727\regasm.exe (
	%SystemRoot%\Microsoft.NET\Framework\v2.0.50727\regasm.exe /tlb GrabzIt.dll /codebase || goto :Paused
	set /p temp="Successfully registered! Hit enter to exit"
	goto :eof
) 

echo ERROR: .NET Framework must be installed in order to register the GrabzIt DLL as a COM object!

:Paused

pause
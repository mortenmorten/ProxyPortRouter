@ECHO OFF
SETLOCAL

SET DESTINATION=%PROGRAMFILES%\Proxy Port Router\
SET SERVICENAME=ProxyPortRouterService

IF EXIST "%DESTINATION%ProxyPortRouterService.exe" (
	ECHO Uninstalling service
	"%DESTINATION%ProxyPortRouterService.exe" UNINSTALL
)

ECHO Deleting folder %DESTINATION%
RMDIR /S /Q "%DESTINATION%"

ECHO Copying to folder %DESTINATION%
XCOPY "%CD%" "%DESTINATION%" /E /Y /C

ECHO Installing service
"%DESTINATION%ProxyPortRouterService.exe" INSTALL

ECHO Starting service
SC START %SERVICENAME%

ENDLOCAL
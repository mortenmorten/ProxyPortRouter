@ECHO OFF
SETLOCAL

SET SOURCE=%~dp0
SET DESTINATION=%PROGRAMFILES%\Proxy Port Router\
SET SERVICENAME=ProxyPortRouterService

IF EXIST "%DESTINATION%ProxyPortRouterService.exe" (
	ECHO Uninstalling service
	"%DESTINATION%ProxyPortRouterService.exe" UNINSTALL
)

ECHO Deleting folder %DESTINATION%
RMDIR /S /Q "%DESTINATION%"

ECHO Copying from folder %SOURCE% to folder %DESTINATION%
XCOPY "%SOURCE%*" "%DESTINATION%" /E /Y /C

ECHO Installing service
"%DESTINATION%ProxyPortRouterService.exe" INSTALL

ECHO Starting service
SC START %SERVICENAME%

:END
PAUSE

ENDLOCAL
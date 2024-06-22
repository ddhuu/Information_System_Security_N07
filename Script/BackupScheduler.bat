@echo off

:: Explicitly set Oracle environment variables
set ORACLE_HOME=C:\app\dhuu2\product\21c\dbhomeXE
set TNS_ADMIN=%ORACLE_HOME%\network\admin
set ORACLE_SID=XE  
set PATH=%ORACLE_HOME%\bin;%PATH%



:: Prepare RMAN backup command
echo BACKUP DATABASE PDBQLHT PLUS ARCHIVELOG; > backup.rman

:: Generate timestamp for unique file naming
set timestamp=%date:~-4,4%%date:~-7,2%%date:~-10,2%_%time:~0,2%%time:~3,2%%time:~6,2%
set timestamp=%timestamp: =0%

:: Execute RMAN backup and log to a file with timestamp
rman target / cmdfile=backup.rman log backQup_%timestamp%.log

:: Check result and log accordingly
if %errorlevel% equ 0 (
    set status=Backup successful
) else (
    set status=Backup failed
)



:: Display status
echo %status%

pause
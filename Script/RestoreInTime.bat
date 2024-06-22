@echo off
(
echo RUN {
echo   SHUTDOWN IMMEDIATE;
echo   STARTUP MOUNT;
echo   ALTER DATABASE OPEN; 
echo   SET UNTIL TIME "to_date('2024-06-22 9:50:00', 'YYYY-MM-DD HH24:MI:SS')";
echo   RESTORE PLUGGABLE DATABASE PDBQLHT;
echo   RECOVER PLUGGABLE DATABASE PDBQLHT;
echo   ALTER PLUGGABLE DATABASE PDBQLHT OPEN RESETLOGS;
echo }
echo EXIT;
) | rman target / log=restore.log

pause
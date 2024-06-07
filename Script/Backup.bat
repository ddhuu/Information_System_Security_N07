@echo off

echo INSERT INTO admin.tblBackupLog (BackupStatus) VALUES ('Backup started'); | sqlplus / as sysdba

echo BACKUP  DATABASE ATBM_CQ_07 PLUS ARCHIVELOG; > backup.rman
rman target / cmdfile=backup.rman log backup.log

if %errorlevel% equ 0 (
    set status=Backup successful
) else (
    set status=Backup failed
)

echo INSERT INTO admin.tblBackupLog (BackupStatus) VALUES ('%status%'); | sqlplus / as sysdba

echo %status%

pause
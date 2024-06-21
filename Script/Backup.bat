@echo off

echo INSERT INTO admin.tblBackupLog (BackupStatus) VALUES ('Backup started'); | sqlplus / as sysdba

echo BACKUP  DATABASE PDBQLHT PLUS ARCHIVELOG; > backup.rman

:: Tạo timestamp
set timestamp=%date:~-4,4%%date:~-7,2%%date:~-10,2%_%time:~0,2%%time:~3,2%%time:~6,2%
set timestamp=%timestamp: =0%

:: Thêm timestamp vào tên file log
rman target / cmdfile=backup.rman log backup_%timestamp%.log

if %errorlevel% equ 0 (
    set status=Backup successful
) else (
    set status=Backup failed
)

echo INSERT INTO admin.tblBackupLog (BackupStatus) VALUES ('%status%'); | sqlplus / as sysdba

echo %status%

pause
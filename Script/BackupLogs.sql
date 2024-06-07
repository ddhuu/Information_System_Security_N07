CREATE TABLE tblBackupLog (
    BackupTimestamp TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    BackupStatus VARCHAR(50)
);

CREATE INDEX idxBackupTimestamp ON tblBackupLog (BackupTimestamp);
CREATE INDEX idxBackupStatus ON tblBackupLog (BackupStatus);
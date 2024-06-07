BEGIN
  DBMS_SCHEDULER.create_program(
    program_name => 'BackupProgram',
    program_type => 'EXECUTABLE',
    program_action => 'C:\Users\dhuu2\OneDrive\Máy tính\Backup.bat',
    enabled => TRUE);

  DBMS_SCHEDULER.create_schedule(
    schedule_name => 'Daily2AM',
    start_date => SYSTIMESTAMP,
    repeat_interval => 'FREQ=DAILY;BYHOUR=2;BYMINUTE=0',
    end_date => NULL,
    comments => 'Every day at 2 AM');

  DBMS_SCHEDULER.create_job(
    job_name => 'BackupJob',
    program_name => 'BackupProgram',
    schedule_name => 'Daily2AM',
    enabled => TRUE);
END;
/

-- Check
SELECT * FROM DBA_SCHEDULER_JOBS WHERE JOB_NAME = 'BACKUPJOB';
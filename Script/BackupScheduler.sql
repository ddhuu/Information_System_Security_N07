-- Create the backup program, schedule, and job
BEGIN
  DBMS_SCHEDULER.create_program(
    program_name => 'BackupProgram',
    program_type => 'EXECUTABLE',
    program_action => '"C:\app\dhuu2\product\21c\dbhomeXE\database\BackupScheduler.bat"',
    enabled => TRUE);

  DBMS_SCHEDULER.create_schedule(
    schedule_name => 'Daily12AM',
    start_date => SYSTIMESTAMP,
    repeat_interval => 'FREQ=DAILY;BYHOUR=12;BYMINUTE=0', 
    end_date => NULL,
    comments => 'Every day at 12:00 AM');

  DBMS_SCHEDULER.create_job(
    job_name => 'BackupJob',
    program_name => 'BackupProgram',
    schedule_name => 'Daily12AM',
    enabled => TRUE);
END;
/


--BEGIN
--  DBMS_SCHEDULER.drop_job(job_name => 'BackupJob');
--  DBMS_SCHEDULER.drop_program(program_name => 'BackupProgram');
--  DBMS_SCHEDULER.drop_schedule(schedule_name => 'Daily4AM');
--END;
--/

-- Check

-- Obtain run details for the 'BACKUPJOB', including the actual start date and run duration
-- Useful for auditing and ensuring the backup job runs as scheduled
SELECT JOB_NAME, 
       TO_CHAR(ACTUAL_START_DATE, 'DD-MM-YYYY HH24:MI:SS') AS ACTUAL_START_DATE_FORMATTED, 
       RUN_DURATION 
FROM DBA_SCHEDULER_JOB_RUN_DETAILS 
WHERE JOB_NAME = 'BACKUPJOB' 
ORDER BY ACTUAL_START_DATE DESC;

-- Retrieve details of the scheduled backup job named 'BACKUPJOB'
-- This includes the job's name, style, associated program, and its start date
SELECT JOB_NAME, 
       JOB_STYLE, 
       PROGRAM_NAME, 
       TO_CHAR(START_DATE, 'DD-MM-YYYY HH24:MI:SS') AS FORMATTED_START_DATE
FROM DBA_SCHEDULER_JOBS 
WHERE JOB_NAME = 'BACKUPJOB';

-- Fetch start and completion times, along with the elapsed seconds for each backup set
-- This helps in monitoring the duration and success of backup operations
SELECT   
    TO_CHAR(START_TIME, 'DD-MON-YYYY HH24:MI:SS') AS START_TIME, 
    TO_CHAR(COMPLETION_TIME, 'DD-MON-YYYY HH24:MI:SS') AS COMPLETION_TIME, 
    ELAPSED_SECONDS
FROM 
    V$BACKUP_SET
ORDER BY START_TIME DESC;

-- Check with rman 
SELECT OPERATION, 
       STATUS, 
       TO_CHAR(START_TIME, 'DD-MM-YYYY HH24:MI:SS') AS START_TIME_FORMATTED
FROM V$RMAN_STATUS
WHERE OPERATION IN ('BACKUP', 'RESTORE')
ORDER BY START_TIME DESC;






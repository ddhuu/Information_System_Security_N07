-- kich hoat va cau hinh
ALTER SYSTEM SET AUDIT_TRAIL=DB, EXTENDED SCOPE=SPFILE;
SHUTDOWN IMMEDIATE;
STARTUP;

-- standard audit
CREATE OR REPLACE PROCEDURE USP_SETTING_AUDIT(p_operation IN VARCHAR2) AUTHID CURRENT_USER AS
BEGIN
    if(p_operation = 'enable') then
        for i in (select * from user_tables) loop
            execute immediate 'audit all on admin.' || i.table_name || ' by access';
            execute immediate 'audit select on admin.' || i.table_name || ' by access whenever successful';
        end loop;
        for i in (select* from user_views) loop
            execute immediate 'audit all on admin.' || i.view_name || ' by access';
            execute immediate 'audit select on admin.' || i.view_name || ' by access whenever successful';
        end loop;
        for i in (select* from user_procedures where object_type='PROCEDURE' and object_name <> 'USP_SETTING_AUDIT') loop
            execute immediate 'audit all on admin.' || i.object_name || ' by access';
        end loop;
        execute immediate 'audit session';
    elsif (p_operation = 'disable') then
        for i in (select * from user_tables) loop
            execute immediate 'noaudit all on admin.' || i.table_name;
        end loop;
        for i in (select* from user_views) loop
            execute immediate 'noaudit all on admin.' || i.view_name;
        end loop;
        for i in (select* from user_procedures where object_type='PROCEDURE' and object_name <> 'USP_SETTING_AUDIT') loop
            execute immediate 'noaudit all on admin.' || i.object_name;
        end loop;
        execute immediate 'noaudit session';
    end if;
END;
/

EXEC USP_SETTING_AUDIT('enable');
--EXEC USP_SETTING_AUDIT('disable');


-- clear audit log before test
--delete from sys.aud$;


-- Check
SELECT 
    USERNAME, 
    OBJ_NAME, 
    ACTION, 
    ACTION_NAME, 
    RETURNCODE,
    TO_CHAR(EXTENDED_TIMESTAMP, 'DD/MM/YYYY HH24:MI:SS') AS "EXTENDED_TIMESTAMP",
    SQL_TEXT
FROM 
    DBA_AUDIT_TRAIL
ORDER BY 
    TIMESTAMP DESC;

    
SELECT * FROM DBA_AUDIT_TRAIL ORDER BY TIMESTAMP DESC;






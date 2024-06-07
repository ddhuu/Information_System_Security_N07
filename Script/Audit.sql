--########################
---------AUDIT----------##
--########################



---===STANDARD AUDIT===---

-- Enable Audit for employee

CREATE OR REPLACE PROCEDURE sp_enable_audit_employee AUTHID CURRENT_USER AS
    CURSOR cur IS (
        SELECT MANV
        FROM NHANSU
        WHERE MANV IN (SELECT username FROM all_users)
    );
BEGIN
    FOR usr IN cur LOOP
        EXECUTE IMMEDIATE 'AUDIT ALL BY ' || usr.MANV || ' BY ACCESS';
        EXECUTE IMMEDIATE 'AUDIT SELECT TABLE, UPDATE TABLE, INSERT TABLE, DELETE TABLE BY ' || usr.MANV || ' BY ACCESS';
        EXECUTE IMMEDIATE 'AUDIT SESSION WHENEVER NOT SUCCESSFUL';
    END LOOP;
END;
/

-- Disable Audit for employee

CREATE OR REPLACE PROCEDURE sp_disable_audit_employee AUTHID CURRENT_USER AS
    CURSOR cur IS (
        SELECT MANV
        FROM NHANSU
        WHERE MANV IN (SELECT username FROM all_users)
    );
BEGIN
    FOR usr IN cur LOOP
        EXECUTE IMMEDIATE 'NOAUDIT ALL BY ' || usr.MANV;
        EXECUTE IMMEDIATE 'NOAUDIT SELECT TABLE, UPDATE TABLE, INSERT TABLE, DELETE TABLE BY ' || usr.MANV || ' BY ACCESS';
        EXECUTE IMMEDIATE 'NOAUDIT SESSION WHENEVER NOT SUCCESSFUL';
    END LOOP;
END;
/

EXEC sp_enable_audit_employee;

-- Check

SELECT 
    USERNAME, 
    OBJ_NAME, 
    ACTION, 
    ACTION_NAME, 
    TO_CHAR(EXTENDED_TIMESTAMP, 'DD/MM/YYYY HH24:MI:SS') AS "EXTENDED_TIMESTAMP"
FROM 
    DBA_AUDIT_TRAIL 
WHERE 
    OWNER = 'ADMIN' 
    AND ACTION_NAME IN ('SELECT', 'INSERT', 'UPDATE', 'DELETE') 
ORDER BY 
    TIMESTAMP DESC;



---===FINE GRAIN AUDIT===--- 


-- 3a) Behavior of updating the DANGKY relation at fields related to scores
--     but the person is not in the role of Lecturer

CREATE OR REPLACE FUNCTION CheckRoleLecturer 
RETURN NUMBER 
AS
    IsLecturer NUMBER := 0;
    CURSOR RoleCursor IS
        SELECT granted_role
        FROM dba_role_privs
        WHERE grantee = SYS_CONTEXT('USERENV', 'SESSION_USER');
BEGIN
    IF (SYS_CONTEXT('USERENV', 'SESSION_USER') = 'ADMIN') THEN 
        RETURN 0;
    END IF;

    FOR RoleRecord IN RoleCursor LOOP
        IF RoleRecord.granted_role = 'RL_GIANGVIEN' THEN
            IsLecturer := 1; 
            EXIT;
        END IF;
    END LOOP;

    RETURN IsLecturer;
END;
/

BEGIN
    DBMS_FGA.ADD_POLICY(
        object_schema   => 'ADMIN',
        object_name     => 'DANGKY',
        policy_name     => 'FGA_POLICY_POINT',
        audit_condition => 'CheckRoleLecturer = 0',
        audit_column    => 'DIEMTH, DIEMQT, DIEMCK, DIEMTK',
        statement_types => 'INSERT,UPDATE,DELETE'
    );
END;
/


begin
    dbms_fga.enable_policy(
        object_schema => 'ADMIN',
        object_name   => 'DANGKY',
        policy_name   => 'FGA_POLICY_POINT',
        enable        => true
    );
    
end;
/

--Check
SELECT * FROM DBA_FGA_AUDIT_TRAIL WHERE POLICY_NAME = 'FGA_POLICY_POINT';

-- 3b) The user's behavior can be audited when they read the 'PHUCAP' column of other users in the 'NHANSU' table
BEGIN
    DBMS_FGA.ADD_POLICY(
        OBJECT_SCHEMA   => 'ADMIN',  
        OBJECT_NAME     => 'NHANSU',     
        POLICY_NAME     => 'FGA_AUDIT_PHUCAP',
        AUDIT_CONDITION => 'MANV <> SYS_CONTEXT(''USERENV'', ''SESSION_USER'')', 
        AUDIT_COLUMN    => 'PHUCAP',     
        STATEMENT_TYPES => 'SELECT'      
    );
END;
/


begin
    dbms_fga.enable_policy(
        object_schema => 'ADMIN',
        object_name   => 'NHANSU',
        policy_name   => 'FGA_AUDIT_PHUCAP',
        enable        => true
    );
end;
/

-- Check
SELECT * FROM DBA_FGA_AUDIT_TRAIL WHERE POLICY_NAME = 'FGA_AUDIT_PHUCAP';






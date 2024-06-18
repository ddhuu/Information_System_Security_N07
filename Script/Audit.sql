--########################
---------AUDIT----------##
--########################



---===STANDARD AUDIT===---

-- Enable Audit for users

CREATE OR REPLACE PROCEDURE sp_audit_user(p_table_name IN VARCHAR2, p_operation IN VARCHAR2) AUTHID CURRENT_USER AS
    TYPE refcur IS REF CURSOR;
    cur refcur;
    v_sql VARCHAR2(1000);
    v_username VARCHAR2(100);
BEGIN
    IF p_table_name = 'NHANSU' THEN
        OPEN cur FOR 'SELECT MANV FROM NHANSU WHERE MANV IN (SELECT username FROM all_users)';
    ELSIF p_table_name = 'SINHVIEN' THEN
        OPEN cur FOR 'SELECT MASV FROM SINHVIEN WHERE MASV IN (SELECT username FROM all_users)';
    END IF;

    LOOP
        FETCH cur INTO v_username;
        EXIT WHEN cur%NOTFOUND;

        IF p_operation = 'enable' THEN
            v_sql := 'AUDIT ALL BY ' || v_username || ' BY ACCESS';
            EXECUTE IMMEDIATE v_sql;
            v_sql := 'AUDIT SELECT TABLE, UPDATE TABLE, INSERT TABLE, DELETE TABLE BY ' || v_username || ' BY ACCESS';
            EXECUTE IMMEDIATE v_sql;
            v_sql := 'AUDIT SESSION WHENEVER NOT SUCCESSFUL';
            EXECUTE IMMEDIATE v_sql;
        ELSIF p_operation = 'disable' THEN
            v_sql := 'NOAUDIT ALL BY ' || v_username;
            EXECUTE IMMEDIATE v_sql;
            v_sql := 'NOAUDIT SELECT TABLE, UPDATE TABLE, INSERT TABLE, DELETE TABLE BY ' || v_username;
            EXECUTE IMMEDIATE v_sql;
            v_sql := 'NOAUDIT SESSION WHENEVER NOT SUCCESSFUL';
            EXECUTE IMMEDIATE v_sql;
        END IF;
    END LOOP;

    CLOSE cur;
END;
/


EXEC sp_audit_user('NHANSU', 'enable');
EXEC sp_audit_user('SINHVIEN', 'enable');

-- EXEC sp_audit_user('NHANSU', 'disable');
-- EXEC sp_audit_user('SINHVIEN', 'disable');



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
        FROM user_role_privs
        WHERE username = SYS_CONTEXT('USERENV', 'SESSION_USER');
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
        policy_name     => 'FGA_POLICY_DIEM',
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
        policy_name   => 'FGA_POLICY_DIEM',
        enable        => true
    );
    
end;
/

--Check
SELECT * FROM DBA_FGA_AUDIT_TRAIL WHERE POLICY_NAME = 'FGA_POLICY_DIEM';

-- 3b) The user's behavior can be audited when they read the 'PHUCAP' column of other users in the 'NHANSU' table
BEGIN
    DBMS_FGA.ADD_POLICY(
        OBJECT_SCHEMA   => 'ADMIN',  
        OBJECT_NAME     => 'NHANSU',     
        POLICY_NAME     => 'FGA_POLICY_PHUCAP',
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
        policy_name   => 'FGA_POLICY_PHUCAP',
        enable        => true
    );
end;
/

-- Check
SELECT * FROM DBA_FGA_AUDIT_TRAIL WHERE POLICY_NAME = 'FGA_POLICY_PHUCAP';




-- Utility

CREATE OR REPLACE PROCEDURE sp_get_audit_status(result OUT NUMBER) AS 
    v_count NUMBER;
BEGIN
    SELECT COUNT(*)
    INTO v_count
    FROM DBA_PRIV_AUDIT_OPTS;

    IF v_count > 0 THEN
        result := 1;
    ELSE
        result := 0;
    END IF;
END;
/

CREATE OR REPLACE PROCEDURE get_fg_audit_status(result  OUT NUMBER)
AS
    v_count NUMBER;
BEGIN
    SELECT COUNT(*)
    INTO v_count
    FROM DBA_AUDIT_POLICIES
    WHERE (OBJECT_SCHEMA = 'ADMIN' AND POLICY_OWNER = 'ADMIN' AND POLICY_NAME = 'FGA_POLICY_DIEM' AND ENABLED = 'YES')
    OR (OBJECT_SCHEMA = 'ADMIN' AND POLICY_OWNER = 'ADMIN'  AND POLICY_NAME = 'FGA_POLICY_PHUCAP' AND ENABLED = 'YES');

    IF v_count = 2 THEN
        result := 1;
    ELSE
        result := 0;
    END IF;
END;
/






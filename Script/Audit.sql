--########################
---------AUDIT----------##
--########################

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






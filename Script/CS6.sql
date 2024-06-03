CREATE OR REPLACE FUNCTION STUDENT_StudentSelect_Policy_Function(p_schema VARCHAR2, p_object VARCHAR2)
    RETURN VARCHAR2
    AS
        v_username VARCHAR2(10);
     BEGIN
         v_username := SYS_CONTEXT('USERENV', 'SESSION_USER');
        return 'MASV = ''' || v_username || '''';
     END;

CREATE OR REPLACE FUNCTION STUDENT_StudentUpdate_Policy_Function(p_schema VARCHAR2, p_object VARCHAR2)
RETURN VARCHAR2
AS
    v_username VARCHAR2(10);
BEGIN
    v_username := SYS_CONTEXT('USERENV', 'SESSION_USER');
    RETURN 'MASV = ''' || v_username || '''';
END;

BEGIN
    DBMS_RLS.ADD_POLICY(
        object_schema   => 'ADMIN',
        object_name     => 'SINHVIEN',
        policy_name     => 'Student_Select_Policy',
        function_schema => 'ADMIN',
        policy_function => 'STUDENT_StudentSelect_Policy_Function',
        statement_types => 'SELECT'
    );
    DBMS_RLS.ADD_POLICY(
        object_schema   => 'ADMIN',
        object_name     => 'SINHVIEN',
        policy_name     => 'Student_Update_Policy',
        function_schema => 'ADMIN',
        policy_function => 'STUDENT_StudentUpdate_Policy_Function',
        statement_types => 'UPDATE'
    );
END;


-- BEGIN
--     DBMS_RLS.DROP_POLICY(
--         object_schema   => 'ADMIN',
--         object_name     => 'SINHVIEN',
--         policy_name     => 'Student_Select_Policy'
--     );
--
--     DBMS_RLS.DROP_POLICY(
--         object_schema   => 'ADMIN',
--         object_name     => 'SINHVIEN',
--         policy_name     => 'Student_Update_Policy'
--     );
-- END;

--Grant select and update to SINHVIEN
GRANT SELECT ON SINHVIEN TO RL_SINHVIEN;
GRANT UPDATE(DCHI, DT) ON ADMIN.SINHVIEN TO RL_SINHVIEN;

CREATE OR REPLACE FUNCTION KHMO_StudentSelect_Policy_Function(p_schema VARCHAR2, p_object VARCHAR2)
RETURN VARCHAR2
AS
    v_username VARCHAR2(10);
    v_program VARCHAR2(10);
    v_role VARCHAR2(255);
    BEGIN
        v_username := SYS_CONTEXT('USERENV', 'SESSION_USER');
        IF(v_username = 'ADMIN') THEN
            RETURN '1 = 1';
        END IF;
        SELECT GRANTED_ROLE INTO v_role FROM DBA_ROLE_PRIVS WHERE GRANTEE = v_username;
        IF v_role = 'RL_SINHVIEN' THEN
            SELECT MACT INTO v_program FROM SINHVIEN WHERE MASV = v_username;
            return 'MACT = ''' || v_program || '''';
        END IF;
        RETURN '1 = 1';
    END;

BEGIN
    DBMS_RLS.ADD_POLICY(
        object_schema   => 'ADMIN',
        object_name     => 'KHMO',
        policy_name     => 'KHMO_Select_Policy',
        function_schema => 'ADMIN',
        policy_function => 'KHMO_StudentSelect_Policy_Function',
        statement_types => 'SELECT'
    );
END;
GRANT SELECT ON KHMO TO RL_SINHVIEN;


CREATE OR REPLACE FUNCTION DANGKY_StudentSelect_Policy_Function(p_schema VARCHAR2, p_object VARCHAR2)
RETURN VARCHAR2
AS
    v_username VARCHAR2(10);
    v_role VARCHAR2(255);
    BEGIN
        v_username := SYS_CONTEXT('USERENV', 'SESSION_USER');

        SELECT GRANTED_ROLE INTO v_role FROM DBA_ROLE_PRIVS WHERE GRANTEE = v_username;
        IF v_role = 'RL_SINHVIEN' THEN
            RETURN 'MASV = ''' || v_username || '''';
        END IF;
        RETURN '1 = 1';
    END;

CREATE OR REPLACE FUNCTION DANGKY_StudentDelete_Policy_Function(p_schema VARCHAR2, p_object VARCHAR2)
RETURN VARCHAR2
AS
    v_username VARCHAR2(10);
    v_sem_start_date DATE;
    v_current_date DATE := SYSDATE;
    v_role VARCHAR(255);
    v_current_semester NUMBER;
    v_year NUMBER := EXTRACT(YEAR FROM SYSDATE);
    BEGIN
        v_username := SYS_CONTEXT('USERENV', 'SESSION_USER');
        --Check role of the user
        SELECT GRANTED_ROLE INTO v_role FROM DBA_ROLE_PRIVS WHERE GRANTEE = v_username;
        IF v_role = 'RL_SINHVIEN' THEN
            SELECT CASE
                WHEN EXTRACT(MONTH FROM v_current_date) BETWEEN 1 AND 4 THEN 1
                WHEN EXTRACT(MONTH FROM v_current_date) BETWEEN 5 AND 8 THEN 2
                ELSE 3
                END INTO v_current_semester
                FROM DUAL;
            --Get semester start date base on current dte
            SELECT CASE
                WHEN EXTRACT(MONTH FROM v_current_date) BETWEEN 1 AND 4 THEN TO_DATE('01-JAN-' || TO_CHAR(v_current_date, 'YYYY'), 'DD-MON-YYYY')
                WHEN EXTRACT(MONTH FROM v_current_date) BETWEEN 5 AND 8 THEN TO_DATE('01-MAY-' || TO_CHAR(v_current_date, 'YYYY'), 'DD-MON-YYYY')
                ELSE TO_DATE('01-SEP-' || TO_CHAR(v_current_date, 'YYYY'), 'DD-MON-YYYY')
                END INTO v_sem_start_date
            FROM DUAL;
            IF current_date BETWEEN v_sem_start_date AND (v_sem_start_date + 14) THEN
                -- Student can only delete record in the current semester
                RETURN 'MASV = ''' || v_username || '''' || ' AND NAM = ' || v_year || ' AND HK = ' || v_current_semester;
            END IF;
--             RETURN '1 = 0';
           RETURN 'MASV = ''' || v_username || '''' || ' AND NAM = ' || v_year || ' AND HK = ' || v_current_semester;
        END IF;
        RETURN '1 = 1';
    END;
CREATE OR REPLACE FUNCTION DANGKY_StudentInsert_Policy_Function(p_schema VARCHAR2, p_object VARCHAR2)
RETURN VARCHAR2
AS
    v_username VARCHAR2(10);
    v_sem_start_date DATE;
    v_current_date DATE := SYSDATE;
    v_role VARCHAR(255);
    v_current_semester INT;
    v_year INT := EXTRACT(YEAR FROM SYSDATE);
    v_program VARCHAR2(10);
    BEGIN
        v_username := SYS_CONTEXT('USERENV', 'SESSION_USER');
        IF(v_username = 'ADMIN') THEN
            RETURN '1 = 1';
        END IF;
        --Check role of the user
        SELECT GRANTED_ROLE INTO v_role FROM DBA_ROLE_PRIVS WHERE GRANTEE = v_username;
        IF v_role = 'RL_SINHVIEN' THEN
            SELECT CASE
                WHEN EXTRACT(MONTH FROM v_current_date) BETWEEN 1 AND 4 THEN 1
                WHEN EXTRACT(MONTH FROM v_current_date) BETWEEN 5 AND 8 THEN 2
                ELSE 3
                END INTO v_current_semester
                FROM DUAL;
            --Get semester start date base on current date
            SELECT CASE
                WHEN EXTRACT(MONTH FROM v_current_date) BETWEEN 1 AND 4 THEN TO_DATE('01-JAN-' || TO_CHAR(v_current_date, 'YYYY'), 'DD-MON-YYYY')
                WHEN EXTRACT(MONTH FROM v_current_date) BETWEEN 5 AND 8 THEN TO_DATE('01-MAY-' || TO_CHAR(v_current_date, 'YYYY'), 'DD-MON-YYYY')
                ELSE TO_DATE('01-SEP-' || TO_CHAR(v_current_date, 'YYYY'), 'DD-MON-YYYY')
                END INTO v_sem_start_date
            FROM DUAL;
            SELECT MACT INTO v_program FROM SINHVIEN WHERE MASV = v_username;
            IF current_date BETWEEN v_sem_start_date AND (v_sem_start_date + 14) THEN
                -- Student can only delete record in the current semester
                RETURN 'MASV = ''' || v_username || '''' || ' AND NAM = ' || v_year || ' AND HK = ' || v_current_semester;
            END IF;
--             RETURN '1 = 0';
            RETURN 'MASV = ''' || v_username || '''' || ' AND NAM = ' || v_year || ' AND HK = ' || v_current_semester || ' AND MACT = ''' || v_program || '''' ;

        END IF;
        RETURN '1 = 1';
    END;
GRANT SELECT, INSERT, DELETE ON ADMIN.DANGKY TO RL_SINHVIEN;
BEGIN
    DBMS_RLS.ADD_POLICY(
        object_schema   => 'ADMIN',
        object_name     => 'DANGKY',
        policy_name     => 'DANGKY_Select_Policy',
        function_schema => 'ADMIN',
        policy_function => 'DANGKY_StudentSelect_Policy_Function',
        statement_types => 'SELECT'
    );
END;
BEGIN
    DBMS_RLS.ADD_POLICY(
        object_schema   => 'ADMIN',
        object_name     => 'DANGKY',
        policy_name     => 'DANGKY_Delete_Policy',
        function_schema => 'ADMIN',
        policy_function => 'DANGKY_StudentDelete_Policy_Function',
        statement_types => 'DELETE'
    );
    DBMS_RLS.ADD_POLICY(
        object_schema   => 'ADMIN',
        object_name     => 'DANGKY',
        policy_name     => 'DANGKY_Insert_Policy',
        function_schema => 'ADMIN',
        policy_function => 'DANGKY_StudentInsert_Policy_Function',
        statement_types => 'INSERT',
        update_check => TRUE
    );
END;

BEGIN
    DBMS_RLS.DROP_POLICY(
        object_name => 'DANGKY',
        policy_name => 'DANGKY_Delete_Policy',
        object_schema => 'ADMIN'
    );
    DBMS_RLS.DROP_POLICY(
        object_name => 'DANGKY',
        policy_name => 'DANGKY_Insert_Policy',
        object_schema => 'ADMIN'
    );
end;

SELECT * FROM DBA_ROLE_PRIVS;

select * from DBA_ROLE_PRIVS WHERE GRANTEE = 'NV001'

--Connect to db as user with RL_SINHVIEN
SELECT * FROM ADMIN.sinhvien;
UPDATE ADMIN.SINHVIEN SET DCHI = 'Dong Nai';
    SELECT * FROM ADMIN.khmo;
SELECT * FROM ADMIN.dangky;
INSERT INTO ADMIN.DANGKY VALUES('SV1002','NV324','HP504', 2, 2024, 'CQ', null, null, null, null);
INSERT INTO ADMIN.DANGKY VALUES('SV1002','NV329','HP604', 2, 2024, 'CQ', null, null, null, null);
INSERT INTO ADMIN.DANGKY VALUES('SV1002','NV311','HP301', 1, 2024, 'CQ', 6, 6, 6, 6);
INSERT INTO ADMIN.DANGKY VALUES('SV1002','NV321','HP501', 1, 2024, 'CQ', 6, 6, 6, 6);
DELETE FROM ADMIN.DANGKY WHERE MAHP = 'HP311';
DELETE FROM ADMIN.DANGKY WHERE MAHP = 'HP504';
DELETE FROM ADMIN.DANGKY WHERE MAHP = 'HP604';
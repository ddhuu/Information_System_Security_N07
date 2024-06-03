--#DOTO: GRANT ALL PRIVILEGES TO ADMIN
--#DOTO: Grant execute privilege on package DBMS_RLS to ADMIN
CREATE OR REPLACE PROCEDURE usp_GrantPrivilegesToHeadDep
AS
    v_head_dep VARCHAR2(10);
    BEGIN
        SELECT MANV INTO v_head_dep FROM NHANSU WHERE VAITRO = 'TRUONG KHOA';
        EXECUTE IMMEDIATE 'GRANT RL_GIANGVIEN TO ' || v_head_dep;
        EXECUTE IMMEDIATE 'GRANT SELECT, UPDATE, INSERT, DELETE ON ADMIN.NHANSU TO ' || v_head_dep;
        EXECUTE IMMEDIATE  'GRANT SELECT, UPDATE, INSERT, DELETE ON ADMIN.PHANCONG TO ' || v_head_dep;
        -- Select any table privilege and select any dictionary ???
        FOR t IN (SELECT TABLE_NAME FROM ALL_TABLES WHERE OWNER = 'ADMIN') LOOP
            EXECUTE IMMEDIATE 'GRANT SELECT ON ADMIN.' || t.TABLE_NAME || ' TO ' || v_head_dep;
        END LOOP;
        FOR v IN (SELECT VIEW_NAME FROM ALL_VIEWS WHERE OWNER = 'ADMIN') LOOP
            EXECUTE IMMEDIATE 'GRANT SELECT ON ADMIN.' || v.VIEW_NAME || ' TO ' || v_head_dep;
        END LOOP;
    END;
-- CREATE OR REPLACE TRIGGER trg_GrantSelectNewObject
-- AFTER CREATE ON ADMIN.SCHEMA
-- DECLARE
--     v_sql VARCHAR(255);
--     v_head_dep VARCHAR2(10);
--     BEGIN
--         SELECT MANV INTO v_head_dep FROM NHANSU WHERE VAITRO = 'TRUONG KHOA';
--         IF ORA_DICT_OBJ_TYPE = 'TABLE' OR ORA_DICT_OBJ_TYPE = 'VIEW' THEN
--             v_sql := 'GRANT SELECT ON ' || ORA_DICT_OBJ_OWNER || '.' || ORA_DICT_OBJ_NAME || ' TO ' || v_head_dep;
--             EXECUTE IMMEDIATE v_sql;
--         END IF;
--     END;
BEGIN
    usp_GrantPrivilegesToHeadDep();
END;


CREATE OR REPLACE FUNCTION PHANCONG_HeadDep_Policy_Function(p_schema VARCHAR2, p_object VARCHAR2)
RETURN VARCHAR2
AS
    v_username VARCHAR2(10);
    v_count NUMBER;
    BEGIN
        v_username := SYS_CONTEXT('USERENV', 'SESSION_USER');
        SELECT COUNT(*)
        INTO v_count
        FROM NHANSU
        WHERE MANV = v_username AND VAITRO = 'TRUONG KHOA';
        IF v_count > 0 THEN
            RETURN 'EXISTS (SELECT 1 FROM ADMIN.HOCPHAN HP WHERE HP.MAHP = PHANCONG.MAHP AND HP.MADV = ''VPK'') ';
        END IF;
        return '1 = 1';
    END;
BEGIN
    DBMS_RLS.ADD_POLICY(
        object_schema   => 'ADMIN',
        object_name     => 'PHANCONG',
        policy_name     => 'PHANCONG_HeapDep_Policy',
        function_schema => 'ADMIN',
        policy_function => 'PHANCONG_HeadDep_Policy_Function',
        statement_types => 'INSERT, UPDATE, DELETE',
        update_check => TRUE
    );
END;
BEGIN
    DBMS_RLS.DROP_POLICY(
        object_name => 'PHANCONG',
        policy_name => 'PHANCONG_HeapDep_Policy',
        object_schema => 'ADMIN'
    );
end;
SELECT *
FROM DBA_POLICIES
WHERE OBJECT_OWNER = 'ADMIN' AND OBJECT_NAME = 'PHANCONG';
create view DANGKY_VIEW AS SELECT * FROM DANGKY;
SELECT * FROM DBA_TAB_PRIVS WHERE GRANTEE = 'NV001';

select * from USER_ROLE_PRIVS;




--Testing privilege on PHANCONG table
INSERT INTO ADMIN.PHANCONG VALUES('NV102','HP103', 2, 2024, 'CQ');
INSERT INTO ADMIN.PHANCONG VALUES('NV102','HP003', 2, 2024, 'CQ');
DELETE FROM ADMIN.PHANCONG WHERE MAGV = 'NV102' AND NAM = 2024 AND HK = 2 AND MAHP = 'HP003';
DELETE FROM ADMIN.PHANCONG WHERE MAGV = 'NV102' AND NAM = 2024 AND HK = 2 AND MAHP = 'HP103';
SELECT * FROM ADMIN.phancong where NAM = 2024 AND HK = 2;
UPDATE ADMIN.PHANCONG SET MAGV = 'NV101' where NAM = 2024 AND HK = 2 AND MAHP = 'HP003' AND MAGV = 'NV102';
select * from ADMIN.PHANCONG where MAHP = 'HP003' AND MAGV = 'NV101' AND NAM = 2024 AND HK = 2;
select * from ADMIN.PHANCONG where MAGV = 'NV202';
SELECT * FROM ADMIN.PHANCONG WHERE MAGV = 'NV303';

--Testing privilege on NHANSU table
SELECT * FROM ADMIN.NHANSU;
INSERT INTO ADMIN.NHANSU VALUES('NV331', 'Nguy?n Th? M?ng', 'N?', TO_DATE('02/06/1993', 'DD/MM/YYYY'), 1100, '0919123329', 'GIANG VIEN', 'MMT');
DELETE FROM ADMIN.NHANSU WHERE MANV = 'NV331';
UPDATE ADMIN.NHANSU SET HOTEN = 'TRAN VAN A' WHERE MANV = 'NV331';


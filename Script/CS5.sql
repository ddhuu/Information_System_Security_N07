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
/
BEGIN
    usp_GrantPrivilegesToHeadDep();
END;


-- tao trigger xoa du lieu tren phan cong, truoc khi xoa nhan su
create or replace trigger UTR_DELETE_NHANSU
before delete on NHANSU
for each row
begin
    delete from PHANCONG 
    where MAGV = :OLD.MANV;
end;

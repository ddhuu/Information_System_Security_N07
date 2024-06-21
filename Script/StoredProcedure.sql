-- SP tao user moi
-- Nho alter session set "_ORACLE_SCRIPT"=true; truoc khi goi procedure nay
-- admin can co du quyen
-- grant create session to admin with admin option;
-- grant create user to admin;
CREATE OR REPLACE PROCEDURE USP_CREATE_USERS AS 
  CURSOR c1 IS SELECT MANV FROM ADMIN.NHANSU WHERE MANV NOT IN (SELECT USERNAME FROM ALL_USERS);
  CURSOR c2 IS SELECT MASV FROM ADMIN.SINHVIEN WHERE MASV NOT IN (SELECT USERNAME FROM ALL_USERS);
  v_username VARCHAR2(10);
BEGIN
  FOR i IN c1 LOOP
    v_username := i.MANV;
    EXECUTE IMMEDIATE 'CREATE USER ' || v_username || ' IDENTIFIED BY ' || v_username;
    EXECUTE IMMEDIATE 'GRANT CREATE SESSION TO ' || v_username;
  END LOOP;
  FOR i IN c2 LOOP
    v_username := i.MASV;
    EXECUTE IMMEDIATE 'CREATE USER ' || v_username || ' IDENTIFIED BY ' || v_username;
    EXECUTE IMMEDIATE 'GRANT CREATE SESSION TO ' || v_username;
  END LOOP;
END;

/


-- USP grant role to user
CREATE OR REPLACE PROCEDURE USP_GRANT_ROLES AS 
  CURSOR c1 IS SELECT MANV, VAITRO FROM ADMIN.NHANSU;
  CURSOR c2 IS SELECT MASV FROM ADMIN.SINHVIEN;
  v_role VARCHAR2(20);
  v_username VARCHAR2(10);
BEGIN
  FOR i IN c1 LOOP
    v_role := i.VAITRO;
    IF v_role = 'TRUONG DON VI' THEN
        EXECUTE IMMEDIATE 'GRANT RL_TRUONGDONVI TO ' || i.MANV;
    ELSIF v_role = 'NHAN VIEN' THEN
        EXECUTE IMMEDIATE 'GRANT RL_NHANVIENCOBAN TO ' || i.MANV;
    ELSIF v_role = 'GIAO VU' THEN
        EXECUTE IMMEDIATE 'GRANT RL_GIAOVU TO ' || i.MANV;
    ELSIF v_role = 'GIANG VIEN' THEN
        EXECUTE IMMEDIATE 'GRANT RL_GIANGVIEN TO ' || i.MANV;
    END IF;
  END LOOP;
  
  FOR i IN c2 LOOP
    v_username := i.MASV;
    EXECUTE IMMEDIATE 'GRANT RL_SINHVIEN TO ' || i.MASV;
  END LOOP;
END;

-- Create the context
CREATE CONTEXT user_ctx USING user_ctx_pkg;

-- Create the package
CREATE OR REPLACE PACKAGE user_ctx_pkg IS
  PROCEDURE set_role(p_role VARCHAR2);
END;
/

-- Create the package body
CREATE OR REPLACE PACKAGE BODY user_ctx_pkg IS
  PROCEDURE set_role(p_role VARCHAR2) IS
  BEGIN
    DBMS_SESSION.SET_CONTEXT('user_ctx', 'role', p_role);
  END;
END;
/

-- Create the trigger
CREATE OR REPLACE TRIGGER set_role_at_logon
AFTER LOGON ON DATABASE
DECLARE
  v_role VARCHAR2(20);
BEGIN
  SELECT VAITRO INTO v_role FROM ADMIN.NHANSU WHERE MANV = USER;
  IF v_role = 'TRUONG DON VI' THEN
    user_ctx_pkg.set_role('RL_TRUONGDONVI');
  ELSIF v_role = 'NHAN VIEN' THEN
    user_ctx_pkg.set_role('RL_NHANVIENCOBAN');
  ELSIF v_role = 'GIAO VU' THEN
    user_ctx_pkg.set_role('RL_GIAOVU');
  ELSIF v_role = 'GIANG VIEN' THEN
    user_ctx_pkg.set_role('RL_GIANGVIEN');
  ELSE
    user_ctx_pkg.set_role('RL_SINHVIEN');
  END IF;
EXCEPTION
  WHEN NO_DATA_FOUND THEN
    user_ctx_pkg.set_role('RL_SINHVIEN');
END;
/



-- Goi 2 thuc thi 2 procedure tren
alter session set "_ORACLE_SCRIPT"=true;
EXEC USP_CREATE_USERS;
EXEC USP_GRANT_ROLES;



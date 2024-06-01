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



-- Goi 2 thuc thi 2 procedure tren
alter session set "_ORACLE_SCRIPT"=true;
EXEC USP_CREATE_USERS;
EXEC USP_GRANT_ROLES;



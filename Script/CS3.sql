-- CS3: GIAO VU

-- Co quyen nhu "Nhan vien co ban"
grant RL_NHANVIENCOBAN to RL_GIAOVU;

-- xem, them cap nhat tren bang sinhvien, donvi, hocphan, khmo
GRANT INSERT, UPDATE ON SINHVIEN TO RL_GIAOVU;
GRANT INSERT, UPDATE ON DONVI TO RL_GIAOVU;
GRANT INSERT, UPDATE ON HOCPHAN TO RL_GIAOVU;
GRANT INSERT, UPDATE ON KHMO TO RL_GIAOVU;

-- tao trigger khi update tren 



-- xem du lieu tren toan bo quan he phancong
GRANT SELECT ON PHANCONG TO RL_GIAOVU;

-- Chi duoc sua nhung dong lien quan den hoc phan cua 'Van phong khoa'
CREATE OR REPLACE FUNCTION CS3_FUNCTION1
    (P_SCHEMA VARCHAR2, P_OBJ VARCHAR2)
RETURN VARCHAR2
AS
    v_str varchar2(200);
    v_role NHANSU.VAITRO%TYPE;
BEGIN
    if USER = 'ADMIN' then
        return '';
    end if;

    -- kiem tra user co role RL_GIAOVU hay khong
    select VAITRO into v_role from NHANSU where upper(MANV) = USER;
    if v_role = 'GIAO VU' then
        for i in (select MAHP from ADMIN.HOCPHAN where MADV = 'VPK') loop
            if v_str is not null then
                v_str := v_str || ''', ''';
            end if;
            v_str := v_str || i.MAHP;
        end loop;
        return 'MAHP IN (''' || v_str || ''')';
    end if;
    
    -- other role
    return '';
END;

/
BEGIN
	dbms_rls.add_policy(
    	OBJECT_SCHEMA => 'ADMIN',
    	OBJECT_NAME => 'PHANCONG',
    	POLICY_NAME => 'CS3_UPDATE_PHANCONG',
    	POLICY_FUNCTION => 'CS3_FUNCTION1',
    	STATEMENT_TYPES => 'UPDATE',
    	UPDATE_CHECK => TRUE
	);
END;
/
GRANT UPDATE ON PHANCONG TO RL_GIAOVU;

-- Xoa hoac them moi vao quan he DANGKY khi con thoi han
CREATE OR REPLACE FUNCTION CS3_FUNCTION2
    (P_SCHEMA VARCHAR2, P_OBJ VARCHAR2)
RETURN VARCHAR2
AS
    v_str varchar2(200);
    v_role NHANSU.VAITRO%TYPE;
    v_day integer;
    v_month integer;
    v_year integer;
BEGIN
    if USER = 'ADMIN' then
        return '';
    end if;

    -- kiem tra user co role RL_GIAOVU hay khong
    select VAITRO into v_role from NHANSU where upper(MANV) = USER;
    if v_role = 'GIAO VU' then
        SELECT EXTRACT(DAY FROM SYSDATE) into v_day FROM dual;
        SELECT EXTRACT(MONTH FROM SYSDATE) into v_month FROM dual;
        SELECT EXTRACT(YEAR FROM SYSDATE) into v_year FROM dual;
        if(v_day <= 15) then
            if (v_month = 1) then
                return 'HK = 1 AND NAM = ' || v_year;
            elsif (v_month = 6) then
                return 'HK = 2 AND NAM = ' || v_year;
            elsif (v_month = 9) then
                return 'HK = 3 AND NAM = ' || v_year;
            end if;
        end if;
        return '1 = 0';
    end if;
    -- other role
    return '';
END;
/
BEGIN
	dbms_rls.add_policy(
    	OBJECT_SCHEMA => 'ADMIN',
    	OBJECT_NAME => 'DANGKY',
    	POLICY_NAME => 'CS3_INSERT_DELETE_DANGKY',
    	POLICY_FUNCTION => 'CS3_FUNCTION2',
    	STATEMENT_TYPES => 'INSERT, DELETE',
    	UPDATE_CHECK => TRUE
	);
END;
/
GRANT SELECT, INSERT, DELETE ON DANGKY TO RL_GIAOVU;







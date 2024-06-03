-- CS4: Truong don vi

-- co vai tro nhu 'Giang vien'
grant RL_GIANGVIEN to RL_TRUONGDONVI;


-- Them, xoa, cap nhat phan cong cua cac hoc phan thuoc don vi minh lam truong
CREATE OR REPLACE FUNCTION CS4_FUNCTION1
    (P_SCHEMA VARCHAR2, P_OBJ VARCHAR2)
RETURN VARCHAR2
AS
    v_str varchar2(1000);
    v_donvi varchar2(10);
BEGIN
    if USER = 'ADMIN' then
        return '';
    end if;
    
    -- tim don vi ma user lam truong don vi
    begin
        select madv into v_donvi from DONVI where UPPER(TRGDV) = USER;
    exception
        when NO_DATA_FOUND then
            v_donvi := '';
    end;
    
    -- user la truong don vi
    if length(v_donvi) > 0 then
        for i in (select MAHP from HOCPHAN where MADV = v_donvi) loop
            if v_str is not null then
                v_str := v_str || ''', ''';
            end if;
            v_str := v_str || i.MAHP;
        end loop;
        return 'MAHP IN (''' || v_str || ''')';
    end if;
    
    -- user khong phai truong don vi
    return '';
END;

/
BEGIN
	dbms_rls.add_policy(
    	OBJECT_SCHEMA => 'ADMIN',
    	OBJECT_NAME => 'PHANCONG',
    	POLICY_NAME => 'CS4_INSERT_DELETE_UPDATE_PHANCONG',
    	POLICY_FUNCTION => 'CS4_FUNCTION1',
    	STATEMENT_TYPES => 'INSERT, DELETE, UPDATE',
    	UPDATE_CHECK => TRUE
	);
END;
/
GRANT INSERT, DELETE, UPDATE ON PHANCONG TO RL_TRUONGDONVI;


-- tao trigger xoa du lieu tren dang ky, truoc khi xoa phan con
create or replace trigger UTR_DELETE_PHANCONG
before delete on PHANCONG
for each row
begin
    delete from DANGKY 
    where MAGV = :OLD.MAGV 
        AND MAHP = :OLD.MAHP
        AND HK = :OLD.HK
        AND NAM = :OLD.NAM
        AND MACT = :OLD.MACT;
end;


-- xem phan cong giang day cua cac giang vien thuoc don vi minh lam truong
CREATE OR REPLACE FUNCTION CS4_FUNCTION2
    (P_SCHEMA VARCHAR2, P_OBJ VARCHAR2)
RETURN VARCHAR2
AS
    v_str1 varchar2(1000);
    v_str2 varchar2(1000);
    v_donvi varchar2(10);
BEGIN
    if USER = 'ADMIN' then
        return '';
    end if;
    
    -- tim don vi ma user lam truong don vi
    begin
        select madv into v_donvi from DONVI where UPPER(TRGDV) = USER;
    exception
        when NO_DATA_FOUND then
            v_donvi := '';
    end;
    
    -- user la truong don vi
    if length(v_donvi) > 0 then
        -- NHUNG NHAN VIEN THUOC DON VI MINH LAM TRUONG
        for i in (select MANV from NHANSU where MADV = v_donvi) loop
            if v_str1 is not null then
                v_str1 := v_str1 || ''', ''';
            end if;
            v_str1 := v_str1 || i.MANV;
        end loop;
        -- NHUNG HOC PHAN THUOC DON VI MINH LAM TRUONG
        for i in (select MAHP from HOCPHAN where MADV = v_donvi) loop
            if v_str2 is not null then
                v_str2 := v_str2 || ''', ''';
            end if;
            v_str2 := v_str2 || i.MAHP;
        end loop;
        return '(MAGV IN (''' || v_str1 || ''') OR MAHP IN (''' || v_str2 || '''))';
    end if;
    
    -- user khong phai truong don vi
    return '';
END;

/
BEGIN
	dbms_rls.add_policy(
    	OBJECT_SCHEMA => 'ADMIN',
    	OBJECT_NAME => 'PHANCONG',
    	POLICY_NAME => 'CS4_SELECT_PHANCONG',
    	POLICY_FUNCTION => 'CS4_FUNCTION2',
    	STATEMENT_TYPES => 'SELECT'
	);
END;
/
GRANT SELECT ON PHANCONG TO RL_TRUONGDONVI;



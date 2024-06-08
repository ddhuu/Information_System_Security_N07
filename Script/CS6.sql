-- cs4: Sinh vien

-- xem va sua (dien thoai, dia chi) cua ca nhan
CREATE OR REPLACE FUNCTION CS6_FUNCTION1(p_schema VARCHAR2, p_object VARCHAR2)
    RETURN VARCHAR2
    AS
        v_username VARCHAR2(10);
        v_isnhansu NUMBER;
    BEGIN
        v_username := SYS_CONTEXT('USERENV', 'SESSION_USER');
        if v_username = 'ADMIN' then
            return '';
        end if;

        -- check co phai la nhan vien trong bang nhan su hay khong hay khong
        select count(*) into v_isnhansu from admin.NHANSU where UPPER(MANV) = v_username;
        
        if v_isnhansu > 0 then
            return '';
        end if;

        return 'MASV = ''' || v_username || '''';
    END;
/
BEGIN
    dbms_rls.add_policy(
    	OBJECT_SCHEMA => 'ADMIN',
    	OBJECT_NAME => 'SINHVIEN',
    	POLICY_NAME => 'CS6_SELECT_UPDATE_SINHVIEN',
    	POLICY_FUNCTION => 'CS6_FUNCTION1',
    	STATEMENT_TYPES => 'SELECT, UPDATE',
        UPDATE_CHECK => TRUE
	);
END;
/
--Grant select and update to SINHVIEN
GRANT SELECT ON ADMIN.SINHVIEN TO RL_SINHVIEN;
GRANT UPDATE(DCHI, DT) ON ADMIN.SINHVIEN TO RL_SINHVIEN;



-- Xem tat ca hoc phan cua chuong trinh ma sv dang theo hoc
GRANT SELECT ON ADMIN.HOCPHAN TO RL_SINHVIEN;

-- Xem KHMO cua chuong trinh ma sinh vien dang theo hoc
CREATE OR REPLACE FUNCTION CS6_FUNCTION2(p_schema VARCHAR2, p_object VARCHAR2)
RETURN VARCHAR2
AS
    v_username VARCHAR2(10);
    v_program VARCHAR2(10);
    v_isnhansu NUMBER;
    BEGIN
        v_username := SYS_CONTEXT('USERENV', 'SESSION_USER');
        IF(v_username = 'ADMIN') THEN
            RETURN '';
        END IF;
        
         -- check co phai la nhan vien trong bang nhan su hay khong hay khong
        select count(*) into v_isnhansu from admin.NHANSU where UPPER(MANV) = v_username;
        IF v_isnhansu > 0 THEN
            return '';
        END IF;
        
        -- khong can where masv=v_ussername tai vi da them policy vao bang SINHVIEN o truoc do roi
        SELECT MACT INTO v_program FROM SINHVIEN;
        return 'MACT = ''' || v_program || '''';
    END;
/
BEGIN
    DBMS_RLS.ADD_POLICY(
        object_schema   => 'ADMIN',
        object_name     => 'KHMO',
        policy_name     => 'CS6_SELECT_KHMO',
        policy_function => 'CS6_FUNCTION2',
        statement_types => 'SELECT'
    );
END;
/
GRANT SELECT ON KHMO TO RL_SINHVIEN;


--Xem, them, xoa tren quan he dang ky
-- xem thong tin ca nhan
CREATE OR REPLACE FUNCTION CS6_FUNCTION3(p_schema VARCHAR2, p_object VARCHAR2)
RETURN VARCHAR2
AS
    v_username VARCHAR2(10);
    v_isnhansu NUMBER;
    BEGIN
        v_username := SYS_CONTEXT('USERENV', 'SESSION_USER');
        if v_username = 'ADMIN' then
            return '';
        end if;

        -- check co phai la nhan vien trong bang nhan su hay khong hay khong
        select count(*) into v_isnhansu from admin.NHANSU where UPPER(MANV) = v_username;
        
        if v_isnhansu > 0 then
            return '';
        end if;

        return 'MASV = ''' || v_username || '''';
    END;
/
BEGIN
    DBMS_RLS.ADD_POLICY(
        object_schema   => 'ADMIN',
        object_name     => 'DANGKY',
        policy_name     => 'CS6_SELECT_DANGKY',
        policy_function => 'CS6_FUNCTION3',
        statement_types => 'SELECT'
    );
END;
/
GRANT SELECT ON ADMIN.DANGKY TO RL_SINHVIEN;




-- Them xoa, trong thoi gian dang ky (ko duoc sua diem)
CREATE OR REPLACE FUNCTION CS6_FUNCTION4(p_schema VARCHAR2, p_object VARCHAR2)
RETURN VARCHAR2
AS
    v_username VARCHAR2(10);
    v_sem_start_date DATE;
    v_current_date DATE := SYSDATE;
    v_isnhansu NUMBER;
    v_current_semester NUMBER;
    v_year NUMBER := EXTRACT(YEAR FROM SYSDATE);
    BEGIN
        v_username := SYS_CONTEXT('USERENV', 'SESSION_USER');
        if v_username = 'ADMIN' then
            return '';
        end if;
        
        -- check co phai la nhan vien trong bang nhan su hay khong hay khong
        select count(*) into v_isnhansu from admin.NHANSU where UPPER(MANV) = v_username;
        if v_isnhansu > 0 then
            return '';
        end if;
        
        -- lay semester hien tai
        SELECT CASE
            WHEN EXTRACT(MONTH FROM v_current_date) BETWEEN 1 AND 4 THEN 1
            WHEN EXTRACT(MONTH FROM v_current_date) BETWEEN 5 AND 8 THEN 2
            ELSE 3
            END INTO v_current_semester
        FROM DUAL;
        
        -- NOTE: CO THE THAY DOI NGAY BAT DAU HOC CUA MOI HOC KY O DAY
        --Get semester start date base on current date
        SELECT CASE
            WHEN EXTRACT(MONTH FROM v_current_date) BETWEEN 1 AND 4 THEN TO_DATE('01-JAN-' || TO_CHAR(v_current_date, 'YYYY'), 'DD-MON-YYYY')
            WHEN EXTRACT(MONTH FROM v_current_date) BETWEEN 5 AND 8 THEN TO_DATE('01-JUN-' || TO_CHAR(v_current_date, 'YYYY'), 'DD-MON-YYYY')
            ELSE TO_DATE('01-SEP-' || TO_CHAR(v_current_date, 'YYYY'), 'DD-MON-YYYY')
            END INTO v_sem_start_date
        FROM DUAL;
        
        -- NOTE: CO THE THAY DOI THOI HAN DIEU CHINH HOC PHAN O DAY
        IF current_date BETWEEN v_sem_start_date AND (v_sem_start_date + 14) THEN
            -- Student can only delete record in the current semester
            RETURN 'MASV = ''' || v_username || '''' || ' AND NAM = ' || v_year || ' AND HK = ' || v_current_semester;
        END IF;
        
        RETURN '1 = 0';
    END;
/
BEGIN
    DBMS_RLS.ADD_POLICY(
        object_schema   => 'ADMIN',
        object_name     => 'DANGKY',
        policy_name     => 'CS6_DELETE_DANGKY',
        policy_function => 'CS6_FUNCTION4',
        statement_types => 'DELETE'
    );
END;
/
GRANT DELETE ON ADMIN.DANGKY TO RL_SINHVIEN;

    
CREATE OR REPLACE FUNCTION CS6_FUNCTION5(p_schema VARCHAR2, p_object VARCHAR2)
RETURN VARCHAR2
AS
    v_username VARCHAR2(10);
    v_sem_start_date DATE;
    v_current_date DATE := SYSDATE;
    v_isnhansu NUMBER;
    v_current_semester INT;
    v_year INT := EXTRACT(YEAR FROM SYSDATE);
    v_program VARCHAR2(10);
    BEGIN
        v_username := SYS_CONTEXT('USERENV', 'SESSION_USER');
        IF(v_username = 'ADMIN') THEN
            RETURN '';
        END IF;
        
        --Check xem co phai la nhan vien trong bang nhan su hay khong
        select count(*) into v_isnhansu from admin.NHANSU where UPPER(MANV) = v_username;
        if v_isnhansu > 0 then
            return '';
        end if;
        
        
        -- la sinh vien
        SELECT CASE
            WHEN EXTRACT(MONTH FROM v_current_date) BETWEEN 1 AND 4 THEN 1
            WHEN EXTRACT(MONTH FROM v_current_date) BETWEEN 5 AND 8 THEN 2
            ELSE 3
            END INTO v_current_semester
        FROM DUAL;
        --NOTE: CO THE THAY DOI NGAY BAT DAU HOC KY O DAY
        --Get semester start date base on current date
        SELECT CASE
            WHEN EXTRACT(MONTH FROM v_current_date) BETWEEN 1 AND 4 THEN TO_DATE('01-JAN-' || TO_CHAR(v_current_date, 'YYYY'), 'DD-MON-YYYY')
            WHEN EXTRACT(MONTH FROM v_current_date) BETWEEN 5 AND 8 THEN TO_DATE('01-JUN-' || TO_CHAR(v_current_date, 'YYYY'), 'DD-MON-YYYY')
            ELSE TO_DATE('01-SEP-' || TO_CHAR(v_current_date, 'YYYY'), 'DD-MON-YYYY')
            END INTO v_sem_start_date
        FROM DUAL;
        -- bang sinhvien da co policy CS6_SELECT_UPDATE_SINHVIEN nen khong can where masv=v_username
        SELECT MACT INTO v_program FROM SINHVIEN;
        --NOTE: CO THE THAY DOI THOI HAN DIEU CHINH DANG KY HOC PHAN O DAY
        IF current_date BETWEEN v_sem_start_date AND (v_sem_start_date + 14) THEN
            -- Student can only insert record in the current semester
            RETURN 'MASV = ''' || v_username || '''' || ' AND NAM = ' || v_year || ' AND HK = ' || v_current_semester || ' AND MACT = ''' || v_program || ''''
                || ' AND DIEMTH is null AND DIEMQT is null AND DIEMCK is null AND DIEMTK is null';
        END IF;

        RETURN '1 = 0';
    END;
/
BEGIN
    DBMS_RLS.ADD_POLICY(
        object_schema   => 'ADMIN',
        object_name     => 'DANGKY',
        policy_name     => 'CS6_INSERT_DANGKY',
        policy_function => 'CS6_FUNCTION5',
        statement_types => 'INSERT',
        update_check => TRUE
    );
END;
/
GRANT INSERT ON ADMIN.DANGKY TO RL_SINHVIEN;


-- CS#2: Nguoi dung co VAITRO la Giang vien co quyen truy cap du lieu:
-- - Nhu mot nguoi dung co vai tro Nhan vien co ban(xem mo ta CS#1).
-- - Xem du lieu phan cong giang day lien quan den ban than minh (PHANCONG).
-- - Xem du lieu tren quan he DANGKY lien quan den cac lop hoc phan ma giang vien duoc phan cong giang day.
-- - Cap nhat du lieu tai cac truong lien quan diem so (trong quan he DANGKY) cua cac sinh vien co tham gia lop hoc phan ma giang vien do duoc phan cong giang day. Cac
-- truong lien quan diem so bao gom: DIEMTH, DIEMQT, DIEMCK, DIEMTK.

-- - Nhu mot nguoi dung co vai tro Nhan vien co ban (xem mo ta CS#1)
grant RL_NHANVIENCOBAN TO RL_GIANGVIEN;

-- Xem thong tin phan cong cua chinh minh
CREATE OR REPLACE VIEW UV_CANHAN_PHANCONG AS
SELECT * FROM ADMIN.PHANCONG WHERE MAGV = SYS_CONTEXT('USERENV', 'SESSION_USER');

GRANT SELECT ON ADMIN.UV_CANHAN_PHANCONG TO RL_GIANGVIEN;


-- Xem thong tin DANG KY cua chinh minh, cap nhat cac truong diem so
CREATE OR REPLACE VIEW UV_CANHAN_DANGKY AS
SELECT * FROM ADMIN.DANGKY WHERE UPPER(MAGV) =  SYS_CONTEXT('USERENV', 'SESSION_USER');

CREATE OR REPLACE TRIGGER UTR_UPDATE_DIEM_DANGKY
INSTEAD OF UPDATE 
ON UV_CANHAN_DANGKY
FOR EACH ROW
BEGIN
    UPDATE DANGKY 
    SET DIEMQT = :NEW.DIEMQT, 
        DIEMTH = :NEW.DIEMTH,
        DIEMCK = :NEW.DIEMCK,
        DIEMTK = :NEW.DIEMTK
    WHERE UPPER(MAGV) = USER 
        AND MASV = :OLD.MASV
        AND MAHP = :OLD.MAHP
        AND NAM = :OLD.NAM
        AND HK = :OLD.HK
        AND MACT = :OLD.MACT;
END;
/

GRANT SELECT, UPDATE(DIEMTH, DIEMQT, DIEMCK, DIEMTK) ON ADMIN.UV_CANHAN_DANGKY TO RL_GIANGVIEN;

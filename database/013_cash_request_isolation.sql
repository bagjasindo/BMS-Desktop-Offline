BEGIN;
COMMENT ON TABLE finance_cash_requests IS 'Dokumen pengajuan administratif berdiri sendiri; tidak boleh otomatis menjadi realisasi transaksi.';
COMMENT ON TABLE finance_cash_request_items IS 'Rincian pengajuan administratif; tidak terhubung otomatis ke BOP, kasbon, arus kas, hutang, RHPP, laba/rugi.';
CREATE OR REPLACE FUNCTION validate_cash_request_group() RETURNS trigger LANGUAGE plpgsql AS $$ BEGIN IF NEW.group_type='KANDANG' AND NEW.barn_id IS NULL THEN RAISE EXCEPTION 'Kelompok KANDANG wajib memilih kandang'; END IF; IF NEW.group_type<>'KANDANG' THEN NEW.barn_id=NULL; END IF; RETURN NEW; END $$;
DROP TRIGGER IF EXISTS trg_cash_request_group ON finance_cash_request_items; CREATE TRIGGER trg_cash_request_group BEFORE INSERT OR UPDATE ON finance_cash_request_items FOR EACH ROW EXECUTE FUNCTION validate_cash_request_group();
COMMIT;

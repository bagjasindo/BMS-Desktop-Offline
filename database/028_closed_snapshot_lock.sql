BEGIN;
CREATE OR REPLACE FUNCTION reject_rhpp_snapshot_mutation() RETURNS trigger LANGUAGE plpgsql AS $$ BEGIN RAISE EXCEPTION 'RHPP snapshot historis tidak boleh diubah/dihapus'; END $$;
DROP TRIGGER IF EXISTS trg_rhpp_snapshot_lock ON rhpp_snapshots;
CREATE TRIGGER trg_rhpp_snapshot_lock BEFORE UPDATE OR DELETE ON rhpp_snapshots FOR EACH ROW EXECUTE FUNCTION reject_rhpp_snapshot_mutation();
DROP TRIGGER IF EXISTS trg_cycle_closing_lock ON cycle_closings;
CREATE TRIGGER trg_cycle_closing_lock BEFORE UPDATE OR DELETE ON cycle_closings FOR EACH ROW EXECUTE FUNCTION reject_rhpp_snapshot_mutation();
COMMIT;

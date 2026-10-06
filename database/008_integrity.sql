BEGIN;
ALTER TABLE daily_recordings ADD CONSTRAINT daily_recordings_nonnegative CHECK(mortality_qty>=0 AND cull_qty>=0 AND feed_used_kg>=0);
CREATE OR REPLACE FUNCTION prevent_closed_cycle_mutation() RETURNS trigger LANGUAGE plpgsql AS $$
DECLARE cid uuid;
BEGIN
 cid:=COALESCE(NEW.cycle_id,OLD.cycle_id);
 IF EXISTS(SELECT 1 FROM production_cycles WHERE id=cid AND status='CLOSED') THEN RAISE EXCEPTION 'Cycle CLOSED tidak boleh diubah'; END IF;
 RETURN COALESCE(NEW,OLD);
END $$;
DROP TRIGGER IF EXISTS trg_daily_closed ON daily_recordings;
CREATE TRIGGER trg_daily_closed BEFORE INSERT OR UPDATE OR DELETE ON daily_recordings FOR EACH ROW EXECUTE FUNCTION prevent_closed_cycle_mutation();
DROP TRIGGER IF EXISTS trg_harvest_closed ON harvests;
CREATE TRIGGER trg_harvest_closed BEFORE INSERT OR UPDATE OR DELETE ON harvests FOR EACH ROW EXECUTE FUNCTION prevent_closed_cycle_mutation();
COMMIT;

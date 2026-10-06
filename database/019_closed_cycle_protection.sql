BEGIN;
CREATE OR REPLACE FUNCTION prevent_closed_cycle_mutation() RETURNS trigger LANGUAGE plpgsql AS $$ DECLARE cid uuid; BEGIN cid=coalesce(NEW.cycle_id,OLD.cycle_id);IF EXISTS(SELECT 1 FROM production_cycles WHERE id=cid AND status='CLOSED') THEN RAISE EXCEPTION 'Cycle CLOSED tidak boleh diubah';END IF;RETURN coalesce(NEW,OLD);END $$;
DO $$ DECLARE t text; BEGIN FOREACH t IN ARRAY ARRAY['daily_recordings','harvests','body_weight_samples','medicine_usage','barn_operational_costs'] LOOP EXECUTE format('DROP TRIGGER IF EXISTS trg_closed_guard ON %I',t);EXECUTE format('CREATE TRIGGER trg_closed_guard BEFORE INSERT OR UPDATE OR DELETE ON %I FOR EACH ROW EXECUTE FUNCTION prevent_closed_cycle_mutation()',t);END LOOP;END $$;
COMMIT;

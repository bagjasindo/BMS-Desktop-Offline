BEGIN;
DO $$ DECLARE t text; BEGIN FOREACH t IN ARRAY ARRAY['feed_usage','abk_work_entries','extra_sapronak'] LOOP EXECUTE format('DROP TRIGGER IF EXISTS trg_closed_guard ON %I',t);EXECUTE format('CREATE TRIGGER trg_closed_guard BEFORE INSERT OR UPDATE OR DELETE ON %I FOR EACH ROW EXECUTE FUNCTION prevent_closed_cycle_mutation()',t);END LOOP;END $$;
COMMIT;

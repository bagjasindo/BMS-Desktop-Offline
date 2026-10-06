BEGIN;
DROP TRIGGER IF EXISTS trg_closed_guard ON additional_meat_purchases;
CREATE TRIGGER trg_closed_guard BEFORE INSERT OR UPDATE OR DELETE ON additional_meat_purchases FOR EACH ROW EXECUTE FUNCTION prevent_closed_cycle_mutation();
COMMIT;

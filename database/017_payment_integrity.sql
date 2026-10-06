BEGIN;
CREATE OR REPLACE FUNCTION check_payment_allocation() RETURNS trigger LANGUAGE plpgsql AS $$
DECLARE total_alloc numeric(18,2); pay_total numeric(18,2);
BEGIN
 SELECT amount INTO pay_total FROM payments WHERE id=NEW.payment_id FOR UPDATE;
 SELECT coalesce(sum(amount),0) INTO total_alloc FROM payment_allocations WHERE payment_id=NEW.payment_id AND id<>NEW.id;
 IF total_alloc+NEW.amount>pay_total THEN RAISE EXCEPTION 'Alokasi pembayaran melebihi nilai pembayaran'; END IF;
 RETURN NEW;
END $$;
DROP TRIGGER IF EXISTS trg_payment_allocation ON payment_allocations;
CREATE TRIGGER trg_payment_allocation BEFORE INSERT OR UPDATE ON payment_allocations FOR EACH ROW EXECUTE FUNCTION check_payment_allocation();
COMMIT;

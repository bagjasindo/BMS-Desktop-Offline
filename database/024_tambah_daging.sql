BEGIN;
CREATE TABLE IF NOT EXISTS additional_meat_purchases(id uuid PRIMARY KEY,cycle_id uuid NOT NULL REFERENCES production_cycles(id),supplier_id uuid REFERENCES business_partners(id),purchase_date date NOT NULL,bird_qty integer NOT NULL CHECK(bird_qty>=0),weight_kg numeric(18,3) NOT NULL CHECK(weight_kg>=0),actual_unit_price numeric(18,2) NOT NULL CHECK(actual_unit_price>=0),actual_amount numeric(18,2) GENERATED ALWAYS AS (weight_kg*actual_unit_price) STORED,notes text NOT NULL DEFAULT '',created_at timestamptz NOT NULL DEFAULT now());
CREATE INDEX IF NOT EXISTS additional_meat_cycle_idx ON additional_meat_purchases(cycle_id,purchase_date);
COMMIT;

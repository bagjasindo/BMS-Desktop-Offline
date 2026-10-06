BEGIN;

CREATE TABLE IF NOT EXISTS production_cycles (
 id uuid PRIMARY KEY,
 barn_id uuid NOT NULL REFERENCES barns(id),
 cycle_code varchar(60) NOT NULL UNIQUE,
 cycle_type varchar(20) NOT NULL CHECK(cycle_type IN ('MITRA','MANDIRI')),
 status varchar(20) NOT NULL DEFAULT 'DRAFT' CHECK(status IN ('DRAFT','ACTIVE','CLOSED','CANCELLED')),
 chick_in_date date,
 closed_at timestamptz,
 created_at timestamptz NOT NULL DEFAULT now(),
 updated_at timestamptz NOT NULL DEFAULT now()
);
CREATE UNIQUE INDEX IF NOT EXISTS one_active_cycle_per_barn ON production_cycles(barn_id) WHERE status='ACTIVE';

CREATE TABLE IF NOT EXISTS contracts (
 id uuid PRIMARY KEY,
 cycle_id uuid NOT NULL UNIQUE REFERENCES production_cycles(id),
 contract_number varchar(80) NOT NULL UNIQUE,
 partner_id uuid REFERENCES business_partners(id),
 effective_date date NOT NULL,
 status varchar(20) NOT NULL DEFAULT 'ACTIVE' CHECK(status IN ('ACTIVE','CLOSED','CANCELLED')),
 created_at timestamptz NOT NULL DEFAULT now()
);

CREATE TABLE IF NOT EXISTS contract_item_prices (
 id uuid PRIMARY KEY,
 contract_id uuid NOT NULL REFERENCES contracts(id) ON DELETE CASCADE,
 item_id uuid NOT NULL REFERENCES item_master(id),
 unit_price numeric(18,2) NOT NULL CHECK(unit_price>=0),
 UNIQUE(contract_id,item_id)
);

CREATE TABLE IF NOT EXISTS contract_bw_prices (
 id uuid PRIMARY KEY,
 contract_id uuid NOT NULL REFERENCES contracts(id) ON DELETE CASCADE,
 min_bw numeric(8,3) NOT NULL,
 max_bw numeric(8,3) NOT NULL,
 price_per_kg numeric(18,2) NOT NULL CHECK(price_per_kg>=0),
 CHECK(max_bw>=min_bw)
);

COMMIT;

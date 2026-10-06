BEGIN;
ALTER TABLE rhpp_snapshots ADD COLUMN IF NOT EXISTS contract_revenue numeric(18,2);
ALTER TABLE rhpp_snapshots ADD COLUMN IF NOT EXISTS extra_cost numeric(18,2);
ALTER TABLE rhpp_snapshots ADD COLUMN IF NOT EXISTS operational_profit numeric(18,2);
ALTER TABLE rhpp_snapshots ADD COLUMN IF NOT EXISTS mortality_pct numeric(12,4);
ALTER TABLE rhpp_snapshots ADD COLUMN IF NOT EXISTS avg_bw_kg numeric(12,4);
ALTER TABLE rhpp_snapshots ADD COLUMN IF NOT EXISTS fcr numeric(12,4);
ALTER TABLE rhpp_snapshots ADD COLUMN IF NOT EXISTS ip numeric(12,4);
COMMIT;

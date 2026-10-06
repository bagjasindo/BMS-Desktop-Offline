BEGIN;
ALTER TABLE business_partners ADD COLUMN IF NOT EXISTS supplier_scope varchar(20) CHECK(supplier_scope IN('MITRA','MANDIRI','UMUM'));
CREATE INDEX IF NOT EXISTS business_partners_supplier_scope_idx ON business_partners(partner_type,supplier_scope);
COMMIT;

BEGIN;
CREATE TABLE IF NOT EXISTS finance_cash_request_groups(id uuid PRIMARY KEY,request_id uuid NOT NULL REFERENCES finance_cash_requests(id) ON DELETE CASCADE,group_type varchar(30) NOT NULL CHECK(group_type IN('KANDANG','BOP_UMUM','BOP_KANTOR','PROYEK_LAINNYA')),barn_id uuid REFERENCES barns(id),title varchar(160) NOT NULL DEFAULT '',sort_order integer NOT NULL DEFAULT 0,CHECK((group_type='KANDANG' AND barn_id IS NOT NULL) OR (group_type<>'KANDANG' AND barn_id IS NULL)));
ALTER TABLE finance_cash_request_items ADD COLUMN IF NOT EXISTS group_id uuid REFERENCES finance_cash_request_groups(id) ON DELETE CASCADE;
CREATE INDEX IF NOT EXISTS cash_request_groups_request_idx ON finance_cash_request_groups(request_id,sort_order);
CREATE INDEX IF NOT EXISTS cash_request_items_group_idx ON finance_cash_request_items(group_id);
COMMIT;

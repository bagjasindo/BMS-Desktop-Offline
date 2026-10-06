BEGIN;
CREATE OR REPLACE FUNCTION reject_audit_mutation() RETURNS trigger LANGUAGE plpgsql AS $$ BEGIN RAISE EXCEPTION 'audit_log bersifat append-only'; END $$;
DROP TRIGGER IF EXISTS trg_audit_no_update ON audit_log; CREATE TRIGGER trg_audit_no_update BEFORE UPDATE OR DELETE ON audit_log FOR EACH ROW EXECUTE FUNCTION reject_audit_mutation();
CREATE TABLE IF NOT EXISTS app_settings(setting_key varchar(100) PRIMARY KEY,setting_value jsonb NOT NULL,updated_at timestamptz NOT NULL DEFAULT now());
INSERT INTO app_settings(setting_key,setting_value) VALUES('system_identity','{"app":"BMS Desktop Offline","mode":"LOCAL_LAN"}'::jsonb) ON CONFLICT(setting_key) DO NOTHING;
COMMIT;

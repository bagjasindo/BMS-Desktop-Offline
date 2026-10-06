BEGIN;
CREATE TABLE IF NOT EXISTS app_users (
 id uuid PRIMARY KEY, username varchar(80) NOT NULL UNIQUE, display_name varchar(160) NOT NULL,
 password_hash text NOT NULL, role varchar(30) NOT NULL CHECK (role IN ('ADMIN','PRODUKSI','PPL','LOGISTIK','MARKETING','KEUANGAN','OWNER','PELANGGAN')),
 is_active boolean NOT NULL DEFAULT true, created_at timestamptz NOT NULL DEFAULT now(), updated_at timestamptz NOT NULL DEFAULT now()
);
CREATE TABLE IF NOT EXISTS audit_log (
 id bigserial PRIMARY KEY, user_id uuid NULL REFERENCES app_users(id), event_type varchar(80) NOT NULL,
 entity_type varchar(80), entity_id text, details jsonb NOT NULL DEFAULT '{}'::jsonb, created_at timestamptz NOT NULL DEFAULT now()
);
CREATE INDEX IF NOT EXISTS audit_log_created_at_idx ON audit_log(created_at DESC);
CREATE INDEX IF NOT EXISTS audit_log_user_id_idx ON audit_log(user_id);
CREATE TABLE IF NOT EXISTS company_profile (
 id smallint PRIMARY KEY DEFAULT 1 CHECK (id=1), legal_name text NOT NULL DEFAULT '', trade_name text NOT NULL DEFAULT '',
 address text NOT NULL DEFAULT '', phone text NOT NULL DEFAULT '', email text NOT NULL DEFAULT '', tax_id text NOT NULL DEFAULT '', updated_at timestamptz NOT NULL DEFAULT now()
);
INSERT INTO company_profile(id) VALUES (1) ON CONFLICT (id) DO NOTHING;
COMMIT;

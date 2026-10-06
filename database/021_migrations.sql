BEGIN;
CREATE TABLE IF NOT EXISTS schema_migrations(version varchar(80) PRIMARY KEY,applied_at timestamptz NOT NULL DEFAULT now());
COMMIT;

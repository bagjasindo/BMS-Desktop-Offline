BEGIN;
CREATE UNIQUE INDEX IF NOT EXISTS app_users_username_ci_unique ON app_users(lower(username));
COMMIT;

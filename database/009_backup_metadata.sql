BEGIN;
CREATE TABLE IF NOT EXISTS backup_history(id uuid PRIMARY KEY,file_name text NOT NULL,backup_type varchar(30) NOT NULL CHECK(backup_type IN('FULL','ROLE_PACKAGE')),role_name varchar(30),created_by uuid REFERENCES app_users(id),created_at timestamptz NOT NULL DEFAULT now(),sha256 varchar(64) NOT NULL,status varchar(20) NOT NULL CHECK(status IN('CREATED','VERIFIED','RESTORED','FAILED')));
CREATE TABLE IF NOT EXISTS restore_history(id uuid PRIMARY KEY,backup_id uuid REFERENCES backup_history(id),restored_by uuid REFERENCES app_users(id),restored_at timestamptz NOT NULL DEFAULT now(),status varchar(20) NOT NULL CHECK(status IN('SUCCESS','FAILED')),details text NOT NULL DEFAULT '');
COMMIT;

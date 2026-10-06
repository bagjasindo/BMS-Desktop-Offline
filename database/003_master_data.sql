BEGIN;

CREATE TABLE IF NOT EXISTS barns (
 id uuid PRIMARY KEY,
 code varchar(40) NOT NULL UNIQUE,
 name varchar(160) NOT NULL,
 capacity integer NOT NULL CHECK(capacity > 0),
 status varchar(20) NOT NULL DEFAULT 'AVAILABLE' CHECK(status IN ('AVAILABLE','ACTIVE','CLOSED','INACTIVE')),
 is_active boolean NOT NULL DEFAULT true,
 created_at timestamptz NOT NULL DEFAULT now(),
 updated_at timestamptz NOT NULL DEFAULT now()
);

CREATE TABLE IF NOT EXISTS business_partners (
 id uuid PRIMARY KEY,
 partner_type varchar(20) NOT NULL CHECK(partner_type IN ('CUSTOMER','SUPPLIER')),
 code varchar(40) NOT NULL,
 name varchar(200) NOT NULL,
 phone varchar(60) NOT NULL DEFAULT '',
 address text NOT NULL DEFAULT '',
 is_active boolean NOT NULL DEFAULT true,
 created_at timestamptz NOT NULL DEFAULT now(),
 updated_at timestamptz NOT NULL DEFAULT now(),
 UNIQUE(partner_type,code)
);

CREATE TABLE IF NOT EXISTS item_master (
 id uuid PRIMARY KEY,
 code varchar(50) NOT NULL UNIQUE,
 name varchar(200) NOT NULL,
 category varchar(30) NOT NULL CHECK(category IN ('PAKAN','DOC','OVK_OBAT','LAINNYA')),
 unit varchar(30) NOT NULL,
 is_active boolean NOT NULL DEFAULT true,
 created_at timestamptz NOT NULL DEFAULT now(),
 updated_at timestamptz NOT NULL DEFAULT now()
);

CREATE TABLE IF NOT EXISTS company_bank_accounts (
 id uuid PRIMARY KEY,
 bank_name varchar(120) NOT NULL,
 account_number varchar(100) NOT NULL,
 account_name varchar(160) NOT NULL,
 is_active boolean NOT NULL DEFAULT true,
 created_at timestamptz NOT NULL DEFAULT now()
);

COMMIT;

BEGIN;
CREATE TABLE IF NOT EXISTS document_sequences(document_type varchar(40) NOT NULL,year integer NOT NULL,last_number integer NOT NULL DEFAULT 0,PRIMARY KEY(document_type,year));
CREATE TABLE IF NOT EXISTS document_events(id bigserial PRIMARY KEY,document_type varchar(40) NOT NULL,document_id text NOT NULL,event_type varchar(20) NOT NULL CHECK(event_type IN('CREATED','PRINTED','EXPORTED','VOIDED')),user_id uuid REFERENCES app_users(id),created_at timestamptz NOT NULL DEFAULT now(),details jsonb NOT NULL DEFAULT '{}'::jsonb);
CREATE INDEX IF NOT EXISTS document_events_doc_idx ON document_events(document_type,document_id);
COMMIT;

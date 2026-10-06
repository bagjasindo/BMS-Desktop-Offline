BEGIN;
CREATE TABLE IF NOT EXISTS sales_orders(id uuid PRIMARY KEY,order_number varchar(80) NOT NULL UNIQUE,customer_id uuid NOT NULL REFERENCES business_partners(id),order_date date NOT NULL,status varchar(20) NOT NULL DEFAULT 'DRAFT' CHECK(status IN('DRAFT','CONFIRMED','PARTIAL','COMPLETED','CANCELLED')),created_at timestamptz NOT NULL DEFAULT now());
CREATE TABLE IF NOT EXISTS delivery_notes(id uuid PRIMARY KEY,delivery_number varchar(80) NOT NULL UNIQUE,sales_order_id uuid REFERENCES sales_orders(id),delivery_date date NOT NULL,status varchar(20) NOT NULL DEFAULT 'POSTED' CHECK(status IN('POSTED','VOID')),created_at timestamptz NOT NULL DEFAULT now());
CREATE TABLE IF NOT EXISTS supplier_invoice_receipts(id uuid PRIMARY KEY,supplier_invoice_id uuid NOT NULL REFERENCES supplier_invoices(id),goods_receipt_id uuid NOT NULL REFERENCES goods_receipts(id),UNIQUE(supplier_invoice_id,goods_receipt_id));
COMMIT;

# Arsitektur BMS Desktop Offline

Client Windows (.NET 8 WinForms) terhubung ke PostgreSQL pada server lokal melalui LAN/Wi-Fi lokal. Internet tidak dibutuhkan untuk transaksi operasional.

## Batas proyek
BMS Desktop berdiri sendiri dan tidak mengubah database/source BMS Mobile/Web.

## Keamanan dasar
Password tidak disimpan plaintext. Role diverifikasi dari database. Audit log disiapkan sejak fondasi. Kredensial database lokal tidak disimpan di repository.

## Urutan implementasi
1. Fondasi database, login, role, audit.
2. Administrator dan master data.
3. Kandang/kontrak/periode.
4. Logistics.
5. Production/PPL.
6. Marketing/panen.
7. RHPP.
8. Finance.
9. Dokumen, Excel/PDF, backup/restore.
10. Audit integrasi dan build installer.

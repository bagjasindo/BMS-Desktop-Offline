# Deployment Lokal/LAN

## Server lokal
- Windows PC/server kantor.
- PostgreSQL dipasang pada mesin server.
- Database `bms_desktop` memakai user database khusus aplikasi.
- Firewall hanya membuka PostgreSQL untuk subnet LAN tepercaya.
- Backup disimpan terpisah dari database aktif.

## Client
- Windows 10/11 64-bit.
- BMS Desktop dipasang pada tiap PC.
- `appsettings.json` menunjuk IP server lokal dan tidak masuk Git.
- Internet tidak diperlukan.

## Build
Target awal: `dotnet publish -c Release -r win-x64 --self-contained true`.
Installer dibuat setelah migrasi dan alur kritis lolos pengujian.

Repo belum release-ready sampai build nyata, migrasi database kosong, transaksi role, backup/restore, dan multi-PC LAN diuji.

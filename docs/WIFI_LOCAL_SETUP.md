# Jaringan Wi-Fi Lokal BMS Desktop

BMS Desktop dapat bekerja melalui Wi-Fi lokal tanpa internet.

## Topologi yang disarankan
`PC Server PostgreSQL -> Router/AP lokal -> PC/Laptop pengguna BMS Desktop`

Server idealnya tersambung kabel LAN ke router untuk kestabilan. Client boleh menggunakan Wi-Fi. Server juga dapat memakai Wi-Fi jika diperlukan.

## Aturan
- Server memakai IP lokal tetap/reservasi DHCP.
- Client menunjuk `Database.Host` ke IP lokal server, bukan `127.0.0.1`.
- PostgreSQL hanya menerima koneksi dari subnet lokal yang dipercaya.
- Windows Firewall hanya membuka port database untuk jaringan Private/subnet lokal.
- Jangan membuka PostgreSQL langsung ke internet.
- Jika Wi-Fi/router terputus, transaksi harus dianggap gagal; aplikasi tidak boleh menganggap data sudah tersimpan.

## Uji sebelum operasional
1. Login dari minimal dua PC client.
2. Simpan transaksi bersamaan.
3. Putuskan Wi-Fi saat transaksi dan pastikan rollback/error jelas.
4. Sambungkan kembali dan pastikan data tidak ganda.
5. Uji backup dari server dan restore pada database uji.

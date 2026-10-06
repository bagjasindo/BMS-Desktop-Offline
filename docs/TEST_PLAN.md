# Rencana Uji BMS Desktop Offline

Status PASS hanya boleh diberikan setelah aplikasi benar-benar dibuild dan transaksi diuji terhadap PostgreSQL lokal.

1. Database migration 001-009 pada database kosong.
2. Buat Admin pertama tanpa password default di source.
3. Login tiap role dan verifikasi menu/assignment.
4. Admin: master perusahaan, kandang, partner, item, bank, kontrak.
5. Logistik: PO, penerimaan, stock movement, assignment kandang.
6. Produksi/PPL: Chick-In, Day 1 H+1, mortalitas/culling, BW, pakan, OVK.
7. Marketing: panen per kandang/cycle.
8. RHPP: MITRA/MANDIRI, kontrak, snapshot CLOSED, histori.
9. Finance: BOP Produksi, perawatan kandang, BOP Kantor/Luar, invoice/payment.
10. Form Pengajuan Kas dipastikan tidak memposting transaksi realisasi.
11. Laba/Rugi kandang dan global direkonsiliasi manual.
12. Backup/restore diuji pada database salinan.
13. Multi-PC LAN concurrency.
14. Print/PDF/Excel.
15. Build Release + installer Windows.

Belum ada bagian yang boleh dinyatakan final PASS sebelum pengujian ini dilakukan.

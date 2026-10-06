# Alur Dokumen

Pembelian dipisahkan menjadi PO → penerimaan fisik → invoice supplier → pembayaran. Relasi invoice supplier ke penerimaan disimpan untuk rekonsiliasi/three-way matching.

Penjualan dipisahkan menjadi Sales Order → Surat Jalan/Delivery → Invoice → pembayaran.

Nomor dokumen diambil dari sequence database agar aman saat beberapa PC menggunakan Wi-Fi/LAN secara bersamaan. Void/cancel harus dicatat, bukan menghapus histori dokumen.

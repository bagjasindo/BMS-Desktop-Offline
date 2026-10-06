# Aturan Bisnis Inti

Dokumen ini menjadi pagar implementasi Desktop dan tidak mengubah source BMS Web.

- Kandang menjadi ACTIVE setelah Chick-In.
- DOC received menjadi dasar populasi awal; DOA/Mati Box disimpan terpisah.
- Recording Day 1 dimulai H+1 setelah DOC arrival/Chick-In.
- Satu kandang hanya boleh memiliki satu cycle ACTIVE.
- MITRA/RHPP menggunakan harga kontrak yang ditetapkan, bukan harga pembelian aktual.
- Tambah Daging: performa RHPP memakai harga BW kontrak; hutang/cost supplier memakai harga beli aktual.
- Sapronak/feed di luar kontrak dan biaya perusahaan dipisahkan dari komponen kontrak RHPP.
- BOP Produksi per kandang/cycle terpisah dari Perawatan/Renovasi Kandang.
- Perawatan/Renovasi Kandang mengurangi Laba/Rugi Global, bukan RHPP/laba operasional cycle.
- BOP Kantor dan BOP Umum/Luar Kantor dipisahkan.
- Form Pengajuan Kas berdiri sendiri dan tidak otomatis posting ke BOP, Kasbon, Arus Kas, Hutang, RHPP, Laba/Rugi, atau realisasi.
- CLOSED menyimpan snapshot final/historis.
- Role dan assignment kandang membatasi akses operasional.
- Tidak ada kredensial default atau data transaksi dummy di source.

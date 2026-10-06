# CLOSED dan Histori Final

Saat cycle ditutup, hasil final RHPP harus disimpan sebagai snapshot historis. Snapshot dan record closing bersifat immutable: tidak boleh diedit atau dihapus.

Laporan historis CLOSED harus membaca snapshot final, bukan menghitung ulang menggunakan master/kontrak yang mungkin sudah berubah kemudian.

Nilai yang memang tidak tersedia ditampilkan `-`; jangan mengubah ketidaktersediaan menjadi angka nol palsu.

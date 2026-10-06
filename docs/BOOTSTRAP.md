# Bootstrap Instalasi Pertama

1. Buat PostgreSQL lokal dan database BMS pada PC server.
2. Isi `appsettings.json` lokal; file kredensial tidak masuk Git.
3. Jalankan migration runner terhadap folder `database`.
4. Jika database benar-benar baru dan belum memiliki akun, buat ADMIN pertama melalui bootstrap service.
5. Password ADMIN tidak pernah ditanam sebagai default di source/repository.
6. Setelah ADMIN pertama tersedia, akun lain dibuat dari modul Administrator.

Bootstrap harus ditolak bila database sudah memiliki akun.

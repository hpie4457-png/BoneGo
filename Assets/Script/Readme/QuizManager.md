# Dokumentasi Script: `QuizManager.cs`

## 📖 Apa fungsi script ini?
Script ini adalah **"SANG BOS / SUTRADARA KUIS"**.
Ini adalah script paling penting di dalam mode Kuis. Semua script lain (tombol, waktu, layar, suara) tunduk dan dikomando oleh script ini.
Tugas sutradara ini meliputi:
1. Memulai kuis, memutar lagu kuis, dan menyuruh `QuizLoader` mengambil soal.
2. Menampilkan soal nomor 1 ke layar dan menjalankan waktu.
3. Menilai apakah pemain mencet jawaban yang Benar atau Salah, lalu menambah skor.
4. Pindah ke soal berikutnya.
5. Memunculkan layar Nilai Akhir (Result) jika soal sudah habis.

---

## 💻 Kode Lengkap

*(Kode sangat panjang, sekitar 200 baris. Kita akan bahas bagian terpentingnya!)*

---

## 🧩 Penjelasan Cara Kerjanya (Block per Block)

### 1. Mengenali Semua Anak Buahnya (`Awake`)
```csharp
private void Awake()
{
    // Menerima laporan dari layar kuis
    if (quizView != null)
    {
        quizView.OnOptionSelected += HandleOptionSelected; // Kalau tombol dijawab
        quizView.OnNextClicked += HandleNextClicked; // Kalau dipencet tombol lanjut
    }

    // Menerima laporan dari waktu
    if (quizTimer != null)
    {
        quizTimer.OnTimeExpired += HandleTimeExpired; // Kalau waktu habis
    }
    
    // ... dan laporan dari menu pause & nilai akhir
}
```
- Di sini Sang Bos memasang kuping. Dia bilang: *"Layar kuis, kalau pemain milih jawaban, lapor ke aku lewat fungsi `HandleOptionSelected` ya! Hai Waktu, kalau kamu habis, teriak ke aku lewat `HandleTimeExpired`!"*

### 2. Memulai Pertunjukan (`Start`)
```csharp
private void Start()
{
    // Ganti lagu Menu Utama jadi Lagu Kuis
    if (AudioManager.Instance != null)
    {
        AudioManager.Instance.StopBGM(0.5f);
        AudioManager.Instance.PlayQuizBGM(0.5f);
    }

    LoadQuestions(); // Ambil soal
    ShowQuestion(_currentIndex); // Tampilkan soal pertama (index ke-0)
}
```
- Sutradara berteriak *"Action!"*. Dia menyuruh ganti lagu, menyiapkan soal, lalu memunculkan soal pertama ke layar.

### 3. Saat Pemain Menjawab (`HandleOptionSelected`)
```csharp
private void HandleOptionSelected(int selectedIndex)
{
    if (_answerLocked) return; // Kalau udah dikunci (karena udah jawab), jangan bisa jawab lagi
    _answerLocked = true;

    if (quizTimer != null) quizTimer.StopTimer(); // Stop waktunya!

    var q = _questions[_currentIndex]; // Lihat soal saat ini
    bool correct = q.IsCorrect(selectedIndex); // Apakah jawaban pemain benar?

    if (correct)
    {
        _score += 10; // Tambah nilai 10!
        AudioManager.Instance?.PlayCorrectSFX(); // Bunyikan "Ting-tong!"
    }
    else
    {
        AudioManager.Instance?.PlayWrongSFX(); // Bunyikan "Tetooot!"
    }

    // ... memunculkan pop-up benar/salah ke layar
}
```
- Ini adalah inti dari kuis. Saat pemain memilih, Bos langsung mengunci kuis biar nggak dipencet lagi.
- Waktu dihentikan, jawaban dicek. Kalau benar dapat poin, kalau salah poin tetap. Musik efek juga dimainkan sesuai hasilnya.

### 4. Kalau Waktu Habis (`HandleTimeExpired`)
Mirip dengan saat pemain menjawab, tapi ini terjadi kalau pemain bengong kelamaan.
Bos akan mengunci pilihan, memunculkan pop-up *"Warning"* (waktu habis), dan langsung memberitahu jawaban mana yang seharusnya benar, tapi pemain tidak dapat nilai.

### 5. Pindah ke Soal Berikutnya (`HandleNextClicked`)
```csharp
private void HandleNextClicked()
{
    _currentIndex++; // Maju ke nomor soal berikutnya

    // Kalau soalnya sudah habis (misal sudah 10 dari 10)
    if (_currentIndex >= _questions.Count)
    {
        ShowResults(); // Tampilkan nilai akhirnya!
        return;
    }

    ShowQuestion(_currentIndex); // Kalau belum, tampilkan soal selanjutnya
}
```
- Mengatur urutan soal. Kalau masih ada sisa, lanjut. Kalau sudah mentok, panggil layar Rapor/Nilai Akhir.

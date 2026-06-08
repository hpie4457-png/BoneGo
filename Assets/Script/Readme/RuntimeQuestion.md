# Dokumentasi Script: `RuntimeQuestion.cs`

## 📖 Apa fungsi script ini?
Script ini adalah **"Kantong Belanja Soal yang Sudah Siap Pakai"**.
Sebelumnya kita tahu ada *QuizData* (Bentuk soal mentah). Nah, *RuntimeQuestion* ini adalah bentuk soal yang sudah "dimasak" dan **sudah diacak (di-shuffle)** pilihan gandanya, jadi siap untuk dilempar ke pemain.

---

## 💻 Kode Lengkap

```csharp
using System.Collections.Generic;

namespace QuizSystem.Models
{
    // Ini mewakili 1 soal yang sudah siap dimainkan
    public class RuntimeQuestion
    {
        public int Id { get; private set; } // Nomor unik soal
        public string QuestionText { get; private set; } // Bunyi pertanyaannya
        public string ImageName { get; private set; } // Nama gambarnya
        
        // Daftar pilihan ganda yang SUDAH DIACAK
        public List<string> ShuffledChoices { get; private set; }
        
        // Posisi jawaban benar yang baru (karena sudah diacak)
        public int CorrectAnswerIndex { get; private set; }

        // Fungsi ini bertugas memasukkan data dari luar ke dalam kantong ini
        public RuntimeQuestion(QuestionData source, List<string> shuffledChoices, int correctIndex)
        {
            Id = source.id;
            QuestionText = source.question;
            ImageName = source.image;
            ShuffledChoices = shuffledChoices;
            CorrectAnswerIndex = correctIndex;
        }

        // Fungsi bantuan untuk mengecek apakah tebakan pemain benar
        public bool IsCorrect(int selectedIndex) => selectedIndex == CorrectAnswerIndex;
    }
}
```

---

## 🧩 Penjelasan Cara Kerjanya (Block per Block)

### 1. Kotak Penyimpanan Data (Variabel)
- Di sini banyak tertulis `{ get; private set; }`. 
- Artinya: *"Data ini cuma boleh dibaca (get) oleh orang lain, tapi tidak boleh diubah (private set) secara sembarangan oleh script lain."* 
- Ini supaya gamenya aman dari error (misalnya jawaban benar tiba-tiba terganti di tengah jalan).

### 2. Memasak Soal (Fungsi Constructor)
```csharp
public RuntimeQuestion(QuestionData source, List<string> shuffledChoices, int correctIndex)
{
    Id = source.id;
    // ... dst
}
```
- Fungsi yang namanya sama persis dengan nama class-nya (`RuntimeQuestion`) disebut **Constructor** (Pembangun).
- `QuizLoader` (Si Pengocok Soal) akan memanggil fungsi ini. Dia membawa soal mentah (`source`), daftar jawaban yang sudah dia acak (`shuffledChoices`), dan memberitahu posisi jawaban benar yang baru (`correctIndex`).
- Lalu fungsi ini menyimpannya rapat-rapat di dalam kantong.

### 3. Cek Jawaban Singkat (`IsCorrect`)
```csharp
public bool IsCorrect(int selectedIndex) => selectedIndex == CorrectAnswerIndex;
```
- Ini adalah jalan pintas. Kalau Bos Kuis mau tahu apakah pemain menjawab dengan benar, dia tinggal kirim nomor jawaban pemain (misal pemain milih C / nomor 2).
- Script ini akan mengecek: *"Apakah angka 2 sama dengan CorrectAnswerIndex? Kalau sama kembalikan BENAR (True), kalau tidak SALAH (False)."*

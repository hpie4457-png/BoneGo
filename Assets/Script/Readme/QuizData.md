# Dokumentasi Script: `QuizData.cs`

## 📖 Apa fungsi script ini?
Script ini bukan program yang menjalankan perintah, melainkan sebuah **"Bentuk Cetakan Data"**.
Bayangkan ini seperti formulir pendaftaran. Formulir ini memberi tahu sistem kita:
*"Nanti kalau baca file soal dari file teks (.json), bentuknya harus begini ya: Ada nomor ID, ada teks pertanyaan, ada pilihan ganda, dan ada jawaban benarnya!"*

---

## 💻 Kode Lengkap

```csharp
using System.Collections.Generic;

namespace QuizSystem.Models
{
    // Ini mewakili keseluruhan isi file kuis (Kumpulan soal-soal)
    [System.Serializable]
    public class QuizData
    {
        public List<QuestionData> questions; // Daftar pertanyaan
    }

    // Ini mewakili bentuk dari 1 buah soal
    [System.Serializable]
    public class QuestionData
    {
        public int id; // Nomor unik soal
        public string question; // Teks pertanyaannya
        public List<string> choices; // Daftar pilihan ganda (A, B, C, D)
        
        // Nomor urut jawaban yang benar (dimulai dari 0, jadi 0 itu A, 1 itu B, dst)
        public int correctAnswerIndex; 
        
        public string image; // Nama gambar soal (kalau ada)
    }
}
```

---

## 🧩 Penjelasan Cara Kerjanya (Block per Block)

### 1. Kenapa Kodenya Sangat Pendek?
Ini karena script ini hanya berisi **Variabel (tempat menyimpan data)** tanpa ada perintah/fungsi seperti `Start()`, `Update()`, atau `Show()`.

### 2. Arti `[System.Serializable]`
Ini adalah perintah wajib untuk memberitahu Unity: *"Tolong ingat dan simpan bentuk formulir ini, supaya nanti bisa dicocokkan dengan isi teks JSON"*.

### 3. Struktur Data
- **`QuizData`**: Mengibaratkan buku kumpulan soal. Di dalamnya ada "List" (daftar) dari soal-soal.
- **`QuestionData`**: Mengibaratkan 1 halaman soal. Di halaman itu ada:
  - `id`: Nomor seri soal.
  - `question`: Bunyi pertanyaannya (Misal: *"Tulang terbesar di tubuh manusia adalah?"*)
  - `choices`: Pilihan jawabannya (Misal: *["Paha", "Lengan", "Rusuk", "Jari"]*)
  - `correctAnswerIndex`: Menentukan mana yang benar. Karena komputer menghitung dari angka 0, maka jika jawaban benar adalah "Paha" (pilihan pertama), angkanya ditulis `0`.
  - `image`: Kalau pertanyaannya butuh gambar, sebutkan nama gambarnya di sini.

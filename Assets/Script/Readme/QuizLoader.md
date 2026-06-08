# Dokumentasi Script: `QuizLoader.cs`

## 📖 Apa fungsi script ini?
Script ini adalah **"Si Pengambil & Pengocok Soal"**.
Bayangkan dia seperti guru yang:
1. Pergi ke lemari (Folder `Resources/Quiz/`) untuk mengambil buku kumpulan soal.
2. Memilih beberapa soal saja (misalnya 10 soal).
3. **Mengocok** urutan soal-soal tersebut dan juga **mengocok** pilihan gandanya, supaya kalau pemain main lagi, soal dan jawabannya tidak di posisi yang sama.

---

## 💻 Kode Lengkap

*(Ini adalah versi potongannya agar mudah dipahami)*

```csharp
using System.Collections.Generic;
using UnityEngine;
using QuizSystem.Models;

namespace QuizSystem.Core
{
    public class QuizLoader
    {
        private const string ResourcePath = "Quiz";

        // Fungsi utama yang akan dipanggil oleh Bos Kuis
        public List<RuntimeQuestion> LoadAndShuffle(int maxQuestions)
        {
            var raw = LoadJson(); // Ambil buku soal
            
            // Ubah soal mentah jadi soal siap pakai
            var list = BuildRuntimeQuestions(raw.questions);

            // Kocok semua pertanyaannya
            Shuffle(list);

            // Ambil sesuai jumlah maksimal (misal cuma mau 10 soal)
            if (list.Count > maxQuestions)
            {
                list = list.GetRange(0, maxQuestions);
            }

            return list; // Kembalikan soal yang sudah siap ke bos!
        }
        
        // ... (Fungsi bantuan lainnya di bawah)
    }
}
```

---

## 🧩 Penjelasan Cara Kerjanya (Block per Block)

### 1. Mengambil Soal dari File (`LoadJson`)
```csharp
private QuizData LoadJson()
{
    var asset = Resources.Load<TextAsset>(ResourcePath);
    return JsonUtility.FromJson<QuizData>(asset.text);
}
```
- Dia masuk ke folder `Resources/Quiz` di dalam Unity, lalu mencari file teks (biasanya `.json`).
- Kemudian dia mengubah teks yang ribet itu menjadi bentuk cetakan data (`QuizData`) yang sudah kita buat sebelumnya.

### 2. Mempersiapkan Soal Mentah (`BuildRuntimeQuestions`)
```csharp
private List<RuntimeQuestion> BuildRuntimeQuestions(List<QuestionData> raw)
{
    var result = new List<RuntimeQuestion>(raw.Count);
    foreach (var data in raw)
    {
        // Cari tahu teks jawaban benarnya apa
        string correctText = data.choices[data.correctAnswerIndex];

        // Kocok pilihan gandanya (A, B, C, D diacak)
        var shuffled = new List<string>(data.choices);
        Shuffle(shuffled);

        // Cari tahu sekarang jawaban benarnya pindah ke posisi berapa
        int newCorrectIndex = shuffled.IndexOf(correctText);
        
        // Simpan sebagai soal siap main
        result.Add(new RuntimeQuestion(data, shuffled, newCorrectIndex));
    }
    return result;
}
```
- Ini adalah bagian tersulit! Karena pilihan gandanya mau kita acak, script ini harus ingat apa jawaban benarnya.
- Misalnya jawaban benar adalah "Tulang Paha" di posisi A (0).
- Lalu pilihan gandanya diacak. Sekarang "Tulang Paha" ada di posisi C (2).
- Dia mencatat *"Oke, untuk soal ini sekarang jawaban benarnya ada di posisi nomor 2 ya!"*

### 3. Jurus Mengocok Kartu (`Shuffle`)
```csharp
private void Shuffle<T>(List<T> list)
{
    for (int i = list.Count - 1; i > 0; i--)
    {
        int j = Random.Range(0, i + 1);
        (list[i], list[j]) = (list[j], list[i]); // Tukar posisi
    }
}
```
- Ini adalah algoritma matematika sederhana (namanya *Fisher-Yates Shuffle*) untuk menukar-nukar posisi barang secara acak (Random). Ibarat kita mengocok kartu remi dengan tangan.

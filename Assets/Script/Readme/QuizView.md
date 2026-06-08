# Dokumentasi Script: `QuizView.cs`

## 📖 Apa fungsi script ini?
Script ini adalah **"Si Pengatur Tata Letak Layar Kuis"**.
Mirip dengan *QuestionView*, tapi script ini lebih berfokus pada mengatur bagaimana bentuk layar saat kuis berjalan.
Tugasnya:
1. Menaruh teks soal dan angka nomor soal ke tempat yang pas.
2. Mencari dan menampilkan gambar yang berhubungan dengan soal.
3. Memunculkan tombol-tombol A, B, C, D ke layar.
4. Membesarkan atau mengecilkan area gambar kalau pemain sudah menjawab (supaya area jawaban bisa terlihat lebih jelas).

---

## 💻 Kode Lengkap

*(Ini adalah versi kerangka dari kode aslinya yang cukup panjang)*

```csharp
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace QuizSystem.UI
{
    public class QuizView : MonoBehaviour
    {
        [Header("── Question UI ──────────────────────────")]
        [SerializeField] private TextMeshProUGUI questionNumberText;
        [SerializeField] private TextMeshProUGUI questionBodyText;
        [SerializeField] private Image questionImage;
        [SerializeField] private GameObject headerPanel; // Hiasan atas

        [Header("── Options ─────────────────────────────")]
        [SerializeField] private Transform optionContainer; // Tempat tombol
        [SerializeField] private GameObject optionPrefab; // Cetakan tombol

        [Header("── Navigation ──────────────────────────")]
        [SerializeField] private Button nextButton; // Tombol Lanjut

        // Saluran lapor ke bos
        public event Action OnNextClicked;
        public event Action<int> OnOptionSelected;

        // ... Fungsi-fungsi di bawah
    }
}
```

---

## 🧩 Penjelasan Cara Kerjanya (Block per Block)

### 1. Menampilkan Semua Informasi (`DisplayQuestion`)
```csharp
public void DisplayQuestion(int index, int total, RuntimeQuestion question)
{
    // Mengubah nomor (contoh: 1 / 10)
    questionNumberText.text = $"{index + 1} / {total}";
    
    // Mengubah pertanyaan
    questionBodyText.text = question.QuestionText;

    // Memasang gambar (fungsi khusus di bawah)
    UpdateQuestionImage(question.ImageName);

    // Bikin layarnya kembali normal (bukan mode pamer jawaban)
    SetResultState(false);

    // Mencetak tombol jawaban
    SpawnOptions(question);
}
```
- Bos kuis akan mengirim data soal ke fungsi ini.
- Fungsi ini seperti mandor bangunan yang menyuruh anak buahnya merapikan teks, gambar, dan tombol ke layar HP kita.

### 2. Mengubah Tata Letak (`SetResultState`)
```csharp
public void SetResultState(bool isRevealed)
{
    if (headerPanel != null) headerPanel.SetActive(!isRevealed);
    nextButton.gameObject.SetActive(isRevealed);

    // Mengubah ukuran gambar
    if (imageWrapperLayout != null)
    {
        imageWrapperLayout.preferredHeight = isRevealed ? 250f : 350f;
    }
}
```
- Fungsi ini dipakai saat pemain SUDAH memilih jawaban.
- Saat jawaban terbongkar (`isRevealed = true`), hiasan atas dihilangkan, tombol "Lanjut" (Next) dimunculkan.
- Ukuran area gambar dikecilkan dari `350` menjadi `250` supaya tidak menghalangi tombol jawaban di bawahnya.

### 3. Jago Cari Gambar (`UpdateQuestionImage`)
Di dalam script ini, ada fungsi panjang bernama `UpdateQuestionImage`.
- Tugasnya adalah mencari gambar di dalam folder `Resources/QuizImages/`.
- Hebatnya, kalau kita tidak memberitahu gambar itu ada di folder mana (misalnya di sub-folder *Tengkorak*, *Tulang Tangan*, dsb), script ini akan otomatis **keliling mencari** ke semua folder tersebut sampai gambarnya ketemu!

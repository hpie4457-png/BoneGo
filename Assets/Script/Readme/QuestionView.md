# Dokumentasi Script: `QuestionView.cs`

## 📖 Apa fungsi script ini?
Script ini adalah **"Papan Tulis Kuis"**. 
Tugas utamanya adalah menampilkan semua informasi soal ke layar agar bisa dibaca pemain. Ini meliputi:
1. Menampilkan teks pertanyaan (Misal: *"Tulang apakah ini?"*)
2. Menampilkan nomor soal (Misal: *"Question 1 / 5"*)
3. Menampilkan gambar soal (jika ada gambarnya).
4. **Mencetak Tombol Jawaban**: Script ini bertugas menciptakan (Spawn) tombol-tombol pilihan A, B, C, D ke layar berdasarkan data soal.

---

## 💻 Kode Lengkap

*(Hanya menampilkan kerangka utamanya agar mudah dimengerti)*

```csharp
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using QuizSystem.Models;

namespace QuizSystem.UI
{
    public class QuestionView : MonoBehaviour
    {
        [Header("Question UI")]
        [SerializeField] private TextMeshProUGUI questionNumberText; // Nomor soal
        [SerializeField] private TextMeshProUGUI questionText; // Teks soal
        [SerializeField] private Image questionImage; // Gambar soal

        [Header("Options")]
        [SerializeField] private Transform optionContainer; // Tempat menaruh tombol
        [SerializeField] private GameObject optionPrefab; // Cetakan tombol jawaban

        // Daftar tombol yang saat ini muncul di layar
        private readonly List<OptionView> _activeOptions = new();

        // Saluran komunikasi kalau pemain milih jawaban
        public System.Action<int> OnOptionSelected;

        // ... (Fungsi-fungsi ada di bawah)
    }
}
```

---

## 🧩 Penjelasan Cara Kerjanya (Block per Block)

### 1. Menampilkan Soal di Papan Tulis (`Display`)
```csharp
public void Display(RuntimeQuestion question, int questionNumber, int totalQuestions)
{
    // Mengubah teks nomor soal (Contoh: "Question 1 / 5")
    questionNumberText.text = $"Question {questionNumber} / {totalQuestions}";
    
    // Mengubah teks pertanyaannya
    questionText.text = question.QuestionText;

    // Menampilkan gambar dan mencetak tombol-tombolnya
    LoadImage(question.ImageName);
    SpawnOptions(question.ShuffledChoices);
}
```
- Fungsi ini dipanggil setiap kali pindah ke soal baru. Dia meminta data soal dari bos kuis, lalu menuliskannya di papan tulis (layar HP).

### 2. Membongkar Rahasia Jawaban (`RevealResult`)
```csharp
public void RevealResult(int selectedIndex, int correctIndex)
{
    foreach (var option in _activeOptions)
    {
        option.Lock(); // Kunci semua tombol

        if (option.OptionIndex == correctIndex)
        {
            option.SetState(OptionState.Correct); // Yang benar dihijaukan
        }
        else if (option.OptionIndex == selectedIndex)
        {
            option.SetState(OptionState.Wrong); // Pilihan pemain yang salah dimerahkan
        }
    }
}
```
- Setelah pemain memilih, fungsi ini dipanggil untuk "buka-bukaan" jawaban.
- Dia mengunci semua tombol biar nggak bisa diklik lagi. Lalu, menyuruh tombol yang benar untuk pakai warna Benar (Hijau). Kalau pemain salah milih, tombol yang dipilih pemain disuruh pakai warna Salah (Merah).

### 3. Menciptakan Tombol Jawaban (`SpawnOptions`)
```csharp
private void SpawnOptions(List<string> choices)
{
    // Hapus tombol-tombol dari soal sebelumnya
    foreach (var old in _activeOptions)
        Destroy(old.gameObject);
    _activeOptions.Clear();

    // Buat tombol baru sebanyak jumlah pilihan ganda
    for (int i = 0; i < choices.Count; i++)
    {
        var go = Instantiate(optionPrefab, optionContainer);
        var view = go.GetComponent<OptionView>();

        view.Setup(i, choices[i]);

        // Kalau tombol diklik, lapor ke atas!
        int captured = i;
        view.OnSelected += _ => OnOptionSelected?.Invoke(captured);

        _activeOptions.Add(view);
    }
}
```
- `Instantiate` adalah sihir Unity untuk "mencetak/memperbanyak" objek (dalam hal ini, tombol jawaban).
- Sebelum bikin tombol baru, dia sapu bersih dulu tombol-tombol dari soal sebelumnya supaya layarnya bersih.

### 4. Memasang Gambar Soal (`LoadImage`)
Fungsi ini akan mencari gambar berdasarkan namanya di folder khusus Unity (bernama folder `Resources/QuizImages`). Kalau nama gambarnya kosong, tempat gambarnya dimatikan (`SetActive(false)`) supaya tidak menuh-menuhin layar.

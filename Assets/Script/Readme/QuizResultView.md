# Dokumentasi Script: `QuizResultView.cs`

## 📖 Apa fungsi script ini?
Script ini bertugas sebagai **"Papan Rapor Akhir"**.
Tugasnya adalah memunculkan layar nilai ketika pemain sudah selesai menjawab semua pertanyaan kuis.

---

## 💻 Kode Lengkap

```csharp
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace QuizSystem.UI
{
    public class QuizResultView : MonoBehaviour
    {
        [SerializeField] private GameObject scoreCanvas; // Layar tempat menaruh nilai
        [SerializeField] private TextMeshProUGUI scoreText; // Teks angkanya (misal: "100")
        [SerializeField] private Button homeButton; // Tombol kembali ke menu utama

        public event Action OnHomeClicked; // Saluran komunikasi kalau tombol ditekan

        private void Awake()
        {
            // Daftarkan tombol
            if (homeButton != null)
                homeButton.onClick.AddListener(() => OnHomeClicked?.Invoke());
            
            // Sembunyikan papan rapor di awal game
            if (scoreCanvas != null)
                scoreCanvas.SetActive(false);
        }

        // Fungsi ini dipanggil bos kuis di akhir permainan
        public void ShowResult(int score)
        {
            if (scoreCanvas != null)
            {
                scoreCanvas.SetActive(true); // Munculkan layar
                if (scoreText != null)
                {
                    scoreText.text = score.ToString(); // Tuliskan nilainya!
                }
            }
        }

        public void Hide()
        {
            if (scoreCanvas != null)
                scoreCanvas.SetActive(false); // Sembunyikan layar
        }
    }
}
```

---

## 🧩 Penjelasan Cara Kerjanya (Block per Block)

### 1. Sembunyi Dulu (`Awake`)
- Saat permainan kuis baru dimulai, tentu kita tidak mau langsung melihat layar nilai. Makanya di bagian `Awake`, layar nilainya dimatikan (`scoreCanvas.SetActive(false)`).
- Di sini juga dia mengajari tombol "Home": *"Hei tombol Home, kalau dipencet, kasih tahu Bos Kuis ya!"*

### 2. Tampil Mentereng (`ShowResult`)
```csharp
public void ShowResult(int score)
{
    if (scoreCanvas != null)
    {
        scoreCanvas.SetActive(true); // Munculkan layar
        if (scoreText != null)
        {
            scoreText.text = score.ToString(); // Ubah angka jadi teks
        }
    }
}
```
- Saat kuis selesai, Bos Kuis (`QuizManager`) akan memanggil fungsi ini dan membawa nilai pemain (misalnya `score = 80`).
- Layar rapor langsung dimunculkan ke depan.
- Angka `80` tadi diubah menjadi teks tulisan dan dimasukkan ke dalam `scoreText` agar bisa dibaca pemain di layar HP.

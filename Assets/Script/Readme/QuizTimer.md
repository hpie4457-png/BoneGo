# Dokumentasi Script: `QuizTimer.cs`

## 📖 Apa fungsi script ini?
Script ini adalah **"Jam Pasir / Pengatur Waktu"**.
Tugasnya memastikan pemain tidak berpikir terlalu lama:
1. Menghitung mundur (misalnya dari 15 detik ke 0).
2. Menggerakkan *Slider* (Bar waktu) agar semakin lama semakin habis.
3. Mengubah angka detik di layar (15... 14... 13...).
4. Berteriak *"Waktu Habis!"* kalau angkanya sudah menyentuh 0.

---

## 💻 Kode Lengkap

*(Menampilkan bagian yang paling penting saja)*

```csharp
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace QuizSystem.Core
{
    public class QuizTimer : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private Slider timerSlider; // Bar waktu yang bisa memendek
        [SerializeField] private TextMeshProUGUI timerText; // Teks angkanya

        [Header("Settings")]
        [SerializeField] private float questionTime = 15f; // Jatah waktu per soal

        public event Action OnTimeExpired; // Saluran komunikasi untuk lapor waktu habis

        private float _remaining; // Sisa waktu sekarang
        private bool _isRunning; // Apakah waktunya sedang jalan?

        // ... (Fungsi-fungsi ada di bawah)
    }
}
```

---

## 🧩 Penjelasan Cara Kerjanya (Block per Block)

### 1. Menyalakan dan Mematikan Waktu
```csharp
public void StartTimer()
{
    _remaining = questionTime; // Isi bensin waktu (misal: 15 detik)
    _isRunning = true;         // Mulai jalan!
    UpdateUI();                // Perbarui gambar layar
}

public void StopTimer()
{
    _isRunning = false; // Stop! (Biasa dipakai saat pemain sudah ngeklik jawaban)
}
```
- Seperti namanya, ini adalah remot kontrol untuk menyalakan dan memberhentikan waktu.

### 2. Mesin Penghitung Mundur (`Update`)
```csharp
private void Update()
{
    if (!_isRunning) return; // Kalau lagi distop, diam saja.

    // Kurangi sisa waktu dengan pergerakan jam dunia nyata
    _remaining -= Time.deltaTime;

    // Kalau waktunya sudah habis (kurang dari 0)
    if (_remaining <= 0f)
    {
        _remaining = 0f; // Mentokin di 0 (biar nggak jadi minus)
        _isRunning = false; // Stop waktunya
        UpdateUI(); // Perbarui layar jadi 0
        OnTimeExpired?.Invoke(); // Teriak: "BOS, WAKTU HABIS!"
        return;
    }

    UpdateUI(); // Kalau belum habis, terus perbarui layarnya
}
```
- Fungsi **`Update()`** adalah jantung dari script ini. Di Unity, fungsi `Update()` akan dijalankan **terus-menerus setiap detik berkali-kali** (setiap frame).
- `Time.deltaTime` adalah hitungan waktu dunia nyata. Jadi kalau kita kurangi sisa waktu (`_remaining`) dengan `Time.deltaTime`, angkanya akan benar-benar berkurang seperti jam biasa (15.00, 14.99, 14.98...).

### 3. Menggerakkan Bar dan Teks Layar (`UpdateUI`)
```csharp
private void UpdateUI()
{
    if (timerSlider != null)
        timerSlider.value = _remaining / questionTime; 

    if (timerText != null)
        timerText.text = Mathf.CeilToInt(_remaining).ToString();
}
```
- **Slider**: Untuk membuat efek bar waktu yang berkurang, kita membagi sisa waktu dengan total waktu. (Contoh: Sisa 7.5 detik / Total 15 detik = 0.5 atau setengah bar).
- **Teks**: Angka waktu komputer itu ada koma-komanya (misal: 14.3 detik). Tentu jelek dilihat. `Mathf.CeilToInt` berfungsi membulatkan angka tersebut ke atas (jadi 15 detik penuh) agar enak dilihat pemain.

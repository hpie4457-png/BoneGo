# Dokumentasi Script: `QuizPauseManager.cs`

## 📖 Apa fungsi script ini?
Script ini adalah **"Si Pengatur Waktu Istirahat (Pause)"**.
Tugasnya sangat simpel:
1. Menyediakan tombol "Pause" (Jeda).
2. Kalau dipencet, dia akan memunculkan menu Pause yang isinya tombol "Lanjut" (Resume) dan "Keluar" (Exit).
3. Melaporkan ke Bos Kuis (*QuizManager*): *"Hei Bos, gamenya di-pause nih, tolong berhentikan waktunya sementara ya!"*

---

## 💻 Kode Lengkap

```csharp
using UnityEngine;
using UnityEngine.UI;
using System;

namespace QuizSystem.UI
{
    public class QuizPauseManager : MonoBehaviour
    {
        [SerializeField] private GameObject pauseCanvas; // Layar menu Pause
        [SerializeField] private Button pauseButton;     // Tombol Jeda
        [SerializeField] private Button resumeButton;    // Tombol Lanjut Main
        [SerializeField] private Button exitButton;      // Tombol Keluar

        // Saluran komunikasi untuk lapor ke Bos Kuis
        public event Action<bool> OnPauseToggled;
        public event Action OnExitClicked;

        private void Awake()
        {
            // Pasang fungsi ke masing-masing tombol
            if (pauseButton != null) pauseButton.onClick.AddListener(() => TogglePause(true));
            if (resumeButton != null) resumeButton.onClick.AddListener(() => TogglePause(false));
            if (exitButton != null) exitButton.onClick.AddListener(() => OnExitClicked?.Invoke());

            // Sembunyikan layar Pause di awal permainan
            if (pauseCanvas != null)
                pauseCanvas.SetActive(false);
        }

        public void TogglePause(bool pause)
        {
            // Munculkan atau Sembunyikan layar Pause
            if (pauseCanvas != null)
                pauseCanvas.SetActive(pause);
            
            // Lapor ke Bos Kuis: "Bos, status pause sekarang = [benar/salah]"
            OnPauseToggled?.Invoke(pause);
        }

        public void SetPauseButtonActive(bool active)
        {
            // Mengatur apakah tombol Pause boleh dipencet atau tidak
            if (pauseButton != null)
                pauseButton.gameObject.SetActive(active);
        }
    }
}
```

---

## 🧩 Penjelasan Cara Kerjanya (Block per Block)

### 1. Menyiapkan Tombol-tombol (`Awake`)
```csharp
private void Awake()
{
    if (pauseButton != null) pauseButton.onClick.AddListener(() => TogglePause(true));
    if (resumeButton != null) resumeButton.onClick.AddListener(() => TogglePause(false));
    if (exitButton != null) exitButton.onClick.AddListener(() => OnExitClicked?.Invoke());

    if (pauseCanvas != null)
        pauseCanvas.SetActive(false); // Sembunyikan saat mulai
}
```
- Mirip dengan script-script UI lainnya, di sini kita "memprogram" tombolnya.
- **`pauseButton`**: Kalau diklik, nyalakan layar pause (`TogglePause(true)`).
- **`resumeButton`**: Kalau diklik, matikan layar pause (`TogglePause(false)`).
- **`exitButton`**: Kalau diklik, langsung teriak ke atas *"Pemain mau keluar nih!"* (`OnExitClicked`). Nanti yang urus pindah layarnya adalah Bos Kuis.
- Di awal permainan, layar menu pause disembunyikan agar tidak menutupi soal kuis.

### 2. Jurus Mengaktifkan Pause (`TogglePause`)
```csharp
public void TogglePause(bool pause)
{
    if (pauseCanvas != null)
        pauseCanvas.SetActive(pause); // Nyalakan/Matikan menu Pause
    
    OnPauseToggled?.Invoke(pause); // Lapor!
}
```
- Fungsi ini sangat penting karena dia bertugas menyalakan atau mematikan tampilan menu.
- Tapi yang lebih penting adalah baris kedua: `OnPauseToggled?.Invoke(pause);`. Ini adalah teriakan dari script ini ke `QuizManager` untuk memberitahu bahwa game sedang di-pause, sehingga waktu (timer) bisa dihentikan sementara.

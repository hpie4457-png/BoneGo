# Dokumentasi Script: `ARView.cs`

## 📖 Apa fungsi script ini?
Script ini bertugas mengurus **Layar Antarmuka (UI)** saat pemain sedang berada di mode Kamera AR (Augmented Reality). 
Tugas utamanya sangat simpel:
1. **Tombol Kembali**: Memberikan fungsi pada tombol "Home" agar pemain bisa kembali ke menu utama.
2. **Mengatur Suara**: Mengecilkan volume lagu latar (Background Music) secara otomatis saat masuk ke mode AR, agar suara narasi (penjelasan tulang) bisa terdengar jelas. Saat keluar dari AR, lagu akan dibesarkan kembali.

---

## 💻 Kode Lengkap

```csharp
using UnityEngine;
using UnityEngine.UI;
using QuizSystem.Core;

namespace QuizSystem.UI
{
    /// <summary>
    /// Controller for the AR View.
    /// Handles navigation back to the Main Menu.
    /// </summary>
    public class ARView : MonoBehaviour
    {
        [Header("── UI Buttons ──────────────────────────")]
        [SerializeField] private Button homeButton;

        private void Awake()
        {
            if (homeButton != null)
            {
                homeButton.onClick.AddListener(OnHomeButtonClicked);
            }
            else
            {
                Debug.LogWarning("[ARView] Home Button is not assigned in the inspector.");
            }
        }

        private void Start()
        {
            if (AudioManager.Instance != null)
            {
                // Smoothly lower the background volume in AR Scene so TTS is clearly audible (1 second fade)
                AudioManager.Instance.SetBGMVolume(0.15f, 1.0f);
            }
        }

        private void OnDestroy()
        {
            if (homeButton != null)
            {
                homeButton.onClick.RemoveListener(OnHomeButtonClicked);
            }
            
            if (AudioManager.Instance != null)
            {
                // Smoothly restore normal background volume when exiting AR Scene (1 second fade)
                AudioManager.Instance.SetBGMVolume(1.0f, 1.0f);
            }
        }

        private void OnHomeButtonClicked()
        {
            if (SceneLoader.Instance != null)
            {
                SceneLoader.Instance.LoadMainMenu();
            }
            else
            {
                Debug.LogWarning("[ARView] SceneLoader.Instance not found. Falling back to direct scene loading.");
                UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
            }
        }
    }
}
```

---

## 🧩 Penjelasan Cara Kerjanya (Block per Block)

Mari kita pahami langkah demi langkah:

### 1. Mengenalkan Tombol Home
```csharp
[Header("── UI Buttons ──────────────────────────")]
[SerializeField] private Button homeButton;
```
- Ini adalah tempat kita mendaftarkan tombol "Home" yang ada di layar. `[SerializeField]` adalah kode khusus agar kita bisa memasukkan tombol tersebut langsung dari tampilan Unity Editor (tanpa harus ngoding keras).

### 2. Persiapan Sebelum Layar Muncul (`Awake`)
```csharp
private void Awake()
{
    if (homeButton != null)
    {
        homeButton.onClick.AddListener(OnHomeButtonClicked);
    }
    // ... peringatan kalau tombol lupa dimasukkan
}
```
- **`Awake()`** adalah fungsi yang dipanggil paling pertama kali saat objek ini diaktifkan, bahkan sebelum game benar-benar berjalan.
- Di sini kita memberi tahu tombol Home: *"Hei tombol, kalau kamu diklik (`onClick`), jalankan fungsi yang namanya `OnHomeButtonClicked` ya!"*

### 3. Saat Layar AR Mulai Berjalan (`Start`)
```csharp
private void Start()
{
    if (AudioManager.Instance != null)
    {
        AudioManager.Instance.SetBGMVolume(0.15f, 1.0f);
    }
}
```
- **`Start()`** dipanggil sesaat setelah `Awake()`.
- Di sini, kita memanggil pengatur suara (`AudioManager`). Kita perintahkan dia untuk menurunkan volume lagu latar menjadi sangat pelan (`0.15f` atau 15%). Angka `1.0f` di sebelahnya berarti proses pengecilan suaranya dilakukan secara halus selama 1 detik.

### 4. Saat Layar AR Ditutup atau Dihancurkan (`OnDestroy`)
```csharp
private void OnDestroy()
{
    if (homeButton != null)
    {
        homeButton.onClick.RemoveListener(OnHomeButtonClicked);
    }
    
    if (AudioManager.Instance != null)
    {
        AudioManager.Instance.SetBGMVolume(1.0f, 1.0f);
    }
}
```
- **`OnDestroy()`** adalah fungsi bersih-bersih. Ini dipanggil saat kita pindah dari layar AR (objek layar ini dihancurkan).
- Pertama, kita melepaskan tugas dari tombol Home.
- Kedua, kita mengembalikan volume lagu (`AudioManager`) kembali ke penuh (`1.0f` atau 100%) dengan durasi 1 detik juga. Supaya di menu utama lagunya kencang lagi.

### 5. Apa yang Terjadi Saat Tombol Diklik? (`OnHomeButtonClicked`)
```csharp
private void OnHomeButtonClicked()
{
    if (SceneLoader.Instance != null)
    {
        SceneLoader.Instance.LoadMainMenu();
    }
    // ...
}
```
- Ini adalah perintah yang akan dijalankan ketika pemain memencet tombol Home.
- Kita menyuruh "Tukang Pindah Layar" (`SceneLoader`) untuk memuat ulang layar Menu Utama (`LoadMainMenu`).

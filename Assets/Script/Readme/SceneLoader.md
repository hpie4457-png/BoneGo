# Dokumentasi Script: `SceneLoader.cs`

## 📖 Apa fungsi script ini?
Script ini adalah **"Tukang Transportasi"** atau Supir di game kita.
Tugas tunggalnya adalah **memindahkan pemain dari satu layar (Scene) ke layar lainnya**. Misalnya dari Menu Utama pindah ke Kuis, atau dari Kuis pindah ke Kamera AR.

---

## 💻 Kode Lengkap

```csharp
using UnityEngine;
using UnityEngine.SceneManagement; // Alat dari Unity khusus buat pindah layar

namespace QuizSystem.Core
{
    public class SceneLoader : MonoBehaviour
    {
        // Menjadikan supir ini sebagai BOS TUNGGAL (Singleton)
        public static SceneLoader Instance { get; private set; }

        [Header("Scene Names")]
        [SerializeField] private string mainMenuScene = "MainMenu"; // Nama file layar menu
        [SerializeField] private string quizScene = "QuizScene"; // Nama file layar kuis
        [SerializeField] private string arCameraScene = "ARCameraScene"; // Nama file layar kamera

        private void Awake()
        {
            // Aturan Singleton: Cuma boleh ada 1 Tukang Pindah Layar di game ini
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject); // Supir ini tidak boleh hancur saat pindah layar
        }

        // Tombol pintasan untuk pindah ke menu tertentu
        public void LoadMainMenu() { LoadScene(mainMenuScene); }
        public void LoadQuiz() { LoadScene(quizScene); }
        public void LoadARCamera() { LoadScene(arCameraScene); }

        // Fungsi Inti untuk pindah layar
        public void LoadScene(string sceneName)
        {
            if (string.IsNullOrEmpty(sceneName)) return; // Kalau namanya kosong, diam saja
            
            // Perintah sihir Unity untuk memuat layar baru
            SceneManager.LoadScene(sceneName); 
        }

        // Fungsi untuk mengulang layar saat ini (misal mau main lagi)
        public void ReloadCurrentScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        // Fungsi untuk mematikan game (Keluar ke HP)
        public void QuitGame()
        {
            Application.Quit();
        }
    }
}
```

---

## 🧩 Penjelasan Cara Kerjanya (Block per Block)

### 1. Sang Supir Abadi (`Singleton` & `DontDestroyOnLoad`)
```csharp
private void Awake()
{
    if (Instance != null && Instance != this) { Destroy(gameObject); return; }
    
    Instance = this;
    DontDestroyOnLoad(gameObject);
}
```
- Seperti `AudioManager`, `SceneLoader` ini juga menggunakan sistem **Singleton**. Artinya dia adalah supir tunggal di dalam game.
- `DontDestroyOnLoad`: Ketika pemain berpindah dari Menu ke Kuis, biasanya semua barang di layar Menu akan dihancurkan. Tapi supir ini diberi kekebalan! Dia tidak akan hancur dan akan ikut terbawa ke layar Kuis, supaya nanti bisa menyetir kembali ke layar lain.

### 2. Jalan Pintas (`LoadMainMenu`, `LoadQuiz`, dll)
```csharp
public void LoadMainMenu() { LoadScene(mainMenuScene); }
```
- Script lain yang mau pindah layar tidak perlu repot mengingat nama file layarnya (misal *"MainMenu"*). Mereka cukup teriak: *"Pak Supir, LoadMainMenu dong!"*.
- Supir ini yang akan menerjemahkannya dan melakukan perpindahan.

### 3. Mesin Pemindah (`LoadScene`)
```csharp
public void LoadScene(string sceneName)
{
    SceneManager.LoadScene(sceneName); 
}
```
- Ini adalah perintah asli bawaan dari Unity (`SceneManager`). Dia yang bertugas menghancurkan layar lama dan membangun layar baru di dalam memori HP.

### 4. Mengulang dan Keluar
- **`ReloadCurrentScene()`**: Fungsi untuk bertanya kepada sistem *"Saya sekarang ada di layar apa ya?"* (`GetActiveScene().name`), lalu memuat ulang layar tersebut dari awal. Cocok untuk tombol "Main Lagi / Restart".
- **`QuitGame()`**: Fungsi untuk menutup aplikasinya (kembali ke menu utama HP). `Application.Quit()` hanya bekerja kalau gamenya sudah di-build (di HP Android/iPhone), dan tidak akan mempan saat dicoba di dalam Unity Editor.

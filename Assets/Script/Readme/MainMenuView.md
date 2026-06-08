# Dokumentasi Script: `MainMenuView.cs`

## 📖 Apa fungsi script ini?
Script ini sangat sederhana, dia adalah **"Pengurus Layar Menu Utama"**.
Tugas utamanya hanya dua:
1. **Memainkan Lagu Pembuka**: Menyuruh Manajer Suara (`AudioManager`) memutar lagu ceria khas menu utama.
2. **Mengurus Tombol Pindah Layar**: Kalau pemain memencet tombol "Mulai Kuis", dia pindah ke layar kuis. Kalau pemain memencet tombol "Kamera AR", dia pindah ke layar AR.

---

## 💻 Kode Lengkap

```csharp
using UnityEngine;
using UnityEngine.UI;
using QuizSystem.Core;

namespace QuizSystem.UI
{
    public class MainMenuView : MonoBehaviour
    {
        [Header("── Buttons ─────────────────────────────")]
        [SerializeField] private Button quizButton;
        [SerializeField] private Button arCameraButton;

        private void Awake()
        {
            if (quizButton != null)
                quizButton.onClick.AddListener(OnQuizButtonClicked);

            if (arCameraButton != null)
                arCameraButton.onClick.AddListener(OnARCameraButtonClicked);
        }

        private void Start()
        {
            if (AudioManager.Instance != null)
                AudioManager.Instance.PlayMainMenuBGM();
        }

        private void OnQuizButtonClicked()
        {
            if (SceneLoader.Instance != null)
                SceneLoader.Instance.LoadQuiz();

        }

        private void OnARCameraButtonClicked()
        {
            if (SceneLoader.Instance != null)
                SceneLoader.Instance.LoadARCamera();

        }
    }
}
```

---

## 🧩 Penjelasan Cara Kerjanya (Block per Block)

### 1. Mengenalkan Tombol-tombol (`Awake`)
```csharp
[SerializeField] private Button quizButton;
[SerializeField] private Button arCameraButton;

private void Awake()
{
    if (quizButton != null)
        quizButton.onClick.AddListener(OnQuizButtonClicked);

    if (arCameraButton != null)
        arCameraButton.onClick.AddListener(OnARCameraButtonClicked);
}
```
- Seperti menyiapkan pelayan restoran. Kita mengenalkan dua tombol utama: `quizButton` dan `arCameraButton`.
- **`Awake()`**: Saat game pertama kali dibuka, kita memberi instruksi: *"Tombol Kuis, kalau kamu ditekan, jalankan tugas `OnQuizButtonClicked`. Tombol AR, kalau ditekan jalankan `OnARCameraButtonClicked`!"*

### 2. Menyambut dengan Musik (`Start`)
```csharp
private void Start()
{
    if (AudioManager.Instance != null)
        AudioManager.Instance.PlayMainMenuBGM();
}
```
- **`Start()`**: Sesaat setelah layar menu utama muncul, script ini memanggil Sang Manajer Suara (`AudioManager`).
- Dia memberi perintah: *"Hei DJ, tolong putarkan lagu Menu Utama (`PlayMainMenuBGM`) sekarang!"*

### 3. Eksekusi Pindah Layar
```csharp
private void OnQuizButtonClicked()
{
    SceneLoader.Instance.LoadQuiz();
}

private void OnARCameraButtonClicked()
{
    SceneLoader.Instance.LoadARCamera();
}
```
- Ini adalah aksi yang terjadi ketika tombol ditekan.
- Alih-alih memindahkan layar sendiri, script ini meminta bantuan teman baiknya, yaitu si **"Tukang Pindah Layar" (`SceneLoader`)** untuk melakukan pekerjaannya.
- `LoadQuiz()` untuk memuat arena kuis.
- `LoadARCamera()` untuk memuat kamera AR.

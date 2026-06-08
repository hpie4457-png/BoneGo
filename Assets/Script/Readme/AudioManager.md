# Dokumentasi Script: `AudioManager.cs`

## 📖 Apa fungsi script ini?
Script ini adalah **"Manajer Suara"** atau DJ utama di dalam game kita.
Fungsi utamanya adalah:
1. **Memainkan Musik & Efek Suara**: Mengatur jalannya lagu latar (BGM) dan efek suara (SFX) seperti suara benar/salah.
2. **Tidak Hilang Saat Pindah Layar**: Script ini spesial karena dia akan terus hidup (tidak hancur) meskipun pemain pindah dari menu utama ke kuis atau kamera AR. Lagunya jadi tidak terpotong!
3. **Efek Halus (Fading & Ducking)**: Bisa mengecilkan atau membesarkan lagu secara halus (Fading), dan otomatis mengecilkan lagu latar sesaat ketika ada efek suara yang berbunyi (Ducking).

---

## 💻 Kode Lengkap

*(Catatan: Kode cukup panjang, kita bisa lihat potongan-potongannya di bagian penjelasan)*

```csharp
using UnityEngine;

namespace QuizSystem.Core
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }

        [Header("── Audio Sources ───────────────────────")]
        [SerializeField] private AudioSource bgmSource;
        [SerializeField] private AudioSource sfxSource;

        [Header("── Audio Clips ─────────────────────────")]
        [SerializeField] private AudioClip mainMenuBGM;
        [SerializeField] private AudioClip quizBGM;
        [SerializeField] private AudioClip correctSFX;
        [SerializeField] private AudioClip wrongSFX;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        // ... (Kode lengkap lainnya ada di file AudioManager.cs asli)
        // Kita akan bahas bagian-bagian pentingnya di bawah ini!
    }
}
```

---

## 🧩 Penjelasan Cara Kerjanya (Block per Block)

### 1. Menjadikannya Sang Penguasa Tunggal (Singleton)
```csharp
public static AudioManager Instance { get; private set; }

private void Awake()
{
    if (Instance == null)
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    else
    {
        Destroy(gameObject);
    }
}
```
- **Singleton**: Ini adalah teknik agar di seluruh game, **hanya boleh ada 1** Manajer Suara.
- `DontDestroyOnLoad`: Perintah sakti yang artinya *"Tolong jangan hancurkan objek ini walau layarnya berganti"*. Ini yang membuat musik bisa terus berputar saat kita pindah scene (layar).
- Jika tiba-tiba ada Manajer Suara kedua yang muncul (misal saat kita balik ke Menu Utama), dia akan langsung dihancurkan (`Destroy`) agar tidak ada 2 lagu yang bertabrakan.

### 2. Memutar Musik dan Efek
```csharp
public void PlayMainMenuBGM() { PlayBGM(mainMenuBGM); }
public void PlayQuizBGM(float fadeDuration = 1.0f) { PlayBGM(quizBGM, fadeDuration); }

public void PlayCorrectSFX() { PlayOneShot(correctSFX); }
public void PlayWrongSFX() { PlayOneShot(wrongSFX); }
```
- Ini adalah tombol-tombol yang bisa dipencet oleh script lain. Misalnya, script Kuis tinggal memanggil `PlayCorrectSFX()` kalau pemain menjawab benar, dan sang Manajer Suara akan membunyikannya.

### 3. Mengatur Volume dengan Halus (Fading)
```csharp
public void SetBGMVolume(float volume, float fadeDuration = 0f)
{
    // ... mengecek apakah butuh waktu untuk mengecilkan/membesarkan suara
}
```
- Daripada suara musik tiba-tiba putus atau tiba-tiba kencang (yang bikin kaget), fungsi ini menggunakan **Coroutine** (proses yang berjalan berulang-ulang seiring waktu) untuk membesarkan/mengecilkan suara sedikit demi sedikit selama beberapa detik.

### 4. Menurunkan Suara Latar Sementara (Ducking)
```csharp
private void PlayOneShot(AudioClip clip)
{
    if (clip != null && sfxSource != null)
    {
        sfxSource.PlayOneShot(clip); // Putar efek suara
        
        // Kalau lagu latar lagi nyala, kecilkan sementara!
        if (bgmSource != null && bgmSource.isPlaying)
        {
            StartCoroutine(DuckingCoroutine(clip.length));
        }
    }
}
```
- **Ducking**: Pernah dengar penyiar radio bicara dan tiba-tiba musik di belakangnya mengecil? Itu namanya Ducking.
- Saat ada efek suara (SFX) yang diputar, script ini menyuruh lagu latar untuk mengecil ke 30% (`duckVolume = 0.3f`), lalu setelah efek suaranya selesai, lagu latar dibesarkan kembali ke 100%. Keren kan!

# Dokumentasi Script: `SafeArea.cs`

## 📖 Apa fungsi script ini?
Script ini adalah **"Si Penyelamat Layar HP Poni (Notch)"**.
Pernah main game lalu ada tombol yang tertutup kamera depan (poni layar) atau tertutup pinggiran lengkung di HP modern?
Tugas script ini adalah **mendorong layar permainan (UI)** agak ke tengah, supaya tidak ada tombol yang tertutup oleh poni kamera tersebut. Area aman ini disebut *Safe Area*.

---

## 💻 Kode Lengkap

*(Ini adalah versi yang disederhanakan)*

```csharp
using UnityEngine;

// Memaksa objek ini harus punya komponen pengaturan posisi kotak (RectTransform)
[RequireComponent(typeof(RectTransform))]
public class SafeArea : MonoBehaviour
{
    private RectTransform rectTransform;
    private Rect lastSafeArea = Rect.zero;
    private ScreenOrientation lastOrientation;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        Refresh(); // Coba paskan layarnya saat pertama main
    }

    private void Update()
    {
        Refresh(); // Cek terus layarnya setiap detik
    }

    private void Refresh()
    {
        // Tanya ke sistem HP: "Area yang aman dari poni mana saja?"
        Rect safeArea = Screen.safeArea;

        // Kalau ukuran layarnya berubah (misal HP diputar miring), atur ulang posisinya!
        if (safeArea != lastSafeArea || Screen.orientation != lastOrientation)
        {
            lastOrientation = Screen.orientation;
            ApplySafeArea(safeArea);
        }
    }

    // ... (Fungsi matematika rumit ada di bawah)
}
```

---

## 🧩 Penjelasan Cara Kerjanya (Block per Block)

### 1. Mengecek Secara Berkala (`Update` & `Refresh`)
```csharp
private void Refresh()
{
    Rect safeArea = Screen.safeArea;

    if (safeArea != lastSafeArea || Screen.orientation != lastOrientation)
    {
        // Lakukan penyesuaian layar!
    }
}
```
- Script ini akan bertanya kepada HP (`Screen.safeArea`): *"Berapa sih area layar yang aman?"*
- Kenapa harus dicek terus di `Update`? Karena pemain bisa memutar HP-nya (dari berdiri menjadi tidur/miring). Saat HP diputar, posisi poni kamera akan berpindah, jadi gamenya juga harus ikut menyesuaikan.

### 2. Mendorong Tampilan (Matematika `ApplySafeArea`)
Di bagian bawah script asli ada fungsi `ApplySafeArea`.
- Fungsi itu melakukan hitungan matematika dengan membagi batas aman (`safeArea.position`) dengan lebar dan tinggi layar HP secara keseluruhan (`Screen.width` & `Screen.height`).
- Tujuannya adalah mencari **Persentase** (misalnya 0 sampai 1).
- Lalu persentase ini disetorkan ke `rectTransform.anchorMin` dan `anchorMax`.
- Efeknya: Kalau di HP yang ada poninya di kiri, seluruh gambar permainan di game kita akan tergeser sedikit ke kanan, menyelamatkan tombol-tombol agar bisa tetap dipencet!

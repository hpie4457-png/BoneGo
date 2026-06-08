# Dokumentasi Script: `QuizAlertManager.cs`

## 📖 Apa fungsi script ini?
Script ini adalah **"Si Pembuat Notifikasi / Pop-up"**.
Tugasnya sangat spesifik: Memunculkan pop-up pemberitahuan besar di tengah layar saat pemain sedang menjawab kuis. 
Misalnya, memunculkan gambar centang besar *"BENAR!"*, atau silang besar *"SALAH!"*, atau peringatan *"WAKTU HABIS!"* (Warning).

---

## 💻 Kode Lengkap

```csharp
using UnityEngine;

namespace QuizSystem.UI
{
    // Tiga jenis notifikasi yang tersedia
    public enum AlertType { Correct, Wrong, Warning }

    public class QuizAlertManager : MonoBehaviour
    {
        [Header("Containers")]
        [SerializeField] private Transform alertContainer; // Tempat naruh pop-up

        [Header("Prefabs")] // Cetakan (blueprint) untuk masing-masing pop-up
        [SerializeField] private GameObject correctPrefab;
        [SerializeField] private GameObject wrongPrefab;
        [SerializeField] private GameObject warningPrefab;

        public void ShowAlert(AlertType type)
        {
            ClearAlerts(); // Bersihkan dulu pop-up sebelumnya

            if (alertContainer == null) return;
            alertContainer.gameObject.SetActive(true); // Tampilkan tempat pop-up

            // Pilih mau pakai cetakan yang mana?
            GameObject prefab = type switch
            {
                AlertType.Correct => correctPrefab,
                AlertType.Wrong => wrongPrefab,
                AlertType.Warning => warningPrefab,
                _ => null
            };

            // Cetak ke layar!
            if (prefab != null)
            {
                Instantiate(prefab, alertContainer);
            }
        }

        public void ClearAlerts()
        {
            if (alertContainer == null) return;
            
            // Hancurkan semua pop-up yang sedang tampil
            foreach (Transform child in alertContainer)
            {
                Destroy(child.gameObject);
            }
            
            alertContainer.gameObject.SetActive(false); // Sembunyikan tempatnya
        }
    }
}
```

---

## 🧩 Penjelasan Cara Kerjanya (Block per Block)

### 1. Menyiapkan Jenis dan Cetakan Pop-up
```csharp
public enum AlertType { Correct, Wrong, Warning }

[SerializeField] private Transform alertContainer;
[SerializeField] private GameObject correctPrefab;
[SerializeField] private GameObject wrongPrefab;
[SerializeField] private GameObject warningPrefab;
```
- **`AlertType`**: Ini adalah pilihan menunya. Ada 3 rasa: Benar, Salah, Peringatan Waktu Habis.
- **`Prefab`**: Ibarat cetakan kue. Daripada bikin gambar centang, warna hijau, dan tulisan "Benar" setiap kali secara manual, kita bikin cetakannya 1 kali di Unity. Script ini tinggal panggil cetakannya kalau butuh.

### 2. Memunculkan Pop-up (`ShowAlert`)
```csharp
public void ShowAlert(AlertType type)
{
    ClearAlerts(); // Sapu bersih dulu
    alertContainer.gameObject.SetActive(true);

    GameObject prefab = type switch
    {
        AlertType.Correct => correctPrefab,
        AlertType.Wrong => wrongPrefab,
        AlertType.Warning => warningPrefab,
        _ => null
    };

    if (prefab != null)
    {
        Instantiate(prefab, alertContainer);
    }
}
```
- Saat pengatur kuis (QuizManager) memanggil fungsi ini, dia harus pesan mau pop-up yang mana (Benar/Salah/Waktu Habis).
- Fungsi `switch` itu kerjanya seperti saklar pilihan: *"Oh, kamu minta yang 'Correct'? Oke aku pakai cetakan 'correctPrefab'."*
- `Instantiate` adalah perintah untuk meletakkan hasil cetakan itu ke layar (di dalam `alertContainer`).

### 3. Membersihkan Pop-up (`ClearAlerts`)
```csharp
public void ClearAlerts()
{
    foreach (Transform child in alertContainer)
    {
        Destroy(child.gameObject);
    }
    
    alertContainer.gameObject.SetActive(false);
}
```
- Saat soal berganti, layar harus bersih lagi.
- Fungsi ini keliling-keliling di dalam wadah pop-up (`alertContainer`), menemukan pop-up yang nyisa, dan menghancurkannya (`Destroy`).
- Setelah bersih, wadah pop-upnya dimatikan agar tidak menghalangi pemain pencet-pencet tombol.

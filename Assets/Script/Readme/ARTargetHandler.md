# Dokumentasi Script: `ARTargetHandler.cs`

## 📖 Apa fungsi script ini?
Bayangkan script ini sebagai **"Mata dan Mulut"** dari kamera AR (Augmented Reality) kita. 
Fungsi utamanya sangat sederhana:
1. **Mendeteksi Target**: Mengenali saat kamera melihat sebuah gambar marker (target).
2. **Memunculkan Tulang**: Saat target terlihat, ia "berteriak" ke sistem untuk memunculkan model 3D tulang yang sesuai dengan gambar tersebut.
3. **Menyembunyikan Tulang**: Kalau gambar sudah tidak terlihat oleh kamera, ia menyuruh sistem untuk menyembunyikan tulang tersebut.

---

## 💻 Kode Lengkap

```csharp
using UnityEngine;
using Vuforia;

public class ARTargetHandler : MonoBehaviour
{
    public BoneViewer boneViewer;
    public string targetName;

    private ObserverBehaviour observer;

    void Start()
    {
        observer = GetComponent<ObserverBehaviour>();

        if (observer != null)
        {
            observer.OnTargetStatusChanged += OnTargetStatusChanged;
        }
    }

    private void OnTargetStatusChanged(ObserverBehaviour behaviour, TargetStatus status)
    {
        if (status.Status == Status.TRACKED || status.Status == Status.EXTENDED_TRACKED)
        {
            OnTargetDetected();
        }
        else
        {
            OnTargetLost();
        }
    }

    public void OnTargetDetected()
    {
        boneViewer.ShowBoneById(targetName);
        Debug.Log("Target Name : " + targetName);
    }

    public void OnTargetLost()
    {
        boneViewer.OnTargetLost(targetName);
        Debug.Log("Target Lost : " + targetName);
    }
}
```

---

## 🧩 Penjelasan Cara Kerjanya (Block per Block)

Mari kita bedah kodenya pelan-pelan agar mudah dipahami:

### 1. Menyiapkan Variabel (Bahan-bahan)
```csharp
public BoneViewer boneViewer;
public string targetName;
private ObserverBehaviour observer;
```
- **`boneViewer`**: Ini ibarat "Tukang Sulap" yang bertugas memunculkan objek 3D. Kita memberitahu script ini siapa tukang sulapnya.
- **`targetName`**: Ini adalah nama marker atau gambar yang dicari (misalnya "TulangTangan").
- **`observer`**: Ini adalah komponen bawaan Vuforia (mesin AR kita) yang tugasnya benar-benar cuma "melototin" kamera untuk mencari marker.

### 2. Saat Permainan Pertama Kali Mulai (`Start`)
```csharp
void Start()
{
    observer = GetComponent<ObserverBehaviour>();

    if (observer != null)
    {
        observer.OnTargetStatusChanged += OnTargetStatusChanged;
    }
}
```
- **`Start()`** adalah fungsi yang dipanggil **hanya sekali** di awal saat objek ini muncul.
- Di sini, kita mencari komponen **Mata Vuforia** (`ObserverBehaviour`) yang menempel pada objek yang sama.
- Lalu kita bilang ke mata itu: *"Hei, kalau status kamu berubah (misal dari 'gak lihat apa-apa' jadi 'lihat gambar'), tolong panggil fungsi `OnTargetStatusChanged` ya!"*

### 3. Mengecek Perubahan Status (`OnTargetStatusChanged`)
```csharp
private void OnTargetStatusChanged(ObserverBehaviour behaviour, TargetStatus status)
{
    if (status.Status == Status.TRACKED || status.Status == Status.EXTENDED_TRACKED)
    {
        OnTargetDetected();
    }
    else
    {
        OnTargetLost();
    }
}
```
- Fungsi ini seperti penjaga gerbang. Setiap ada perubahan, dia mengecek: *"Apakah gambarnya sekarang terlihat jelas (`TRACKED`)?"*
- **Jika YA**: Dia memanggil perintah `OnTargetDetected()` (Target Ditemukan).
- **Jika TIDAK**: Dia memanggil perintah `OnTargetLost()` (Target Hilang).

### 4. Kalau Gambar Terlihat! (`OnTargetDetected`)
```csharp
public void OnTargetDetected()
{
    boneViewer.ShowBoneById(targetName);
    Debug.Log("Target Name : " + targetName);
}
```
- Menyuruh si tukang sulap (`boneViewer`) untuk memunculkan 3D tulang berdasarkan nama targetnya.
- `Debug.Log` hanya untuk mencetak pesan di sistem pembuat (Unity Console) agar pembuat game tahu *"Oh, marker ini berhasil terbaca!"*

### 5. Kalau Gambar Hilang! (`OnTargetLost`)
```csharp
public void OnTargetLost()
{
    boneViewer.OnTargetLost(targetName);
    Debug.Log("Target Lost : " + targetName);
}
```
- Menyuruh si tukang sulap untuk menyembunyikan tulangnya dan menghentikan suara penjelasan.

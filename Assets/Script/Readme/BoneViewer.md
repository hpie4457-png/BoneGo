# Dokumentasi Script: `BoneViewer.cs`

## 📖 Apa fungsi script ini?
Script ini adalah **"Tukang Sulap Tulang"** di layar AR (Kamera).
Tugas utamanya sangat banyak dan penting:
1. **Membaca Data Buku Pintar**: Membaca file tulisan (`.json`) yang berisi daftar nama-nama tulang.
2. **Memunculkan Tulang 3D**: Kalau kamera mendeteksi gambar, dia memunculkan objek 3D tulang yang sesuai.
3. **Membaca Tulisan Menjadi Suara (TTS)**: Dia mengambil nama tulang, lalu mengirimkannya ke Google Translate agar dibacakan (diubah jadi suara) dan diperdengarkan ke pemain.
4. **Tombol Lanjut & Kembali**: Mengatur tombol panah (Next/Previous) agar anak-anak bisa melihat bagian-bagian spesifik dari tulang tersebut satu per satu.

---

## 💻 Kode Lengkap

*(Catatan: Karena kodenya sangat panjang (sekitar 300 baris), kita akan fokus pada potongan-potongan pentingnya di bagian penjelasan).*

---

## 🧩 Penjelasan Cara Kerjanya (Block per Block)

### 1. Menyiapkan Bahan (Variabel)
```csharp
public TextAsset jsonFile;
public TMP_Text boneText;
public AudioSource ttsAudioSource;
```
- **`jsonFile`**: Ibarat buku pintar yang berisi catatan *"Tulang Kaki itu ada apa aja sih?"* (berisi data teks).
- **`boneText`**: Teks di layar HP yang akan menampilkan nama tulang saat ini.
- **`ttsAudioSource`**: Pengeras suara (Speaker) yang tugasnya khusus memutar suara dari Google Translate.

### 2. Membaca Buku Pintar Saat Mulai (`LoadJson`)
```csharp
void LoadJson()
{
    boneData = JsonUtility.FromJson<BoneData>(jsonFile.text);
}
```
- Saat game mulai, script ini langsung membaca file catatan (`.json`) dan mengubahnya menjadi data yang bisa dimengerti oleh Unity (`BoneData`).

### 3. Saat Kamera Melihat Gambar Marker (`ShowBoneById`)
```csharp
public void ShowBoneById(string boneId)
{
    // Mencari tulang di dalam daftar catatan buku pintar
    for (int i = 0; i < boneData.items.Count; i++)
    {
        if (boneData.items[i].id == boneId)
        {
            // Tulang ketemu! Tampilkan dan bacakan suaranya
            UpdateTextAndObject();
            return;
        }
    }
}
```
- Script `ARTargetHandler` (yang kita bahas sebelumnya) akan memanggil fungsi ini dan bilang: *"Hei, aku lihat gambar 'TulangTangan' nih!"*.
- Script ini akan mencari tulisan "TulangTangan" di catatannya, kalau ketemu, dia akan menyuruh sistem untuk menampilkannya ke layar (`UpdateTextAndObject`).

### 4. Memunculkan 3D yang Tepat (`ShowOnlySelectedBone`)
```csharp
void ShowOnlySelectedBone(string selectedBoneName)
{
    foreach (BoneObjectData boneObject in boneObjects)
    {
        if (boneObject.boneName == selectedBoneName)
        {
            boneObject.boneObject.SetActive(true); // Tampilkan!
        }
        else
        {
            boneObject.boneObject.SetActive(false); // Sembunyikan!
        }
    }
}
```
- Ini cara kerjanya ibarat saklar lampu. Dia akan mengecek satu per satu daftar 3D tulang yang kita punya. Kalau namanya cocok dengan yang mau ditampilkan, saklarnya dinyalakan (`SetActive(true)`). Yang lainnya dimatikan (`SetActive(false)`).

### 5. Meminta Google Membacakan Teks (TTS - Text To Speech)
```csharp
private IEnumerator FetchAndPlayTTS(string text)
{
    // Kunci tombol panah supaya anak-anak tidak pencet-pencet terus saat loading
    SetButtonsInteractable(false);

    // Kirim teks ke Google Translate
    string url = $"https://translate.google.com/translate_tts?...&text={text}&tl=id";
    using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.MPEG))
    {
        yield return www.SendWebRequest(); // Tunggu sampai suaranya selesai di-download

        // Putar suaranya!
        AudioClip clip = DownloadHandlerAudioClip.GetContent(www);
        ttsAudioSource.clip = clip;
        ttsAudioSource.Play();
    }
    
    // Buka kunci tombol panah lagi
    SetButtonsInteractable(true);
}
```
- Ini adalah bagian paling ajaib! Script ini menyambungkan game kita ke **Internet (Google Translate)**.
- Dia mengirim tulisan (misal: "Tulang Paha") dan meminta Google membalas dengan file suara berbahasa Indonesia (`tl=id`).
- Selama menunggu suara didownload, tombol "Next" dan "Previous" dimatikan sementara supaya anak-anak tidak nge-spam (tekan berkali-kali) yang bisa membuat gamenya error. Setelah suara siap, tombol dinyalakan lagi.

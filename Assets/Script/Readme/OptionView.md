# Dokumentasi Script: `OptionView.cs`

## 📖 Apa fungsi script ini?
Bayangkan satu soal pilihan ganda punya 4 tombol jawaban (A, B, C, D). Nah, script ini tugasnya mengatur **1 buah tombol jawaban** saja.
Tugas utamanya:
1. **Menampilkan Teks Pilihan**: Menuliskan huruf (A/B/C/D) dan teks jawabannya di tombol.
2. **Berubah Warna**: Tombol ini pintar, dia bisa berganti warna. Kalau jawabannya benar dia bisa berubah jadi hijau, kalau salah berubah jadi merah, dan kalau belum dijawab warnanya biasa saja.
3. **Melapor Saat Ditekan**: Kalau pemain memencet tombol ini, dia akan lapor ke sistem utama, *"Hei, pemain memilih aku (jawaban nomor 2)!"*

---

## 💻 Kode Lengkap

*(Kode aslinya ada sekitar 130 baris, kita bahas bagian intinya saja agar tidak pusing)*

```csharp
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace QuizSystem.UI
{
    // Ini daftar status yang bisa dimiliki tombol: Biasa, Benar, atau Salah
    public enum OptionState { Default, Correct, Wrong }

    public class OptionView : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private Image backgroundImage;
        [SerializeField] private TextMeshProUGUI labelText;
        [SerializeField] private TextMeshProUGUI choiceText;
        [SerializeField] private Button button;

        // ... Variabel warna untuk status Default, Correct (Benar), dan Wrong (Salah)

        // Variabel untuk mengingat tombol ini nomor berapa (0, 1, 2, dst)
        public int OptionIndex { get; private set; }
        
        // Alat untuk lapor kalau ditekan
        public System.Action<int> OnSelected;

        // ── Fungsi-fungsi Utama di bawah ini ──
    }
}
```

---

## 🧩 Penjelasan Cara Kerjanya (Block per Block)

### 1. Mempersiapkan Tombol Saat Muncul (`Setup`)
```csharp
public void Setup(int index, string choiceValue)
{
    OptionIndex = index;
    // Mengubah angka indeks menjadi huruf A, B, C, D
    labelText.text = index < Labels.Length ? Labels[index] : (index + 1).ToString();
    choiceText.text = choiceValue;

    SetState(OptionState.Default); // Jadikan warna standar dulu
    button.interactable = true; // Pastikan tombol bisa dipencet

    // Kalau tombol dipencet, lapor nomor urutannya!
    button.onClick.RemoveAllListeners();
    button.onClick.AddListener(() => OnSelected?.Invoke(OptionIndex));
}
```
- Ini adalah fungsi yang dipanggil oleh pengatur kuis untuk "menyetel" tombol.
- Pengatur kuis bilang: *"Hei kamu jadi tombol nomor 1 (B) ya, teks jawabannya 'Tulang Rusuk'."*
- Tombol akan menuruti perintah itu, mengubah teksnya, mengembalikan warnanya ke warna dasar, dan bersiap-siap untuk dipencet.

### 2. Berubah Wujud / Warna (`SetState`)
```csharp
public void SetState(OptionState state)
{
    switch (state)
    {
        case OptionState.Default:
            ApplyColors(defaultBorder, defaultBackground, defaultLabelCircle);
            button.interactable = true; // Bisa dipencet
            break;
        case OptionState.Correct:
            ApplyColors(correctBorder, correctBackground, correctLabelCircle);
            button.interactable = false; // Kunci tombol
            break;
        case OptionState.Wrong:
            ApplyColors(wrongBorder, wrongBackground, wrongLabelCircle);
            button.interactable = false; // Kunci tombol
            break;
    }
}
```
- Ini adalah mesin pengubah warnanya.
- Kalau statusnya **Default**, warnanya biasa dan tombol bisa ditekan.
- Kalau statusnya **Correct (Benar)**, dia akan pakai warna Benar (biasanya hijau) dan tombolnya langsung **dikunci** (`interactable = false`) agar pemain tidak bisa mencet-mencet lagi setelah menjawab.
- Kalau statusnya **Wrong (Salah)**, sama seperti Correct, tapi pakai warna Salah (merah) dan dikunci.

### 3. Mengunci Tombol (`Lock`)
```csharp
public void Lock() => button.interactable = false;
```
- Perintah singkat untuk langsung mengunci tombol agar tidak bisa diklik sama sekali.

### 4. Mengoleskan Warna (`ApplyColors` & `SetImageColor`)
Di bagian bawah script (bagian `Private`), ada fungsi rumit untuk mewarnai gambar tombol tanpa merusak bentuk aslinya (Sprite). Ini semacam efek pewarna di Photoshop yang ditimpa ke atas gambar aslinya.

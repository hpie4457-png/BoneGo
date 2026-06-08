using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;

public class BoneViewer : MonoBehaviour
{
    [Header("JSON File")]
    public TextAsset jsonFile;

    [Header("UI")]
    public TMP_Text boneText;

    [Header("TTS Audio (Online API)")]
    [Tooltip("AudioSource used to play TTS audio clips. If left empty, one will be added automatically.")]
    public AudioSource ttsAudioSource;

    [Tooltip("Automatically play the audio clip when a bone or bone part is loaded.")]
    public bool playAudioOnLoad = true;

    private BoneData boneData;
    private int currentBoneIndex = 0;
    private int currentPartIndex = 0;
    private string activeTargetId = "";
    
    private Coroutine ttsCoroutine;

    public List<BoneObjectData> boneObjects;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button previousButton;

    void Start()
    {
        LoadJson();
        SetButtonsInteractable(false); // Initially disable buttons until a target is tracked
        
        if (boneText != null) boneText.text = "Scan Kartu";

        // Auto-assign or create an AudioSource if not assigned via Inspector
        if (ttsAudioSource == null)
        {
            ttsAudioSource = GetComponent<AudioSource>();

            if (ttsAudioSource == null)
            {
                ttsAudioSource = gameObject.AddComponent<AudioSource>();
                ttsAudioSource.playOnAwake = false;
                ttsAudioSource.loop = false;
                Debug.Log("[BoneViewer] No AudioSource assigned. Created one automatically.");
            }
            else
            {
                Debug.Log("[BoneViewer] AudioSource found on GameObject and assigned automatically.");
            }
        }
    }

    void OnEnable()
    {
        nextButton.onClick.AddListener(NextPart);
        previousButton.onClick.AddListener(PreviousPart);
    }

    void OnDisable()
    {
        nextButton.onClick.RemoveListener(NextPart);
        previousButton.onClick.RemoveListener(PreviousPart);
    }

    void LoadJson()
    {
        boneData = JsonUtility.FromJson<BoneData>(jsonFile.text);
    }

    public void ShowBoneById(string boneId)
    {
        for (int i = 0; i < boneData.items.Count; i++)
        {
            if (boneData.items[i].id == boneId)
            {
                currentBoneIndex = i;
                currentPartIndex = -1; // -1 represents the group overview (all bone parts hidden)
                activeTargetId = boneId;
                SetButtonsInteractable(true);
                UpdateTextAndObject();
                return;
            }
        }

        boneText.text = "Data tidak ditemukan";
    }

    public void OnTargetLost(string boneId)
    {
        if (activeTargetId == boneId)
        {
            activeTargetId = "";
            SetButtonsInteractable(false);
            
            if (boneText != null) boneText.text = "Scan Kartu";

            // Stop fetching and stop any currently playing TTS audio when the target is lost
            if (ttsCoroutine != null)
            {
                StopCoroutine(ttsCoroutine);
            }
            
            if (ttsAudioSource != null && ttsAudioSource.isPlaying)
            {
                ttsAudioSource.Stop();
                Debug.Log("[BoneViewer] TTS audio stopped because target was lost.");
            }
        }
    }

    public void SetButtonsInteractable(bool interactable)
    {
        if (nextButton != null) nextButton.interactable = interactable;
        if (previousButton != null) previousButton.interactable = interactable;
    }

    public void NextPart()
    {
        if (string.IsNullOrEmpty(activeTargetId)) return;

        BoneItem currentBone = boneData.items[currentBoneIndex];

        currentPartIndex++;

        if (currentPartIndex >= currentBone.boneParts.Count)
        {
            currentPartIndex = -1; // Wraps around to show group overview
        }

        UpdateTextAndObject();
    }

    public void PreviousPart()
    {
        if (string.IsNullOrEmpty(activeTargetId)) return;

        BoneItem currentBone = boneData.items[currentBoneIndex];

        currentPartIndex--;

        if (currentPartIndex < -1)
        {
            currentPartIndex = currentBone.boneParts.Count - 1; // Wraps around to the last part
        }

        UpdateTextAndObject();
    }

    void UpdateTextAndObject()
    {
        BoneItem currentBone = boneData.items[currentBoneIndex];

        if (currentPartIndex == -1)
        {
            boneText.text = currentBone.name;
            ShowOnlySelectedBone(""); // Hides all bone objects

            if (playAudioOnLoad)
                PlayTTSAudio(currentBone.name);
        }
        else
        {
            string currentPartName = currentBone.boneParts[currentPartIndex];
            boneText.text = currentPartName;
            ShowOnlySelectedBone(currentPartName);

            if (playAudioOnLoad)
                PlayTTSAudio(currentPartName);
        }
    }

    void ShowOnlySelectedBone(string selectedBoneName)
    {
        foreach (BoneObjectData boneObject in boneObjects)
        {
            Debug.Log("Selected Bone Name : " + selectedBoneName);

            if (!string.IsNullOrEmpty(selectedBoneName) && boneObject.boneName == selectedBoneName)
            {
                Debug.Log("Bone Name : " + boneObject.boneName);
                boneObject.boneObject.SetActive(true);
            }
            else
            {
                boneObject.boneObject.SetActive(false);
            }
        }
    }

    // ─────────────────────────────────────────────────────────────────────
    // TTS Audio Helpers (Online API)
    // ─────────────────────────────────────────────────────────────────────

    /// <summary>
    /// Fetches and plays the TTS audio dynamically from Google Translate API.
    /// </summary>
    private void PlayTTSAudio(string text)
    {
        if (ttsAudioSource == null)
        {
            Debug.LogWarning("[BoneViewer] ttsAudioSource is null. Cannot play TTS audio.");
            return;
        }

        if (ttsCoroutine != null)
        {
            StopCoroutine(ttsCoroutine);
        }
        
        ttsCoroutine = StartCoroutine(FetchAndPlayTTS(text));
    }

    private IEnumerator FetchAndPlayTTS(string text)
    {
        // Disable buttons to prevent kids from spamming while the audio is loading
        SetButtonsInteractable(false);

        // Google Translate unofficial TTS endpoint (Language is Indonesian 'tl=id')
        string url = $"https://translate.google.com/translate_tts?ie=UTF-8&total=1&idx=0&textlen={text.Length}&client=tw-ob&q={UnityWebRequest.EscapeURL(text)}&tl=id";

        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.MPEG))
        {
            Debug.Log($"[BoneViewer] Fetching TTS audio for: {text}");
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"[BoneViewer] Error fetching TTS audio: {www.error}");
            }
            else
            {
                AudioClip clip = DownloadHandlerAudioClip.GetContent(www);
                if (clip != null)
                {
                    ttsAudioSource.Stop();
                    ttsAudioSource.clip = clip;
                    ttsAudioSource.Play();
                    Debug.Log($"[BoneViewer] Playing TTS audio successfully.");
                }
            }
        }
        
        // Re-enable buttons if the target is still active
        if (!string.IsNullOrEmpty(activeTargetId))
        {
            SetButtonsInteractable(true);
        }
    }

    /// <summary>
    /// Replays the TTS audio for the currently displayed bone or bone part.
    /// Can be wired to a UI "Replay" button in the Inspector.
    /// </summary>
    public void PlayCurrentPartAudio()
    {
        if (string.IsNullOrEmpty(activeTargetId)) return;

        BoneItem currentBone = boneData.items[currentBoneIndex];
        string text = (currentPartIndex == -1)
            ? currentBone.name
            : currentBone.boneParts[currentPartIndex];

        PlayTTSAudio(text);
    }
}

[System.Serializable]
public class BoneObjectData
{
    public string boneId;
    public string boneName;
    public GameObject boneObject;
}


[System.Serializable]
public class BoneData
{
    public List<BoneItem> items;
}

[System.Serializable]
public class BoneItem
{
    public string id;
    public string name;
    public List<string> boneParts;
}

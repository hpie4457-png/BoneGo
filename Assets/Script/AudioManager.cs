using UnityEngine;

namespace QuizSystem.Core
{
    /// <summary>
    /// Singleton AudioManager to handle BGM and SFX throughout the game.
    /// Attach this to a persistent GameObject in the first scene.
    /// </summary>
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
            // Singleton Pattern with Persistence
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

        private Coroutine volumeFadeCoroutine;

        // ═══════════════════════════════════════════════════════════════
        // Public Play Methods
        // ═══════════════════════════════════════════════════════════════

        public void SetBGMVolume(float volume, float fadeDuration = 0f)
        {
            if (bgmSource == null) return;

            if (fadeDuration > 0f)
            {
                if (volumeFadeCoroutine != null)
                {
                    StopCoroutine(volumeFadeCoroutine);
                }
                volumeFadeCoroutine = StartCoroutine(FadeVolumeCoroutine(Mathf.Clamp01(volume), fadeDuration));
            }
            else
            {
                if (volumeFadeCoroutine != null)
                {
                    StopCoroutine(volumeFadeCoroutine);
                    volumeFadeCoroutine = null;
                }
                bgmSource.volume = Mathf.Clamp01(volume);
            }
        }

        public void PlayMainMenuBGM()
        {
            PlayBGM(mainMenuBGM);
        }

        public void PlayQuizBGM(float fadeDuration = 1.0f)
        {
            PlayBGM(quizBGM, fadeDuration);
        }

        public void PlayCorrectSFX()
        {
            PlayOneShot(correctSFX);
        }

        public void PlayWrongSFX()
        {
            PlayOneShot(wrongSFX);
        }

        public void StopBGM(float fadeDuration = 1.0f)
        {
            if (bgmSource != null && bgmSource.isPlaying)
            {
                StartCoroutine(FadeOutCoroutine(fadeDuration));
            }
        }

        // ═══════════════════════════════════════════════════════════════
        // Internal Helpers
        // ═══════════════════════════════════════════════════════════════

        private void PlayBGM(AudioClip clip, float fadeDuration = 0f)
        {
            if (bgmSource == null)
            {
                Debug.LogError("[AudioManager] BgmSource is missing! Please assign it in the Inspector.");
                return;
            }

            if (clip == null)
            {
                Debug.LogWarning("[AudioManager] Trying to play a null BGM clip. Make sure you assigned the clip in the Inspector.");
                return;
            }

            // If we are already playing this clip, just stop any fade-out and ensure volume is up
            if (bgmSource.isPlaying && bgmSource.clip == clip)
            {
                StopAllCoroutines();
                bgmSource.volume = 1.0f;
                return;
            }

            Debug.Log($"[AudioManager] Playing BGM: {clip.name}");
            StopAllCoroutines(); 
            
            bgmSource.clip = clip;
            bgmSource.Play();

            if (fadeDuration > 0)
            {
                StartCoroutine(FadeInCoroutine(fadeDuration));
            }
            else
            {
                bgmSource.volume = 1.0f;
            }
        }

        private System.Collections.IEnumerator FadeVolumeCoroutine(float targetVolume, float duration)
        {
            float startVolume = bgmSource.volume;
            float timer = 0;

            while (timer < duration)
            {
                timer += Time.deltaTime;
                bgmSource.volume = Mathf.Lerp(startVolume, targetVolume, timer / duration);
                yield return null;
            }

            bgmSource.volume = targetVolume;
            volumeFadeCoroutine = null;
        }

        private System.Collections.IEnumerator FadeInCoroutine(float duration)
        {
            float timer = 0;
            bgmSource.volume = 0;

            while (timer < duration)
            {
                timer += Time.deltaTime;
                bgmSource.volume = Mathf.Lerp(0, 1.0f, timer / duration);
                yield return null;
            }

            bgmSource.volume = 1.0f;
        }

        private void PlayOneShot(AudioClip clip)
        {
            if (clip != null && sfxSource != null)
            {
                sfxSource.PlayOneShot(clip);
                
                // Trigger ducking effect
                if (bgmSource != null && bgmSource.isPlaying)
                {
                    StartCoroutine(DuckingCoroutine(clip.length));
                }
            }
        }

        private System.Collections.IEnumerator FadeOutCoroutine(float duration)
        {
            float startVolume = bgmSource.volume;
            float timer = 0;

            while (timer < duration)
            {
                timer += Time.deltaTime;
                bgmSource.volume = Mathf.Lerp(startVolume, 0, timer / duration);
                yield return null;
            }

            bgmSource.Stop();
            bgmSource.volume = 1.0f; // Reset for next play
        }

        private System.Collections.IEnumerator DuckingCoroutine(float clipLength)
        {
            float duckVolume = 0.3f; // 30% volume
            float fadeTime = 0.2f;
            float originalVolume = 1.0f;

            // Fade Down
            float timer = 0;
            while (timer < fadeTime)
            {
                timer += Time.deltaTime;
                bgmSource.volume = Mathf.Lerp(originalVolume, duckVolume, timer / fadeTime);
                yield return null;
            }

            // Stay low while SFX plays
            yield return new WaitForSeconds(clipLength - (fadeTime * 2));

            // Fade Up
            timer = 0;
            while (timer < fadeTime)
            {
                timer += Time.deltaTime;
                bgmSource.volume = Mathf.Lerp(duckVolume, originalVolume, timer / fadeTime);
                yield return null;
            }

            bgmSource.volume = originalVolume;
        }
    }
}

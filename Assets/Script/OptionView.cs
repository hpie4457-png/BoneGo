using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace QuizSystem.UI
{
    public enum OptionState { Default, Correct, Wrong }

    public class OptionView : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private Image backgroundImage;
        [SerializeField] private Image borderImage;
        [SerializeField] private Image labelCircleImage;
        [SerializeField] private TextMeshProUGUI labelText;
        [SerializeField] private TextMeshProUGUI choiceText;
        [SerializeField] private Button button;

        [Header("Colors")]
        [SerializeField] private Color defaultBorder;
        [SerializeField] private Color defaultBackground;
        [SerializeField] private Color defaultLabelCircle;

        [SerializeField] private Color correctBorder;
        [SerializeField] private Color correctBackground;
        [SerializeField] private Color correctLabelCircle;

        [SerializeField] private Color wrongBorder;
        [SerializeField] private Color wrongBackground;
        [SerializeField] private Color wrongLabelCircle;

        public int OptionIndex { get; private set; }
        public System.Action<int> OnSelected;

        private static readonly string[] Labels = { "A", "B", "C", "D", "E" };

        private Sprite _originalBorderSprite;
        private Sprite _originalBackgroundSprite;
        private Sprite _originalLabelCircleSprite;

        // ── Unity lifecycle ───────────────────────────────────────────

        private void Awake()
        {
            // Cache original sprites from the prefab
            if (borderImage != null) _originalBorderSprite = borderImage.sprite;
            if (backgroundImage != null) _originalBackgroundSprite = backgroundImage.sprite;
            if (labelCircleImage != null) _originalLabelCircleSprite = labelCircleImage.sprite;

            // Set transition to None so the Button component doesn't overwrite 
            // our manual colors with its ColorBlock (normal/highlighted/etc).
            if (button != null)
            {
                button.transition = Selectable.Transition.None;
            }
        }

        // ── Setup ────────────────────────────────────────────────────

        public void Setup(int index, string choiceValue)
        {
            OptionIndex = index;
            labelText.text = index < Labels.Length ? Labels[index] : (index + 1).ToString();
            choiceText.text = choiceValue;

            SetState(OptionState.Default);
            button.interactable = true;

            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => OnSelected?.Invoke(OptionIndex));
        }

        // ── State ────────────────────────────────────────────────────

        public void SetState(OptionState state)
        {
            switch (state)
            {
                case OptionState.Default:
                    ApplyColors(defaultBorder, defaultBackground, defaultLabelCircle);
                    button.interactable = true;
                    break;
                case OptionState.Correct:
                    ApplyColors(correctBorder, correctBackground, correctLabelCircle);
                    button.interactable = false;
                    break;
                case OptionState.Wrong:
                    ApplyColors(wrongBorder, wrongBackground, wrongLabelCircle);
                    button.interactable = false;
                    break;
            }
        }

        public void Lock() => button.interactable = false;

        // ── Private ──────────────────────────────────────────────────

        private void ApplyColors(Color border, Color background, Color labelCircle)
        {
            SetImageColor(borderImage, _originalBorderSprite, border);
            SetImageColor(backgroundImage, _originalBackgroundSprite, background);
            SetImageColor(labelCircleImage, _originalLabelCircleSprite, labelCircle);
        }

        private static void SetImageColor(Image image, Sprite originalSprite, Color color)
        {
            if (image == null) return;
            
            // Apply the color tint
            image.color = color;
            
            // If the original sprite is null, use a pure white sprite so the color 
            // shows as a solid block. If the original sprite is NOT white (e.g. grey), 
            // the final color will be darker/muted because Image.color is a multiplier.
            image.sprite = originalSprite != null ? originalSprite : WhiteSprite;
        }

        private static Sprite _whiteSprite;
        private static Sprite WhiteSprite
        {
            get
            {
                if (_whiteSprite != null) return _whiteSprite;
                var tex = new Texture2D(1, 1, TextureFormat.RGBA32, false);
                tex.SetPixel(0, 0, Color.white);
                tex.Apply();
                _whiteSprite = Sprite.Create(tex, new Rect(0, 0, 1, 1), Vector2.one * 0.5f);
                return _whiteSprite;
            }
        }
    }
}
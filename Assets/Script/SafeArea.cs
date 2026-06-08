using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class SafeArea : MonoBehaviour
{
    private RectTransform rectTransform;
    private Canvas canvas;
    private Rect lastSafeArea = Rect.zero;
    private ScreenOrientation lastOrientation;
    private Vector2Int lastScreenSize = Vector2Int.zero;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        // Get the ROOT canvas, not just any parent canvas
        canvas = GetComponentInParent<Canvas>().rootCanvas;
        
        Refresh();
    }

    private void Update()
    {
        Refresh();
    }

    private void Refresh()
    {
        Rect safeArea = Screen.safeArea;

        if (safeArea != lastSafeArea || 
            Screen.width != lastScreenSize.x || 
            Screen.height != lastScreenSize.y || 
            Screen.orientation != lastOrientation)
        {
            lastScreenSize.x = Screen.width;
            lastScreenSize.y = Screen.height;
            lastOrientation = Screen.orientation;
            
            ApplySafeArea(safeArea);
        }
    }

    private void ApplySafeArea(Rect safeArea)
    {
        lastSafeArea = safeArea;

        if (Screen.width == 0 || Screen.height == 0) return;

        Vector2 anchorMin = safeArea.position;
        Vector2 anchorMax = safeArea.position + safeArea.size;

        // Use actual screen dimensions
        anchorMin.x /= Screen.width;
        anchorMin.y /= Screen.height;
        anchorMax.x /= Screen.width;
        anchorMax.y /= Screen.height;

        anchorMin.x = Mathf.Clamp01(anchorMin.x);
        anchorMin.y = Mathf.Clamp01(anchorMin.y);
        anchorMax.x = Mathf.Clamp01(anchorMax.x);
        anchorMax.y = Mathf.Clamp01(anchorMax.y);

        rectTransform.anchorMin = anchorMin;
        rectTransform.anchorMax = anchorMax;
        rectTransform.offsetMin = Vector2.zero;
        rectTransform.offsetMax = Vector2.zero;

        Debug.Log($"SafeArea applied: anchorMin={anchorMin}, anchorMax={anchorMax}");
    }
}
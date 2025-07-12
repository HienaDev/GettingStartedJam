using UnityEngine;
using UnityEngine.EventSystems;

public class RotateAroundCenter : MonoBehaviour
{
    public RectTransform center;
    public float speed = 30f; // degrees per second
    public float ellipseWidth = 400f;
    public float ellipseHeight = 225f;
    public float repelRadius = 200f;
    public float repelStrength = 10f;
    public float repelLerpSpeed = 8f;

    private float angle; // in radians
    private RectTransform rectTransform;
    private Canvas canvas;
    private Vector3 currentRepelOffset = Vector3.zero;

    public void Initialize(RectTransform centerPoint, float initialAngle, float width, float height, float rotationSpeed)
    {
        center = centerPoint;
        angle = initialAngle;
        ellipseWidth = width;
        ellipseHeight = height;
        speed = rotationSpeed;

        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
    }

    void Update()
    {
        // Update angle
        angle += speed * Mathf.Deg2Rad * Time.deltaTime;

        // Base elliptical position
        float x = Mathf.Cos(angle) * ellipseWidth;
        float y = Mathf.Sin(angle) * ellipseHeight;
        Vector3 basePosition = new Vector3(x, y, 0f);

        // Mouse position in screen space
        Vector2 mouseScreen = Input.mousePosition;

        // Check if mouse is inside this button
        bool mouseInside = RectTransformUtility.RectangleContainsScreenPoint(rectTransform, mouseScreen, canvas.worldCamera);

        Vector3 targetRepelOffset = Vector3.zero;

        if (mouseInside)
        {
            // Get world position of mouse
            Vector3 mouseWorld;
            RectTransformUtility.ScreenPointToWorldPointInRectangle(canvas.transform as RectTransform, mouseScreen, canvas.worldCamera, out mouseWorld);

            // Direction from mouse to object center
            Vector3 toMouse = rectTransform.position - mouseWorld;
            float dist = toMouse.magnitude;

            if (dist > 0.01f)
            {
                float repelFactor = (1f - Mathf.Clamp01(dist / repelRadius)) * repelStrength;
                targetRepelOffset = toMouse.normalized * repelFactor;
            }
        }

        // Lerp the repel offset for smooth animation
        currentRepelOffset = Vector3.Lerp(currentRepelOffset, targetRepelOffset, Time.deltaTime * repelLerpSpeed);

        // Apply final position
        rectTransform.localPosition = basePosition + currentRepelOffset;
    }
}

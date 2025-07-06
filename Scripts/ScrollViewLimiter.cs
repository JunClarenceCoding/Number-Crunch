using UnityEngine;
using UnityEngine.UI;

public class ScrollViewLimiter : MonoBehaviour
{
    public RectTransform content;
    public RectTransform image;
    private ScrollRect scrollRect;

    void Start()
    {
        scrollRect = GetComponent<ScrollRect>();

        // Set content height to the height of the image at the start
        if (content && image)
        {
            content.sizeDelta = new Vector2(content.sizeDelta.x, image.rect.height);
        }

        // Adjust the scrollability based on the content and viewport sizes
        LimitScrolling();
    }

    void LimitScrolling()
    {
        // Disable vertical scrolling if content height is less than or equal to viewport height
        if (content.rect.height <= scrollRect.viewport.rect.height)
        {
            scrollRect.vertical = false;
        }
        else
        {
            scrollRect.vertical = true;
        }
    }
}

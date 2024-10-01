using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactSafeArea : MonoBehaviour
{
    Vector2 minAnchor;
    Vector2 maxAnchor;

    // Start is called before the first frame update
    void Start()
    {
        var myRect = this.GetComponent<RectTransform>();

        minAnchor = Screen.safeArea.min;
        maxAnchor = Screen.safeArea.max;

        minAnchor.x /= Screen.width;
        minAnchor.y /= Screen.height;

        maxAnchor.x /= Screen.width;
        maxAnchor.y /= Screen.height;
        

        myRect.anchorMin = minAnchor;
        myRect.anchorMax = maxAnchor;
    }
}

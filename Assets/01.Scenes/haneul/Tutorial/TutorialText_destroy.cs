using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialText_destroy : MonoBehaviour
{
    void Awake()
    {
        Destroy(gameObject, 3.0f);
    }
}

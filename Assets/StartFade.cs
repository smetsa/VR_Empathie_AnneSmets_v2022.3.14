using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartFade : MonoBehaviour
{
    public FadeScreen fadeScreen;
    // Start is called before the first frame update
    void Start()
    {
        fadeScreen.FadeOut();
    }

}

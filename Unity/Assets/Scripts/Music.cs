using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    public GameObject source;
    public static GameObject playing;
    void Start()
    {
        if (playing == null)
        {
            playing = source;
            DontDestroyOnLoad(source);
        }
        else
        {
            Destroy(source);
        }
    }
}

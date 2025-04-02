using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepObjectInScenes : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}

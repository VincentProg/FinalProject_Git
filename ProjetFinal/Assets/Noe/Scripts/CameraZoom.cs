using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    // Start is called before the first frame update
    public float nbOfTileHeight;
    void Awake()
    {
        print(Screen.height);
        float orthosize = nbOfTileHeight * 30 * 0.5f;
        Camera.main.orthographicSize = orthosize;
    }
}

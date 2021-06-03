using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    // Start is called before the first frame update
    public float nbOfTileHeight;
    void Start()
    {
        float orthosize = nbOfTileHeight * 14 * Screen.height / Screen.width * 0.5f;
        Camera.main.orthographicSize = orthosize;
    }
}

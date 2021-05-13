using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderTest : MonoBehaviour
{
    public PlayerTest player;
    public Slider _slider;
    float cache = 0;

    private void Update()
    {
        if(_slider.value != cache)
        {
            UpdateSlide(_slider.value);
            cache = _slider.value;
        }
    }


    public void UpdateSlide(float value)
    {
        Debug.Log("ok");
        TilesManager.instance.ClearTiles();
        TilesManager.instance.GetPath(player.myTile.coordinates, TilesManager.instance.target, (int) value);
    }
}

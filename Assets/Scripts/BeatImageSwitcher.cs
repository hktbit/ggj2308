using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatImageSwitcher : MonoBehaviour
{
    [SerializeField]
    UnityEngine.UI.Image image;

    [SerializeField]
    Sprite[] sprites;

    private int index;

    void Update()
    {
        if (Music.IsJustChangedBeat())
        {
            index = (index + 1) % 2;
            image.sprite = sprites[index];
        }
    }
}

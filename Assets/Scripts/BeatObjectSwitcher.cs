using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatObjectSwitcher : MonoBehaviour
{
    [SerializeField]
    GameObject[] gameObjects;

    void Update()
    {
        if (Music.IsJustChangedBar())
        {
            if (Random.Range(0, 2) != 0)
            {
                return;
            }

            foreach (var i in this.gameObjects)
            {
                i.gameObject.SetActive(false);
            }
            this.gameObjects[Random.Range(0, this.gameObjects.Length)].SetActive(true);
        }
    }
}

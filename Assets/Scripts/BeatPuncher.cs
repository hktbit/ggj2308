using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BeatPuncher : MonoBehaviour
{
    [SerializeField]
    float punch = 0.1f;

    [SerializeField]
    float duration = 0.1f;

    void Update()
    {
        if (Music.IsJustChangedBeat())
        {
            transform.DOPunchScale(Vector3.one * punch, duration);
        }
    }
}

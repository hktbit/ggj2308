using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SuccessText : MonoBehaviour
{
    float yShihtPos = 100f;
    // Start is called before the first frame update
    void Start()
    {
        transform.DOLocalMoveY(yShihtPos, 0.1f).SetRelative();
    }

    // Update is called once per frame
    void Update()
    {

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class SuccessText : MonoBehaviour
{
    float yShihtPos = 200f;
    // Start is called before the first frame update
    void Start()
    {
        Sequence seq = DOTween.Sequence()
            .AppendCallback(() =>
            {
                transform.DOLocalMoveY(yShihtPos, 0.25f).SetEase(Ease.OutExpo).SetRelative();
            })
            .AppendInterval(0.5f)
            .AppendCallback(() =>
            {
                GetComponent<Image>().DOFade(0f, 0.25f);
            })
            .AppendInterval(0.5f)
             .AppendCallback(() =>
             {
                 Destroy(gameObject);
             })
            ;
    }

    // Update is called once per frame
    void Update()
    {

    }
}

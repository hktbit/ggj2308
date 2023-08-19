using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KoitanLib;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    GameObject playerSheep;
    bool isJumping = false;
    float bpm = 120f;
    float beatSeconds;
    // Start is called before the first frame update
    void Start()
    {
        beatSeconds = 60f / bpm;
        playerSheep.transform.DOMoveX(16f, beatSeconds * 4f).SetEase(Ease.Linear).SetRelative().SetLoops(-1, LoopType.Restart);
        Sequence seq = DOTween.Sequence()
            .AppendCallback(() =>
            {
                playerSheep.transform.DOPunchScale(Vector3.one * 0.1f, 0.1f);
            })
            .AppendInterval(beatSeconds)
            .SetLoops(-1, LoopType.Restart)
            ;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
        {
            Sequence seq = DOTween.Sequence()
                .AppendCallback(() =>
                {
                    isJumping = true;
                    playerSheep.transform.DOMoveY(3f, 0.15f).SetRelative().SetEase(Ease.OutExpo).SetLoops(2, LoopType.Yoyo);
                })
                .AppendInterval(0.3f)
                .AppendCallback(() =>
                {
                    isJumping = false;
                })
                ;
        }
        KoitanDebug.Display($"デバッグ表示です\n");
        KoitanDebug.Display($"Time.time = {Time.time}\n");
    }
}

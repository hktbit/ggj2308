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
    int cnt;
    // Start is called before the first frame update
    void Start()
    {
        beatSeconds = 60f / bpm;
        //playerSheep.transform.DOMoveX(16f, beatSeconds * 4f).SetEase(Ease.Linear).SetRelative().SetLoops(-1, LoopType.Restart);

        Sequence seq = DOTween.Sequence()
            .AppendCallback(() =>
            {
                playerSheep.transform.DOPunchScale(Vector3.one * 0.2f, 0.1f);
                playerSheep.transform.DOMoveX(4f, beatSeconds).SetEase(Ease.OutExpo).SetRelative();
            })
            .AppendInterval(beatSeconds)
            .AppendCallback(() =>
            {
                playerSheep.transform.DOPunchScale(Vector3.one * 0.2f, 0.1f);
                playerSheep.transform.DOMoveX(4f, beatSeconds).SetEase(Ease.OutExpo).SetRelative();
            })
            .AppendInterval(beatSeconds)
            .AppendCallback(() =>
            {
                playerSheep.transform.DOPunchScale(Vector3.one * 0.2f, 0.1f);
                playerSheep.transform.DOMoveX(4f, beatSeconds).SetEase(Ease.OutExpo).SetRelative();
            })
            .AppendInterval(beatSeconds)
            .AppendCallback(() =>
            {
                playerSheep.transform.DOPunchScale(Vector3.one * 0.2f, 0.1f);
                playerSheep.transform.DOMoveX(4f, beatSeconds).SetEase(Ease.OutExpo).SetRelative();
            })
            .AppendInterval(beatSeconds)
            .AppendCallback(() =>
            {
                playerSheep.transform.DOPunchScale(Vector3.one * 0.2f, 0.1f);
                playerSheep.transform.DOMoveX(4f, beatSeconds).SetEase(Ease.OutExpo).SetRelative();
            })
            .AppendInterval(beatSeconds)
            .AppendCallback(() =>
            {
                playerSheep.transform.DOPunchScale(Vector3.one * 0.2f, 0.1f);
                playerSheep.transform.DOMoveX(4f, beatSeconds).SetEase(Ease.OutExpo).SetRelative();
            })
            .AppendInterval(beatSeconds)
            .AppendCallback(() =>
            {
                playerSheep.transform.DOPunchScale(Vector3.one * 0.2f, 0.1f);
                playerSheep.transform.DOMoveX(4f, beatSeconds).SetEase(Ease.OutExpo).SetRelative();
            })
            .AppendInterval(beatSeconds)
            .AppendCallback(() =>
            {
                playerSheep.transform.DOPunchScale(Vector3.one * 0.2f, 0.1f);
                playerSheep.transform.DOMoveX(4f, beatSeconds).SetEase(Ease.OutExpo).SetRelative();
            })
            .AppendInterval(beatSeconds)
            .AppendCallback(() =>
            {
                playerSheep.transform.DOKill();
                playerSheep.transform.position = new Vector3(-16f, 0f);
            })
            .SetLoops(-1, LoopType.Restart);

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
                    playerSheep.transform.DOMoveY(3f, 0.125f).SetRelative().SetEase(Ease.OutExpo).SetLoops(2, LoopType.Yoyo);
                })
                .AppendInterval(0.25f)
                .AppendCallback(() =>
                {
                    isJumping = false;
                })
                ;
        }
        KoitanDebug.Display($"デバッグ表示です\n");
        KoitanDebug.Display($"Time.time = {Time.time}\n");
    }

    /*
    void MoveOneStep()
    {
        Sequence seq = DOTween.Sequence()
            .AppendCallback(() =>
            {
                playerSheep.transform.DOPunchScale(Vector3.one * 0.2f, 0.1f);
                playerSheep.transform.DOMoveX(2f, beatSeconds).SetEase(Ease.OutExpo).SetRelative();
            })
            .AppendInterval(beatSeconds)
            .AppendCallback(() =>
            {
                cnt++;
                if (cnt == 4)
                {
                    cnt = 0;
                    playerSheep.transform.position = Vector3.zero;
                    Debug.Log($"cnt = {cnt}");
                }
                MoveOneStep();
            })
            ;
    }
    */
}

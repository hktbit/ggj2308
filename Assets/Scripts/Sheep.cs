using DG.Tweening;
using KoitanLib;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sheep : MonoBehaviour
{
    public GameManager gameManager;
    bool isJumping = false;
    TrailRenderer trailRenderer;
    public bool isTutorial;
    public MusicalScore musicalScore;
    Sequence scaleSeq;

    Vector3 position;
    Vector3 velocity;
    [SerializeField]
    Vector3 acc;
    [SerializeField]
    Vector3 jumpSpeed;
    bool isGround = true;
    int airJumpCount = 1;
    int airJumpCountMax = 1;

    void Start()
    {
        scaleSeq = DOTween.Sequence()
            .AppendCallback(() =>
            {
                transform.DOPunchScale(Vector3.one * 0.1f, 0.1f);
            })
            .AppendInterval(gameManager.BeatSeconds)
            .SetLoops(-1, LoopType.Restart)
            ;
        Sequence seq = DOTween.Sequence()
            .AppendCallback(() =>
            {
                transform.DOMoveX(32f, gameManager.BeatSeconds * 8f).SetEase(Ease.Linear).SetRelative();
            })
            .AppendInterval(gameManager.BeatSeconds * 8f)
            .AppendCallback(() =>
            {
                // 破棄
                transform.DOKill();
                scaleSeq.Kill();
                Destroy(gameObject);
            })
            ;
        if (isTutorial)
        {
            // ジャンプ
            Sequence jumpSeq = DOTween.Sequence()
            .AppendInterval(gameManager.BeatSeconds * 2f)
            .AppendCallback(() =>
            {
                switch (musicalScore.notes[0])
                {
                    case NoteType.None:
                        break;
                    case NoteType.Single:
                    case NoteType.Double:
                        Jump();
                        break;
                }
            })
            .AppendInterval(gameManager.BeatSeconds)
            .AppendCallback(() =>
            {
                switch (musicalScore.notes[1])
                {
                    case NoteType.None:
                        break;
                    case NoteType.Single:
                    case NoteType.Double:
                        Jump();
                        break;
                }
            })
            .AppendInterval(gameManager.BeatSeconds)
            .AppendCallback(() =>
            {
                switch (musicalScore.notes[2])
                {
                    case NoteType.None:
                        break;
                    case NoteType.Single:
                    case NoteType.Double:
                        Jump();
                        break;
                }
            })
            .AppendInterval(gameManager.BeatSeconds)
            .AppendCallback(() =>
            {
                switch (musicalScore.notes[3])
                {
                    case NoteType.None:
                        break;
                    case NoteType.Single:
                    case NoteType.Double:
                        Jump();
                        break;
                }
            })
            ;

        }
    }

    void Update()
    {
        // 座標計算
        position = transform.position;
        velocity += acc * Time.deltaTime;
        position += velocity * Time.deltaTime;
        // 地面に着地
        if (position.y < 0)
        {
            airJumpCount = airJumpCountMax;
            position.y = 0;
            isGround = true;
        }
        transform.position = position;

        //transform.position += Vector3.up * 1 * Time.deltaTime;
        if (isTutorial)
        {

        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (isGround)
                {
                    Jump();
                }
                else
                {
                    if (airJumpCount > 0)
                    {
                        airJumpCount--;
                        Jump();
                    }
                }
            }
        }
        /*
        KoitanDebug.Display($"デバッグ表示です\n");
        KoitanDebug.Display($"Time.time = {Time.time}\n");
        */
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("GameOver");
        gameManager.StartGameOver();
    }

    void Jump()
    {
        isGround = false;
        velocity = jumpSpeed;
        /*
        Sequence seq = DOTween.Sequence()
                    .AppendCallback(() =>
                    {
                        isJumping = true;
                        transform.DOMoveY(2f, 0.2f).SetRelative().SetEase(Ease.OutCubic).SetLoops(2, LoopType.Yoyo);
                    })
                    .AppendInterval(0.4f)
                    .AppendCallback(() =>
                    {
                        isJumping = false;
                    })
                    ;
        */
    }
}

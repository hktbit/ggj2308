using DG.Tweening;
using KoitanLib;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sheep : MonoBehaviour
{
    public event System.Action Jumped = () => { };

    public GameManager gameManager;
    bool isJumping = false;
    TrailRenderer trailRenderer;
    public bool isTutorial;
    public MusicalScore musicalScore;

    Vector3 position;
    Vector3 velocity;
    [SerializeField]
    Vector3 acc;
    [SerializeField]
    Vector3 jumpSpeed;
    bool isGround = true;
    int airJumpCount = 1;
    int airJumpCountMax = 1;

    Timing[] timings = new Timing[] {
        new Timing(2, 0, 0),
        new Timing(2, 1, 0),
        new Timing(2, 2, 0),
        new Timing(2, 3, 0),
    };

    void Start()
    {
        Sequence seq = DOTween.Sequence()
            .AppendCallback(() =>
            {
                transform.DOMoveX(32f, gameManager.BeatSeconds * 8f).SetEase(Ease.Linear).SetRelative();
            })
            .AppendInterval(gameManager.BeatSeconds * 8f)
            .AppendCallback(() =>
            {
                // ?j??
                transform.DOKill();
                if (!gameManager.IsGameOver)
                {
                    Destroy(gameObject);
                }
            })
            ;
        /*
        if (isTutorial)
        {
            // ?W?????v
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
        */
    }

    void Update()
    {
        // ???W?v?Z
        position = transform.position;
        // ?????W?????v????????2?{??
        float jumpSpeedScale = airJumpCount == 0 ? 1.5f : 1f;

        velocity += acc * Time.deltaTime * gameManager.BeatTimeScale * jumpSpeedScale;
        position += velocity * Time.deltaTime * gameManager.BeatTimeScale * jumpSpeedScale;
        // ?n???????n
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
            if (Music.IsJustChangedAt(timings[0]))
            {
                AutoJump(0);
            }
            if (Music.IsJustChangedAt(timings[1]))
            {
                AutoJump(1);
            }
            if (Music.IsJustChangedAt(timings[2]))
            {
                AutoJump(2);
            }
            if (Music.IsJustChangedAt(timings[3]))
            {
                AutoJump(3);
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                Jump();
            }
        }
        /*
        KoitanDebug.Display($"?f?o?b?O?\??????\n");
        KoitanDebug.Display($"Time.time = {Time.time}\n");
        */
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Block")
        {
            Debug.Log("GameOver");
            gameManager.StartGameOver();
        }
        else if (other.tag == "Success")
        {
            gameManager.Success();
        }
    }

    void Jump()
    {
        if (!isGround)
        {
            if (airJumpCount > 0)
            {
                airJumpCount--;
            }
            else
            {
                return;
            }
        }
        Jumped.Invoke();
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

    void AutoJump(int index)
    {
        switch (musicalScore.notes[index])
        {
            case NoteType.None:
                break;
            case NoteType.Single:
                Jump();
                break;
            case NoteType.Double:
                StartCoroutine(AutoJumpDouble());
                break;
        }
    }

    IEnumerator AutoJumpDouble()
    {
        Jump();
        yield return new WaitForSeconds(gameManager.BeatSeconds * 0.3f);
        Jump();
    }
}

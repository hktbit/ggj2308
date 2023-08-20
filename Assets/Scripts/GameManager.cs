using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KoitanLib;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    Sheep playerSheepPrefab;
    [SerializeField]
    Sheep senpaiSheepPrefab;
    [SerializeField]
    Transform playerSheepTf;
    [SerializeField]
    Camera renderCamera;
    [SerializeField]
    Image successImage;
    [SerializeField]
    TextMeshProUGUI scoreTextMesh;
    [SerializeField]
    GameObject[] textPrefabs;
    [SerializeField]
    Transform textParent;
    int score = 0;
    int successCount;
    [SerializeField]
    GameObject ouchiPrefab;
    [SerializeField]
    Image gameoverBG, gameoverImage;
    [SerializeField]
    GameObject gameoverSheep;
    Sheep currentSheep;

    public float Bpm { get; private set; } = 90f;
    public float BeatSeconds { get; private set; }
    public float BeatTimeScale { get; private set; } = 1f;
    public bool IsGameOver { get; private set; }
    int cnt;
    [SerializeField]
    MusicalScore musicalScore;
    [SerializeField]
    MusicalScore[] stages;
    [SerializeField]
    Transform[] barikans;
    // Start is called before the first frame update
    void Awake()
    {
        //Time.timeScale = 1f;
        BeatSeconds = 60f / Bpm;
        BeatTimeScale = Bpm / 120f;
    }

    private void Start()
    {
        //StartLoop();
    }

    // Update is called once per frame
    void Update()
    {
        KoitanDebug.Display($"デバッグ表示です\n");
        KoitanDebug.Display($"Time.time = {Time.time}\n");

        if (this.isPlay)
        {
            this.OnTiming();
        }
    }

    void StartLoop()
    {
        Sequence seq = DOTween.Sequence()
            .AppendCallback(() =>
            {
                cnt++;
                InstantiateSheep(cnt % 2 == 1);
            })
            .AppendInterval(BeatSeconds * 2f)
            .AppendCallback(() =>
            {
                // 先輩
                if (cnt % 2 == 1)
                {
                    // ステージ生成
                    NoteType preNote = NoteType.None;
                    for (int i = 0; i < 4; i++)
                    {
                        // 前が高い壁の場合はなし
                        if (preNote == NoteType.Double)
                        {
                            musicalScore.notes[i] = NoteType.None;
                            preNote = NoteType.None;
                            continue;
                        }

                        NoteType note;
                        float r = Random.Range(0f, 1f);
                        if (r < 1f / 3f)
                        {
                            note = NoteType.None;
                        }
                        else if (r < 2f / 3f)
                        {
                            note = NoteType.Single;
                        }
                        else
                        {
                            note = NoteType.Double;
                        }
                        musicalScore.notes[i] = note;
                        preNote = note;
                    }
                    for (int i = 0; i < 4; i++)
                    {
                        Vector3 scale = barikans[i].localScale;
                        Vector3 pos = barikans[i].position;
                        switch (musicalScore.notes[i])
                        {
                            case NoteType.None:
                                scale.y = 0f;
                                pos.y = -2f;
                                break;
                            case NoteType.Single:
                                scale.y = 1f;
                                pos.y = 0f;
                                break;
                            case NoteType.Double:
                                scale.y = 3f;
                                pos.y = 1f;
                                break;
                        }
                        //barikans[i].localScale = scale;
                        barikans[i].position = pos;
                    }
                }
            })
            .AppendInterval(BeatSeconds * 2f)
            .AppendCallback(() =>
            {
                if (!IsGameOver)
                {
                    StartLoop();
                }
            })
            ;
    }

    void InstantiateSheep(bool isTutorial)
    {
        Sheep sheep;
        if (isTutorial)
        {
            sheep = Instantiate(senpaiSheepPrefab, playerSheepTf.position, playerSheepTf.rotation);
        }
        else
        {
            sheep = Instantiate(playerSheepPrefab, playerSheepTf.position, playerSheepTf.rotation);
            currentSheep = sheep;
        }
        sheep.gameObject.SetActive(true);
        sheep.gameManager = this;
        sheep.musicalScore = musicalScore;
        sheep.isTutorial = isTutorial;

        sheep.Jumped += OnJumped;
    }

    void OnJumped()
    {
        ses[successCount >= 3 ? 1 : 0].Play();
    }

    public void StartGameOver()
    {
        if (IsGameOver)
        {
            return;
        }
        IsGameOver = true;
        currentSheep.gameObject.SetActive(false);
        GameObject gs = Instantiate(gameoverSheep);
        gs.transform.position = currentSheep.transform.position;
        gs.transform.rotation = currentSheep.transform.rotation;
        GameObject tObj = Instantiate(ouchiPrefab, textParent);
        tObj.transform.position = textParent.position;
        Sequence seq = DOTween.Sequence()
            .AppendInterval(1f)
            .AppendCallback(() =>
            {
                renderCamera.transform.DOPunchPosition(Random.onUnitSphere * 0.25f, 0.25f);
                gameoverImage.transform.localScale = Vector3.zero;
                gameoverImage.transform.DOScale(Vector3.one, 1f).SetEase(Ease.OutBack);
                gameoverImage.gameObject.SetActive(true);
                gameoverBG.color = new Color(1, 1, 1, 0);
                gameoverBG.DOFade(0.5f, 1f);
                gameoverBG.gameObject.SetActive(true);
            })
            .AppendInterval(1f)
            .AppendCallback(() =>
            {
                naichilab.RankingLoader.Instance.SendScoreAndShowRanking(score);
            })
            ;
    }

    bool isPlay;
    bool isInit;

    Timing[] timings = new Timing[] {
        new Timing(0, 0, 0),
        new Timing(0, 1, 0),
        new Timing(0, 2, 0),
        new Timing(0, 3, 0),
        new Timing(1, 0, 0),
        new Timing(1, 1, 0),
        new Timing(1, 2, 0),
        new Timing(1, 3, 0),
        new Timing(2, 3, 0),
        new Timing(3, 4, 0),
    };

    [SerializeField]
    AudioSource[] ses;

    [SerializeField]
    GameObject canvas;

    [SerializeField]
    GameObject uiCanvas;

    [SerializeField]
    GameObject titleCanvas;

    public void Play()
    {
        this.isPlay = true;
        this.titleCanvas.gameObject.SetActive(false);
        this.canvas.gameObject.SetActive(true);
        this.uiCanvas.gameObject.SetActive(true);
        ses[2].Play();
        Music.Play("Music");
    }

    void OnTiming()
    {
        if (Music.IsJustChangedAt(timings[0]))
        {
            isInit = true;
            GenerateStage();
            ResetBlock(0);
            return;
        }

        if (!isInit)
        {
            return;
        }

        if (IsGameOver)
        {
            return;
        }

        if (Music.IsJustChangedAt(timings[1]))
        {
            ResetBlock(1);
        }
        if (Music.IsJustChangedAt(timings[2]))
        {
            ResetBlock(2);
        }
        if (Music.IsJustChangedAt(timings[3]))
        {
            ResetBlock(3);
        }

        if (Music.IsJustChangedAt(timings[4]))
        {
            SetBlock(0);
        }

        if (Music.IsJustChangedAt(timings[5]))
        {
            SetBlock(1);
        }

        if (Music.IsJustChangedAt(timings[6]))
        {
            SetBlock(2);
        }

        if (Music.IsJustChangedAt(timings[7]))
        {
            SetBlock(3);
            InstantiateSheep(isTutorial: true);
        }

        if (Music.IsJustChangedAt(timings[8]))
        {
            InstantiateSheep(isTutorial: false);
            successCount = 0;
            /*
            Sequence seq = DOTween.Sequence()
                .AppendInterval(BeatSeconds * 5f)
                .AppendCallback(() =>
                {
                    //文字表示
                    //判定
                    GameObject tObj = Instantiate(textPrefabs[0], textParent);
                    tObj.transform.position = textParent.position;
                    //if (successCount > )
                })
                ;
            */
        }

        /*
        if (Music.IsJustChangedAt(timings[9]))
        {
            //判定
            GameObject tObj = Instantiate(textPrefabs[0], textParent);
            tObj.transform.position = textParent.position;
            successCount++;
            //if (successCount > )
            Debug.Log("4.4");
        }
        */
    }

    void GenerateStage()
    {
        /*
        NoteType preNote = NoteType.None;
        for (int i = 0; i < 4; i++)
        {
            // 前が高い壁の場合はなし
            if (preNote == NoteType.Double)
            {
                musicalScore.notes[i] = NoteType.None;
                preNote = NoteType.None;
                continue;
            }

            NoteType note;
            float r = Random.Range(0f, 1f);
            if (r < 1f / 3f)
            {
                note = NoteType.None;
            }
            else if (r < 2f / 3f)
            {
                note = NoteType.Single;
            }
            else
            {
                note = NoteType.Double;
            }
            musicalScore.notes[i] = note;
            preNote = note;
        }
        */
        musicalScore = stages[Random.Range(0, stages.Length)];
    }

    void ResetBlock(int i)
    {
        Vector3 pos = barikans[i].position;
        pos.y = -1.5f;
        //barikans[i].position = pos;
        barikans[i].DOMove(pos, BeatSeconds / 2f);
        barikans[i].DOPunchScale(Vector3.one * 0.1f, BeatSeconds / 2f);
    }

    void SetBlock(int i)
    {
        Vector3 scale = barikans[i].localScale;
        Vector3 pos = barikans[i].position;
        switch (musicalScore.notes[i])
        {
            case NoteType.None:
                scale.y = 1f;
                pos.y = -1.5f;
                break;
            case NoteType.Single:
                scale.y = 1f;
                pos.y = 0f;
                break;
            case NoteType.Double:
                scale.y = 1f;
                pos.y = 1f;
                break;
        }
        //barikans[i].localScale = scale;
        //barikans[i].position = pos;
        barikans[i].DOMove(pos, BeatSeconds / 2f);
        barikans[i].DOPunchScale(Vector3.one * 0.1f, BeatSeconds / 2f);
    }

    public void Success()
    {
        // スコア加算
        score++;
        scoreTextMesh.text = $"{score}";
        scoreTextMesh.transform.DOKill();
        scoreTextMesh.transform.localScale = Vector3.one;
        scoreTextMesh.transform.DOPunchScale(Vector3.one * 0.1f, 0.1f);
        Debug.Log("Success");
        /*
        successImage.DOKill();
        successImage.color = Color.white;
        successImage.DOFade(0f, 0.1f).SetDelay(0.2f);
        successImage.transform.localScale = Vector3.one;
        successImage.transform.DOPunchScale(Vector3.one * 0.1f, 0.1f);
        */
        GameObject tObj = Instantiate(textPrefabs[successCount], textParent);
        tObj.transform.position = textParent.position;
        successCount++;
        if (successCount > 3)
        {
            successCount = 3;
        }
    }

    public void Retry()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}

[System.Serializable]
public class MusicalScore
{
    public NoteType[] notes;
}

public enum NoteType
{
    None,
    Single,
    Double,
}

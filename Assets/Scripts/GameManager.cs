using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KoitanLib;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    Sheep playerSheepPrefab;
    [SerializeField]
    Transform playerSheepTf;
    [SerializeField]
    Camera renderCamera;
    public float Bpm { get; private set; } = 120f;
    public float BeatSeconds { get; private set; }
    public bool IsGameOver { get; private set; }
    int cnt;
    [SerializeField]
    MusicalScore musicalScore;
    [SerializeField]
    Transform[] barikans;
    // Start is called before the first frame update
    void Awake()
    {
        Time.timeScale = 1f;
        BeatSeconds = 60f / Bpm;
    }

    private void Start()
    {
        StartLoop();
    }

    // Update is called once per frame
    void Update()
    {
        KoitanDebug.Display($"デバッグ表示です\n");
        KoitanDebug.Display($"Time.time = {Time.time}\n");
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
        Sheep sheep = Instantiate(playerSheepPrefab, playerSheepTf.position, playerSheepTf.rotation);
        sheep.gameObject.SetActive(true);
        sheep.gameManager = this;
        sheep.musicalScore = musicalScore;
        sheep.isTutorial = isTutorial;
    }

    public void StartGameOver()
    {
        renderCamera.transform.DOPunchPosition(Random.onUnitSphere * 0.25f, 0.25f);
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
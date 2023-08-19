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
    public float Bpm { get; private set; } = 120f;
    public float BeatSeconds { get; private set; }
    public bool IsGameOver { get; private set; }
    int cnt;
    [SerializeField]
    MusicalScore musicalScore;
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
                InstantiateSheep(cnt % 2 == 0);
                cnt++;
            })
            .AppendInterval(BeatSeconds * 4f)
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
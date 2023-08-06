using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public event EventHandler OnGameRestart;


    //Transform slime;
    //[SerializeField] private Transform prefab;
    [SerializeField] private LeanGameObjectPool pool;



    [SerializeField]
    private float rightSide = 7.5f;
    [SerializeField]
    private float upSide = 4.3f;
    [SerializeField]
    private float intervalSpawnTime;
    private bool gameEnd;
    private float timer=0f;

    private void Awake()
    {
        Instance = this;
        gameEnd = false;
    }

    private void Start()
    {
        SpawnSlime();
        PlayerController.Instance.OnPlayerDead += PlayerController_OnPlayerDead;
    }

    private void PlayerController_OnPlayerDead(object sender, System.EventArgs e)
    {
        gameEnd = true;
        int score = Score.Instance.GetScore();
        if (PlayerPrefs.GetInt("Score") < score)
        {
            PlayerPrefs.SetInt("Score", score);
            PlayerPrefs.Save();
        }
    }

    private void Update()
    {
        if (!gameEnd)
        {
            if (timer < intervalSpawnTime)
            {
                timer += Time.deltaTime;
            }
            if (timer >= intervalSpawnTime)
            {
                SpawnSlime();
                timer = 0f;
            }
        }
        
    }

    public void RestartGame()
    {
        gameEnd = false;

        OnGameRestart?.Invoke(this, EventArgs.Empty);
        pool.DespawnAll();
    }

    public void SpawnSlime()
    {
        pool.Spawn(GetRandomSpawnPos());
    }
    public void DespawnSlime()
    {
        pool.DespawnOldest();
    }

    public LeanGameObjectPool GetPool()
    {
        return pool;
    }

    public Vector3 GetRandomSpawnPos()
    {
        int pos = UnityEngine.Random.Range(0, 4);

        switch (pos)
        {
            case 0://up
                return new Vector3(
                    UnityEngine.Random.Range(-rightSide, rightSide),
                    upSide,
                    0
                    );
            case 1://down
                return new Vector3(
                    UnityEngine.Random.Range(-rightSide, rightSide),
                    -upSide,
                    0
                    );
            case 2://right
                return new Vector3(
                    rightSide,
                    UnityEngine.Random.Range(-upSide, upSide),
                    0
                    );
            case 3://left
                return new Vector3(
                    -rightSide,
                    UnityEngine.Random.Range(-upSide, upSide),
                    0
                    );
            default:
                return new Vector3(-rightSide, rightSide);
        }
    }
}

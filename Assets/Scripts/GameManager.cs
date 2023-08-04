using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;

public class GameManager : MonoBehaviour
{


    [SerializeField] private Transform prefab;
    public void SpawnSlime()
    {
        LeanPool.Spawn(prefab,
            GetRandomSpawnPos()
            , Quaternion.identity);
    }
    public void DespawnSlime()
    {
        
    }


    public Vector2 GetRandomSpawnPos()
    {
        int pos = Random.Range(0, 3);

        switch (pos)
        {
            case 0://up
                return new Vector2(
                    Random.Range(-7.5f, 7.5f),
                    4.3f
                    );
            case 1://down
                return new Vector2(
                    Random.Range(-7.5f, 7.5f),
                    -4.3f
                    );
            case 2://right
                return new Vector2(
                    7.5f,
                    Random.Range(-4.3f, 4.3f)
                    );
            case 3://left
                return new Vector2(
                    -7.5f,
                    Random.Range(-4.3f, 4.3f)
                    );
            default:
                return new Vector2(-7.5f, 4.3f);
        }
    }
}

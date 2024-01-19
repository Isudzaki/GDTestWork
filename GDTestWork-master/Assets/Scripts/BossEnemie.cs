using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemie : Enemie
{
    [SerializeField] private LevelConfig Config;

    [SerializeField] private Transform[] miniGoblinsSpawnPosition;
    [SerializeField] private GameObject goblin;

    public override void Die()
    {
        for (int i = 0; i < miniGoblinsSpawnPosition.Length; i++)
        {
            Instantiate(goblin, miniGoblinsSpawnPosition[i].position, Quaternion.identity);
            Debug.Log("Boss");
        }
        Config.Waves[curr]


        base.Die();

    }
}

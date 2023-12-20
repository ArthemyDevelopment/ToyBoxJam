using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class GameManager : SingletonManager<GameManager>
{

    public int Score;

    [SerializeField]private Dictionary<int, GameObject> ProgressSpawn = new Dictionary<int, GameObject>();

    private List<int> DicKeys;

    private void OnEnable()
    {
        DicKeys = new List<int>(ProgressSpawn.Keys);
    }

    public void CheckProgress(int score)
    {
        int ScoreToCheck = GetKeyToCheck(score);
        Debug.Log(ScoreToCheck);
        if (ProgressSpawn.ContainsKey(ScoreToCheck))
        {
            ProgressSpawn[ScoreToCheck].SetActive(true);
            Debug.Log(ProgressSpawn[ScoreToCheck].name);
        }
    }

    public int GetKeyToCheck(int score)
    {
        for (int i = 0; i < DicKeys.Count; i++)
        {
            if (i == DicKeys.Count - 1) return DicKeys[i];

            if (score >= DicKeys[i] && score < DicKeys[i + 1]) return DicKeys[i];
            
        }
        Debug.Log(score);
        return score;
    }
    

    [Button]
    public void StartGame()
    {
        BallController.current.InitBall();
        CheckProgress(0);
    }


    public void AddScore()
    {
        Score++;
        //UpdateUI
        CheckProgress(Score);
    }
}

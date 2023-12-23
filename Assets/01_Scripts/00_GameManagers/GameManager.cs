
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class GameManager : SingletonManager<GameManager>
{

    public int Score;
    public TMP_Text Tx_Score;
    public List<GameObject> Enemies;
    public GameObject LoseCanvas;

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
        Tx_Score.text = Score.ToString();
        CheckProgress(Score);
    }

    public void Reset()
    {
        foreach (GameObject G in Enemies)
        {
            G.SetActive(false);
        }
        ResetScore();
        PlayerController.current.ActiveMovement();
        StartGame();
    }

    public void ResetScore()
    {
        Score=0;
        Tx_Score.text = Score.ToString();
        CheckProgress(Score);
    }

    public void LoseGame()
    {
        LoseCanvas.SetActive(true);
    }
}

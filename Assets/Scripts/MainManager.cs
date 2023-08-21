using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    // player info
    public TextMeshProUGUI currentPlayer;
    public Text bestPlayerAndScore;

    public Text ScoreText;
    public GameObject GameOverText;
    
    private bool m_Started = false;
    private int m_Points;
    
    private bool m_GameOver = false;

    //static variables for storing player data 
    private static int bestScore;
    private static string bestPlayer;

    private void Awake()
    {
        LoadGameRank();
    }

    // Start is called before the first frame update
    void Start()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }

        currentPlayer.text = PlayerDataHandle.Instance.playerName;
        SetBestPlayer();
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        PlayerDataHandle.Instance.score = m_Points;
        ScoreText.text = $"Score : {m_Points}";
    }

    public void GameOver()
    {
        CheckBestPlayer();
        m_GameOver = true;
        GameOverText.SetActive(true);
    }

    

    private void CheckBestPlayer()
    {
        int currentScore = PlayerDataHandle.Instance.score;

        if (currentScore > bestScore)
        {
            bestScore = currentScore;
            bestPlayer = PlayerDataHandle.Instance.playerName;

            bestPlayerAndScore.text = $"Best Score: - {bestPlayer}: {bestScore}";

            SaveGameRank(bestPlayer, bestScore);
        }

    }
    // Set text to display best player and score if it is stored in data
    private void SetBestPlayer()
    {
        if (bestPlayer == null && bestScore == 0)
        {
            bestPlayerAndScore.text = "";
        }
        else
        {
            bestPlayerAndScore.text = "Best Score: " + bestScore + " - Best Player: " + bestPlayer;
        }
    }
    public void SaveGameRank(string bestPlayerName, int bestPlayerScore)
    {
        SaveData data = new SaveData();

        data.highScore = bestPlayerScore;
        data.theBestPlayer = bestPlayerName;

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/savedata.json", json);
    }
    public void LoadGameRank()
    {
        string path = Application.persistentDataPath + "/savedata.json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            bestScore = data.highScore;
            bestPlayer = data.theBestPlayer;
        }
    }

    [System.Serializable]
    class SaveData
    {
        public int highScore;
        public string theBestPlayer;
    }
}

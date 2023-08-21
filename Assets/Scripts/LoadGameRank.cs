using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;

public class LoadGameRank : MonoBehaviour
{
    public TextMeshProUGUI bestPlayerName;

    private static int bestScore;
    private static string bestPlayer;

    private void Awake()
    {
        LoadRank();
    }

    private void SetBestPlayer()
    {
       if (bestPlayer == null && bestScore == 0)
        {
            bestPlayerName.text = "";
        } 
       else
        {
            bestPlayerName.text = "Best Score: " + bestScore + " - Best Player: " + bestPlayer;
        }
    }
    private void LoadRank()
    {
        string path = Application.persistentDataPath + "/savefile.json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            bestPlayer = data.theBestPlayer;
            bestScore = data.highScore;

            SetBestPlayer();
        }
    }
   [System.Serializable]
   class SaveData
    {
        public int highScore;
        public string theBestPlayer;
    }
}

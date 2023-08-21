using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class UIMain : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI nameInput;

    public void SetPlayerName()
    {
        PlayerDataHandle.Instance.playerName = nameInput.text;
        Debug.Log(nameInput.text);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
    public void ReturntoMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif  
    }

    
}

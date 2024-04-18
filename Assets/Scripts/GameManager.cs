using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private bool _isGameOver = false;
    
    public bool _isCoOpMode = false;
    private UIManager _uiManager;
    private void Start()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if(_uiManager == null)
        {
            Debug.LogError("UI Manager is null!");
        }

    }

    private void Update()
    {
        // if the R key was pressed
        // restart the current scene
        if(Input.GetKeyDown(KeyCode.R) && _isGameOver == true)
        {
            if (!_isCoOpMode)
            {
                SceneManager.LoadScene(1);
            }
            else
            {
                SceneManager.LoadScene(3);
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            PauseGame();
            _uiManager.ShowPausePanel();
        }
    }

    public void GameOver()
    {
        _isGameOver = true;
    }
    public void PauseGame()
    {
        Time.timeScale = 0;
    }
    public void ResumeGame()
    {
        Time.timeScale = 1;
    }
}

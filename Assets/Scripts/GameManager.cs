using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private bool _isGameOver = false;
    
    public bool _isCoOpMode = false;

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
    }

    public void GameOver()
    {
        _isGameOver = true;
    }
}

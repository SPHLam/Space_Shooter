using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewMainMenu : MonoBehaviour
{
    public void LoadSinglePlayerGame()
    {
        SceneManager.LoadScene(1);
    }
    public void LoadCoOpModeGame()
    {
        SceneManager.LoadScene(2);
    }
}

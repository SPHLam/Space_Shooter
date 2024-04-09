using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // Handle to Text
    [SerializeField]
    private Text _scoreText;
    // Start is called before the first frame update
    void Start()
    {
        // Assign text component to handle
        _scoreText.text = "Score: " + 0;
    }

    // Update is called once per frame
    public void UpdateScore(int playerScore)
    {
        _scoreText.text = "Score: " + playerScore;
    }
}

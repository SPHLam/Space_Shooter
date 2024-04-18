using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // Handle to Text
    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private Text _bestScoreText;

    [SerializeField]
    private Image _livesImage;
    [SerializeField]
    private Sprite[] _liveSprites;

    [SerializeField]
    private Text _gameOverText;

    [SerializeField]
    private Text _restartText;

    private GameManager _gameManager;

    [SerializeField]
    private GameObject _pausePanel;

    private Animator _pausePanelAnimator;

    private int _bestScore;

    // Start is called before the first frame update
    void Start()
    {
        // Assign text component to handle
        _scoreText.text = "Score: " + 0;
        _bestScore = PlayerPrefs.GetInt("Best Score", 0);
        _bestScoreText.text = "Best: " + _bestScore;
        _gameOverText.gameObject.SetActive(false);
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        if(_gameManager == null)
        {
            Debug.LogError("GameManager is null!");
        }

        _pausePanelAnimator = GameObject.Find("Pause_Menu_Panel").GetComponent<Animator>();
        if(_pausePanelAnimator == null)
        {
            Debug.LogError("Pause panel animator is null!");
        }
        else
        {
            _pausePanelAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;
        }
    }
    

    // Update is called once per frame
    public void UpdateScore(int playerScore)
    {
        _scoreText.text = "Score: " + playerScore;
    }

    public void UpdateBestScore(int newBestScore)
    {
        _bestScoreText.text = "Best: " + newBestScore;
    }

    public void UpdateLives(int currentLives)
    {
        _livesImage.sprite = _liveSprites[currentLives];
    }
    public void ShowGameOver()
    {
        _gameOverText.gameObject.SetActive(true);
        _restartText.gameObject.SetActive(true);
        _gameManager.GameOver();
        StartCoroutine(GameOverFlickerRoutine());
    }

    IEnumerator GameOverFlickerRoutine()
    {
        while (true)
        {
            _gameOverText.text = "GAME OVER";
            yield return new WaitForSeconds(0.5f);
            _gameOverText.text = "";
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void ShowPausePanel()
    {
        _pausePanel.SetActive(true);
        _pausePanelAnimator.SetBool("isPaused", true);
    }

    public void ResumeGame()
    {
        _pausePanel.SetActive(false);
        _gameManager.ResumeGame();
        _pausePanelAnimator.SetBool("isPaused", false);
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(2);
        ResumeGame();
    }
}

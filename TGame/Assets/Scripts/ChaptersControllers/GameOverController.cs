using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverController : MonoBehaviour
{
    [SerializeField] private Button playAgainButton;
    [SerializeField] private Button quitButton;

    private void OnEnable()
    {
        playAgainButton.onClick.AddListener(PlayAgain);
        quitButton.onClick.AddListener(QuitGame);
    }

    private void PlayAgain()
    {
        SceneManager.LoadScene("MenuScene");
    }
    
    private void QuitGame()
    {
        Application.Quit();
    }

    private void OnDisable()
    {
        playAgainButton.onClick.RemoveAllListeners();
        quitButton.onClick.RemoveAllListeners();
    }
}

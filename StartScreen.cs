using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartScreen : MonoBehaviour
{


    [SerializeField] private GameObject loadGame;
    [SerializeField] private GameObject exitGame;


    private Button _loadGame;
    private Button _exitGame;

    private void Awake()
    {

        _loadGame = loadGame.GetComponent<Button>();
        _exitGame = exitGame.GetComponent<Button>();
    }

    private void Start()
    {

        _loadGame.onClick.AddListener(loadGameButton);
        _exitGame.onClick.AddListener(exitGameButton);

        Time.timeScale = 0f;
    }


    void loadGameButton()
    {
        
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;
        gameObject.SetActive(false);
    }

    void exitGameButton()
    {
        Application.Quit();
    }
}
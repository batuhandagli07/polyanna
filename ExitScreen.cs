using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitScreen : MonoBehaviour
{



    [SerializeField] private GameObject exitGame;
    



    private Button _exitGame;

    private void Awake()
    {

        _exitGame = exitGame.GetComponent<Button>();
    }

    private void Start()
    {

        _exitGame.onClick.AddListener(exitGameButton);

        Time.timeScale = 0f;
    }


    void exitGameButton()
    {
        Application.Quit();
    }
}

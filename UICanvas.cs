using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICanvas : MonoBehaviour
{

    private Image greenBar;
    private GameObject uiCanvas;
    [SerializeField] private float health= 100f;

    private void Awake()
    { 
         uiCanvas = GameObject.Find("UI Canvas/green");
         greenBar = uiCanvas.GetComponent<Image>();
         
    }

    void Update()
    {
  
        greenBar.fillAmount = health / 100;

    }

    
}

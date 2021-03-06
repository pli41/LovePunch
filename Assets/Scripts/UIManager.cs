﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    [SerializeField]
    Slider HP_Bar;
    [SerializeField]
    Slider HP_Bar_Player;
    [SerializeField]
    GameManager gameManager;
    Prince prince;

    public GameObject[] beforeGameComponents;
    public GameObject[] inGameComponents;
    public GameObject[] betweenLevelsComponents;
    public GameObject[] afterGameComponents;

    // Use this for initialization
    void Start () {
        prince = gameManager.prince;
	}
	
	// Update is called once per frame
	void Update () {
        HandleUI();
    }

    void HandleUI()
    {
        //HP_Bar
        HP_Bar.value = prince.GetHealth();
        HP_Bar_Player.value = gameManager.player.hp;
        if (GameManager.state == GameManager.gameState.BeforeGame)
        {
            SetGameObjects(beforeGameComponents, true);
            SetGameObjects(inGameComponents, false);
            SetGameObjects(betweenLevelsComponents, false);
            SetGameObjects(afterGameComponents, false);
        }
        else if (GameManager.state == GameManager.gameState.InGame)
        {
            SetGameObjects(beforeGameComponents, false);
            SetGameObjects(inGameComponents, true);
            SetGameObjects(betweenLevelsComponents, false);
            SetGameObjects(afterGameComponents, false);


        }
        else if (GameManager.state == GameManager.gameState.BetweenLevels)
        {
            SetGameObjects(beforeGameComponents, false);
            SetGameObjects(inGameComponents, false);
            SetGameObjects(betweenLevelsComponents, true);
            SetGameObjects(afterGameComponents, false);
        }
        else if (GameManager.state == GameManager.gameState.AfterGame)
        {
            SetGameObjects(beforeGameComponents, false);
            SetGameObjects(inGameComponents, false);
            SetGameObjects(betweenLevelsComponents, false);
            SetGameObjects(afterGameComponents, true);
        }
    }

    void SetGameObjects(GameObject[] gameObjects, bool state)
    {
        foreach (GameObject gameObject in gameObjects)
        {
            gameObject.SetActive(state);
        }
    }


}

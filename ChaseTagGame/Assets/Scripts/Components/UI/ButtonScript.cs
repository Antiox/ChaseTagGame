using GameLibrary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonScript : MonoBehaviour
{
    public void StartGame()
    {
        GameManager.Reset();
        SceneManager.LoadScene(1); //Main Game
    }
}

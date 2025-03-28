using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public Button restartButton;
    public PlayerController player;

    public void restartGame()
    {
        SceneManager.LoadSceneAsync(1);
    }


    private void OnEnable()
    {
        player.onPlayerDies += OnPlayerDead;
    }

    private void OnPlayerDead()
    {
        restartButton.gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        player.onPlayerDies -= OnPlayerDead;
    }
}

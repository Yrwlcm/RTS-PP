using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinConditionManager : MonoBehaviour
{
    public GameObject winScreen;
    private Coroutine checkWinCoroutine;

    private void Start()
    {
        checkWinCoroutine = StartCoroutine(CheckWin());
    }

    private IEnumerator CheckWin()
    {
        var teamsWithUnits = 0;
        var lastTeamId = 0;
        while (true)
        {
            teamsWithUnits = 0;
            foreach (var (team, selectionManager) in SelectionManager.Instances)
            {
                if (selectionManager.allUnits.Count <= 0) continue;
                teamsWithUnits++;
                lastTeamId = team;
            }

            if (teamsWithUnits == 1)
                WinScreen(lastTeamId);

            yield return new WaitForSeconds(1f);
        }
    }

    public void WinScreen(int teamId)
    {
        StopCoroutine(checkWinCoroutine);
        var winHeader = winScreen.transform.Find("Header").GetComponent<TextMeshProUGUI>();
        winHeader.text =  $"Победила команда {teamId}!";
        winScreen.SetActive(true);
        Time.timeScale = 0f;
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("StartingScene");
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
        Application.Quit();
    }
}
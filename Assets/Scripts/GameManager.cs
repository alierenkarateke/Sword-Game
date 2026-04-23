using System.Collections;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    #region Settings

    [Header("Players")]
    [SerializeField] public GameObject player1;
    [SerializeField] public GameObject player2;

    [Header("UI Objects")]
    [SerializeField] private TextMeshProUGUI roundText;
    [SerializeField] private TextMeshProUGUI player1Score;
    [SerializeField] private TextMeshProUGUI player2Score;
    [SerializeField] private GameObject endScreen;
    [SerializeField] private TextMeshProUGUI winner;

    [Header("Round Number")]
    [SerializeField] private int totalRound = 3;

    #endregion Settings
    
    #region State
        
    private int player1Wins = 0;
    private int player2Wins = 0;
    private int currentRound = 1;
    private int winsNeeded;
    
    public bool matchOver = false;

    #endregion State

    #region UnityLifecycle

    void Awake()
    {
        Instance = this;
        winsNeeded = (totalRound / 2) + 1;
        roundText.text = "Round " + currentRound;
        player1Score.text = "Player 1 Score : " + player1Wins;
        player2Score.text = "Player 2 Score : " + player2Wins;
    }

    #endregion UnityLifecycle

    #region RoundManagement

    public void PlayerDied(string tag)
    {
        StartCoroutine(RoundEnd(tag));
    }

    IEnumerator RoundEnd(string tag)
    {
        if(matchOver)  yield break;

        if(tag == "Player1")
        {
            player2Wins++;
            player2Score.text = "Player 2 Score : " + player2Wins;
            
            winner.text = currentRound + ". Round winner is Player 2 !!!";
            StartCoroutine(ShowWinnerText());
        }
        else if(tag == "Player2")
        {
            player1Wins++;
            player1Score.text = "Player 1 Score : " + player1Wins;
            
            winner.text = currentRound + ". Round winner is Player 1 !!!";
            StartCoroutine(ShowWinnerText());
        }
        currentRound++;

        if(player2Wins >= winsNeeded)
        {
            endScreen.SetActive(true);

            winner.text = "Player 2 Win Whe Match!";
            StartCoroutine(ShowWinnerText());
            matchOver = true;
            yield return new WaitForSecondsRealtime(2.0f);
            Time.timeScale = 0;

            yield break;
        }
        
        if(player1Wins >= winsNeeded)
        {
            endScreen.SetActive(true);
            
            winner.text = "Player 1 Win Whe Match!";
            StartCoroutine(ShowWinnerText());
            matchOver = true;
            yield return new WaitForSecondsRealtime(2.0f);
            Time.timeScale = 0;
            
            yield break;
        }

        yield return new WaitForSecondsRealtime(1.5f);

        DisablePlayer();

        yield return new WaitForSecondsRealtime(2f);

        player1.GetComponent<PlayerStats>().ResetPlayer();
        player2.GetComponent<PlayerStats>().ResetPlayer();

        player1.GetComponentInChildren<Animator>().SetBool("isDead", false);
        player2.GetComponentInChildren<Animator>().SetBool("isDead", false);

        roundText.text = roundText.text = "Round " + currentRound;
        EnablePlayer();
    }

    

    IEnumerator ShowWinnerText()
    {
        winner.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(2);
        winner.gameObject.SetActive(false);
    }

    #endregion RoundManagement

    #region PlayerManagement

    void DisablePlayer()
    {
        player1.GetComponent<PlayerController>().isActive = false;
        player2.GetComponent<PlayerController>().isActive = false;
        player1.GetComponent<PlayerCombat>().isActive = false;
        player2.GetComponent<PlayerCombat>().isActive = false;
    }

    void EnablePlayer()
    {
        
        player1.GetComponent<PlayerController>().isActive = true;
        player2.GetComponent<PlayerController>().isActive = true;
        player1.GetComponent<PlayerCombat>().isActive = true;
        player2.GetComponent<PlayerCombat>().isActive = true;
    }
    #endregion PlayerManagement

}

using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MySceneManager : MonoBehaviour
{

    public GameManager gameManager;
    public string winnerText;
    public string winnerScene;

    public TMP_Text winnerTextUI;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "EndScene")
        {
            winnerTextUI = GameObject.FindGameObjectWithTag("WinnerText").GetComponent<TMP_Text>();
            winnerTextUI.text = winnerText + " Wins!";
        }



        winnerText = gameManager.GetWinnerName();
        if (gameManager.ThereIsAWinner())
        {
            SceneManager.LoadScene(winnerScene);
        }
        
    }
}

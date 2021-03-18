using UnityEngine;
using TMPro; //biblioteka potrzebna do uzycia TextMeshPro

public class ScoreUpdate : MonoBehaviour
{
    //pobieranie potrzebnych uchwytow obiektow
    public GameObject player;
    public TextMeshProUGUI topScoreText;
    public TextMeshProUGUI currentScoreText;

    void Start()
    {
        //wyswietlenie najlepszego dotychczas osiagnietego wyniku
        topScoreText.text = PlayerPrefs.GetFloat("topScore", 0).ToString("0");
    }

    void Update()
    {
        //aktualizacja obecnego wyniku gracza, dopóki jego zycie nie jest mniejsze lub rowne 0
        if(player.GetComponent<Player>().health > 0)
            currentScoreText.text = player.transform.position.z.ToString("0");
    }
}

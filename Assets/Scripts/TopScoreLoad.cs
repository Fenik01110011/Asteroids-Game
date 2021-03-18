using UnityEngine;
using TMPro; //biblioteka potrzebna do uzycia TextMeshPro

public class TopScoreLoad: MonoBehaviour
{
    public TextMeshProUGUI topScoreText; //pobiera potrzebny uchwyt do obiektu przechowujacego tekst

    void Start()
    {
        //wypisuje najlepszy dotychczas osiagniety wynik w grze
        topScoreText.text = PlayerPrefs.GetFloat("topScore", 0).ToString("0");
    }
}

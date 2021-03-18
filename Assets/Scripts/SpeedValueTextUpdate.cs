using UnityEngine;
using TMPro; //biblioteka potrzebna do uzycia TextMeshPro

public class SpeedValueTextUpdate : MonoBehaviour
{
    //pobieranie potrzebnych uchwytow obiektow
    public GameObject player;
    public TextMeshProUGUI speedValueText;

    void Update()
    {
        //jesli zycie gracza jest powyzej 0, to wyswietla aktualna predkosc gracza
        if (player.GetComponent<Player>().health > 0)
            speedValueText.text = player.GetComponent<Rigidbody>().velocity.z.ToString("0");
        else
            speedValueText.text = "0"; //wyswietla predkosc 0 jesli zycie gracza jest mniejsze lub rowne 0
    }
}

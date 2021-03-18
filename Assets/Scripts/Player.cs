using UnityEngine;
using TMPro; //biblioteka potrzebna do uzycia TextMeshPro

public class Player : MonoBehaviour
{
    //pobieranie potrzebnych uchwytow obiektow
    public GameObject playerContainer;
    public GameObject Camera1;
    public GameObject Camera2;
    public GameObject obstacleGenerator;
    public GameObject endGamePanel;
    public GameObject explosionPrefab;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI healtText;

    //deklaracja potrzebnych zmiennych dostepnych do edycji w Unity oraz przez zewnetrzne skrypty
    public float forwardForce = 2000f;
    public float sidewaysForce = 200f;
    public float maxSidewaysSpeed = 150f;
    public float basicMinSpeed = 150;
    public float increasingSpeed = 1;
    public float currentMinSpeed = 0;
    public float spacecraftDurability = 100;
    public int health = 100;

    public float fireRate = 1f;
    public bool fireRateScale = true;
    public GameObject[] weapons;

    private float[] nextTimeToFire;

    Rigidbody rb;

    bool lags = false; //zmienna przechowujaca to czy pomiedzy ostatnimi klatkami pojawily sie duze opoznienia

    void Start() //funkcja wywolywana w momencie powstania obiektu, wczesniej od funkcji Update()
    {
        //ustawianie poczatkowych wartosci zmiennych
        health = 100;
        currentMinSpeed = basicMinSpeed;

        rb = GetComponent<Rigidbody>();
        Camera2.SetActive(false); //wylacza kamere widoku statku z przodu

        nextTimeToFire = new float[weapons.Length]; //tworzy odpowiednia ilosc zmiennych w tablicy odpowiadajacych liczbie dzialek statku kosmicznego
        for (int i = 0; i < weapons.Length; i++)
            nextTimeToFire[i] = 0; //przypisuje wartosc 0 do wszystkich powstałych nowych zmiennych w tablicy
    }

    //funckaj FixedUpdate() wywolywana jest przy każdym przeliczaniu fizyki w grze
    //używana jest przy np. dodawaniu siły do Rigidbody
    void FixedUpdate()
    {
        //sterowanie statkiem kosmicznym w gore, dol i boki za pomoca klawiszy "a","d","w","s" lub strzalek
        //poprzez dodawanie do Rigidbody odpowiednio skierowanej sily oddzialujacej na statek  
        if ((Input.GetKey("a") || Input.GetKey(KeyCode.LeftArrow)) && rb.velocity.x > -maxSidewaysSpeed)
            rb.AddForce(-sidewaysForce * Time.deltaTime, 0, 0, ForceMode.VelocityChange); // Time.deltaTime przechowuje czas jaki minal od ostatniej klatki
        if ((Input.GetKey("d") || Input.GetKey(KeyCode.RightArrow)) && rb.velocity.x < maxSidewaysSpeed)
            rb.AddForce(sidewaysForce * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
        if ((Input.GetKey("w") || Input.GetKey(KeyCode.UpArrow)) && rb.velocity.y < maxSidewaysSpeed)
            rb.AddForce(0, sidewaysForce * Time.deltaTime, 0, ForceMode.VelocityChange);
        if ((Input.GetKey("s") || Input.GetKey(KeyCode.DownArrow)) && rb.velocity.y > -maxSidewaysSpeed)
            rb.AddForce(0, -sidewaysForce * Time.deltaTime, 0, ForceMode.VelocityChange);

        //spowalnianie i przyspieszanie statku klawiszami "q" i "e"
        if (Input.GetKey("q") && rb.velocity.z > currentMinSpeed)
            rb.AddForce(0, 0, -forwardForce * Time.deltaTime * rb.mass);
        if (Input.GetKey("e"))
            rb.AddForce(0, 0, forwardForce * Time.deltaTime * rb.mass);

        //strzelanie po wcisnieciu spacji
        if(Input.GetKey(KeyCode.Space))
        {
            if(health > 0) //jesli gracz "zyje"
            {
                for (int i = 0; i < nextTimeToFire.Length; i++) //wykonuje dzialania dla wszystkich dzialek statku
                    if (nextTimeToFire[i] <= Time.time) //jesli uplynela juz odpowiednia ilosc czasu, to pozwala na wystrzelenie z dzialka
                    {
                        weapons[i].GetComponent<Shot>().Shoot(); //wystrzeliwuje pocisk z danego dzialka

                        //jesli "fireRateScale" ustawione jest na "true", to zmienia czestotliwosc wystrzeliwania pocisków zaleznie od wielkosci dzialka
                        //w przypadku gdy ustawione jest na "false", wszystkie dzialka wystrzeliwuja pociski z taka sama czestotliwoscia
                        if (fireRateScale)
                            nextTimeToFire[i] = Time.time + 1f / fireRate * Mathf.Abs(Mathf.Pow(weapons[i].transform.localScale.x, 3));
                        else
                            nextTimeToFire[i] = Time.time + 1f / fireRate;
                    }
            }
        }

        //zapobieganie przeciazeniu gry poprzez zmniejszanie predkosci gracza
        //w momencie spadku klatek poniżej 30 na sekunde (odpowiada za to zmienna lags ustawiana wtedy na true)
        if (lags && rb.velocity.z > 0)
            rb.AddForce(0, 0, -forwardForce * Time.deltaTime * rb.mass);

        //zwiekszanie minimalnej predkosci statku kosmicznego kiedy nie ma znacznego spadku klatek,
        //lub zwiekszanie minimalnej predkosci mimo spadku klatek kiedy gracz leci szybciej niz minimalna predkosc
        if (!lags || currentMinSpeed + 20 < rb.velocity.z)
            currentMinSpeed += increasingSpeed * Time.deltaTime;

        //przyspieszanie tatku kiedy leci z mniejsza predkosci niz minimalna
        if (currentMinSpeed > rb.velocity.z)
            rb.AddForce(0, 0, forwardForce * Time.deltaTime * rb.mass);

        //ograniczenie predkosci lotu w pionie i poziomie do okreslonej wartosci oraz zmiejszanie tych predkosci stopniowo do zera
        rb.AddForce(sidewaysForce * Time.deltaTime * (-rb.velocity.x / maxSidewaysSpeed), sidewaysForce * Time.deltaTime * (-rb.velocity.y / maxSidewaysSpeed), 0, ForceMode.VelocityChange);

        //Obraca obiekt w podanym kierunku o okreslona ilosc stopni. 
        //Dziala dobrze, ale ustawiajac obiekt nie wyglada jakby dzialaly na niego jakies sily,
        //tylko po porstu wraca na dana pozycje.
        //transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(Vector3.forward), 1); 

        //plynna stabilizacja obrotu, tak aby statek kosmiczny byl skierowany zgodnie z wektorem (0, 0, 1)
        Vector3 spaceShipRotationVector = new Vector3(transform.rotation.x, transform.rotation.y, transform.rotation.z);
        if (spaceShipRotationVector.magnitude > 0.001f)
            rb.angularVelocity -= spaceShipRotationVector / 50;
    }

    private void Update() //funkcja uruchamiajaca sie w kazdej klatce gry 
    {
        //przelaczanie sie miedzy widokie z pierwszej i trzeciej osoby
        if (Input.GetKeyDown("c"))
        {
            if (Camera2.activeSelf)
                Camera2.SetActive(false);
            else
                Camera2.SetActive(true);
        }

        //sprawdzanie czy nienastapil spadek klatek ponizej 30 na sekunde
        if (1 / Time.deltaTime < 30)
            lags = true;
        else
            lags = false;
    }

    private void OnCollisionEnter(Collision collision) //wywolywana jest w przypadku wystapienia kolizji z innym obiektem
    {
        if (health > 0)
        {
            //odjecie zycia graczowi po zderzeniu z innym obiektem, zaleznie od sily jaka temu towarzyszyla oraz ustawionej wytrzymalosci statku
            health -= (int)((collision.impulse / Time.fixedDeltaTime).magnitude / (GetComponent<Rigidbody>().mass * spacecraftDurability));
            if(health <= 0)
            {
                health = 0;
                EndGame(); //wywolanie funkcji konczoncej gre w przypadku spadku zycia do zerowej lub mniejszej wartosci
            }
            healtText.text = health.ToString() + "%"; //wypisanie aktualnej ilosci zycia gracza w %
        }
    }

    //funkcja konczaca gre
    private void EndGame ()
    {
        obstacleGenerator.SetActive(false); //zatrzymanie generowania nowych obiektow
        playerContainer.SetActive(false); //ukrycie statku gracza (nie dezaktywacja calego obiektu gracza, poniewaz posiada na sobie aktyne kamery)
        GetComponent<Rigidbody>().isKinematic = true; //wylaczenie interakcji gracza z otoczeniem
        GameObject explosion = Instantiate(explosionPrefab, transform.position, transform.rotation); //aktywacja eksplozji statku gracza
        Destroy(explosion, 3f); //usuniecie obiektu eksplozji gracza po 3 skundach
        endGamePanel.SetActive(true); //aktywacja menu konca gry
        scoreText.text = transform.position.z.ToString("0"); //wypisanie wyniku gracza na ekranie

        if (PlayerPrefs.GetFloat("topScore", 0) < transform.position.z) //jesli wynik byl wiekszy od najlepszego poprzedniego wyniku, 
            PlayerPrefs.SetFloat("topScore", transform.position.z); //to zapisz go na stale jako nowy najlepszy wynik
    }
}

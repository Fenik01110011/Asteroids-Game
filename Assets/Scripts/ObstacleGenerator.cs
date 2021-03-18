using UnityEngine;

public class ObstacleGenerator : MonoBehaviour
{
    //deklaracja potrzebnych zmiennych dostepnych do edycji w Unity oraz przez zewnetrzne skrypty
    public GameObject player;
    public GameObject[] asteroidPrefabs;
    public GameObject[] spaceShipsPrefabs;

    //deklaracja potrzebnych zmiennych dostepnych do edycji w Unity oraz przez zewnetrzne skrypty
    public float numberOfAsteroids = 300f;
    public float incresingNumberOfAsteroids = 2f;
    public float renderDistance = 1000f;
    public float renderSpace = 1000f;
    public float removalDistance = 1000f;
    public float asteroidMaxRandomForce = 3000f;
    public float asteroidScale = 50f;

    public float numberOfSpaceShips = 10f;
    public float spaceShipMinSpeed = 50f;
    public float spaceShipMaxSpeed = 1000f;

    //funckaj FixedUpdate() wywolywana jest przy każdym przeliczaniu fizyki w grze
    //używana jest przy np. dodawaniu siły do Rigidbody
    void FixedUpdate() 
    {
        //zwiekszanie maksymalnej ilosci asteroid w danym czasie o ustalona wartosc na sekunde
        numberOfAsteroids += incresingNumberOfAsteroids * Time.deltaTime;

        GameObject[] asteroids = GameObject.FindGameObjectsWithTag("Asteroid"); //pobieranie tablicy utworzonych asteroid

        //korekcja odleglosci w ktorej tworzone sa obiekty, tak aby nie bylo
        //nagle pojawiajacych sie obiektow obok gracza kiedy osiaga duza szybkosc
        float playerSpeedCorrection = player.GetComponent<Rigidbody>().velocity.magnitude; 

        //usuwanie asteroid bedacych daleko od gracza
        for (int i = 0; i < asteroids.Length; i++)
            if (asteroids[i].transform.position.z - player.transform.position.z < -300 || (asteroids[i].transform.position - player.transform.position).magnitude > (renderDistance + removalDistance + playerSpeedCorrection))
                Destroy(asteroids[i]);

        GameObject[] spaceShips = GameObject.FindGameObjectsWithTag("SpaceShip"); //pobieranie tablicy utworzonych statkow kosmicznych
        //usuwanie statkow kosmicznych bedacych daleko od gracza
        for (int j = 0; j < spaceShips.Length; j++)
            if (spaceShips[j].transform.position.z - player.transform.position.z < -300 || (spaceShips[j].transform.position - player.transform.position).magnitude > (renderDistance + removalDistance + playerSpeedCorrection))
                Destroy(spaceShips[j]);

        //zwiekszanie limitu ilosci asteroid i statkow tak, aby gestosc pojawiania się ich byla podobna
        //mimo powiekszania sie przesterzeni ich tworzenia wraz ze wzrostem predkosci gracza 
        float multiplier = 1 + playerSpeedCorrection / renderDistance; 

        //tworzenie nowych asteroid w odpowiedniej odleglosci od gracza oraz losowych wartosciach masy, skali i predkosci
        GameObject asteroid;
        if (asteroids.Length < numberOfAsteroids * multiplier)
        {
            for (int i = (int)(numberOfAsteroids * multiplier) - asteroids.Length; i > 0; i--)
            {
                asteroid = Instantiate(asteroidPrefabs[Random.Range(0, asteroidPrefabs.Length)], new Vector3(Random.Range(-renderSpace, renderSpace), Random.Range(-renderSpace, renderSpace), renderDistance + Random.Range(0, 500)) + player.GetComponent<Rigidbody>().velocity + player.transform.position, Quaternion.identity);
                float randomNumber = Random.Range(0, asteroidScale);
                asteroid.GetComponent<Rigidbody>().mass *= Mathf.Pow(randomNumber, 3);
                asteroid.transform.localScale *= randomNumber;
                asteroid.GetComponent<Rigidbody>().AddForce(Random.insideUnitSphere * Random.Range(0, asteroidMaxRandomForce) * asteroid.GetComponent<Rigidbody>().mass);
            }
        }

        //tworzenie nowych statkow kosmicznych w odpowiedniej odleglosci od gracza oraz losowym zwrotem i predkoscia z okreslonego przedzialu
        GameObject spaceShip;
        if (spaceShips.Length < numberOfSpaceShips * multiplier)
        {
            for (int i = (int)(numberOfSpaceShips * multiplier) - spaceShips.Length; i > 0; i--)
            {
                spaceShip = Instantiate(spaceShipsPrefabs[Random.Range(0, spaceShipsPrefabs.Length)], new Vector3(Random.Range(-renderSpace, renderSpace), Random.Range(-renderSpace, renderSpace), renderDistance + Random.Range(0, 500)) + player.GetComponent<Rigidbody>().velocity + player.transform.position, Random.rotation);
                spaceShip.GetComponent<Rigidbody>().velocity = spaceShip.transform.forward * Random.Range(spaceShipMinSpeed, spaceShipMaxSpeed);
            }
        }
    }
}

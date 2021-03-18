using UnityEngine;

public class SpaceShip : MonoBehaviour
{
    public GameObject explosionPrefab; //gotowa eksplozja

    //ustawienia statkow kosmicznych
    public float increasingSpeed = 1;
    public float spacecraftDurability = 100;
    public int health = 100;

    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        //zwieksza predkosc statku o okreslona wartosc na sekunde
        rb.velocity += (transform.forward * increasingSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (health > 0)
        {
            //zmniejsza zycie statku po zderzeniu z innym obiektem, zaleznie od sily jaka temu towarzyszyla oraz ustawionej wytrzymalosci statku
            health -= (int)((collision.impulse / Time.fixedDeltaTime).magnitude / (GetComponent<Rigidbody>().mass * spacecraftDurability));
            if (health <= 0)
            {
                health = 0;

                GameObject explosion = Instantiate(explosionPrefab, transform.position, transform.rotation); //tworzy eksplozje
                Destroy(explosion, 3f); //usuwa obiekt eksplozji po 3 sekundach

                Destroy(gameObject); //usuwa statek kosmiczny
            }
        }
    }
}
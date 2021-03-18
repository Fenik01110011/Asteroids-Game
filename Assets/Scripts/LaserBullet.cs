using UnityEngine;

public class LaserBullet : MonoBehaviour
{
    public float bulletDeleteTime = 3f; //czas po ktorym pocisk ma zniknac zostajac usunietym

    public GameObject explosionPrefab; //gotowa eksplozja pocisku
    void Start()
    {
        Destroy(gameObject, bulletDeleteTime); //ustawia zniszczenie pocisku po okreslonym czasie w sekundach
    }
    private void OnCollisionEnter(Collision collision)
    {
        //w zaleznosci od masy napotkanego obiektu usuwa go lub zmienisza jego skale i mase
        if (collision.rigidbody && collision.gameObject.CompareTag("Asteroid"))
        {
            if (collision.rigidbody.mass < GetComponent<Rigidbody>().mass)
                Destroy(collision.gameObject); //usuniecie obiektu
            else
            {
                //zmniejszenie odpowiednio skali i masy trafionego obiektu
                collision.transform.localScale *= Mathf.Pow(((collision.rigidbody.mass - GetComponent<Rigidbody>().mass) / collision.rigidbody.mass), 3);
                collision.rigidbody.mass -= GetComponent<Rigidbody>().mass;
            }
        }

        //utworzenie wybuchu pocisku o odpowiedniej wielkosci oraz odtworzenie dzwieku wybuchu z odpowiednia glosnoscia
        GameObject explosion = Instantiate(explosionPrefab, transform.position, transform.rotation);
        explosion.transform.localScale *= (1 / ((Mathf.Pow(transform.localScale.magnitude, 3) * 100) + 0.1f));
        explosion.GetComponent<AudioSource>().volume = Mathf.Clamp(Mathf.Pow(transform.localScale.magnitude, 3) * 100, 0, 1);
        Destroy(explosion, 3f); //usuniecie obiektu wybuchu po okreslonej ilosci sekund

        Destroy(gameObject); //usuniecie pocisku
    }
}

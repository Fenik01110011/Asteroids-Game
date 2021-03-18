using UnityEngine;

public class RandomRotation : MonoBehaviour
{
    [SerializeField] //sprawia, ze zmienna moze byc edytowana w Unity mimo deklaracji jako private
    private float rotationRangeSpeed = 5f; //maksymalna szybkosc obrotu

    void Start()
    {
        //ustawienie losowego kierunku i szybkosci obracania się obiektu
        GetComponent<Rigidbody>().angularVelocity = Random.insideUnitSphere * rotationRangeSpeed;
    }
}
using UnityEngine;

public class Bomb : MonoBehaviour
{
    private ParticleSystem explosion;
    private void Start()
    {
        explosion = GetComponent<ParticleSystem>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            explosion.Play();
            FindObjectOfType<GameManager>().Explode();
        }
    }
}

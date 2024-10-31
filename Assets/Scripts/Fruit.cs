using System.Threading;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    public GameObject whole;
    public GameObject sliced;
    public bool isKiwi;
    public bool isBadApple;
    public bool isBombWatermelon;
    public float points = 1;

    private Rigidbody fruitRigidbody;
    private Collider fruitCollider;
    private Spawner spawner;
    private GameManager gameManager;
    private AudioSource audioSource;
    private ParticleSystem juice;

    private void Awake()
    {
        fruitRigidbody = GetComponent<Rigidbody>();
        fruitCollider = GetComponent<Collider>();
        spawner = FindObjectOfType<Spawner>();
        gameManager = GameManager.Instance;
        audioSource = GetComponent<AudioSource>();
        juice = GetComponentInChildren<ParticleSystem>();
    }

    private void Slice(Vector3 direction, Vector3 position, float force)
    {
        // Level 2
        if (isKiwi && GameManager.Instance != null && GameManager.Instance.score >= 100)
        {
            points *= 1.5f;
        }
        
        // Level 3
        if (isBadApple && GameManager.Instance != null && GameManager.Instance.score >= 200)
        {
            points *= -2.0f;
            spawner.SetBombChance(0.1f);
        }

        // Level 4
        if (GameManager.Instance != null && GameManager.Instance.score >= 300)
        {
            spawner.SetBombChance(0.2f);
        }
        
        if (isBombWatermelon)
        {
            gameManager.Explode();
        }

        // Level 5
        if (GameManager.Instance != null && GameManager.Instance.score >= 400)
        {
            points *= 0.5f;
            spawner.SetBombChance(0.25f);
        }

        audioSource.Play();
        juice.Play();

        GameManager.Instance?.IncreaseScore(points);

        whole.SetActive(false);
        sliced.SetActive(true);

        fruitCollider.enabled = false;
    
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        sliced.transform.rotation = Quaternion.Euler(0f, 0f, angle);

        Rigidbody[] slices = sliced.GetComponentsInChildren<Rigidbody>();

        foreach (Rigidbody slice in slices)
        {
            slice.velocity = fruitRigidbody.velocity;
            slice.AddForceAtPosition(direction * force, position, ForceMode.Impulse);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Blade blade = other.GetComponent<Blade>();
            Slice(blade.direction, blade.transform.position, blade.sliceForce);
        }
    }
}
// Importa los namespace necesarios
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Hace que este script se ejecute primero
[DefaultExecutionOrder(-1)]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public Blade blade; 
    public Spawner spawner;
    public Text scoreText;
    public Image fadeImage;
    public GameObject[] initialFruits;
    public GameObject fruitLevel2;
    public GameObject fruitLevel3;
    public GameObject fruitLevel4;
    public GameObject fruitLevel5;

    public float score { get; private set; } = 0;

    // Verifica si ya existe un GameManager
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        blade = FindObjectOfType<Blade>();
        spawner = FindObjectOfType<Spawner>();
    }

    // Cuando destryue un GameManager libera su referencia estática
    private void OnDestroy()
    {
        if (Instance == this) {
            Instance = null;
        }
    }

    // Inicia un nuevo juego
    private void Start()
    {
        NewGame();
    }

    // Crea un nuevo juego
    private void NewGame()
    {
        Time.timeScale = 1f;

        ClearScene();

        blade.enabled = true;
        spawner.enabled = true;

        score = 0;
        scoreText.text = score.ToString();
        spawner.ResetFruits();
    }

    // Destruye todos los objetos para limpiar la escena
    private void ClearScene()
    {
        Fruit[] fruits = FindObjectsOfType<Fruit>();

        foreach (Fruit fruit in fruits) {
            Destroy(fruit.gameObject);
        }

        Bomb[] bombs = FindObjectsOfType<Bomb>();

        foreach (Bomb bomb in bombs) {
            Destroy(bomb.gameObject);
        }
    }

    // Aumento de puntuación y añade nuevas frutas
    public void IncreaseScore(float points)
    {
        score += points;
        scoreText.text = score.ToString();

        //float hiscore = PlayerPrefs.GetFloat("hiscore", 0);

        if (score == 100)
        {
            spawner.AddFruitPrefab(fruitLevel2);
        }
        
        if (score >= 200 && score <= 201)
        {
            spawner.AddFruitPrefab(fruitLevel3);
        }

        if (score >= 300 && score <= 301)
        {
            spawner.AddFruitPrefab(fruitLevel4);
        }

        if (score >= 400 && score <= 401)
        {
            spawner.AddFruitPrefab(fruitLevel5);
        }

        //if (score == hiscore)
        //{
         //   hiscore = score;
         //   PlayerPrefs.SetFloat("hiscore", hiscore);
        //}
    }

    public void Explode()
    {
        blade.enabled = false;
        spawner.enabled = false;

        StartCoroutine(ExplodeSequence());
    }

    private IEnumerator ExplodeSequence()
    {
        float elapsed = 0f;
        float duration = 0.5f;

        while (elapsed < duration)
        {
            float t = Mathf.Clamp01(elapsed / duration);
            fadeImage.color = Color.Lerp(Color.clear, Color.white, t);

            Time.timeScale = 1f - t;
            elapsed += Time.unscaledDeltaTime;

            yield return null;
        }

        yield return new WaitForSecondsRealtime(1f);

        NewGame();

        elapsed = 0f;

        while (elapsed < duration)
        {
            float t = Mathf.Clamp01(elapsed / duration);
            fadeImage.color = Color.Lerp(Color.white, Color.clear, t);

            elapsed += Time.unscaledDeltaTime;

            yield return null;
        }
    }
}
using TMPro;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    private static SceneController _instance;

    public static SceneController Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    public TextMeshProUGUI scoreText, livesText;
    public GameObject player;

    public int score = 0;
    private int _lives = 3;
    public int lives
    {
        get { return _lives; }
        set
        {
            if (value < _lives)
            {
                player.GetComponent<PlayerPlatformerController>().TakeDamage();
            }

            if (value == 0)
            {
                player.GetComponent<PlayerPlatformerController>().Die();
            }

            _lives = value;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        scoreText.text = $"Score: {score}";
        livesText.text = $"Lives: {lives}";
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = $"Score: {score}";
        livesText.text = $"Lives: {lives}";
    }
}

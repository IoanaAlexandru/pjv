using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SceneController : MonoBehaviour
{
    [SerializeField] private Card originalCard;
    [SerializeField] private Sprite[] images;

    private int gridRows = 2, gridCols = 5;
    private float offsetX = 4f, offsetY = 4.5f;

    private Card firstRevealed, secondRevealed;

    public int score = 0;
    [SerializeField] private TextMeshPro scoreLabel;

    public bool canReveal { get { return secondRevealed == null; } }

    public void OnReveal(Card card)
    {
        StartCoroutine(OnRevealCoroutine(card));
    }

    private IEnumerator OnRevealCoroutine(Card card)
    {
        Debug.Assert(canReveal == true);
        if (firstRevealed == null)
        {
            firstRevealed = card;
        } else
        {
            secondRevealed = card;
            if (firstRevealed.cardFront == secondRevealed.cardFront)
            {
                score += 10;
                firstRevealed = secondRevealed = null;
            } else
            {
                yield return new WaitForSeconds(1);
                firstRevealed.Hide();
                secondRevealed.Hide();
                firstRevealed = secondRevealed = null;
                score--;
            }
            UpdateScore();
        }
    }

    private void UpdateScore()
    {
        scoreLabel.text = "Score: " + score;
        scoreLabel.ForceMeshUpdate();
    }

    private void Start()
    {
        // Select random images from sprite array
        int count = gridRows * gridCols;
        int[] imageIndices = new int[count];
        HashSet<int> alreadySelected = new HashSet<int>();
        int i = 0;
        while (i < count)
        {
            int index;
            do
            {
                index = Random.Range(0, images.Length);
            } while (alreadySelected.Contains(index));
            alreadySelected.Add(index);
            imageIndices[i] = imageIndices[i + 1] = index;
            i += 2;
        }

        // Shuffle card order
        imageIndices = ShuffleArray<int>(imageIndices);

        // Initialize original card
        int id = 0;
        originalCard.enabled = true;
        originalCard.SetCard(id, images[imageIndices[id]]);
        id++;
        Vector3 startPos = originalCard.transform.position;


        for (i = 0; i < gridCols; i++)
        {
            for (int j = 0; j < gridRows; j++)
            {
                Card card;
                if (i == 0 && j == 0)
                {
                    continue;
                }
                card = Instantiate(originalCard) as Card;
                card.SetCard(id, images[imageIndices[id]]);
                id++;

                float posX = (offsetX * i) + startPos.x;
                float posY = -(offsetY * j) + startPos.y;
                card.transform.position = new Vector3(posX, posY, startPos.z);
            }
        }
    }

    public void Reset()
    {
        SceneManager.LoadScene("Main");
    }

    private T[] ShuffleArray<T>(T[] array)
    {
        T[] shuffled = array.Clone() as T[];
        for (int i = 0; i < shuffled.Length; i++)
        {
            T tmp = shuffled[i];
            int r = Random.Range(i, shuffled.Length);
            shuffled[i] = shuffled[r];
            shuffled[r] = tmp;
        }
        return shuffled;
    }
}

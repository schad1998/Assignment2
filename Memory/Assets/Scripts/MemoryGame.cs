using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MemoryGame : MonoBehaviour
{
    private int[] cardIndexes;
    private Button firstCard;
    private Button secondCard;
    private int numCardsFlipped;
    private int numPairsMatched;
    private List<Sprite> sprites = new List<Sprite>();
    private Sprite blankSprite;

    void Start()
    {
        // Load the blank image
        blankSprite = Resources.Load<Sprite>("blank");

        // Load the cartoon character images
        foreach (string path in Resources.LoadAll<Sprite>("cartoon_characters").Select(x => x.name))
        {
            sprites.Add(Resources.Load<Sprite>("cartoon_characters/" + path));
        }

        // Create an array of card indexes (pairs of numbers)
        cardIndexes = Enumerable.Range(0, sprites.Count).Concat(Enumerable.Range(0, sprites.Count)).ToArray();

        Shuffle(cardIndexes);

        // Set each button's background image to the blank image and attach the CardClicked function to its onClick event
        for (int i = 0; i < 16; i++)
        {
            Button button = transform.GetChild(i).GetComponent<Button>();
            button.image.sprite = blankSprite;
            button.onClick.AddListener(() => CardClicked(button));
        }
        Debug.Log(string.Join(", ", cardIndexes));
    }




public void CardClicked(Button button)
{
    int index = GetButtonIndex(button);

    if (!IsCardFlippedOver(button))
    {
        if (numCardsFlipped == 0)
        {
            firstCard = button;
            firstCard.image.sprite = sprites[cardIndexes[index]];
            numCardsFlipped++;
        }
        else if (numCardsFlipped == 1)
        {
            secondCard = button;
            secondCard.image.sprite = sprites[cardIndexes[index]];
            numCardsFlipped++;

            if (firstCard.image.sprite == secondCard.image.sprite)
            {
                numPairsMatched++;
                firstCard.interactable = false;
                secondCard.interactable = false;
                firstCard.image.color = Color.black;
                secondCard.image.color = Color.black;
                firstCard = null;
                secondCard = null;
                numCardsFlipped = 0;

                if (numPairsMatched == 8)
                {
                    // Game Over
                    StartCoroutine(ExampleCoroutine());
                }
            }
            else
            {
                StartCoroutine(FlipCardsBackOver());
            }
        }
    }
}

    IEnumerator ExampleCoroutine()
    {

        yield return new WaitForSeconds(5);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }


    private bool IsCardFlippedOver(Button button)
    {
        return button.image.sprite != blankSprite;
    }

    private int GetButtonIndex(Button button)
    {
        return button.transform.GetSiblingIndex();
    }

    private IEnumerator FlipCardsBackOver()
    {
        yield return new WaitForSeconds(1.0f);

        firstCard.image.sprite = blankSprite;
        secondCard.image.sprite = blankSprite;
        firstCard = null;
        secondCard = null;
        numCardsFlipped = 0;
    }

    private void Shuffle(int[] array)
    {
        for (int i = array.Length - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            int temp = array[i];
            array[i] = array[j];
            array[j] = temp;
        }
    }
}


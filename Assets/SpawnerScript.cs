using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class script : MonoBehaviour
{
    public GameObject nucleotide;
    public GameOverScript gameOver;
    public float offset = 2.5f;
    public float spawnPeriod = 1;
    private float timer = 0;
    private char[] allowedChars = { 'A', 'C', 'G', 'T' };
    public int counter = 0;
    bool spawn = true;

    private List<GameObject> nucleotides = new List<GameObject>();

    // Start is called before the first frame update
    void Start() {}

    // Update is called once per frame
    void Update() {
        if (timer < spawnPeriod) {
            timer += Time.deltaTime;
        } else if (spawn) {
            spawnNucleotide();
            timer = 0;
        }

        if (nucleotides.Count > 0) {
            GameObject oldestNucleotide = nucleotides[0];
            TextMeshPro textComponent = oldestNucleotide.GetComponentInChildren<TextMeshPro>();
            if (textComponent != null) {
                if (Input.GetKeyDown(textComponent.text.ToLower())) {
                    nucleotides.RemoveAt(0);
                    Destroy(oldestNucleotide);
                    counter += 1;
                }
            }

            while (true) {
                if (nucleotides.Count > 0) {
                    GameObject currentNucleotide = nucleotides[0];
                    if (currentNucleotide.transform.position.y < -6) {
                        nucleotides.RemoveAt(0);
                        Destroy(currentNucleotide);
                        Debug.Log("Game over. Final score: " + counter); // Debug log
                        if (gameOver != null) {
                            gameOver.Setup(counter);
                        } else {
                            Debug.Log("Setup is nullptr");
                        }
                        spawn = false;
                    } else {
                        break;
                    }
                }
            }
        }
    }

    void spawnNucleotide()
    {
        float minX = transform.position.x - offset;
        float maxX = transform.position.x + offset;

        GameObject instance = Instantiate(nucleotide, new Vector3(Random.Range(minX, maxX), transform.position.y, 0), transform.rotation);
        nucleotides.Add(instance);

        TextMeshPro textComponent = instance.GetComponentInChildren<TextMeshPro>();
        if (textComponent != null)
        {
            char randomLetter = allowedChars[Random.Range(0, allowedChars.Length)];
            textComponent.text = randomLetter.ToString();
        }
    }
}

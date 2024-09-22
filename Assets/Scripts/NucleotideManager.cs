using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class SpawnerScript : MonoBehaviour
{
    public GameObject nucleotide;
    public GameObject nucleotideDestroyParticles;
    public GameObject dnaStrand;

    [System.NonSerialized]
    private float baseSpawnPeriod = 1f,
                  currRelativeTime = 1f;
    private float spawnPeriod;
    private float timer = 0f;
    private char[] allowedChars = { 'A', 'C', 'G', 'T' };
    public int counter = 0;
    bool spawn = true;

    private List<GameObject> nucleotides = new List<GameObject>();
    private List<GameObject> dnaStrands = new List<GameObject>();

    // Start is called before the first frame update
    void Start() {
        spawnPeriod = baseSpawnPeriod;
        MusicManager.Instance.SetMusicVolume(0.25f);
        MusicManager.Instance.PlayMusic(1);
    }

    // Update is called once per frame
    void Update() {
        if (timer < spawnPeriod) {
            timer += Time.deltaTime;
        } else if (spawn) {
            SpawnNucleotide();
            if (dnaStrands.Count == 0) {
                SpawnDnaStrand();
            }
            timer = 0;
        }

        if (nucleotides.Count > 0) {
            // Get the oldest i.e. botom most nucleotide
            GameObject oldestNucleotide = nucleotides[0];

            // Check its text vs user ipt, if they match delete it
            TextMeshPro textComponent = oldestNucleotide.GetComponentInChildren<TextMeshPro>();
            char expectedKey = textComponent.text.ToLower()[0];
            if (textComponent != null) {
                if (Input.GetKeyDown(expectedKey.ToString())) {
                    KillNucleotide(oldestNucleotide);
                    UpdateRelativeTime(currRelativeTime += 0.02f);
                    return;
                }
                // Punish for bad key 
                else {
                    foreach (char key in allowedChars) {
                        if (key != expectedKey && Input.GetKeyDown(key.ToString().ToLower())) {
                            MusicManager.Instance.PlaySFX(1, 0.2f);
                            UpdateRelativeTime(currRelativeTime += 0.2f);
                            break;
                        }
                    }
                }
            }

            // If its y position, if outside the screen, game over
            if (oldestNucleotide.transform.position.y < -6) {
                nucleotides.RemoveAt(0);
                Destroy(oldestNucleotide);
                spawn = false;
                MusicManager.Instance.PlaySFX(1, 0.2f);
                Debug.Log("Game over. Final score: " + counter);
                ScoreManager.SetScore(counter);
                SceneManager.LoadSceneAsync("MainMenu");
                
            }
        }

        // For the first dna strand, spawn another one when it reaches y = 0
        if (dnaStrands.Count == 1 && dnaStrands[0].transform.position.y < 0) {
            SpawnDnaStrand();
        }
        // If the oldest dna strand is outside the screen, remove it and spawn another one
        else if (dnaStrands.Count > 0 && dnaStrands[0].transform.position.y < -11) {
            Destroy(dnaStrands[0]);
            dnaStrands.RemoveAt(0);
            SpawnDnaStrand();
        }
    }

    void SpawnNucleotide() {
        GameObject instance = Instantiate(nucleotide, new Vector3(transform.position.y, transform.position.y, 0), transform.rotation);
        nucleotide.GetComponent<NucleotideMoveScript>().SetStartPos(Time.time);
        UpdateGameObjectRelativeTime<NucleotideMoveScript>(instance);
        nucleotides.Add(instance);

        TextMeshPro textComponent = instance.GetComponentInChildren<TextMeshPro>();
        if (textComponent != null)
        {
            char randomLetter = allowedChars[Random.Range(0, allowedChars.Length)];
            textComponent.text = randomLetter.ToString();
        }
    }

    void UpdateGameObjectRelativeTime<T>(GameObject gameObject) where T : class, IRelativeTime {
        T moveScript = nucleotide.GetComponent<T>();
        if (moveScript != null) {
            moveScript.relativeTime = currRelativeTime;
        }
    }

    void UpdateRelativeTime(float newTime) {
        // Increase speed of objects
        currRelativeTime = newTime;
        spawnPeriod = baseSpawnPeriod / currRelativeTime;
        foreach (GameObject nucleotide in nucleotides) {
            UpdateGameObjectRelativeTime<NucleotideMoveScript>(nucleotide);
        }

        // Also increase music speed. Not nice to change pitch
        // MusicManager.Instance.SetMusicSpeed(currRelativeTime);
    }

    void KillNucleotide(GameObject nucleotide) {
        GameObject particleEffect = Instantiate(nucleotideDestroyParticles,
                                                nucleotide.transform.position,
                                                nucleotide.transform.rotation);

        nucleotides.RemoveAt(0);
        Destroy(nucleotide);
        MusicManager.Instance.PlaySFX(0, 0.2f);
        counter += 1;
    }

    void SpawnDnaStrand() {
        GameObject instance = Instantiate(dnaStrand, new Vector3(0, 11, 0), transform.rotation);
        UpdateGameObjectRelativeTime<MotionDNA>(instance);
        dnaStrands.Add(instance);
    }
}

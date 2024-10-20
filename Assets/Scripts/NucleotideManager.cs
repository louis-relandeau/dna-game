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
    
    private float relativeCreationTime = 0f;

    // Start is called before the first frame update
    void Start() {
        spawnPeriod = baseSpawnPeriod;
        MusicManager.Instance.SetMusicVolume(0.25f);
        MusicManager.Instance.PlayMusic(1);

        // Immediately spawn 2 dna strands (one in the middle of the screen and one at the top)
        SpawnDnaStrand(0);
        SpawnDnaStrand(10);
    }

    // Update is called once per frame
    void Update() {
        relativeCreationTime += Time.deltaTime * currRelativeTime;
        if (timer < spawnPeriod) {
            timer += Time.deltaTime;
        } else if (spawn) {
            SpawnNucleotide();
            timer = 0;
        }

        // If the oldest dna strand is outside the screen, remove it and spawn another one
        if (dnaStrands.Count > 0 && dnaStrands[0].transform.position.y < -10) {
            Destroy(dnaStrands[0]);
            dnaStrands.RemoveAt(0);
            SpawnDnaStrand();
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
    }

    void SpawnNucleotide() {
        // Spawn it above scree so that it can move down
        GameObject instance = Instantiate(nucleotide, new Vector3(transform.position.y, transform.position.y + 1, 0), transform.rotation);
        instance.GetComponent<NucleotideMoveScript>().SetStartPosX(relativeCreationTime);
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
        T moveScript = gameObject.GetComponent<T>();
        if (moveScript != null) {
            moveScript.relativeTime = currRelativeTime;
            Debug.Log("updated relative time smth");
        }
    }

    void UpdateRelativeTime(float newTime) {
        // Increase speed of objects
        currRelativeTime = newTime;
        spawnPeriod = baseSpawnPeriod / currRelativeTime;
        foreach (GameObject nucleotide in nucleotides) {
            UpdateGameObjectRelativeTime<NucleotideMoveScript>(nucleotide);
        }
        foreach (GameObject dnaStrand in dnaStrands) {
            UpdateGameObjectRelativeTime<MotionDNA>(dnaStrand);
        }
        Debug.Log("Relative time: " + currRelativeTime);

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

    void SpawnDnaStrand(float optionalStartY = 10) {
        GameObject instance = Instantiate(dnaStrand, new Vector3(0, optionalStartY, 0), transform.rotation);
        UpdateGameObjectRelativeTime<MotionDNA>(instance);
        dnaStrands.Add(instance);
    }
}

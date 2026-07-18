using System;
using System.Collections;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using Random = UnityEngine.Random;

[System.Serializable]
public class AnxietyThought
{
    public string text;
    [Range(0, 100)]
    public float intensity;
}

[System.Serializable]
public class Choices
{
    public string choice;
    public AnxietyThought[] choiceThoughts;
    public string reactionText;
    public int nextDialogue;
}

[System.Serializable]
public class Dialogue
{
    public int id;
    [Range(0, 100)]
    public int anxietyLevel;
    public string speaker;
    public string caricatureName; // Changed from Sprite to string for JSON compatibility
    [TextArea]
    public string dialogue;
    public bool JustText;
    public bool hideUI;
    public Choices[] choices;
}

// Crucial wrapper class because JsonUtility cannot read/write raw top-level arrays
[System.Serializable]
public class DialogueDatabase
{
    public Dialogue[] dialogues;
}
public class DialogueManager : MonoBehaviour
{
    public Volume globalVolume;
    [Range(0, 100)]
    public float anxietyLevel;
    private float currentAnxietyLevel;
    public float lerpSpeed = 2f; 
    public TMP_Text speakerText;
    public TMP_Text dialogueText;
    public TMP_Text ReactText;
    public Image caricatureImage; // Reference to UI Image to display the character sprite
    public string[] VignetteAnxietyTexts;
    public ChoiceButton[] choiceButtons;
    public RectTransform testEllipsisImage;
    
    [Header("JSON Configuration")]
    public string jsonFileName = "DialogueData.json";

    // This array is now populated dynamically from the JSON file at runtime
    private Dialogue[] dialogues;
    public int currentDialogue;

    void Update()
    {
        // 1. Smoothly lerp the anxiety level
        currentAnxietyLevel = Mathf.Lerp(currentAnxietyLevel, anxietyLevel, Time.deltaTime * lerpSpeed);

        if (Mathf.Abs(currentAnxietyLevel - anxietyLevel) < 0.001f)
        {
            currentAnxietyLevel = anxietyLevel;
        }
        float t = anxietyLevel / 100f;
        float scale = Mathf.Lerp(0.95f, 0.35f, t);
        testEllipsisImage.localScale = Vector3.one * scale;
        // 2. Apply your formula directly using the smoothly shifting currentAnxiety
        if (globalVolume != null)
        {
            float calculatedWeight = 0.236f + currentAnxietyLevel * 0.005f;
            
            // Clamp01 ensures the value stays safely between 0.0 and 1.0
            globalVolume.weight = Mathf.Clamp01(calculatedWeight);
        }
    }

    void Start()
    {
        LoadDialogueFromJson();
        currentDialogue = 0;
        ShowDialogue();
    }

    private void LoadDialogueFromJson()
    {
        // Path works across target platforms (PC, Mac, Mobile)
        string filePath = Path.Combine(Application.streamingAssetsPath, jsonFileName);

        if (File.Exists(filePath))
        {
            string jsonText = File.ReadAllText(filePath);
            
            // Deserialize JSON into the wrapper database class
            DialogueDatabase database = JsonUtility.FromJson<DialogueDatabase>(jsonText);
            if (database == null || database.dialogues == null)
            {
                Debug.LogError("Failed to parse dialogue JSON.");
                return;
            }

            dialogues = database.dialogues;
            
            Debug.Log($"Successfully loaded {dialogues.Length} dialogue points from JSON.");
        }
        else
        {
            Debug.LogError($"Dialogue JSON file not found at path: {filePath}");
        }
    }

    public void ShowDialogue()
    {
        if (dialogues == null || dialogues.Length == 0) return;

        Dialogue current = GetDialogueByID(currentDialogue);

        if (current == null)
            return;
        speakerText.text = current.speaker;
        dialogueText.text = current.dialogue;

        // Dynamic Sprite Loading: Looks inside Assets/Resources/ folder for a matching sprite name
        if (caricatureImage != null)
        {
            if (!string.IsNullOrEmpty(current.caricatureName))
            {
                Sprite loadedSprite = Resources.Load<Sprite>(current.caricatureName);

                if (loadedSprite != null)
                {
                    caricatureImage.sprite = loadedSprite;
                    caricatureImage.enabled = true;
                }
                else
                {
                    caricatureImage.sprite = null;
                    caricatureImage.enabled = false;
                }
            }
            else
            {
                caricatureImage.sprite = null;
                caricatureImage.enabled = false;
            }
        }
        
        // Safety check to avoid index out of bounds exceptions on your button configuration
        if (current.JustText)
        {
            if (current.choices.Length == 0)
            {
                Debug.LogError($"Dialogue {current.id} is marked justText but has no choices.");
                return;
            }

            choiceButtons[0].gameObject.SetActive(false);
            choiceButtons[2].gameObject.SetActive(false);

            choiceButtons[1].gameObject.SetActive(true);
            choiceButtons[1].SetChoice(current.choices[0], current.choices[0].nextDialogue);

            choiceButtons[1].choiceText.text =
                string.IsNullOrWhiteSpace(current.choices[0].choice)
                    ? "Continue"
                    : current.choices[0].choice;
        }
        else
        {
            for (int i = 0; i < choiceButtons.Length; i++)
            {
                if (i < current.choices.Length)
                {
                    choiceButtons[i].gameObject.SetActive(true);
                    choiceButtons[i].SetChoice(current.choices[i], current.choices[i].nextDialogue);
                }
                else
                {
                    // Hides buttons if this particular dialogue piece has fewer choices
                    choiceButtons[i].gameObject.SetActive(false); 
                }
            }
        }

        anxietyLevel = current.anxietyLevel;
    }

    public void GoToDialogue(int index, string reactString)
    {
        if (GetDialogueByID(index) == null)
        {
            Debug.LogError($"Dialogue ID {index} doesn't exist.");
            return;
        }

        currentDialogue = index;
        StartCoroutine(ReactionText(reactString));
        ShowDialogue();
    }

    public IEnumerator ReactionText(string text)
    {
        ReactText.gameObject.SetActive(true);
        ReactText.text = text;
        ReactText.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        yield return new WaitForSeconds(Random.Range(0.7f, 1.5f));
        ReactText.gameObject.SetActive(false);
    }
    public float radiusX = 400;
    public float radiusY = 250;
    public int segments = 64;

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Vector3 center = Camera.main.ViewportToWorldPoint(
            new Vector3(0.5f, 0.5f, 10f)
        );

        Vector3 prev = center + new Vector3(radiusX, 0, 0);

        for (int i = 1; i <= segments; i++)
        {
            float angle = i / (float)segments * Mathf.PI * 2f;

            Vector3 next = center + new Vector3(
                Mathf.Cos(angle) * radiusX,
                Mathf.Sin(angle) * radiusY,
                0
            );

            Gizmos.DrawLine(prev, next);
            prev = next;
        }
    }
    public Dialogue GetDialogueByID(int id)
    {
        foreach (Dialogue dialogue in dialogues)
        {
            if (dialogue.id == id)
            {
                return dialogue;
            }
        }

        Debug.LogError($"Dialogue ID {id} not found.");
        return null;
    }
}

using System.Collections;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    public string speaker;
    public string caricatureName; // Changed from Sprite to string for JSON compatibility
    [TextArea]
    public string dialogue;
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
    public TMP_Text speakerText;
    public TMP_Text dialogueText;
    public TMP_Text ReactText;
    public Image caricatureImage; // Reference to UI Image to display the character sprite
    public ChoiceButton[] choiceButtons;
    
    [Header("JSON Configuration")]
    public string jsonFileName = "DialogueData.json";

    // This array is now populated dynamically from the JSON file at runtime
    private Dialogue[] dialogues;
    public int currentDialogue;

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

        Dialogue current = dialogues[currentDialogue];
        speakerText.text = current.speaker;
        dialogueText.text = current.dialogue;

        // Dynamic Sprite Loading: Looks inside Assets/Resources/ folder for a matching sprite name
        if (!string.IsNullOrEmpty(current.caricatureName) && caricatureImage != null)
        {
            Sprite loadedSprite = Resources.Load<Sprite>(current.caricatureName);
            if (loadedSprite != null)
            {
                caricatureImage.sprite = loadedSprite;
            }
        }
        
        // Safety check to avoid index out of bounds exceptions on your button configuration
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

    public void GoToDialogue(int index, string reactString)
    {
        if (index < 0 || index >= dialogues.Length)
        {
            Debug.LogError($"Dialogue index {index} doesn't exist.");
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
        yield return new WaitForSeconds(Random.Range(0.7f, 1.5f));
        ReactText.gameObject.SetActive(false);
    }
}

using System.Collections;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public class ChoiceButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Choices choice;
    public AnxietyThoughtSpawner spawner;
    public TMP_Text choiceText;
    public int goToDialogueIndex;
    public DialogueManager dialogueManager;

    private bool hovering;

    public void OnPointerEnter(PointerEventData eventData)
    {
        hovering = true;
        StartCoroutine(SpawnLoop());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hovering = false;
    }

    private IEnumerator SpawnLoop()
    {
        while (hovering)
        {
            spawner.SpawnRandomThought(choice.choiceThoughts, transform.position);
            yield return new WaitForSeconds(Random.Range(0.1f, 0.3f));
        }
    }
    
    public void SetChoice(Choices newChoice, int index)
    {
        choice = newChoice;
        choiceText.text = newChoice.choice;
        goToDialogueIndex = index;
    }

    public void ButtonClicked()
    {
        dialogueManager.GoToDialogue(goToDialogueIndex, choice.reactionText);
    }
}
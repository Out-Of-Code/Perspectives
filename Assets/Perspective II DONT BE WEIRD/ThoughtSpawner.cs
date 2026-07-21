using System;
using System.Collections;
using UnityEngine;
using TMPro;
using Random = UnityEngine.Random;

public class AnxietyThoughtSpawner : MonoBehaviour
{
    public GameObject thoughtPrefab;
    public GameObject vignetteThoughtPrefab;
    public GameObject canvas;
    public DialogueManager dialogueManager;

    private void Awake()
    {
        StartCoroutine(AmbientThoughtLoop());
    }

    public void SpawnRandomThought(AnxietyThought[] thoughts, Vector3 origin)
    {
        AnxietyThought thought = thoughts[Random.Range(0, thoughts.Length)];
        GameObject obj = Instantiate(
            thoughtPrefab,
            origin,
            Quaternion.identity,
            canvas.transform
        );

        obj.GetComponent<TMP_Text>().text = thought.text;
        
        obj.GetComponent<ThoughtMovement>().direction =
            Random.insideUnitCircle.normalized;
    }
    bool InsideExclusionZone(Vector2 position)
    {
        float scale = Mathf.Lerp(0.95f, 0.35f, dialogueManager.anxietyLevel / 100f);

        float radiusX = (Screen.width * scale) / 2f;
        float radiusY = (Screen.height * scale) / 2f;
        Vector2 center = new Vector2(Screen.width / 2f, Screen.height / 2f);

        float x = (position.x - center.x) / radiusX;
        float y = (position.y - center.y) / radiusY;

        return x * x + y * y <= 1f;
    }
    Vector2 GetSpawnPosition()
    {
        for (int i = 0; i < 100; i++)
        {
            Vector2 position = new Vector2(
                Random.Range(0, Screen.width),
                Random.Range(0, Screen.height)
            );

            if (!InsideExclusionZone(position))
                return position;
        }

        return Vector2.zero;
    }

    public void SpawnAmbientThought()
    {
        Vector2 position = GetSpawnPosition();
        string ChosenText;
        Debug.Log(dialogueManager.VignetteAnxietyTexts.Length);
        ChosenText = dialogueManager.VignetteAnxietyTexts[Random.Range(0, dialogueManager.VignetteAnxietyTexts.Length)];
        GameObject obj = Instantiate(
            vignetteThoughtPrefab,
            position,
            Quaternion.identity,
            canvas.transform
        );

        obj.GetComponent<TMP_Text>().text = ChosenText;
        
        obj.GetComponent<ThoughtMovement1>().direction =
            Random.insideUnitCircle.normalized;
    }
    IEnumerator AmbientThoughtLoop()
    {
        while (true)
        {
            SpawnAmbientThought();

            float minWait = Mathf.Lerp(0.01f, 0.01f, dialogueManager.anxietyLevel / 100f);
            float maxWait = Mathf.Lerp(0.5f, 0.02f, dialogueManager.anxietyLevel / 100f);

            yield return new WaitForSeconds(Random.Range(minWait, maxWait));
        }
    }
}
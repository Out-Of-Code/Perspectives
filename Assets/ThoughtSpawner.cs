using UnityEngine;
using TMPro;

public class AnxietyThoughtSpawner : MonoBehaviour
{
    public GameObject thoughtPrefab;
    public GameObject canvas;

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
}
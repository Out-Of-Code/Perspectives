using UnityEngine;

public class ThoughtMovement1 : MonoBehaviour
{
    public Vector2 direction;
    public float speed = 100f;
    public float TimeOfDeath;

    void Start()
    {
        TimeOfDeath = Time.time + Random.Range(2f, 4f);
    }
    void Update()
    {
        transform.position += 
            (Vector3)direction * speed * Time.deltaTime;
        if (Time.time > TimeOfDeath)
        {
            Destroy(gameObject);
        }
    }
}
using UnityEngine;

public class ThoughtMovement : MonoBehaviour
{
    public Vector2 direction;
    public float speed = 100f;
    public float TimeOfDeath;

    void Start()
    {
        TimeOfDeath = Time.time + Random.Range(0.2f, 2f);
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
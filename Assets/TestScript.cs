using UnityEngine;
using UnityEngine.SceneManagement;
public class TestScript : MonoBehaviour
{
    public GameObject Perspective1Button;
    public GameObject Perspective2Button;
    public GameObject Perspective3Button;
    public GameObject Perspective4Button;
    public GameObject Perspective5Button;
    public GameObject Perspective6Button;
    public GameObject Perspective7Button;
    public GameObject Perspective8Button;
    public GameObject Perspective9Button;
    public GameObject Perspective10Button;
    public void TestButton(GameObject button)
    {
        print(button.name + " Button works (:");
    }
    public void LoadAnxiety()
    {
        SceneManager.LoadScene(1);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

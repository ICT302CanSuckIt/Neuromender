using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ExtLvlReplaceWithButton : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("x")) SceneManager.LoadScene("ArmExtension");

        if (Input.GetKeyDown("r")) SceneManager.LoadScene("Rowing");
    }
}

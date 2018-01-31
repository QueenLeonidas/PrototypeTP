using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitch : MonoBehaviour
{
    public void GotoStartScene()
    {
        SceneManager.LoadScene("StartZustand");
    }

    public void GotoEndScene()
    {
        SceneManager.LoadScene("EndZustand");
    }

    public void GotoMenuScene()
    {

        SceneManager.LoadScene("Menu");

    }

    void Update()

    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GotoMenuScene();
        }
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }


    }
}

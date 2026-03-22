using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void OpenDikkatOyunu()
    {
        SceneManager.LoadScene("DikkatOyunu");
    }

    public void OpenHafizaOyunu()
    {
        SceneManager.LoadScene("HafizaOyunu");
    }
}
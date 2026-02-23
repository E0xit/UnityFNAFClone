using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverscript : MonoBehaviour
{
    [SerializeField] private Button[] buttons;

    void Start()
    {
        buttons = gameObject.GetComponentsInChildren<Button>(true);
        buttons[0].onClick.AddListener(Retry);
        buttons[1].onClick.AddListener(MainMenu);
    }
    private void Retry()
    {
        SceneManager.LoadScene("Gameplay");
    }
    private void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}

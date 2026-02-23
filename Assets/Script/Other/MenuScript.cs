using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.Random;

public class MenuScript : MonoBehaviour
{
    [SerializeField] private GameObject Aun;
    [SerializeField] private int luck;
    [SerializeField] private Button[] buttons;
    [SerializeField] private GameObject guideView;
    [SerializeField] private Button guide;
    [SerializeField] private Button backGuide;

    void Awake()
    {
        Aun.SetActive(false);
        guideView.SetActive(false);
    }
    void Start()
    {
        buttons = gameObject.GetComponentsInChildren<Button>(true);
        luck = Range(0, 10);
        if (luck < 3)
        {
            Aun.SetActive(true);
        }
        buttons[0].onClick.AddListener(StartGame);
        buttons[1].onClick.AddListener(ExitGame);
        guide.onClick.AddListener(OpenGuide);
        backGuide.onClick.AddListener(BackGuide);
    }
    private void StartGame()
    {
        SceneManager.LoadScene("Gameplay");
    }
    private void ExitGame()
    {
        Application.Quit();
    }
    private void OpenGuide()
    {
        guideView.SetActive(true);
    }
    private void BackGuide()
    {
        guideView.SetActive(false);
    }
}

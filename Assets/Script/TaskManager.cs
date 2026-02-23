using TMPro;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class TaskManager : MonoBehaviour
{
    //Unity Objects
    [SerializeField] private OfficeControl office;
    [SerializeField] private Canvas screenButton;
    [SerializeField] private TMP_Text progressDisplay;
    [SerializeField] private SoundMaster sound;

    //Can not Config
    ButtonClicker[] taskButton;
    int[] progress = new int[5];
    float timeDelay = 0f;
    float[] downAcc = new float[5];
    int currentActive = -1;
    float stack;

    //Can Config
    public float stackMax = 2f;
    public float stackMin = 0.2f;
    public float stackStep = 0.1f;

    public float fastDecayInterval = 0.2f;
    public float slowDecayInterval = 1.0f;

    void Start()
    {
        stack = stackMax;
        screenButton.gameObject.SetActive(true);
        taskButton = screenButton.GetComponentsInChildren<ButtonClicker>(true);
    }
    void Update()
    {
        for (int i = 0; i < 5; i++)
        {
            if (taskButton[i].isClick)
            {
                stack = stackMax;
                taskButton[i].isClick = false;
                currentActive = i;
                timeDelay = 0f;
            }
        }

        switch (currentActive)
        {
            case 0 or 1 or 2 or 3 or 4:
                timeDelay += Time.deltaTime;
                if (progress[currentActive] < 100 && timeDelay >= stack && office.ReturnState() == Office.Monitor)
                {
                    timeDelay = 0f;
                    progress[currentActive] += 1;
                    stack = Mathf.Clamp(stack - stackStep, stackMin, stackMax);
                    sound.PlayBeep();
                }
                else if (progress[currentActive] == 100)
                {
                    sound.PlayBeepCom();
                    currentActive = -1;
                }
                break;
        }

        // ออกจาก Monitor = ยกเลิก active
        if (office.ReturnState() != Office.Monitor)
        {
            stack = stackMax;
            currentActive = -1;
            timeDelay = 0f;
        }

        for (int i = 0; i < 5; i++)
        {
            if (i == currentActive) continue;

            float t = progress[i] / 100f;
            float decayInterval = Mathf.Lerp(fastDecayInterval, slowDecayInterval, t);

            downAcc[i] += Time.deltaTime;
            if (downAcc[i] >= decayInterval && progress[i] != 100)
            {
                downAcc[i] = 0f;
                if (progress[i] > 0) progress[i] -= 1;
            }
            else if (progress[i] == 100)
            {
                Argressive();
            }
        }
        if(Argressive() == 5&&taskButton[6].isClick)
        {
            SceneManager.LoadScene("Win");
        }

        SetText(progress);
    }

    public void SetText(int[] n)
    {
        string output = "";
        foreach (int i in n)
        {
            output += i + "%\n";
        }
        progressDisplay.text = output;
    }
    public int Argressive()
    {
        return progress.Count(x => x == 100);
    }
}

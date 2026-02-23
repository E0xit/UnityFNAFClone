using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
public enum Office
{
    Monitor, Under, LeftVent, RightVent, Switching
}

public class OfficeControl : MonoBehaviour
{
    [SerializeField] private Animator OfficeAnim;
    [SerializeField] private ButtonHover down, up, left, right;
    [SerializeField] private ButtonClicker flash;
    [SerializeField] private GameObject monitorUI;
    [SerializeField] private Canvas UI;
    [SerializeField] private SoundMaster sound;
    [SerializeField] private Office officeState, nextState;
    [SerializeField] private float monitordelay = 0f;
    [SerializeField] private AnimatorStateInfo stateInfo;
    [SerializeField] private PlayerInput inputs;
    void Awake()
    {
        inputs = new PlayerInput();
    }
    void Start()
    {
        officeState = Office.Monitor;
    }
    void Update()
    {
        stateInfo = OfficeAnim.GetCurrentAnimatorStateInfo(0);
        switch (officeState)
        {
            case Office.Monitor:
                sound.PlayWindAmbient(0.5f);
                monitordelay += Time.deltaTime;
                down.gameObject.SetActive(true);
                if (monitordelay >= 1f) monitorUI.SetActive(true);
                if (down.IsHover)
                {
                    sound.PlayWindAmbient(0.2f);
                    down.IsHover = false;
                    monitorUI.SetActive(false);
                    down.gameObject.SetActive(false);
                    playSet("DownMove");
                    nextState = Office.Under;
                    monitordelay = 0f;
                }
                break;
            case Office.Under:
                sound.PlantVentSound(true);
                sound.PlantVentSound("Mid");
                sound.PlayWindAmbient(0.2f);
                left.gameObject.SetActive(true);
                right.gameObject.SetActive(true);
                up.gameObject.SetActive(true);
                if (up.IsHover)
                {

                    sound.PlantVentSound(false);
                    up.IsHover = false;
                    up.gameObject.SetActive(false);
                    playSet("UpMove");
                    nextState = Office.Monitor;
                    left.gameObject.SetActive(false);
                    right.gameObject.SetActive(false);
                }
                if (left.IsHover)
                {
                    left.IsHover = false;
                    left.gameObject.SetActive(false);
                    down.gameObject.SetActive(false);
                    up.gameObject.SetActive(false);
                    playSet("LeftMove");
                    nextState = Office.LeftVent;

                }
                if (right.IsHover)
                {
                    right.IsHover = false;
                    right.gameObject.SetActive(false);
                    down.gameObject.SetActive(false);
                    up.gameObject.SetActive(false);
                    playSet("RightMove");
                    nextState = Office.RightVent;
                }
                break;

            case Office.LeftVent:
                sound.PlayWindAmbient(0.1f);
                sound.PlantVentSound("Left");
                if (right.IsHover)
                {
                    right.IsHover = false;
                    right.gameObject.SetActive(false);
                    down.gameObject.SetActive(false);
                    playSet("LeftBack");
                    nextState = Office.Under;
                }

                break;
            case Office.RightVent:
                sound.PlayWindAmbient(0.1f);
                sound.PlantVentSound("Right");
                up.gameObject.SetActive(false);
                if (left.IsHover)
                {
                    left.IsHover = false;
                    left.gameObject.SetActive(false);
                    down.gameObject.SetActive(false);
                    playSet("RightBack");
                    nextState = Office.Under;
                }
                break;


            case Office.Switching:
                UI.gameObject.SetActive(false);
                if (stateInfo.normalizedTime >= 1f && !OfficeAnim.IsInTransition(0))
                {
                    UI.gameObject.SetActive(true);
                    officeState = nextState;
                }
                break;
        }
        if (monitorUI.activeSelf)
        {
            OfficeAnim.GetComponent<Renderer>().enabled = false;
            sound.PlayMonitorSound(true);
            sound.PlayPCDownSound();
        }
        else
        {
            OfficeAnim.GetComponent<Renderer>().enabled = true;
            sound.PlayMonitorSound(false);
            sound.PlayPCStartSound();
        }
    }


    private void playSet(string transition)
    {
        OfficeAnim.Play(transition);
        officeState = Office.Switching;
    }
    public Office ReturnState()
    {
        return officeState;
    }
    void OnEnable()
    {
        inputs.Enable();
        inputs.PlayerInputActions.ExitButton.performed += ToMenu;
    }
    void OnDisable()
    {
        inputs.Disable();
    }
    private void ToMenu(InputAction.CallbackContext context)
    {
        SceneManager.LoadScene("MainMenu");
    }
}
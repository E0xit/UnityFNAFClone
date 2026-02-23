using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JumpSacre : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Canvas ui;
    [SerializeField] private float timer;
    [SerializeField] private float timerEnd = 3f;
    [SerializeField] private bool end = false;
    [SerializeField] private SoundMaster sound;
    [SerializeField] private bool SoundPlay = false;
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }
    void Update()
    {
        if (timer >= timerEnd)
        {
            end = true;
            SceneManager.LoadScene("GameOver");
        }
    }
    public void LeftJump()
    {
        ui.gameObject.SetActive(false);
        if (!end)
        {
            timer += Time.deltaTime;
        }
        Playjump();
        animator.Play("LeftDead");
    }
    public void RightJump()
    {
        ui.gameObject.SetActive(false);
        if (!end)
        {
            timer += Time.deltaTime;
        }
        Playjump();
        animator.Play("RightDead");
    }
    public void Playjump()
    {
        if (!SoundPlay)
        {
            sound.PlayJump();
        }
        SoundPlay = true;
    }

}

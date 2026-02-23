using UnityEngine;
using static UnityEngine.Random;

public class SoundMaster : MonoBehaviour
{
    [SerializeField] private AudioSource windAmbient;
    [SerializeField] private AudioSource monitorSound;
    [SerializeField] private AudioSource ventSound;
    [SerializeField] private AudioSource moveSound;
    [SerializeField] private AudioSource breathingSound;
    [SerializeField] private AudioSource pcDown;
    [SerializeField] private AudioSource pcStart;
    [SerializeField] private AudioSource proBeep;
    [SerializeField] private AudioSource proCom;
    [SerializeField] private AudioSource VentAlert;
    [SerializeField] private AudioSource VentGone;
    [SerializeField] private AudioSource fixing;
    [SerializeField] private AudioSource jumpSound;
    private float time = 0f;
    public void PlayWindAmbient(float volume)
    {
        windAmbient.volume = volume;
        time += Time.deltaTime;
        if (time >= 0.5)
        {
            windAmbient.panStereo = Range(-0.2f, 0.2f);
        }
    }
    public void PlayMonitorSound(bool t)
    {
        if (t) monitorSound.volume = 0.5f;
        else monitorSound.volume = 0f;
    }
    public void PlantVentSound(bool t)
    {
        if (t) ventSound.volume = 1f;
        else ventSound.volume = 0f;
    }
    public void PlantVentSound(string side)
    {
        switch (side)
        {
            case "Left":
                ventSound.panStereo = 0.3f;
                break;
            case "Right":
                ventSound.panStereo = -0.3f;
                break;
            case "Mid":
                ventSound.panStereo = 0f;
                break;
        }
    }
    public void PlayMove()
    {
        moveSound.Play();
    }
    public void PlayBreathingSound(bool t)
    {
        if (t) breathingSound.volume = 0.5f;
        else breathingSound.volume = 0f;
    }
    public void PlayPCDownSound()
    {
        pcDown.Play();
    }
    public void PlayPCStartSound()
    {
        pcStart.Play();
    }
    public void PlayBeep()
    {
        proBeep.Play();
    }
    public void PlayBeepCom()
    {
        proCom.Play();
    }
    public void PlatVentAlert(float vol, float side)
    {
        VentAlert.volume = vol;
        VentAlert.panStereo = side;
        VentAlert.Play();
    }
    public void PlayVentBack()
    {
        VentGone.Play();
    }
    public void PlayFixingSound(bool t)
    {
        if (t) fixing.volume = 0.7f;
        else fixing.volume = 0f;
    }
    public void PlayJump()
    {
        jumpSound.Play();
    }
}

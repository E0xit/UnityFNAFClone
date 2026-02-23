using System.Runtime.Serialization.Formatters;
using UnityEngine;
using UnityEngine.SceneManagement;
public class EnemyManager : MonoBehaviour
{
    [SerializeField] private Enemy LeftVent;
    [SerializeField] private Enemy RightVent;
    [SerializeField] private OfficeControl office;
    [SerializeField] private SoundMaster sound;
    [SerializeField] private JumpSacre jumpSacre;
    
    void Start()
    {
        LeftVent.StartUp();
        RightVent.StartUp();
    }
    void Update()
    {
        LeftVentEnemy();
        RightVentEnemy();
    }
    private void LeftVentEnemy()
    {
        if (LeftVent.State() == Enemy.KillState.Normal)
        {
            LeftVent.MoveChange();
        }
        else if (office.ReturnState() != Office.LeftVent)
        {
            if (LeftVent.KillTimer() < 0.1f)
            {
                LeftVentAudio();
            }
            LeftVent.NotWatch();
            if (LeftVent.ReadyToKill() && office.ReturnState() == Office.Monitor)
            {
                jumpSacre.LeftJump();
            }
        }
        else if (office.ReturnState() == Office.LeftVent)
        {
            LeftVent.GotWatch();
            if (LeftVent.BackTimer() >= LeftVent.BackOn())
            {
                LeftVent.StartUp();
                sound.PlayVentBack();
            }
        }
    }
    private void LeftVentAudio()
    {
        if (LeftVent.State() == Enemy.KillState.Ready && office.ReturnState() == Office.Monitor)
        {
            sound.PlatVentAlert(0.2f, -0.9f);
        }
        else if (LeftVent.State() == Enemy.KillState.Ready && (office.ReturnState() == Office.Under || office.ReturnState() == Office.RightVent || office.ReturnState() == Office.Switching))
        {
            sound.PlatVentAlert(0.5f, -0.9f);
        }
        else if (LeftVent.State() == Enemy.KillState.Ready && office.ReturnState() == Office.LeftVent)
        {
            sound.PlatVentAlert(0.8f, 0f);
        }
    }
    private void RightVentEnemy()
    {
        if (RightVent.State() == Enemy.KillState.Normal)
        {
            RightVent.MoveChange();
        }
        else if (office.ReturnState() != Office.RightVent)
        {
            if (RightVent.KillTimer() < 0.1f)
            {
                RightVentAudio();
            }
            RightVent.NotWatch();
            if (RightVent.ReadyToKill() && office.ReturnState() == Office.Monitor)
            {
                jumpSacre.RightJump();
            }
        }
        else if (office.ReturnState() == Office.RightVent)
        {
            RightVent.GotWatch();
            if (RightVent.BackTimer() >= RightVent.BackOn())
            {
                RightVent.StartUp();
                sound.PlayVentBack();
            }
        }
    }
    private void RightVentAudio()
    {
        if (RightVent.State() == Enemy.KillState.Ready && office.ReturnState() == Office.Monitor)
        {
            sound.PlatVentAlert(0.2f, 0.9f);
        }
        else if (RightVent.State() == Enemy.KillState.Ready && (office.ReturnState() == Office.Under || office.ReturnState() == Office.LeftVent || office.ReturnState() == Office.Switching))
        {
            sound.PlatVentAlert(0.5f, 0.9f);
        }
        else if (RightVent.State() == Enemy.KillState.Ready && office.ReturnState() == Office.RightVent)
        {
            sound.PlatVentAlert(0.8f, 0f);
        }
    }
}
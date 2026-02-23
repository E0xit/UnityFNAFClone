using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    public enum OnWhere { SuperFar, Far, Close, NearVent, Vent }
    public enum KillState { Normal, Ready, Stall }
    [SerializeField] private int AILevel = 10;
    [SerializeField] private TaskManager Agr;
    [SerializeField] private int movefail;
    [SerializeField] private int moveOp;
    [SerializeField] private float moveTimer;
    [SerializeField] private float killTimer;
    [SerializeField] private OnWhere location;
    [SerializeField] private KillState killState;
    [SerializeField] private float moveOn;
    [SerializeField] private float backTimer;
    [SerializeField] private int state;
    [SerializeField] private int rechange;
    [SerializeField] private int killOn;
    [SerializeField] private int backOn;
    [SerializeField] private bool readyToKill;
    private OnWhere[] Cango = {
        OnWhere.SuperFar ,
        OnWhere.Far,
        OnWhere.Close,
        OnWhere.NearVent,
        OnWhere.Vent};
    public void MoveChange()
    {
        if (location != OnWhere.Vent) moveTimer += Time.deltaTime;
        else killState = KillState.Ready;

        if (moveTimer >= moveOn && location != OnWhere.Vent)
        {
            moveTimer -= moveOn;
            moveOn = 20 - ((AILevel + (Agr.Argressive() * 2)) / 2) - Range(0, 5);
            MovementOps();
        }
    }

    private void MovementOps()
    {
        rechange = AILevel + Range(0, 5);
        moveOp = Range(0, 20) - Agr.Argressive();
        if (moveOp < AILevel + (movefail * 2))
        {
            state += 1;
            movefail = 0;
        }
        else if (moveOp >= AILevel && moveOp < rechange && location != OnWhere.SuperFar)
        {
            state -= 1;
            movefail += 1;
        }
        else
        {
            movefail += 1;
        }
        location = Cango[state];
    }
    public void StartUp()
    {
        killState = KillState.Normal;
        readyToKill = false;
        movefail = 0;
        moveOp = 0;
        moveTimer = 0f;
        backTimer = 0f;
        killTimer = 0f;
        if (Range(1, 5) > 2) state = 0;
        else state = 1;
        location = Cango[state];
        moveOn = 20 - ((AILevel + (Agr.Argressive() * 2)) / 2) - Range(0, 5);
        killOn = Range(7, 12) - (Agr.Argressive() * 2);
        backOn = Range(5, 8) + Agr.Argressive();
    }
    public float BackTimer()
    {
        return backTimer;
    }
    public int BackOn()
    {
        return backOn;
    }
    public void GotWatch()
    {
        killState = KillState.Stall;
        backTimer += Time.deltaTime;
    }
    public void NotWatch()
    {
        backTimer = 0f;
        killState = KillState.Ready;
        killTimer += Time.deltaTime;
        if (killTimer >= killOn)
        {
            readyToKill = true;
        }
    }
    public bool ReadyToKill()
    {
        return readyToKill;
    }
    public KillState State()
    {
        return killState;
    }
    public float KillTimer()
    {
        return killTimer;
    }
}
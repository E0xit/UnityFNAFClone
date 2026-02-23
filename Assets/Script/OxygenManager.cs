using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OxygenManager : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private ButtonClicker fixButton;
    [SerializeField] private Animator load;
    [SerializeField] private GameObject airbar;
    [SerializeField] private OfficeControl office;

    [Header("Oxygen")]
    [SerializeField] private int oxygen = 100;
    [SerializeField] private SoundMaster sound;
    [SerializeField] private Image blackout;
    [SerializeField] private TaskManager arg;

    [Header("Decay")]
    [SerializeField] private float decayInterval = 1f;
    [SerializeField] private int decayMin = 0;     // inclusive
    [SerializeField] private int decayMax = 3;     // exclusive (Unity Random.Range int)

    [Header("Fix")]
    [SerializeField] private int fixMinSeconds = 8;   // inclusive
    [SerializeField] private int fixMaxSeconds = 13;  // exclusive

    private Image[] bars;
    private Color trans;
    private enum State { Normal, Fixing }
    private State state = State.Normal;

    private float decayTimer;
    private float fixTimer;
    private float fixTargetTime;   // สุ่มครั้งเดียวตอนเริ่มซ่อม

    private int lastOxygen = -1;
    private bool lastFixPressed;

    void Start()
    {
        load.gameObject.SetActive(false);
        bars = airbar.GetComponentsInChildren<Image>(true);
        trans = blackout.color;
        UpdateBars(force: true);
    }

    void Update()
    {
        bool fixPressed = fixButton.isClick;

        // Detect press edge (เริ่มกดครั้งแรก)
        if (fixPressed && !lastFixPressed)
            BeginFix();

        // Detect release edge (ปล่อยปุ่ม)
        if (!fixPressed && lastFixPressed || office.ReturnState() != Office.Monitor)
            EndFix();

        lastFixPressed = fixPressed;

        // State tick
        if (state == State.Normal)
        {
            TickDecay();
        }
        else // Fixing
        {
            TickFix();
        }

        // Update UI only when oxygen changed
        if (oxygen != lastOxygen)
        {
            UpdateBars(force: false);
            lastOxygen = oxygen;
            sound.PlayBreathingSound(false);
            trans.a = 0f;
            blackout.color = trans;
        }

        if (oxygen < 40)
        {
            sound.PlayBreathingSound(true);
            trans.a = (100 - oxygen) * 0.01f;
            blackout.color = trans;
        }

        if (oxygen == 0)
        {
            SceneManager.LoadScene("GameOver");
        }
    }

    private void TickDecay()
    {
        decayTimer += Time.deltaTime;
        if (decayTimer >= decayInterval)
        {
            decayTimer -= decayInterval;

            int dec = Random.Range(decayMin + (int)(arg.Argressive() * 0.75f), decayMax + (int)(arg.Argressive() * 0.75f));
            oxygen = Mathf.Clamp(oxygen - dec, 0, 100);
        }
    }

    private void BeginFix()
    {
        state = State.Fixing;
        fixTimer = 0f;
        fixTargetTime = Random.Range(fixMinSeconds, fixMaxSeconds); // สุ่มครั้งเดียว!
        sound.PlayFixingSound(true);
        load.gameObject.SetActive(true);
    }

    private void TickFix()
    {
        fixTimer += Time.deltaTime;

        if (fixTimer >= fixTargetTime)
        {
            oxygen = 100;
            decayTimer = 0f; // reset decay ให้แฟร์
            EndFix();
        }
    }

    private void EndFix()
    {
        state = State.Normal;
        fixTimer = 0f;
        sound.PlayFixingSound(false);
        load.gameObject.SetActive(false);
    }

    private void UpdateBars(bool force)
    {
        // จำนวนแท่งที่ควรเปิด (0..bars.Length)
        // ตัวอย่าง bars 10 แท่ง:
        // oxygen 100 => 10 แท่ง
        // oxygen 90  => 9 แท่ง
        int countOn = Mathf.CeilToInt((oxygen / 100f) * bars.Length);
        countOn = Mathf.Clamp(countOn, 0, bars.Length);

        for (int i = 0; i < bars.Length; i++)
        {
            bool shouldBeOn = i < countOn;
            if (force || bars[i].gameObject.activeSelf != shouldBeOn)
                bars[i].gameObject.SetActive(shouldBeOn);
        }
    }
}

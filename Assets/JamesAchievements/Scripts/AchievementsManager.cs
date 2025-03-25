using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class AchievementsManager : MonoBehaviourSingleton<AchievementsManager>
{
    #region Properties and fields

    [Header("Achievement Data")]
    [SerializeField] private List<AchievementData> achievements;

    [Header("UI Prefabs/Container")]
    [SerializeField] private GameObject achivementCellPrefab;
    [SerializeField] private GameObject achivementsContainer;

    /*
     * ===============================
     *        AUDIO & COLORS
     * ===============================
     */
    [Header("Audio & Colors")]
    [SerializeField] private AudioSource unlockAchievementAudio;
    [SerializeField] public AchivementUIProvider uiConfigProvider;

    /*
     * ===============================
     *         ANIMATORS
     * ===============================
     */
    [Header("Animators")]
    [Min(1)]
    [SerializeField] private float animatorSpeed = 1f;
    [SerializeField] private Animator achievementAnimator;
    [SerializeField] private Animator achievementsAnimator;

    /*
     * ===============================
     *        UI TEXT / IMAGE
     * ===============================
     */
    [Header("Achievement Display")]
    [SerializeField] private TextMeshProUGUI achievementText;
    [SerializeField] private Image achievementBackground;

    /*
     * ===============================
     *         INPUT CONTROL
     * ===============================
     */
    [Header("Input")]
    [SerializeField] private InputActionReference achievementsAction;
    [SerializeField]
    [Tooltip("Backward compatibility")]
    private KeyCode achievementsMenuKey = KeyCode.M;

    /*
     * ===============================
     *         INTERNALS
     * ===============================
     */
    private Dictionary<AchievementData, AchievementCellUI> cells
        = new Dictionary<AchievementData, AchievementCellUI>();
    private bool usingAchievementsMenu;

    // QUEUE: Achievements waiting to be shown
    private Queue<AchievementData> achievementsQueue = new Queue<AchievementData>();
    private bool isShowingAchievement; // true while an achievement is being displayed

    #endregion

    #region Unity LifeCycle

    private void Start()
    {
        achievementsAnimator.SetFloat("speed", animatorSpeed);

        cells = new Dictionary<AchievementData, AchievementCellUI>();
        for (int i = 0; i < achievements.Count; ++i)
        {
            GameObject g = Instantiate(achivementCellPrefab, achivementsContainer.transform);
            AchievementCellUI a = g.GetComponent<AchievementCellUI>();
            if (a == null)
            {
                Debug.LogWarning("The achievement cell prefab should have an AchievementCellUI component.");
                return;
            }
            achievements[i].Load();
            a.Init(achievements[i], this);
            cells.Add(achievements[i], a);
        }
    }

    private void Update()
    {
        // Toggle achievements menu on key release
        if (!achievementsAction && Input.GetKeyUp(achievementsMenuKey))
        {
            ToggleMenu();
        }
    }

    private void OnEnable()
    {
        achievementsAction.action.performed += ToggleMenu;
    }

    private void OnDisable()
    {
        achievementsAction.action.performed -= ToggleMenu;

    }

    private void ToggleMenu(InputAction.CallbackContext context = default)
    {
        if (usingAchievementsMenu)
            HideAchievements();
        else
            ShowAchievements();
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Public method to unlock an achievement. 
    /// If another achievement is already showing, this will be queued.
    /// </summary>
    public void UnlockAchievement(AchievementData ach)
    {
        // If it's already unlocked, just return
        if (ach.isUnlocked) return;

        // Queue this achievement
        achievementsQueue.Enqueue(ach);

        // If not already showing an achievement, process the queue
        if (!isShowingAchievement)
        {
            StartCoroutine(ProcessAchievementQueue());
        }
    }

    // Clears all achievements obtained
    public void ResetAchievements()
    {
        foreach (var a in achievements)
        {
            a.ResetAchievement();
        }
    }

    #endregion

    #region Show/Hide UI

    public void ShowAchievements()
    {
        Time.timeScale = 0;
        achievementsAnimator.SetTrigger("open");

        // Attempt to select the first achievement cell if available
        if (achievements.Count > 0 && cells.TryGetValue(achievements[0], out AchievementCellUI a))
        {
            a.Select();
        }

        StartCoroutine(ExecuteAfterDelay(0, () =>
        {
            usingAchievementsMenu = true;
        }));
    }

    public void HideAchievements()
    {
        Time.timeScale = 1;
        achievementsAnimator.SetTrigger("close");

        StartCoroutine(ExecuteAfterDelay(0, () =>
        {
            usingAchievementsMenu = false;
        }));
    }

    private IEnumerator ExecuteAfterDelay(float delay, System.Action action)
    {
        yield return new WaitForSecondsRealtime(delay);
        action?.Invoke();
    }

    #endregion

    #region Achievement Queue Logic

    /// <summary>
    /// Process the queue of achievements, showing them one by one.
    /// </summary>
    private IEnumerator ProcessAchievementQueue()
    {
        isShowingAchievement = true;

        while (achievementsQueue.Count > 0)
        {
            AchievementData nextAchievement = achievementsQueue.Dequeue();
            yield return StartCoroutine(ShowAchievement(nextAchievement));
        }

        isShowingAchievement = false;
    }

    /// <summary>
    /// Show a single achievement (animation, text, etc.) and wait until it's done.
    /// </summary>
    private IEnumerator ShowAchievement(AchievementData ach)
    {
        // 1) Unlock in data (so it doesn't get unlocked again in future sessions).
        ach.Unlock();

        // 2) Play unlock audio if available
        if (unlockAchievementAudio != null)
        {
            unlockAchievementAudio.Play();
        }

        // 3) Update UI visuals for the popup
        var rarityColors = uiConfigProvider.GetColorByRarity(ach.achivementRarity);
        if (rarityColors != null)
        {
            achievementBackground.color = rarityColors.unlockedBackgroundColor;
            achievementText.color = rarityColors.unlockedTextColor;
            achievementText.text =
                $"<sprite index=0 color={rarityColors.rarityTextColorsHex}>{ach.achievementTitle}" +
                $"<sprite index=0 color={rarityColors.rarityTextColorsHex}>";
        }

        // 4) Trigger the unlock animation if assigned
        if (achievementAnimator != null)
        {
            achievementAnimator.SetTrigger("unlockAchievement");
        }

        // 5) Update UI if the corresponding cell is found
        if (cells.TryGetValue(ach, out var cell))
        {
            cell.Unlock();
        }

        // 6) Wait for the animation to finish.
        //    - Here we just wait a set time or you could get the clip length:
        //      e.g. yield return new WaitForSeconds(animationClip.length);
        //    - Or wait until an Animation Event calls back.
        //    - For demo, let's wait 3 seconds:
        yield return new WaitForSeconds(3f);
    }

    #endregion

}


#if UNITY_EDITOR
[CustomEditor(typeof(AchievementsManager)), CanEditMultipleObjects]
public class AchievementsManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        AchievementsManager achievementsManager = (AchievementsManager)target;

        //ZONA MODIFICABLE
        if (GUILayout.Button("Reset Achievements"))
        {
            achievementsManager.ResetAchievements();
        }
        //FIN ZONA MODIFICIABLE
    }
}
#endif

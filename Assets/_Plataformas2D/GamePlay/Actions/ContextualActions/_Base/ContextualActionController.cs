using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using TMPro;

public class ContextualActionController : MonoBehaviour
{
    #region Fields and Properties

    [Header("General Settings")]
    [SerializeField] private InputActionReference contextualAction;
    [SerializeField, Tooltip("If enabled, the contextualAction will be enabled and disabled")] 
    private bool toogleActionEnableState = false;

    [Header("Tag Settings")]
    [Tooltip("One or more tags that identify valid actors who can trigger this.")]
    [SerializeField] private string[] actorTags = { "Player" };
    [SerializeField] private bool tagInArea; // Internal usage to track whether a valid actor is in the zone.

    [Header("UI Text")]
    [SerializeField] private string uiEnableText = "Activar";
    [SerializeField] private Color uiEnableColor = Color.white;
    [SerializeField] private string uiDisableText = "Necesitas X";
    [SerializeField] private Color uiDisableColor = Color.gray;

    [Header("Contextual Canvas")]
    [SerializeField] private bool enableContextualCanvas = true;
    [SerializeField] private Canvas contextualCanvas;
    [SerializeField] private TextMeshProUGUI textField;
    [SerializeField] private PressKeyFromAction pressKey;

    [Header("Events")]
    public UnityEvent PlayerEnter;     // Invoked when a valid actor enters.
    public UnityEvent PlayerExit;      // Invoked when a valid actor exits.
    public UnityEvent ActionTriggered; // Invoked when the action is performed.

    /// <summary>
    /// This bool is used to manually disable/enable the controller's functionality
    /// without disabling the entire component or GameObject.
    /// </summary>
    private bool manualDisabled = false;

    /// <summary>
    /// Determines if a valid actor is within the interaction area.
    /// When set to true or false, it updates the canvas, the input action, and invokes events.
    /// </summary>
    public bool TagInArea
    {
        get => tagInArea;
        private set
        {
            // Only proceed if there's an actual change.
            if (tagInArea == value) return;

            tagInArea = value;
            bool canAct = CheckRequirement();

            // Enable or disable the input action based on conditions.
            if (!manualDisabled && tagInArea && canAct)
            {
                if (toogleActionEnableState) contextualAction?.action.Enable();
            }
            else
            {
                if (toogleActionEnableState) contextualAction?.action.Disable();
            }

            // Update UI
            UpdateCanvas(canAct);

            // Invoke events
            if (tagInArea) PlayerEnter?.Invoke();
            else PlayerExit?.Invoke();
        }
    }

    #endregion

    #region Unity Lifecycle

    private void Start()
    {
        // Try to find references in children if they're not assigned in the Inspector
        if (!contextualCanvas)
            contextualCanvas = GetComponentInChildren<Canvas>();
        if (!textField)
            textField = contextualCanvas ? contextualCanvas.GetComponentInChildren<TextMeshProUGUI>() : null;
        if (!pressKey)
            pressKey = contextualCanvas ? contextualCanvas.GetComponentInChildren<PressKeyFromAction>() : null;

        // Assign the input action to PressKeyFromAction, so the key icon or text is shown.
        if (pressKey)
            pressKey.InputAction = contextualAction;

        // Disable the action at start and hide the UI.
        if (toogleActionEnableState) contextualAction?.action.Disable();
        if (contextualCanvas)
            contextualCanvas.enabled = false;

        // Initialize the 'playerInArea' state to false.
        TagInArea = false;
    }

    private void OnEnable()
    {
        // Subscribe to the performed event (if reference is assigned).
        if (contextualAction)
            contextualAction.action.performed += ExecuteAction;
    }

    private void OnDisable()
    {
        // Unsubscribe to avoid memory leaks/errors.
        if (contextualAction)
            contextualAction.action.performed -= ExecuteAction;
    }

    #endregion

    #region Manual Enable/Disable

    /// <summary>
    /// This method re-enables actor detection and the UI if applicable.
    /// </summary>
    public void Enable()
    {
        manualDisabled = false;

        // If we're already in the trigger zone and meet requirements, enable input again.
        if (TagInArea && CheckRequirement())
            if (toogleActionEnableState) contextualAction?.action.Enable();

        // Update the canvas to reflect the correct UI state (if TagInArea is true).
        UpdateCanvas(CheckRequirement());
    }

    /// <summary>
    /// This method disables actor detection and hides the UI, without disabling the entire component.
    /// </summary>
    public void Disable()
    {
        manualDisabled = true;

        // Hide the UI (canvas off) and disable the input action.
        UpdateCanvas(CheckRequirement());
        if (toogleActionEnableState) contextualAction?.action.Disable();

        // Optionally, reset TagInArea to false so we don't show UI if re-enabled 
        // while the player is physically still in the trigger.
        // TagInArea = false;
        contextualCanvas.enabled = false;
    }

    #endregion

    #region Trigger Handling

    // This single method handles both Enter and Exit states based on the bool isEnter.
    // If isEnter == true, we set PlayerInArea = true; otherwise we set it false.
    private void CheckTrigger(string tag, bool isEnter)
    {
        // Ignore triggers when manually disabled
        if (manualDisabled) return;

        // Only proceed if the object has a valid tag from actorTags.
        if (!IsValidActorTag(tag)) return;

        TagInArea = isEnter;
    }

    // Determine if the provided tag is in the actorTags array.
    private bool IsValidActorTag(string tag)
    {
        foreach (var validTag in actorTags)
        {
            if (tag.Equals(validTag))
                return true;
        }
        return false;
    }

    // 3D triggers
    private void OnTriggerEnter(Collider other)
    {
        CheckTrigger(other.tag, true);
    }

    private void OnTriggerExit(Collider other)
    {
        CheckTrigger(other.tag, false);
    }

    // 2D triggers
    private void OnTriggerEnter2D(Collider2D collision)
    {
        CheckTrigger(collision.tag, true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        CheckTrigger(collision.tag, false);
    }

    #endregion

    #region Action Execution

    /// <summary>
    /// Called when the assigned input action is performed (e.g., the player presses the interaction key).
    /// </summary>
    private void ExecuteAction(InputAction.CallbackContext context)
    {
        if (!manualDisabled && TagInArea && CheckRequirement())
        {
            ActionTriggered?.Invoke();
        }
    }

    /// <summary>
    /// Override this if you have specific requirements (e.g., the player must have a certain item).
    /// </summary>
    protected virtual bool CheckRequirement()
    {
        // Return true if there are no requirements, or do your logic here.
        return true;
    }

    #endregion

    #region UI Updates

    /// <summary>
    /// Enables/disables the UI canvas and updates the displayed text/color depending on requirements.
    /// </summary>
    private void UpdateCanvas(bool canAct)
    {
        if (!enableContextualCanvas || !contextualCanvas) return;

        // If the controller is disabled, always hide the canvas:
        if (manualDisabled)
        {
            contextualCanvas.enabled = false;
            if (textField) textField.text = string.Empty;
            return;
        }

        // Otherwise follow normal logic:
        if (tagInArea)
        {
            contextualCanvas.enabled = true;
            textField.color = canAct ? uiEnableColor : uiDisableColor;
            textField.text = canAct ? uiEnableText : uiDisableText;
        }
        else
        {
            contextualCanvas.enabled = false;
            if (textField) textField.text = string.Empty;
        }
    }
    #endregion
}

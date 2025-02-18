using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using TMPro;

/// <summary>
/// This component allows for a contextual action to be triggered when any of the specified actors (tags) 
/// enters or exits a trigger zone. It displays a UI prompt and invokes events when entering/exiting the zone,
/// and when the contextual action is performed.
/// </summary>
public class ContextualActionController : MonoBehaviour
{
    #region Fields and Properties

    [Header("General Settings")]
    [SerializeField] private InputActionReference contextualAction;

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
            if (tagInArea && canAct)
            {
                contextualAction?.action.Enable();
            }
            else
            {
                contextualAction?.action.Disable();
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
        contextualAction?.action.Disable();
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

    #region Trigger Handling

    // This single method handles both Enter and Exit states based on the bool isEnter.
    // If isEnter == true, we set PlayerInArea = true; otherwise we set it false.
    private void CheckTrigger(string tag, bool isEnter)
    {
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
        CheckTrigger(other.tag,true);
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
        if (TagInArea && CheckRequirement())
        {
            // Once action is triggered successfully, optionally exit the area to prevent repeated triggers.
            TagInArea = false;

            // Invoke the custom event.
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

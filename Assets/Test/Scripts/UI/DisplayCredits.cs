using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections;
using System.Text;
using System.Collections.Generic;
using TMPro;

public class DisplayCredits : MonoBehaviour
{
    [SerializeField]
    private UnityEvent onCreditsInteraction;

    [SerializeField]
    public string triggerer = "Player";

    public Vector3 direction;

    private bool triggered;

    private float Totalspeed = 1;

    public void Initialize(string text, float speed)
    {
        GetComponentInChildren<TextMeshProUGUI>().text = text;
        Totalspeed = speed;
        triggered = false;
    }


    private void Start()
    {
        // Retrieve the CreditsGameManager credit list
        //List<Contributors> contributors = CreditsGameManager.Instance?.GetCreditsList();
        
    }

    private void Update()
    {
        //Mover el texto según transformación
        this.transform.position += Totalspeed * direction.normalized * Time.deltaTime;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(triggerer) && !triggered)
        {
            triggered = true;
            // Trigger the UnityEvent for credits interaction
            onCreditsInteraction.Invoke();
        }
    }


    
}

using UnityEngine;

public class OnEnterWin : MonoBehaviour
{
    [SerializeField] string winnerTag = "Player";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(winnerTag)) GameManager.Instance.Win();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(winnerTag)) GameManager.Instance.Win(); 
    }
}

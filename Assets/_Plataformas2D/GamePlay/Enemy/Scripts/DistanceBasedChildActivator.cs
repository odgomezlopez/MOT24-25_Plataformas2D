using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DistanceBasedChildActivator : MonoBehaviour
{
    [SerializeField] private float m_Distance = 10f;

    [SerializeField] private string targetTag = "Player";
    private Transform target;

    //Lista de hijos
    //List<Transform> childs;         /*childs = new List<Transform>();*/
    [SerializeField]List<Transform> children;

    [SerializeField,Range(1,30)] private int frameRate=15;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        target = GameObject.Find(targetTag).transform;
        if (target == null) Debug.LogError($"Tag {targetTag} not found");

        if(children==null)children = new();
        for (int i = 0; i < transform.childCount; i++)
        {
            children.Add(transform.GetChild(i));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.frameCount % frameRate == 0)
        {
            for(int i = 0; i < children.Count; i++)
            {
                //Compruebo si alg�n hijo ha sido destruido
                if (children[i] == null)
                {
                    children.RemoveAt(i);
                    i--;
                    continue;
                }

                //Compruebo si los hijos est�n a menos de mDistance, si lo est�n los activo y sino los desactivo.
                if (Vector2.Distance(target.position, children[i].position) < m_Distance)
                {
                    children[i].gameObject.SetActive(true);
                }
                else
                {
                    children[i].gameObject.SetActive(false);
                }
            }
        }
    }
}

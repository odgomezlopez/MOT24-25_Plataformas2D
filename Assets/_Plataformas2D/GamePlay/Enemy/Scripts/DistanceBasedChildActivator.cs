using UnityEngine;

public class DistanceBasedChildActivator : MonoBehaviour
{
    [SerializeField] private float m_Distance = 10f;

    [SerializeField] private string targetTag = "Player";
    private Transform target;

    //Lista de hijos
    //List<Transform> childs;         /*childs = new List<Transform>();*/
    Transform[] children;

    [SerializeField,Range(1,30)] private int frameRate=15;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        target = GameObject.Find(targetTag).transform;
        if (target == null) Debug.LogError($"Tag {targetTag} not found");

        children = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            children[i]=transform.GetChild(i);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.frameCount % frameRate == 0)
        {
            for(int i = 0; i < children.Length; i++)
            {
                //Compruebo si los hijos están a menos de mDistance, si lo están los activo y sino los desactivo.
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

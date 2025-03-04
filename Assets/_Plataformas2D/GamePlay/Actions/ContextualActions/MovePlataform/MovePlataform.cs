using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class MovePlataform : MonoBehaviour
{
    [Header("Plataform Movement Parameters")]
    [SerializeField] float speed=5f;

    int nextPosition;
    [SerializeField] List<Vector3> positions;

    //Eventos
    public UnityEvent OnActionEnded;

    //AutoInit
    private bool patrolOnStart = false;

    private void Start()
    {
        //Rellenarlo con la ruta
        Transform patrolGO = gameObject.transform.Find("Patrol");
        for (int i = 0; i < patrolGO.childCount; i++)
        {
            positions.Add(patrolGO.GetChild(i).position);
        }

        positions.Add(transform.position);

        //Siguiente posicion
        nextPosition = 0;

        if (patrolOnStart) MovePatrol();
    }

    //Movimiento a la siguiente posición
    public void Move()
    {
        StartCoroutine(MoveToNextPosition());
    }

    private IEnumerator MoveToNextPosition()
    {
        while (transform.position != positions[nextPosition]) { 
            transform.position = Vector3.MoveTowards(transform.position, positions[nextPosition], speed*Time.fixedDeltaTime);
            yield return new WaitForFixedUpdate();
        }
        nextPosition = (nextPosition+1)%positions.Count;
        OnActionEnded.Invoke();
    }

    //Hacer toda la patrulla en bucle
    public void MovePatrol()
    {
        StartCoroutine(MovePatrolCoroutine());
    }

    private IEnumerator MovePatrolCoroutine()
    {
        while (true)
        {
            yield return MoveToNextPosition();
        }
    }
}
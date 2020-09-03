using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Star : MonoBehaviour, IPointerClickHandler
{
    private Coroutine moveCourutine;
    private Coroutine changeDirection;
    [SerializeField] private int nextWayPointIndex = 1;
    [SerializeField] private Transform wayPointsContainer;
    private Transform[] wayPoints;
    [SerializeField] private int increment = 1;
    [SerializeField] private float speed = 0.1f;
    [SerializeField] private GameObject starVisual;
    private bool clickEnable;
    [SerializeField] private AudioSource hit;
        
    public void OnPointerClick(PointerEventData eventData)
    {
        if (!clickEnable)
            return;
        StartCoroutine(WaitRespawn());
        hit.Play();
        GameController.Instance.AddScore(1);
    }

    //движение, включение и выключение
    // Start is called before the first frame update
    void Start()
    {
        wayPoints = new Transform[wayPointsContainer.childCount];
        for (int i = 0; i < wayPointsContainer.childCount; i++)
            wayPoints[i] = wayPointsContainer.GetChild(i);
        nextWayPointIndex = Random.Range(0, wayPoints.Length);
        transform.position = wayPoints[nextWayPointIndex].position;
        IncrementIndex();
        moveCourutine = StartCoroutine(Move());
        changeDirection = StartCoroutine(ChangeDirection());
    }
    
    IEnumerator Move()
    {
        Vector3 direction = wayPoints[nextWayPointIndex].position - transform.position;
        direction = direction.normalized;
        transform.position += direction * speed * Time.deltaTime;
        yield return new WaitForFixedUpdate();
        if (Vector3.Distance(transform.position, wayPoints[nextWayPointIndex].position) < 0.01f)
            IncrementIndex();
        moveCourutine = StartCoroutine(Move());

    }
    IEnumerator ChangeDirection()
    {
        var waitNextChangeDirectionWait = Random.Range(2f, 5f);
        var сooldown = new WaitForSeconds(waitNextChangeDirectionWait);
        yield return сooldown;
        increment *= -1;
        speed = Random.Range(0.05f, 0.2f);
        IncrementIndex();
        changeDirection = StartCoroutine(ChangeDirection());
    }

    private void IncrementIndex()
    {
        nextWayPointIndex += increment;
        if (nextWayPointIndex < 0)
            nextWayPointIndex = wayPoints.Length - 1;
        if (nextWayPointIndex >= wayPoints.Length)
            nextWayPointIndex = 0;
    }

    IEnumerator WaitRespawn()
    {
        starVisual.SetActive(false);
        StopCoroutine(moveCourutine);
        StopCoroutine(changeDirection);
        yield return new WaitForSeconds(2f);
        nextWayPointIndex = Random.Range(0, wayPoints.Length);
        transform.position = wayPoints[nextWayPointIndex].position;
        IncrementIndex();
        starVisual.SetActive(true);
        moveCourutine = StartCoroutine(Move());
        changeDirection = StartCoroutine(ChangeDirection());
    }
    public void SetClickable(bool enable)
    {
        clickEnable = enable;
    }
}

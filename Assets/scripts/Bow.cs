using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : MonoBehaviour
{
    [Header("Assets")]
    public GameObject arrowPrefab;

    [Header("Bow")]
    public float treshHold = 0.15f;
    public Transform startPoint;
    public Transform endPoint;
    public Transform arrowSocket;

    private Transform pullingHand;
    private Arrow currentArrow;
    private Animator animator;

    private float pullValue;

    private float timeBetweenArrows = 0.25f;

    public List<GameObject> startingParticles;

    void Awake()
    {
        animator = this.GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        StartCoroutine(CreateArrow(0.0f));
    }

    private void OnEnable()
    {
        if (!currentArrow)
            StartCoroutine(CreateArrow(0.0f));
    }

    private IEnumerator CreateArrow(float waitTime)
    {
        //wait
        yield return new WaitForSeconds(waitTime);

        //create arrow, child
        GameObject arrow = Instantiate(arrowPrefab, arrowSocket);

        //orient-it 
        arrow.transform.localPosition = new Vector3(0, 0, 0.425f);
        arrow.transform.localEulerAngles = Vector3.zero;

        //set as current arrow
        currentArrow = arrow.GetComponent<Arrow>();

    }

    private void Update()
    {
        if (!pullingHand || !currentArrow)
        {
            return;
        }

        pullValue = calculatePull(pullingHand);
        pullValue = Mathf.Clamp(pullValue, 0, 1);

        animator.SetFloat("Blend", pullValue);
    }


    private float calculatePull(Transform pullHand)
    {

        Vector3 direction = endPoint.position - startPoint.position;
        float magnitude = direction.magnitude;
        direction.Normalize();

        Vector3 difference = pullHand.position - startPoint.position;

        return Vector3.Dot(difference, direction) / magnitude;
    }

    private void FireArrow(float pullPercentage)
    {
        //TODO chiama la funzione "fire" sulla freccia

        currentArrow.Fire(pullPercentage);

        currentArrow = null;
    }

    public bool Pull(Transform hand)
    {

        //verifico se la mano è vicino o no al punto di partenza dell'arco (metodo semplificato per non usare un collider ad un collider)
        float distance = Vector3.Distance(startPoint.position, hand.position);

        if (distance > treshHold)
        {
            return false;
        }

        pullingHand = hand;

        foreach (var particle in startingParticles)
        {
            var arrowTip = currentArrow.tip;

            //ruoto come la freccia e non come la punta
            Instantiate(particle, arrowTip.transform.position, currentArrow.transform.rotation, arrowTip);
        }
        return true;
    }


    public void Release()
    {

        if (pullValue > 0.25)
        {
            var pullPercentage = Mathf.Clamp(calculatePull(pullingHand), 0, 100);
            FireArrow(pullPercentage);
        }

        pullingHand = null;

        animator.SetFloat("Blend", 0f);

        if (!currentArrow)
            StartCoroutine(CreateArrow(timeBetweenArrows));
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        EventManager.OnLapFinished += OnLapFinished;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnLapFinished(float lapTime)
    {
        animator.SetTrigger("LapFinished");
    }
}

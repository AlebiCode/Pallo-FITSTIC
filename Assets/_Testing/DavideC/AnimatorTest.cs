using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorTest : MonoBehaviour
{

    public Animator myAniamtor;
    public float fadeTime = .25f;
    public Vector2 moveSpeed;
    // Start is called before the first frame update
    void Start()
    {
        myAniamtor.Play("Idle");
        myAniamtor.CrossFade("Idle", fadeTime);


        myAniamtor.SetFloat("moveX", moveSpeed.x);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

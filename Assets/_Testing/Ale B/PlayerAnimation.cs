using StateMachine;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEditor.Animations;
using UnityEngine;

[System.Serializable]
public class PlayerAnimation
{
    //hash values
    public static readonly int hit = Animator.StringToHash("hit");
    public static readonly int death = Animator.StringToHash("death");
    public static readonly int idle = Animator.StringToHash("idle");
    public static readonly int idleBall = Animator.StringToHash("idle_ball");

    public static readonly int dashBackIntro = Animator.StringToHash("dash_b_intro");
    public static readonly int dashBackOutro = Animator.StringToHash("dash_b_outro");
    public static readonly int dashForwardIntro = Animator.StringToHash("dash_f_intro");
    public static readonly int dashForwardOutro = Animator.StringToHash("dash_f_intro");
    public static readonly int dashLeftIntro = Animator.StringToHash("dash_l_intro");
    public static readonly int dashLeftOutro = Animator.StringToHash("dash_l_intro");
    public static readonly int dashRightIntro = Animator.StringToHash("dash_r_intro");
    public static readonly int dashRightOutro = Animator.StringToHash("dash_r_intro");

    public static readonly int takeIntro = Animator.StringToHash("take_intro");
    public static readonly int takeOutroBall = Animator.StringToHash("take_outro_ball");
    public static readonly int takeOutroNoBall = Animator.StringToHash("take_outro_noball");

    public static readonly int throwLeftIntro = Animator.StringToHash("throw_r_intro");
    public static readonly int throwLeftCharge = Animator.StringToHash("throw_r_charge");
    public static readonly int throwLeftOutro = Animator.StringToHash("throw_r_outro");
    public static readonly int throwLeftIntroNoLeg = Animator.StringToHash("throw_r_intro_noleg");
    public static readonly int throwLeftChargeNoLeg = Animator.StringToHash("throw_r_charge_noleg");
    public static readonly int throwLeftOutroNoLeg = Animator.StringToHash("throw_r_outro_noleg");

    public static readonly int throwRightIntro = Animator.StringToHash("throw_l_intro");
    public static readonly int throwRightCharge = Animator.StringToHash("throw_l_charge");
    public static readonly int throwRightOutro = Animator.StringToHash("throw_l_outro");
    public static readonly int throwRightIntroNoLeg = Animator.StringToHash("throw_l_intro_noleg");
    public static readonly int throwRightChargeNoLeg = Animator.StringToHash("throw_l_charge_noleg");
    public static readonly int throwRightOutroNoLeg = Animator.StringToHash("throw_l_outro_noleg");

    /*
    public static readonly int runBack = Animator.StringToHash("run_b");
    public static readonly int runBackBall = Animator.StringToHash("run_b_ball");
    public static readonly int runForward = Animator.StringToHash("run_f");
    public static readonly int runForwardBall = Animator.StringToHash("run_f_ball");
    public static readonly int runLeft = Animator.StringToHash("run_l");
    public static readonly int runLeftBall = Animator.StringToHash("run_l_ball");
    public static readonly int runRight = Animator.StringToHash("run_r");
    public static readonly int runRightBall = Animator.StringToHash("run_r_ball");
    */

    //[SerializeField] private Player player;
    [SerializeField] private Animator animator;

    //private Coroutine goToIdleCo;

    public void PlayAnimation(int hashValue)
    {
        animator.Play(hashValue);
        /*if (idleStateOnAnimEnd)
        {
            if(goToIdleCo != null)
                player.StopCoroutine(goToIdleCo);
            player.StartCoroutine(GoToIdleOnAnimEnd(animator.runtimeAnimatorController.clip));
        }*/
    }

    /*private IEnumerator GoToIdleOnAnimEnd(float time)
    {
        yield return new WaitForSeconds(time);
        animator.Play(player.HeldPallo ? idleBall : idle);
    }*/

    public void SetTopLayerWeight(float weight)
    {
        SetLayerWeight(1, weight);
    }
    public void SetLayerWeight(int layer, float weight)
    {
        animator.SetLayerWeight(layer, weight);
    }

    public void LegMovementParameters(float x, float z)
    {
        animator.SetFloat("Horizontal", x);
        animator.SetFloat("Vertical", z);
    }

}

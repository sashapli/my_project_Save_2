using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playerbike : MonoBehaviour
{
    public GameObject[] acutSystem;
    public Transform Palk;
    public Transform Palk2;
    public Vector2 Acut, Deffolt;
    private WheelJoint2D wheelJoint;
    public HingeJoint2D hingeJoint2;
    public Sprite wheelSport;
    public SpriteRenderer folt, back;
    public CycleControl cycleControl;

    public bool tStatus;
    public bool wStatus;
    public int frotSyst;
    public int bakeSyst;

    public SpriteRenderer Cep;
    public Sprite[] chep;
    public Transform wheelfrot, wheelBake;
    public float[] SizeFrot;
    public float[] SizeBake;
    public void Start()
    {
        tStatus = CoinManager.Instance.GetISArm();
        wStatus = CoinManager.Instance.GetISWheel();
        frotSyst = CoinManager.Instance.GetForwardgears();
        bakeSyst = CoinManager.Instance.Getbakegears();

        Cep.sprite = chep[frotSyst];
        wheelfrot.localScale = new Vector2(SizeFrot[frotSyst], SizeFrot[frotSyst]);
        wheelBake.localScale = new Vector2(SizeBake[bakeSyst], SizeBake[bakeSyst]);
        WheelJoint2D[] joints = Palk2.GetComponents<WheelJoint2D>();
        wheelJoint = joints[1];
        foreach (var i in acutSystem)
            i.SetActive(tStatus);
        hingeJoint2.useLimits = !hingeJoint2;
        if (tStatus)
        {
            Palk.localScale = new Vector3(Deffolt.x, Deffolt.y, 1);
            JointAngleLimits2D limits = hingeJoint2.limits;
            limits.min = 0;
            hingeJoint2.limits = limits;
            wheelJoint.suspension = new JointSuspension2D
            {
                frequency = 7,
                angle = wheelJoint.suspension.angle,
                dampingRatio = wheelJoint.suspension.dampingRatio
            };
        }
        else
        {
            Palk.localScale = new Vector3(Acut.x, Acut.y, 1);
            JointAngleLimits2D limits = hingeJoint2.limits;
            limits.min = 359;
            hingeJoint2.limits = limits;
            wheelJoint.suspension = new JointSuspension2D
            {
                frequency = 20,
                angle = wheelJoint.suspension.angle,
                dampingRatio = wheelJoint.suspension.dampingRatio
            };
        }

        if (wStatus)
        {
            back.sprite = wheelSport;
            folt.sprite = wheelSport;
            cycleControl.SpeesP += 0.2f;
        }
        cycleControl.SpeesP += 0.2f * frotSyst;
        cycleControl.SpeesP += 0.2f * bakeSyst;
    }
}

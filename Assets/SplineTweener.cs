using UnityEngine;
using Pixelplacement;

[RequireComponent(typeof(Spline))]
public class SplineTweener : MonoBehaviour
{
    [SerializeField] Transform myObject;
    [SerializeField] bool faceDirection = true;
    [SerializeField] float duration = 2f;
    [SerializeField] AnimationCurve animationCurve = AnimationCurve.Linear(0,0,1,1);
    [SerializeField] Tween.LoopType loopType = Tween.LoopType.Loop;

    private Spline mySpline;

    void Start()
    {
        mySpline = GetComponent<Spline>();

        Tween.Spline(mySpline, myObject, 0, 1, faceDirection, duration, 0, animationCurve, loopType);
    }
}
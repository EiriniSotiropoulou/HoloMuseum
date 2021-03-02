using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.Utilities.Solvers;
public class ArrowHandler : Solver
{
    // Start is called before the first frame update

    // Update is called once per frame

    public float forward=2.0f;
    public float right = 1.0f;
    public float vertical = 1.0f;

    [Tooltip("The GameObject transform to point the indicator towards when this object is not in view.\nThe frame of reference for viewing is defined by the Solver Handler Tracked Target Type")]
    public Transform DirectionalTarget;


    public override void SolverUpdate()
    {
        var reference = SolverHandler.TransformTarget;

        if (DirectionalTarget is null) return;   

        GoalPosition = reference.position + reference.forward * forward + reference.right * right+ reference.up* vertical;
        Vector3 trackerToTargetDirection = (DirectionalTarget.position - GoalPosition).normalized;

        // Project the vector (from the frame of reference (SolverHandler target) to the Directional Target) onto the "viewable" plane which is the z-x plane. (vector.up is the y axis normal)
        Vector3 indicatorDirection = Vector3.ProjectOnPlane(trackerToTargetDirection, Vector3.up).normalized;

        // If the our indicator direction is 0, set the direction to the right.
        // This will only happen if the frame of reference (SolverHandler target) is facing directly away from the directional target.
        if (indicatorDirection == Vector3.zero)
        {
            indicatorDirection = reference.right;
        }

        GoalRotation = Quaternion.LookRotation(indicatorDirection, Vector3.up);

    }
}

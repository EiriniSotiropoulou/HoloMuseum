using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Utilities.Solvers;
public class ArrowHandler : Solver
{
    // Start is called before the first frame update

    // Update is called once per frame

    public float forward = 2.0f;
    public float right = 1.0f;
    public float vertical = 1.0f;

    [Tooltip("the min distance between camera and target that the arrow disappears.")]
    public float MinDistance = 2f;
    [Tooltip("the max view angle that defines if target is in FOV")]
    public float viewAngle = 45; //

    [Tooltip("The GameObject transform to point the indicator towards when this object is not in view.\nThe frame of reference for viewing is defined by the Solver Handler Tracked Target Type")]
    public Transform DirectionalTarget;
    public bool pinned = false;
    public Component[] renderers;
    public override void SolverUpdate()
    {
        var referenceParent = gameObject.transform.parent.transform; //parent slide
        var referenceCamera = SolverHandler.TransformTarget;
        float cameraToTargetDistance = (referenceCamera.position - DirectionalTarget.position).magnitude;
        if (DirectionalTarget is null) return;

       
        if (cameraToTargetDistance < MinDistance && IsInFOV (DirectionalTarget.gameObject) )
        
        {
            MakeInvisible(gameObject);  //if camera is close to target and target in in field of view
        }
        else
        {
            MakeVisible(gameObject);
        }

        GoalPosition = referenceCamera.position + referenceCamera.forward * forward + referenceCamera.right * right + referenceCamera.up * vertical;
        

        //**Calculating Rotation**//
        Vector3 trackerToTargetDirection = (DirectionalTarget.position - GoalPosition).normalized;

        // Project the vector (from the frame of reference (SolverHandler target) to the Directional Target) onto the "viewable" plane which is the z-x plane. (vector.up is the y axis normal)
        Vector3 indicatorDirection = Vector3.ProjectOnPlane(trackerToTargetDirection, Vector3.up).normalized;

        // If the our indicator direction is 0, set the direction to the right.
        // This will only happen if the frame of reference (SolverHandler target) is facing directly away from the directional target.
        if (indicatorDirection == Vector3.zero)
        {
            indicatorDirection = referenceCamera.right;
        }

        GoalRotation = Quaternion.LookRotation(indicatorDirection, Vector3.up);
        //**************************//
    }

    bool IsInFOV(GameObject obj)
    {
        var referenceCamera = SolverHandler.TransformTarget;

        // Get the direction to the object
        var directionToObject = (obj.transform.position - referenceCamera.position).normalized;

        // Calculate the angle to the object and check if it's inside our viewAngle.
        bool isInsideAngle = Vector3.Angle(referenceCamera.forward, directionToObject) < viewAngle;

        return isInsideAngle;
    }

    public void MakeInvisible(GameObject obj)
    {

        renderers = obj.transform.GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer joint in renderers)
        {
            joint.enabled = false;
        }

    }

    public void MakeVisible(GameObject obj)
    {

        renderers = obj.transform.GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer joint in renderers)
        {
            joint.enabled = true;
        }

    }
}

        


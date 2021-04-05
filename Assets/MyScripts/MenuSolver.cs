using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Utilities.Solvers;
public class MenuSolver : Solver
{
    public float forward = 2.0f;
    public float right = 1.0f;
    public float vertical = 1.0f;

    public override void SolverUpdate()
    {
        /*var reference = SolverHandler.TransformTarget;
        GoalPosition = reference.position + reference.forward * forward + reference.right * right + reference.up * vertical;*/

        //if gaze is on either intro label or button stop the intro window from moving
        if (CoreServices.InputSystem.GazeProvider.GazeTarget == gameObject || CoreServices.InputSystem.GazeProvider.GazeTarget == gameObject.transform.GetChild(6).gameObject || CoreServices.InputSystem.GazeProvider.GazeTarget == gameObject.transform.GetChild(0).gameObject || CoreServices.InputSystem.GazeProvider.GazeTarget == gameObject.transform.GetChild(1).gameObject || CoreServices.InputSystem.GazeProvider.GazeTarget == gameObject.transform.GetChild(2).gameObject || CoreServices.InputSystem.GazeProvider.GazeTarget == gameObject.transform.GetChild(3).gameObject || CoreServices.InputSystem.GazeProvider.GazeTarget == gameObject.transform.GetChild(4).gameObject || CoreServices.InputSystem.GazeProvider.GazeTarget == gameObject.transform.GetChild(5).gameObject)
        {
            gameObject.GetComponent<RadialView>().enabled=false;
            //LogCurrentGazeTarget();
        }
        else
        {
            gameObject.GetComponent<RadialView>().enabled = true;
        }
    }

    void LogCurrentGazeTarget()
    {
        if (CoreServices.InputSystem.GazeProvider.GazeTarget)
        {
            Debug.Log("User gaze is currently over game object: "
                + CoreServices.InputSystem.GazeProvider.GazeTarget);
        }
    }

}

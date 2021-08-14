using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Utilities.Solvers;
public class QuizSolver : Solver
{

    public override void SolverUpdate()
    {
        //if gaze is on either intro label or button stop the intro window from moving
        if (CoreServices.InputSystem.GazeProvider.GazeTarget == gameObject
            || CoreServices.InputSystem.GazeProvider.GazeTarget == gameObject.transform.GetChild(0).transform.GetChild(0).gameObject
            || CoreServices.InputSystem.GazeProvider.GazeTarget == gameObject.transform.GetChild(1).transform.GetChild(0).gameObject
            || CoreServices.InputSystem.GazeProvider.GazeTarget == gameObject.transform.GetChild(2).gameObject
            || CoreServices.InputSystem.GazeProvider.GazeTarget == gameObject.transform.GetChild(3).gameObject
            || CoreServices.InputSystem.GazeProvider.GazeTarget == gameObject.transform.GetChild(4).gameObject
            || CoreServices.InputSystem.GazeProvider.GazeTarget == gameObject.transform.GetChild(5).gameObject
            || CoreServices.InputSystem.GazeProvider.GazeTarget == gameObject.transform.GetChild(6).gameObject)
        {
            
            gameObject.GetComponent<RadialView>().enabled = false;
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

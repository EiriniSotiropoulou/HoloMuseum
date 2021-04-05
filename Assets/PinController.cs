using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.Utilities.Solvers;
public class PinController : MonoBehaviour
{
    // Start is called before the first frame update
    

    public void CheckState()
    {
        //Debug.Log("click");
        bool Toggled = gameObject.GetComponent<Interactable>().IsToggled;

        if (Toggled)
        {
            PinItem();
        }
        else
        {
            UnpinItem();
        }
    }
    // Update is called once per frame
    void PinItem()
    {
        gameObject.transform.Find("UIButtonSquareIcon").transform.eulerAngles = new Vector3( //rotate pin icon
            gameObject.transform.eulerAngles.x,
            gameObject.transform.eulerAngles.y,
            gameObject.transform.eulerAngles.z + 45 );

        if (gameObject.transform.parent.gameObject.HasComponent<MenuSolver>()) //check if the game object is the intro or the quiz
        {
            //Debug.Log("pin" + "menu");
            gameObject.transform.parent.gameObject.GetComponent<MenuSolver>().enabled = false;
        }
        else
        {
            //Debug.Log("pin" + "quiz");
            gameObject.transform.parent.gameObject.GetComponent<QuizSolver>().enabled = false;
        }

        gameObject.transform.GetComponentInParent<RadialView>().enabled = false;
    }

    void UnpinItem()
    {
        gameObject.transform.Find("UIButtonSquareIcon").transform.eulerAngles = new Vector3( //rotate pin icon
         gameObject.transform.eulerAngles.x,
         gameObject.transform.eulerAngles.y,
         gameObject.transform.eulerAngles.z - 45);

        if (gameObject.transform.parent.gameObject.HasComponent<MenuSolver>())
        {
            //Debug.Log("unpin" + "menu");
            gameObject.transform.parent.gameObject.GetComponent<MenuSolver>().enabled = true;
        }
        else
        {
            //Debug.Log("unpin"+"quiz");
            gameObject.transform.parent.gameObject.GetComponent<QuizSolver>().enabled = true;
        }

        gameObject.transform.GetComponentInParent<RadialView>().enabled = true;
    }



}

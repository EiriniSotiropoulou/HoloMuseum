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
        GameObject thisSlide = gameObject.transform.parent.gameObject;
        GameObject Container = thisSlide.transform.parent.gameObject;

        if (Toggled)
        {   // pin this slide and all siblings slides
            for (int i = 0;  i < Container.transform.childCount ; i++)
            {
                PinItem(Container.transform.GetChild(i).gameObject);
            }
            
            
        }
        else
        {   // unpin this slide and all siblings slides
            for (int i = 0; i < Container.transform.childCount; i++)
            {
                UnpinItem(Container.transform.GetChild(i).gameObject);
            }
        }
    }
    
    void PinItem(GameObject slide)
    {
        slide.transform.Find("GuideArrow").GetComponent<ArrowHandler>().pinned = true;

        slide.transform.Find("PinButton").gameObject.GetComponent<Interactable>().IsToggled=true;
        slide.transform.Find("PinButton").transform.Find("UIButtonSquareIcon").transform.eulerAngles = new Vector3( //rotate pin icon
            gameObject.transform.eulerAngles.x,
            gameObject.transform.eulerAngles.y,
            gameObject.transform.eulerAngles.z + 45 );
        
        if (slide.HasComponent<MenuSolver>()) //check if the game object is the intro or the quiz
        {
            //Debug.Log("pin" + "menu");
            slide.GetComponent<MenuSolver>().enabled = false;
        }
        else
        {
            //Debug.Log("pin" + "quiz");
            slide.GetComponent<QuizSolver>().enabled = false;
        }

        slide.transform.GetComponent<RadialView>().enabled = false;
    }

    void UnpinItem(GameObject slide)
    {

        slide.transform.Find("GuideArrow").GetComponent<ArrowHandler>().pinned = false;


        slide.transform.Find("PinButton").gameObject.GetComponent<Interactable>().IsToggled = false;
        slide.transform.Find("PinButton").transform.Find("UIButtonSquareIcon").transform.eulerAngles = new Vector3( //rotate pin icon
         gameObject.transform.eulerAngles.x,
         gameObject.transform.eulerAngles.y,
         gameObject.transform.eulerAngles.z - 45);

        if (slide.HasComponent<MenuSolver>())
        {
            //Debug.Log("unpin" + "menu");
            slide.GetComponent<MenuSolver>().enabled = true;
        }
        else
        {
            //Debug.Log("unpin"+"quiz");
            slide.GetComponent<QuizSolver>().enabled = true;
        }

        slide.GetComponent<RadialView>().enabled = true;
    }



}

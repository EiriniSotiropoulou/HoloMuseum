using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroPageController : MonoBehaviour
{

    public int number; //identifier attribute


    public void NextPage()
    {
        int numOfPages = gameObject.transform.parent.transform.childCount;

        gameObject.SetActive(false);//deactivate this page

        if (number== numOfPages-1) //end of pages
        {
            return;
        }

        //place next page at the same place as this one
        gameObject.transform.parent.GetChild(number + 1).transform.position = gameObject.transform.position;
        gameObject.transform.parent.GetChild(number + 1).transform.rotation = gameObject.transform.rotation;
        
        gameObject.transform.parent.GetChild(number + 1).gameObject.SetActive(true); //activate next page

        // place next guide arrow at the same place as this one
        gameObject.transform.parent.GetChild(number + 1).Find("GuideArrow").transform.position = gameObject.transform.Find("GuideArrow").transform.position;
        gameObject.transform.parent.GetChild(number + 1).Find("GuideArrow").transform.rotation = gameObject.transform.Find("GuideArrow").transform.rotation;


        //Debug.Log(number);
    }

    public void PreviousPage()
    {
        int index = transform.GetSiblingIndex();

        //deactivate this page
        gameObject.SetActive(false);

        //place previous previous at the same place as this one
        gameObject.transform.parent.GetChild(index - 1).transform.position = gameObject.transform.position;
        gameObject.transform.parent.GetChild(index - 1).transform.rotation = gameObject.transform.rotation;

        gameObject.transform.parent.GetChild(index - 1).gameObject.SetActive(true); //activate previous page

        // place previous guide arrow at the same place as this one
        if (index != 0)
        {
            gameObject.transform.parent.GetChild(index - 1).Find("GuideArrow").transform.position = gameObject.transform.Find("GuideArrow").transform.position;
            gameObject.transform.parent.GetChild(index - 1).Find("GuideArrow").transform.rotation = gameObject.transform.Find("GuideArrow").transform.rotation;
        }
/*        gameObject.transform.parent.GetChild(index - 1).Find("GuideArrow").transform.position = gameObject.transform.Find("GuideArrow").transform.position;
        gameObject.transform.parent.GetChild(index - 1).Find("GuideArrow").transform.rotation = gameObject.transform.Find("GuideArrow").transform.rotation;

*/
    }






}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Microsoft.MixedReality.Toolkit.Experimental.Utilities;

public class IntroButtonHandler : MonoBehaviour
{
    const int NumbOfPages= 4;
    int i = 0;
    string[] content = { "1", "2", "3", "4" };

    public GameObject arrow;
    public GameObject art1;
    public GameObject art2;
    public GameObject art3;
    public GameObject art4;
    public void ΝextPage()
    {

        GameObject introText = GameObject.Find("Intro/text");
        GameObject ButtonLabel = gameObject.transform.GetChild(0).transform.GetChild(0).gameObject;
       
       


        if (i < NumbOfPages)
        {

            switch (i)
            {
                case 0:
                    arrow.SetActive(true);
                    arrow.GetComponent<ArrowHandler>().DirectionalTarget = art1.transform;

                    break;
                case 1:
                    arrow.GetComponent<ArrowHandler>().DirectionalTarget = art2.transform;
                    break;
                case 2:
                    arrow.GetComponent<ArrowHandler>().DirectionalTarget = art3.transform;
                    break;
                case 3:
                    arrow.GetComponent<ArrowHandler>().DirectionalTarget = art4.transform;
                    break;
            }

            introText.GetComponent<TextMeshPro>().text = content[i++];

            if (i == NumbOfPages)
            {
                ButtonLabel.GetComponent<TextMesh>().text = "Finish";
                
            }
        }
        else //when the content for display ends
        {
            this.transform.parent.gameObject.SetActive(false);
            arrow.SetActive(false);
        }    
    }

 
}

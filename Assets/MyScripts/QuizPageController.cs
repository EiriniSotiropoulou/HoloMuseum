using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.UI;

public class QuizPageController : MonoBehaviour
{
    public int number; //identifier attribute ------check if needed
    public int correct; // shows correct answer ----
    bool activeQuiz;
    public void NextPage()
    {
        int numOfPages = gameObject.transform.parent.transform.childCount;
        int index = transform.GetSiblingIndex();
        GameObject manager = GameObject.Find("Manager");

        activeQuiz = manager.GetComponent<MainManager>().activeQuiz;

        //deactivate this page
        gameObject.SetActive(false);

        //GameObject ButtonPrevious = gameObject.transform.parent.GetChild(index + 1).transform.Find("ButtonPrevious").gameObject;
        
        if (index == numOfPages - 2) 
        {
            GameObject ButtonNextLabel = gameObject.transform.parent.GetChild(index + 1).transform.Find("ButtonNext").transform.GetChild(0).transform.Find("Label").gameObject;
            //if it is the last page the next button says "Finish"
            ButtonNextLabel.GetComponent<TextMesh>().text = "Finish";
        }


        //if the user is answering questions
        if (activeQuiz)
        {
            SendAnswer();

            if (index == numOfPages - 1) //return if final page
            {
                manager.SendMessage("EvaluateQuiz");
                return;
            }

        }
        else
        {
            //gameObject.transform.Find("GuideArrow").gameObject.SetActive(false);
            if (index == numOfPages - 1) //return if final page
            {
                return;
            }
            gameObject.transform.parent.GetChild(index + 1).transform.Find("GuideArrow").gameObject.SetActive(true);
        }


        //place next page at the same place as this one
        gameObject.transform.parent.GetChild(index + 1).transform.position = gameObject.transform.position;
        gameObject.transform.parent.GetChild(index + 1).transform.rotation = gameObject.transform.rotation;

        //activate next page
        gameObject.transform.parent.GetChild(index + 1).gameObject.SetActive(true);

        //activate next page's previous button
        gameObject.transform.parent.GetChild(index + 1).transform.Find("ButtonPrevious").gameObject.SetActive(true);


        // place next guide arrow at the same place as this one
        gameObject.transform.parent.GetChild(index + 1).Find("GuideArrow").transform.position = gameObject.transform.Find("GuideArrow").transform.position;
        gameObject.transform.parent.GetChild(index + 1).Find("GuideArrow").transform.rotation = gameObject.transform.Find("GuideArrow").transform.rotation;
        
    }

    public void PreviousPage()
    {
        int index = transform.GetSiblingIndex();

        //deactivate this page
        gameObject.SetActive(false);

        if (activeQuiz)
        {
            SendAnswer();
        }
        else
        {
            ;//gameObject.transform.parent.GetChild(index - 1).transform.Find("GuideArrow").gameObject.SetActive(true);
        }


        //place previous previous at the same place as this one
        gameObject.transform.parent.GetChild(index - 1).transform.position = gameObject.transform.position;
        gameObject.transform.parent.GetChild(index - 1).transform.rotation = gameObject.transform.rotation;

        gameObject.transform.parent.GetChild(index - 1).gameObject.SetActive(true); //activate previous page

        // place next guide arrow at the same place as this one
        if (index != 0)
        {
            gameObject.transform.parent.GetChild(index - 1).Find("GuideArrow").transform.position = gameObject.transform.Find("GuideArrow").transform.position;
            gameObject.transform.parent.GetChild(index - 1).Find("GuideArrow").transform.rotation = gameObject.transform.Find("GuideArrow").transform.rotation;
        }
        gameObject.transform.parent.GetChild(index - 1).Find("GuideArrow").transform.position = gameObject.transform.Find("GuideArrow").transform.position;
        gameObject.transform.parent.GetChild(index - 1).Find("GuideArrow").transform.rotation = gameObject.transform.Find("GuideArrow").transform.rotation;

        
    }

    public void SendAnswer() //send selected answer to Manager
    {
        GameObject manager = GameObject.Find("Manager");

        bool i1 = gameObject.transform.Find("answer1").GetComponent<Interactable>().IsToggled;
        bool i2 = gameObject.transform.Find("answer2").GetComponent<Interactable>().IsToggled;
        bool i3 = gameObject.transform.Find("answer3").GetComponent<Interactable>().IsToggled;
        bool i4 = gameObject.transform.Find("answer4").GetComponent<Interactable>().IsToggled;

        int selected = -1;

        if (i1) selected = 1;
        else if (i2) selected = 2;
        else if (i3) selected = 3;
        else if (i4) selected = 4;

        int[] message = { number, selected, correct };
        manager.SendMessage("SaveAnswer", message);
    }

    public void AnswerSwitch(int answer) //control to permit user to select only one answer
    {
        string[] names = { "answer1", "answer2", "answer3", "answer4" };
        string myAnswer = "answer" + answer;

        foreach (string name in names)
        {
            if (myAnswer != name)
            {
                gameObject.transform.Find(name).GetComponent<Interactable>().IsToggled = false;
            }
        }
    }



}

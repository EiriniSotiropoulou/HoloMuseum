using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using TMPro;
using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.Utilities.Solvers;
using System.Threading;

public class MainManager : MonoBehaviour
{
    
    public GameObject quiz;
    public GameObject intro;
    public GameObject arts;

    public GameObject artPrefab;
    public GameObject introPrefab;
    public GameObject quizPrefab;

    public AnswerTriple[] answers; //all answers for a quiz answering session

    public bool activeQuiz; //indicates if an answering session is on
    

    void Awake()
    {
        
        GameObject.Find("MixedRealityPlayspace/Diagnostics").SetActive(false);
  
        PopulateArt();
        PlaceArt();

    }

    void PopulateArt() {

        
        var ArtCollection = ArtContainer.Load(Path.Combine(Application.dataPath, "Resources/data.xml"));
        foreach(Art art in ArtCollection.Arts)
        {
            GameObject myArt=Instantiate(artPrefab, new Vector3(0, 0, 0), Quaternion.identity); //create new art instant

            myArt.transform.parent = arts.transform; //make this child to Artifacts in hierarchy
                                                     //populate it with data from ArtCollection

            //add title
            myArt.transform.Find("Interface").Find("title").GetComponent<TextMeshPro>().text = art.title;
            //add description
            myArt.transform.Find("Interface").Find("description").GetComponent<TextMeshPro>().text = art.decription;

            //add art number
            myArt.transform.GetComponent<ArtController>().number = art.number;
            myArt.name = "Art" + art.number;

            //add category
            myArt.transform.GetComponent<ArtController>().category = art.category;

            //add image
            string ImageName = art.image + "Back";
            MeshRenderer mr = myArt.transform.Find("Sphere").GetComponent<MeshRenderer>();
            mr.material= Resources.Load(ImageName, typeof(Material)) as Material;
            myArt.gameObject.SetActive(false);
            

        }

        

    }

    void populateIntro()
    {
        var IntroPageCollection = IntroContainer.Load(Path.Combine(Application.dataPath, "Resources/data.xml"));
        foreach (IntroPage page in IntroPageCollection.Pages)
        {
            GameObject myPage = Instantiate(introPrefab, new Vector3(0, 0, 0), Quaternion.identity); //create new introPage instant

            myPage.transform.parent = intro.transform; //make this child to intro in hierarchy
                                                       //populate it with data from IntroPageCollection

            //add dialogue
            myPage.transform.Find("text").GetComponent<TextMeshPro>().text = page.dialogue;

            //add page number
            myPage.transform.GetComponent<IntroPageController>().number = page.number;
            myPage.name = "Page" + page.number;


            GameObject ButtonLabel = myPage.transform.Find("ButtonNext").transform.GetChild(0).transform.Find("Text").gameObject;
            GameObject ButtonPrevious = myPage.transform.Find("ButtonPrevious").gameObject;


            if (page.number == IntroPageCollection.Pages.Length - 1) ButtonLabel.GetComponent<TextMeshPro>().text = "Finish"; //Finish label in last page
            if (page.number == 0) ButtonPrevious.SetActive(false); // not previous button at the 1st page 



            //add directional arrow to corresponding artifact or deactivate arrow
            if (page.directional == -1) //for pages that should point at an artifact
            {
                myPage.transform.Find("GuideArrow").gameObject.SetActive(false);
               
            }
            else if(page.directional == -2) //for pages that shouldnt have an arrow nor text
            {
                myPage.transform.Find("GuideArrow").gameObject.SetActive(false);
                myPage.transform.Find("text").gameObject.SetActive(false);
            }
            else
            {
                myPage.transform.Find("GuideArrow").GetComponent<ArrowHandler>().DirectionalTarget = arts.transform.GetChild(page.directional);
            }

            //add image
            MeshRenderer mr = myPage.transform.Find("BigCapsule").GetComponent<MeshRenderer>();
            mr.material = Resources.Load(page.image, typeof(Material)) as Material;

            //deactivate page 
            myPage.gameObject.SetActive(false); 

        }
    }

    void populateQuiz()
    {
        

        var QuizPageCollection = QuizContainer.Load(Path.Combine(Application.dataPath, "Resources/data.xml"));
        
        foreach (QuizPage page in QuizPageCollection.Pages)
        {
            GameObject myPage = Instantiate(quizPrefab, new Vector3(0, 0, 0), Quaternion.identity); //create new quizPage instant
           
            myPage.transform.parent = quiz.transform; //make this child to Quiz in hierarchy
                                                      //populate it with data from QuizPageCollection

            //add question
            myPage.transform.Find("question").GetComponent<TextMeshPro>().text = page.question;

            //pass page number & correct answer to the Controller
            myPage.transform.GetComponent<QuizPageController>().number = page.number;
            myPage.transform.GetComponent<QuizPageController>().correct = page.correct;
            myPage.name = "Page" + page.number;

            //add image
            string ImageName = page.image + "Front";
            MeshRenderer mr = myPage.transform.Find("BigCapsule").GetComponent<MeshRenderer>();
            mr.material = Resources.Load(page.image, typeof(Material)) as Material;
            myPage.gameObject.SetActive(false);

            //populate answers
            myPage.transform.Find("answer1").GetComponent<TextMeshPro>().text = page.answer1;
            myPage.transform.Find("answer2").GetComponent<TextMeshPro>().text = page.answer2;
            myPage.transform.Find("answer3").GetComponent<TextMeshPro>().text = page.answer3;
            myPage.transform.Find("answer4").GetComponent<TextMeshPro>().text = page.answer4;


            //input the target of the arrow
            string corrDirec = "directional" + page.correct;
            myPage.transform.Find("GuideArrow").GetComponent<ArrowHandler>().DirectionalTarget = arts.transform.GetChild(page.directional);


            //make sure only one answer is selected
            myPage.transform.Find("answer1").GetComponent<Interactable>().OnClick.AddListener(delegate () { myPage.SendMessage("AnswerSwitch", 1); });
            myPage.transform.Find("answer2").GetComponent<Interactable>().OnClick.AddListener(delegate () { myPage.SendMessage("AnswerSwitch", 2); });
            myPage.transform.Find("answer3").GetComponent<Interactable>().OnClick.AddListener(delegate () { myPage.SendMessage("AnswerSwitch", 3); });
            myPage.transform.Find("answer4").GetComponent<Interactable>().OnClick.AddListener(delegate () { myPage.SendMessage("AnswerSwitch", 4); });

           
        }

    }
    public void Anchor() //anchor artifacts (or undo anchor) so the user cannot move them
    {
        foreach (Transform child in arts.transform)
        {
            if(child.GetComponent<TapToPlace>().enabled == false) child.GetComponent<TapToPlace>().enabled=true;
            else child.GetComponent<TapToPlace>().enabled = false;
        }
    }
    public void show() //artifacts are visible. used as voice command
    {
        foreach (Transform child in arts.transform)
        {
            child.gameObject.SetActive(true);
        }
    }
    public void hide() //artifacts are invisible. used as voice command
    {
        foreach (Transform child in arts.transform)
        {
            child.gameObject.SetActive(false);
        }
    }
    void PlaceArt()
    //All of the artifacts have an instance of the ArtPrefab that must be placed by the developer before the first session
    //so the app can locate where it is positioned
    //voice commands: hide/show 

    {
        int count = arts.transform.childCount;

        Debug.Log("placing art...");

        arts.SetActive(true);

        //place the artifacts in a circle around the developer
        //they can move them by air-taping (TapToPlace component)
        if (PlayerPrefs.GetInt("first", 0) == 0)
        {
            var radius = 1f;

            int i = 0;
            foreach (Transform child in arts.transform)
            {
                var angle = i * Mathf.PI * 2 / count;
                var pos = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;
                child.gameObject.SetActive(true);
                child.transform.position = pos;
                i++;
            }
            PlayerPrefs.SetInt("first", 1);
        }
        else
        {
            foreach (Transform child in arts.transform)
            {
                child.gameObject.SetActive(true);
            }
        }
            
    }

    public void StartIntro()
    {
        GameObject camera = GameObject.Find("MixedRealityPlayspace").transform.Find("Main Camera").gameObject;

        Debug.Log("starting intro...");

        Empty(intro);

        populateIntro();

        intro.SetActive(true);

        intro.transform.GetChild(0).gameObject.SetActive(true); //activate first page

        //place intro in front of user, looking at the camera
        intro.transform.GetChild(0).transform.position = camera.transform.position + camera.transform.forward * 0.5f + camera.transform.right * 0.4f;
        intro.transform.GetChild(0).transform.LookAt(camera.transform, Vector3.up);

    }


    public void StartQuiz() //start or restart quiz
    {
        GameObject camera = GameObject.Find("MixedRealityPlayspace").transform.Find("Main Camera").gameObject;

        Debug.Log("starting quiz...");

        quiz.SetActive(true);
        activeQuiz = true; //when the quiz appears an answering session starts too

        //populate quiz

        Empty(quiz);
        populateQuiz();
        
        quiz.transform.GetChild(0).gameObject.SetActive(true); //activate first page

        //place quiz in front of user, looking at the camera
        quiz.transform.GetChild(0).transform.position = camera.transform.position + camera.transform.forward * 0.5f +camera.transform.right * 0.4f; 
        quiz.transform.GetChild(0).transform.LookAt(camera.transform, Vector3.up);

        answers =new AnswerTriple[quiz.transform.childCount]; 
        
    }

    public void EvaluateQuiz()
    {
        
        int TotalPages = answers.Length;

        activeQuiz = false;

        GameObject ResPage = Instantiate(quizPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        GameObject EndPage = Instantiate(quizPrefab, new Vector3(0, 0, 0), Quaternion.identity);

        ResPage.name = "Result";
        EndPage.name = "End";

        ResPage.transform.parent = quiz.transform; //parent new pages to quiz
        EndPage.transform.parent = quiz.transform;

        ResPage.SetActive(true);
        EndPage.SetActive(true);

        //
        ResPage.transform.position = quiz.transform.GetChild(TotalPages - 1).transform.position; //set transform same as previous page
        ResPage.transform.rotation = quiz.transform.GetChild(TotalPages - 1).transform.rotation;

        //background 
        MeshRenderer mr = ResPage.transform.Find("BigCapsule").GetComponent<MeshRenderer>();
        mr.material = Resources.Load("Pictures/Materials/Museum", typeof(Material)) as Material;

        MeshRenderer mre = EndPage.transform.Find("BigCapsule").GetComponent<MeshRenderer>();
        mre.material = Resources.Load("Pictures/Materials/Museum", typeof(Material)) as Material;

        //deactivate and activate the appropriate components
        ResPage.transform.Find("answer1").gameObject.SetActive(false);
        ResPage.transform.Find("answer2").gameObject.SetActive(false);
        ResPage.transform.Find("answer3").gameObject.SetActive(false);
        ResPage.transform.Find("answer4").gameObject.SetActive(false);

        ResPage.transform.Find("GuideArrow").gameObject.SetActive(false);
        ResPage.transform.Find("ButtonPrevious").gameObject.SetActive(false);
        ResPage.transform.Find("question").gameObject.SetActive(false);
        ResPage.transform.Find("text").gameObject.SetActive(true);

        EndPage.transform.Find("answer1").gameObject.SetActive(false);
        EndPage.transform.Find("answer2").gameObject.SetActive(false);
        EndPage.transform.Find("answer3").gameObject.SetActive(false);
        EndPage.transform.Find("answer4").gameObject.SetActive(false);

        EndPage.transform.Find("GuideArrow").gameObject.SetActive(false);
        EndPage.transform.Find("ButtonPrevious").gameObject.SetActive(false);
        EndPage.transform.Find("question").gameObject.SetActive(false);
        EndPage.transform.Find("text").gameObject.SetActive(true);

        EndPage.transform.Find("text").GetComponent<TextMeshPro>().text = "Anytime you want to restart the quiz say 'Quiz'.\n\n\nIf you want to take a look again at the introduction say 'Intro'";

        EndPage.SetActive(false);


        //compute and display percentage for eval page

        int percentage = 0;

        foreach (AnswerTriple ans in answers) // compute percentage and place pages with wrong answers after eval page
        {
            
            if (ans.correct == ans.answer) 
            {
                percentage += 1;
                
            }
            else //for every wrong answer show next the correct one
            {
                quiz.transform.Find("Page"+ ans.page).SetAsLastSibling();

                quiz.transform.Find("Page" + ans.page).transform.Find("answer1").GetComponent<Interactable>().IsEnabled = false;
                quiz.transform.Find("Page" + ans.page).transform.Find("answer2").GetComponent<Interactable>().IsEnabled = false;
                quiz.transform.Find("Page" + ans.page).transform.Find("answer3").GetComponent<Interactable>().IsEnabled = false;
                quiz.transform.Find("Page" + ans.page).transform.Find("answer4").GetComponent<Interactable>().IsEnabled = false;

                quiz.transform.Find("Page" + ans.page).transform.Find("answer1").GetComponent<Interactable>().IsToggled = false;
                quiz.transform.Find("Page" + ans.page).transform.Find("answer2").GetComponent<Interactable>().IsToggled = false;
                quiz.transform.Find("Page" + ans.page).transform.Find("answer3").GetComponent<Interactable>().IsToggled = false;
                quiz.transform.Find("Page" + ans.page).transform.Find("answer4").GetComponent<Interactable>().IsToggled = false;

                quiz.transform.Find("Page" + ans.page).transform.Find("answer" + ans.correct).GetComponent<Interactable>().IsEnabled = true;
                quiz.transform.Find("Page" + ans.page).transform.Find("answer" + ans.correct).GetComponent<Interactable>().IsToggled=true;
                quiz.transform.Find("Page" + ans.page).transform.Find("answer" + ans.correct).GetComponent<Interactable>().CanDeselect = false;
                quiz.transform.Find("Page" + ans.page).transform.Find("answer" + ans.correct).GetComponent<Interactable>().CanSelect = false;
                quiz.transform.Find("Page" + ans.page).transform.Find("answer" + ans.correct).Find("border").gameObject.SetActive(true);


            }
        }

        EndPage.transform.SetAsLastSibling(); //place endpage last

        percentage *= 100;
        percentage /= TotalPages;
        ResPage.transform.Find("text").GetComponent<TextMeshPro>().text = "You have scored: "+percentage+"%\n\n\n Air Tap 'next' to check the correct answers.\n\n\n Follow the arrow to check the artifacts on your own.";




    }
    public void SaveAnswer(int[] message) //saves the answer that the user gave for a quiz page
        //receives message from QuizPageController
    {
        AnswerTriple temp = new AnswerTriple() { page = message[0], answer= message[1], correct= message[2] };

        answers[message[0]] = temp;    
    } 


    public struct AnswerTriple
    {
        public int page; //quiz page
        public int answer; //answer given by the user
        public int correct; //coreect answer
    }


    void Empty(GameObject obj)
    {
        var children = new List<GameObject>();
        foreach (Transform child in obj.transform) children.Add(child.gameObject);
        children.ForEach(child => Destroy(child));
        obj.transform.DetachChildren(); // make childcound 0 because destroyed objects don't get removed until the end of the frame
    }

    /* Sources
     
     
     http://wiki.unity3d.com/index.php?title=Saving_and_Loading_Data:_XmlSerializer&_ga=2.18430000.845015915.1611851166-443249922.1599339853
     
    https://forums.hololens.com/discussion/6472/load-xml-files-on-hololens-in-runtime
    https://docs.microsoft.com/en-us/windows/uwp/files/quickstart-reading-and-writing-files
    https://forums.hololens.com/discussion/comment/10580#Comment_10580'
     
     */
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using TMPro;
using Microsoft.MixedReality.Toolkit.UI;


public class MainManager : MonoBehaviour
{
    
    public GameObject quiz;
    public GameObject intro;
    public GameObject arts;

    public GameObject artPrefab;
    public GameObject introPrefab;
    public GameObject quizPrefab;

    public AnswerTriple[] answers; //all answers of a quiz session

    public bool activeQuiz; //indicates if an answering session is on


    void Awake()
    {

        //disable all elements at launch
        GameObject.Find("MixedRealityPlayspace/Diagnostics").SetActive(false);
        intro.SetActive(false);
        quiz.SetActive(false);
        arts.SetActive(false);/**/

        /*At the first run of the app, the artifacts are populated and then
        the designer places them at the correct place. 
        If museum is true the digital artifacts are invisible*/
        if (PlayerPrefs.GetInt("FirstTime")!=1 || !PlayerPrefs.HasKey("FirstTime")) 
        {
            PopulateArt();
            PlaceArt();
            //PlayerPrefs.SetInt("FirstTime", 1);
        }
        populateIntro();
        if (PlayerPrefs.GetInt("FirstTime") == 1)
        {
            intro.SetActive(true);
            intro.transform.GetChild(0).gameObject.SetActive(true); //activate first page
        }
    }

    void PopulateArt() {

        
        var ArtCollection = ArtContainer.Load(Path.Combine(Application.dataPath, "Resources/data.xml"));
        foreach(Art art in ArtCollection.Arts)
        {
            GameObject myArt=Instantiate(artPrefab, new Vector3(0, 0, 0), Quaternion.identity); //create new art instant

            myArt.transform.parent = arts.transform; //make this child to Artifacts in hierarchy
                                                     //populate it with data from ArtCollection

            //add description
            myArt.transform.GetComponent<TextMeshPro>().text = art.decription;

            //add art number
            myArt.transform.GetComponent<ArtController>().number = art.number;
            myArt.name = "Art" + art.number;

            //add category
            myArt.transform.GetComponent<ArtController>().category = art.category;

            //add image
            myArt.transform.Find("image").Find("RawImage").GetComponent<RawImage>().texture = Resources.Load<Texture>(art.image);

            myArt.gameObject.SetActive(false);
            //add art to list

        }

        

    }

    void populateIntro()
    {
        var IntroPageCollection = IntroContainer.Load(Path.Combine(Application.dataPath, "Resources/data.xml"));
        foreach (IntroPage page in IntroPageCollection.Pages)
        {
            GameObject myPage = Instantiate(introPrefab, new Vector3(0, 0, 0), Quaternion.identity); //create new art instant

            myPage.transform.parent = intro.transform; //make this child to Artifacts in hierarchy
                                                      //populate it with data from ArtCollection

            //add dialogue
            myPage.transform.Find("text").GetComponent<TextMeshPro>().text = page.dialogue;

            //add art number
            myPage.transform.GetComponent<IntroPageController>().number = page.number;
            myPage.name = "Page" + page.number;

            //add directional arrow to corresponding artifact
   
            Transform test = myPage.transform.Find("GuideArrow").GetComponent<ArrowHandler>().DirectionalTarget = arts.transform.GetChild(page.directional);

            //add image
            myPage.transform.Find("image").Find("RawImage").GetComponent<RawImage>().texture = Resources.Load<Texture>(page.image);

            //

            GameObject ButtonLabel = myPage.transform.Find("ButtonNext").transform.GetChild(0).transform.Find("Label").gameObject;

            if (page.number == IntroPageCollection.Pages.Length-1) ButtonLabel.GetComponent<TextMesh>().text = "Finish";
            
        }
    }

    void populateQuiz()
    {
        

        var QuizPageCollection = QuizContainer.Load(Path.Combine(Application.dataPath, "Resources/data.xml"));
        
        foreach (QuizPage page in QuizPageCollection.Pages)
        {
            GameObject myPage = Instantiate(quizPrefab, new Vector3(0, 0, 0), Quaternion.identity); //create new art instant
           
            myPage.transform.parent = quiz.transform; //make this child to Artifacts in hierarchy
                                                      //populate it with data from ArtCollection

            //add question
            myPage.transform.Find("question").GetComponent<TextMeshPro>().text = page.question;

            //pass page number & correct answer to the Controller
            myPage.transform.GetComponent<QuizPageController>().number = page.number;
            myPage.transform.GetComponent<QuizPageController>().correct = page.correct;
            myPage.name = "Page" + page.number;

            //add image
            myPage.transform.Find("image").Find("RawImage").GetComponent<RawImage>().texture = Resources.Load<Texture>(page.image);

            //populate answers
            myPage.transform.Find("answer1").GetComponent<TextMeshPro>().text = page.answer1;
            myPage.transform.Find("answer2").GetComponent<TextMeshPro>().text = page.answer2;
            myPage.transform.Find("answer3").GetComponent<TextMeshPro>().text = page.answer3;
            myPage.transform.Find("answer4").GetComponent<TextMeshPro>().text = page.answer4;


            //input the target of the arrow
            string corrDirec = "directional" + page.correct;
            myPage.transform.Find("GuideArrow").GetComponent<ArrowHandler>().DirectionalTarget = arts.transform.GetChild(page.directional);



            /*myPage.transform.Find("answer1").transform.Find("GuideArrow").GetComponent<ArrowHandler>().DirectionalTarget = arts.transform.GetChild(page.directional1);
            myPage.transform.Find("answer2").transform.Find("GuideArrow").GetComponent<ArrowHandler>().DirectionalTarget = arts.transform.GetChild(page.directional1);
            myPage.transform.Find("answer3").transform.Find("GuideArrow").GetComponent<ArrowHandler>().DirectionalTarget = arts.transform.GetChild(page.directional1);
            myPage.transform.Find("answer4").transform.Find("GuideArrow").GetComponent<ArrowHandler>().DirectionalTarget = arts.transform.GetChild(page.directional1);*/

            //make sure only one answer is selected
            myPage.transform.Find("answer1").GetComponent<Interactable>().OnClick.AddListener(delegate () { myPage.SendMessage("AnswerSwitch", 1); });
            myPage.transform.Find("answer2").GetComponent<Interactable>().OnClick.AddListener(delegate () { myPage.SendMessage("AnswerSwitch", 2); });
            myPage.transform.Find("answer3").GetComponent<Interactable>().OnClick.AddListener(delegate () { myPage.SendMessage("AnswerSwitch", 3); });
            myPage.transform.Find("answer4").GetComponent<Interactable>().OnClick.AddListener(delegate () { myPage.SendMessage("AnswerSwitch", 4); });

            


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
    //if the physical space is the museum ,the digital artifacts are invisible and are placed manually right at the physical artifact 
    //so the app can locate where it is positioned
    //voice commands: hide/show 

    {
        int count = arts.transform.childCount;

        Debug.Log("placing art...");

        arts.SetActive(true);

        //place the artifacts in a circle around the developer
        //they will place them using the tap and place component 

        var radius = 0.5f;

        int i = 0;
        foreach (Transform child in arts.transform)
        {
            var angle = i * Mathf.PI * 2 / count;
            var pos = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;
            child.gameObject.SetActive(true);
            child.transform.position = pos;
            i++;
        }
    }

    public void StartIntro()
    {
        GameObject camera = GameObject.Find("MixedRealityPlayspace").transform.Find("Main Camera").gameObject;

        Debug.Log("starting quiz...");

        intro.SetActive(true);

        intro.transform.GetChild(0).gameObject.SetActive(true);

        //place quiz in front of user, looking at the camera
        intro.transform.GetChild(0).transform.position = camera.transform.position + camera.transform.forward * 0.5f + camera.transform.right * 0.4f;
        intro.transform.GetChild(0).transform.LookAt(camera.transform, Vector3.up);


    }


    public void StartQuiz() //start or restart quiz
    {
        GameObject camera = GameObject.Find("MixedRealityPlayspace").transform.Find("Main Camera").gameObject;

        Debug.Log("starting quiz...");

        quiz.SetActive(true);
        activeQuiz = true;

        //populate quiz

        Empty(quiz);
        Debug.Log(quiz.transform.childCount);
        populateQuiz();

        quiz.transform.GetChild(0).gameObject.SetActive(true); 

        //place quiz in front of user, looking at the camera
        quiz.transform.GetChild(0).transform.position = camera.transform.position + camera.transform.forward * 0.5f +camera.transform.right * 0.4f; 
        quiz.transform.GetChild(0).transform.LookAt(camera.transform, Vector3.up);

        answers =new AnswerTriple[quiz.transform.childCount];
        
    }

    public void EvaluateQuiz()
    {
        /* selida me pososto. 
         * 
         * 
         next gia evaluation #DONE
         pages me lathos apantiseis kai me prasino oi diorthwmenes #DONE

         +directional pros ekthema swstis i kai olwn twn apantisewn #DONE
        
        an thelei ksankanei to quiz ----- reorder quiz when starting #DONE
         */
        int TotalPages = answers.Length;

        activeQuiz = false;

        GameObject ResPage = Instantiate(quizPrefab, new Vector3(0, 0, 0), Quaternion.identity); 

        ResPage.name = "Result";
        ResPage.transform.parent = quiz.transform;

        ResPage.SetActive(true);

        //error st 2 iter
        ResPage.transform.position = quiz.transform.GetChild(TotalPages - 1).transform.position; //set transform same as previous page
        ResPage.transform.rotation = quiz.transform.GetChild(TotalPages - 1).transform.rotation;

        
        ResPage.transform.Find("image").gameObject.SetActive(false); //deactivate objects not in use


        ResPage.transform.Find("answer1").gameObject.SetActive(false);
        ResPage.transform.Find("answer2").gameObject.SetActive(false);
        ResPage.transform.Find("answer3").gameObject.SetActive(false);
        ResPage.transform.Find("answer4").gameObject.SetActive(false);

        //compute and display percentage

        int percentage = 0;

        foreach (AnswerTriple ans in answers)
        {
            
            if (ans.correct == ans.answer) 
            {
                percentage += 1;
                
            }
            else //for every wrong answer show next the correct one
            {
                quiz.transform.Find("Page"+ ans.page).SetAsLastSibling();

                quiz.transform.Find("Page" + ans.page).transform.Find("answer1").GetComponent<Interactable>().IsToggled = false;
                quiz.transform.Find("Page" + ans.page).transform.Find("answer2").GetComponent<Interactable>().IsToggled = false;
                quiz.transform.Find("Page" + ans.page).transform.Find("answer3").GetComponent<Interactable>().IsToggled = false;
                quiz.transform.Find("Page" + ans.page).transform.Find("answer4").GetComponent<Interactable>().IsToggled = false;


                quiz.transform.Find("Page" + ans.page).transform.Find("answer" + ans.correct).GetComponent<Interactable>().IsToggled = true;
            }


        }
        percentage *= 100;
        percentage /= TotalPages;
        ResPage.transform.Find("question").GetComponent<TextMeshPro>().text = "You have scored: "+percentage+"%";

        


    }
    public void SaveAnswer(int[] message) //saves the answer that the user gave for a quiz page
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




    /* TO DO:
     * github #DONE
     * Populate art + image to texture #DONE
     * populate intro + intro layout #DONE
     * populate quiz + quiz layout #Done
     * quiz logic and data correction #DONE
     * quiz buttons for answers #DONE
     * artifacts placing logic #DONE
     * application's flow. check files not needed #DONE
     * make two versions for museum or not.. ---
     * the first hides artifacts when application starts #DONE
     * voice commands for hide show artifacts,intro #DONE
     * data collection ---
     * check with hololens .. --- second run are artifacts there? 
     * style ---
     */

    /* IDEAS
     * different colours for different eras
     * different colours to arrows
     * multiple arrows at quiz
     * icon for pin for quiz and intro
     * previous button intro
     * button to pin intro
     * try different shaders
     * random questions every time
     */




    /*:
     * - Code directional arrow #done
     * diagnostics none active #done
     * place art only at first launch #done
     * ~Make Art latch on walls beggining~ 
     * placing process #DONE
     * voice command to start quiz #test!
     * !Create layout for end Quiz! #DONE

     */



    //spatial mapping
    //make a good ui layout
    //first run : placing the artifacts
    //beside artifacts info icon
    //user run: intro just like menu hover
    //after finih arrow to point towards places of interest.. hover above them and you see display image and name
    //start quiz start button
    //after quiz is finished maybe encourage with arrows to again check displays that offer hints
    //when ready retake quiz --> result
    //music
    //goodbye

    //fix : arrows, intro hover , placing on walls (latch) solvers?

    /*arrow #DONE
     * na einai se ena epipedo zx
     * na einai stin katw deksia meria panta

     */



    /* Sources
     
     
     http://wiki.unity3d.com/index.php?title=Saving_and_Loading_Data:_XmlSerializer&_ga=2.18430000.845015915.1611851166-443249922.1599339853
     
     
     
     */
}


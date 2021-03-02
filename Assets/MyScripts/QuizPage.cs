using System.Xml;
using System.Xml.Serialization;

public class QuizPage
{
    [XmlAttribute("number")]
    public int number;

    public string question;
    public string image;

    public string answer1;
    public string answer2;
    public string answer3;
    public string answer4;

    public int correct;

    public int directional;
    public int directional1;
    public int directional2;
    public int directional3;
    public int directional4;
}


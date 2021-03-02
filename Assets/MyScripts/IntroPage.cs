using System.Xml;
using System.Xml.Serialization;

public class IntroPage
{
    [XmlAttribute("number")]
    public int number; //identifier attribute

    public string image;
    public string dialogue; 
    public int directional; //numerical identifier of a corresponding Art GameObject
}
using System.Xml;
using System.Xml.Serialization;

public class Art
{
    [XmlAttribute("number")]
    public int number;

    [XmlAttribute("category")]
    public string category;

    public string decription;
    public string title;
    public string image;

}
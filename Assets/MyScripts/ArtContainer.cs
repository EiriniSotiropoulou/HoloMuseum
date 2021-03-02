using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using UnityEngine;

[XmlRoot("App")]

public class ArtContainer
{
    [XmlArray("arts"), XmlArrayItem("art")]
    public Art[] Arts;


    public void Save(string path)
    {
        var serializer = new XmlSerializer(typeof(ArtContainer));
        using (var stream = new FileStream(path, FileMode.Create))
        {
            serializer.Serialize(stream, this);
        }
    }

    public static ArtContainer Load(string path)
    {
        var serializer = new XmlSerializer(typeof(ArtContainer));
        using (var stream = new FileStream(path, FileMode.Open))
        {
            return serializer.Deserialize(stream) as ArtContainer;
        }
    }

    //Loads the xml directly from the given string. Useful in combination with www.text.
    public static ArtContainer LoadFromText(string text)
    {
        var serializer = new XmlSerializer(typeof(ArtContainer));
        return serializer.Deserialize(new StringReader(text)) as ArtContainer;
    }
}

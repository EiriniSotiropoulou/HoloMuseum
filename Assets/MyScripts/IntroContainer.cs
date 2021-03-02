using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using UnityEngine;

[XmlRoot("App")]

public class IntroContainer
{ 
    [XmlArray("intro"), XmlArrayItem("page")]
    public IntroPage[] Pages;


    public void Save(string path)
    {
        var serializer = new XmlSerializer(typeof(IntroContainer));
        using (var stream = new FileStream(path, FileMode.Create))
        {
            serializer.Serialize(stream, this);
        }
    }

    public static IntroContainer Load(string path)
    {
        var serializer = new XmlSerializer(typeof(IntroContainer));
        using (var stream = new FileStream(path, FileMode.Open))
        {
            return serializer.Deserialize(stream) as IntroContainer;
        }
    }

    //Loads the xml directly from the given string. Useful in combination with www.text.
    public static IntroContainer LoadFromText(string text)
    {
        var serializer = new XmlSerializer(typeof(IntroContainer));
        return serializer.Deserialize(new StringReader(text)) as IntroContainer;
    }
}



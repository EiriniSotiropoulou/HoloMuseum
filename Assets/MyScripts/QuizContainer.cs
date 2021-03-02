using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using UnityEngine;

[XmlRoot("App")]

public class QuizContainer
{
    [XmlArray("quiz"), XmlArrayItem("page")]
    public QuizPage[] Pages;

    public void Save(string path)
    {
        var serializer = new XmlSerializer(typeof(QuizContainer));
        using (var stream = new FileStream(path, FileMode.Create))
        {
            serializer.Serialize(stream, this);
        }
    }

    public static QuizContainer Load(string path)
    {
        var serializer = new XmlSerializer(typeof(QuizContainer));
        using (var stream = new FileStream(path, FileMode.Open))
        {
            return serializer.Deserialize(stream) as QuizContainer;
        }
    }

    //Loads the xml directly from the given string. Useful in combination with www.text.
    public static QuizContainer LoadFromText(string text)
    {
        var serializer = new XmlSerializer(typeof(QuizContainer));
        return serializer.Deserialize(new StringReader(text)) as QuizContainer;
    }
}



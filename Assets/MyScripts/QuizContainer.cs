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


    public static QuizContainer Load(string path)
    {
        var serializer = new XmlSerializer(typeof(QuizContainer));
        QuizContainer deserialized = null;
        string path2;
#if WINDOWS_UWP

        path2 = Path.GetFullPath(Path.Combine(Application.persistentDataPath + "/" + "data.xml"));
        try
        {
            using (FileStream reader = File.Open(path2, FileMode.Open))
            {
                return serializer.Deserialize(reader) as QuizContainer;
            }
        }
        catch (System.Exception e)
        {
            //Debug.LogWarningFormat("Error loading animation : {0}", e.Message);
            return deserialized;
        }

#else

        using (var stream = new FileStream(path, FileMode.Open))
        {
            return serializer.Deserialize(stream) as QuizContainer;
        }


#endif
    }
}




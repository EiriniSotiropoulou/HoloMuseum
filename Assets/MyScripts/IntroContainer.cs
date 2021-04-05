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


    public static IntroContainer Load(string path)
    {
        var serializer = new XmlSerializer(typeof(IntroContainer));
        IntroContainer deserialized = null;
        string path2;
#if WINDOWS_UWP

        path2 = Path.GetFullPath(Path.Combine(Application.persistentDataPath + "/" + "data.xml"));
        try
        {
            using (FileStream reader = File.Open(path2, FileMode.Open))
            {
                return serializer.Deserialize(reader) as IntroContainer;
            }
        }
        catch (System.Exception e)
        {
            Debug.LogWarningFormat("Error loading animation : {0}", e.Message);
            return deserialized;
        }

#else
        using (var stream = new FileStream(path, FileMode.Open))
        {
            return serializer.Deserialize(stream) as IntroContainer;
        }

#endif

    }

}



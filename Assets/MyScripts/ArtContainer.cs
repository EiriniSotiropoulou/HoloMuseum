using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using UnityEngine;
#if WINDOWS_UWP
using Windows.Storage;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using System;
using Windows.Storage.Streams;
#endif

[XmlRoot("App")]

public class ArtContainer
{
    [XmlArray("arts"), XmlArrayItem("art")]
    public Art[] Arts;
    

    public static ArtContainer Load(string path)
    {
        var serializer = new XmlSerializer(typeof(ArtContainer));
        ArtContainer deserialized = null;
        string path2;

#if WINDOWS_UWP

        path2 = Path.GetFullPath(Path.Combine(Application.persistentDataPath + "/" + "data.xml"));
        try
        {
            using (FileStream reader = File.Open(path2, FileMode.Open))
            {
                return serializer.Deserialize(reader) as ArtContainer;
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
            return serializer.Deserialize(stream) as ArtContainer;
        }

#endif

    }
}


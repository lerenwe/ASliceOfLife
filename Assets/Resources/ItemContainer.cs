using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;

[XmlRoot("ChoicesCollection")]
public class ChoicesContainer {

    [XmlArray("Choices")]
    [XmlArrayItem("Choice")]
    public List<Item> choices = new List<Item>();

    public void Save (string path)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(ChoicesContainer));
        FileStream stream = new FileStream(path, FileMode.Create);
        serializer.Serialize(stream, this);
        stream.Close();
    }

    public static ChoicesContainer Load(string path)
    {
        //TextAsset _xml = Resources.Load<TextAsset>(path);

        XmlSerializer serializer = new XmlSerializer(typeof(ChoicesContainer));
        FileStream stream = new FileStream(path, FileMode.Open);
        //StringReader reader = new StringReader(_xml.text);

        ChoicesContainer items = serializer.Deserialize(stream) as ChoicesContainer;

        stream.Close();

        return items;
    }
}

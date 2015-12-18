using UnityEngine;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;

[System.Serializable]
public class Item {

    [XmlAttribute("name")]
    public string name;

    [XmlElement("ChoiceMade")]
    public bool ChoiceMade;

    [XmlElement("First")]
    public bool durability;

}

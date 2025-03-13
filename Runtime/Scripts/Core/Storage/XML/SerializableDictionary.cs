using System;
using System.Collections.Generic;
using System.Xml.Serialization;

[XmlRoot("SerializableDictionary")]
public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, IXmlSerializable
{
    public System.Xml.Schema.XmlSchema GetSchema() => null;

    public void ReadXml(System.Xml.XmlReader reader)
    {
        XmlSerializer keySerializer = new XmlSerializer(typeof(TKey));
        XmlSerializer valueSerializer = new XmlSerializer(typeof(TValue));

        bool isEmpty = reader.IsEmptyElement;
        reader.Read();
        if (isEmpty) return;

        while (reader.NodeType != System.Xml.XmlNodeType.EndElement)
        {
            reader.ReadStartElement("Item");

            reader.ReadStartElement("Key");
            TKey key = (TKey)keySerializer.Deserialize(reader);
            reader.ReadEndElement();

            reader.ReadStartElement("Value");
            TValue value = (TValue)valueSerializer.Deserialize(reader);
            reader.ReadEndElement();

            this[key] = value;

            reader.ReadEndElement();
        }
        reader.ReadEndElement();
    }

    public void WriteXml(System.Xml.XmlWriter writer)
    {
        XmlSerializer keySerializer = new XmlSerializer(typeof(TKey));
        XmlSerializer valueSerializer = new XmlSerializer(typeof(TValue));

        foreach (TKey key in Keys)
        {
            writer.WriteStartElement("Item");

            writer.WriteStartElement("Key");
            keySerializer.Serialize(writer, key);
            writer.WriteEndElement();

            writer.WriteStartElement("Value");
            valueSerializer.Serialize(writer, this[key]);
            writer.WriteEndElement();

            writer.WriteEndElement();
        }
    }
}
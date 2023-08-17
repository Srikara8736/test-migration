using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Retail.DTOs.XML;

[XmlRoot(ElementName = "LocationNo")]
public class LocationNo
{
    [XmlAttribute(AttributeName = "Value")]
    public string Value { get; set; }
}

[XmlRoot(ElementName = "LocationName")]
public class LocationName
{
    [XmlAttribute(AttributeName = "Value")]
    public string Value { get; set; }
}

[XmlRoot(ElementName = "StoreNo")]
public class StoreNo
{
    [XmlAttribute(AttributeName = "Value")]
    public string Value { get; set; }
}

[XmlRoot(ElementName = "StoreName")]
public class StoreName
{
    [XmlAttribute(AttributeName = "Value")]
    public string Value { get; set; }
}

[XmlRoot(ElementName = "EstablishmentNo")]
public class EstablishmentNo
{
    [XmlAttribute(AttributeName = "Value")]
    public string Value { get; set; }
}

[XmlRoot(ElementName = "EstablishmentName")]
public class EstablishmentName
{
    [XmlAttribute(AttributeName = "Value")]
    public string Value { get; set; }
}

[XmlRoot(ElementName = "Createdby")]
public class Createdby
{
    [XmlAttribute(AttributeName = "Value")]
    public string Value { get; set; }
}

[XmlRoot(ElementName = "CreationDate")]
public class CreationDate
{
    [XmlAttribute(AttributeName = "Value")]
    public string Value { get; set; }
}

[XmlRoot(ElementName = "XmlGroup")]
public class XmlGroup
{
    [XmlAttribute(AttributeName = "Value")]
    public string Value { get; set; }
}

[XmlRoot(ElementName = "XmlType")]
public class XmlType
{
    [XmlAttribute(AttributeName = "Value")]
    public string Value { get; set; }
}

[XmlRoot(ElementName = "XmlSubType")]
public class XmlSubType
{
    [XmlAttribute(AttributeName = "Value")]
    public string Value { get; set; }
}

[XmlRoot(ElementName = "HeaderName")]
public class HeaderName
{
    [XmlAttribute(AttributeName = "FileName")]
    public string FileName { get; set; }
    [XmlAttribute(AttributeName = "Version")]
    public string Version { get; set; }
}

[XmlRoot(ElementName = "Info")]
public class Info
{
    [XmlElement(ElementName = "LocationNo")]
    public LocationNo LocationNo { get; set; }
    [XmlElement(ElementName = "LocationName")]
    public LocationName LocationName { get; set; }
    [XmlElement(ElementName = "StoreNo")]
    public StoreNo StoreNo { get; set; }
    [XmlElement(ElementName = "StoreName")]
    public StoreName StoreName { get; set; }
    [XmlElement(ElementName = "EstablishmentNo")]
    public EstablishmentNo EstablishmentNo { get; set; }
    [XmlElement(ElementName = "EstablishmentName")]
    public EstablishmentName EstablishmentName { get; set; }
    [XmlElement(ElementName = "Createdby")]
    public Createdby Createdby { get; set; }
    [XmlElement(ElementName = "CreationDate")]
    public CreationDate CreationDate { get; set; }
    [XmlElement(ElementName = "XmlGroup")]
    public XmlGroup XmlGroup { get; set; }
    [XmlElement(ElementName = "XmlType")]
    public XmlType XmlType { get; set; }
    [XmlElement(ElementName = "XmlSubType")]
    public XmlSubType XmlSubType { get; set; }
    [XmlElement(ElementName = "HeaderName")]
    public HeaderName HeaderName { get; set; }
}

[XmlRoot(ElementName = "AreaTypeList")]
public class AreaTypeList
{
    [XmlElement(ElementName = "Type")]
    public List<string> Type { get; set; }
}

[XmlRoot(ElementName = "AreaTypeGroupList")]
public class AreaTypeGroupList
{
    [XmlElement(ElementName = "Group")]
    public List<string> Group { get; set; }
}

[XmlRoot(ElementName = "Space")]
public class Space
{
    [XmlElement(ElementName = "Name")]
    public string Name { get; set; }
    [XmlElement(ElementName = "Number")]
    public string Number { get; set; }
    [XmlElement(ElementName = "Pieces")]
    public string Pieces { get; set; }
    [XmlElement(ElementName = "Articles")]
    public string Articles { get; set; }
    [XmlElement(ElementName = "Unit")]
    public string Unit { get; set; }
    [XmlElement(ElementName = "Area")]
    public string Area { get; set; }
}

[XmlRoot(ElementName = "Spaces")]
public class Spaces
{
    [XmlElement(ElementName = "Space")]
    public List<Space> Space { get; set; }
}

[XmlRoot(ElementName = "AreaTypeGroups")]
public class AreaTypeGroups
{
    [XmlElement(ElementName = "AreaTypeGroup")]
    public List<string> AreaTypeGroup { get; set; }
}

[XmlRoot(ElementName = "AreaType")]
public class AreaType
{
    [XmlElement(ElementName = "Name")]
    public string Name { get; set; }
    [XmlElement(ElementName = "AreaTypeGroups")]
    public AreaTypeGroups AreaTypeGroups { get; set; }
}

[XmlRoot(ElementName = "Category")]
public class Category
{
    [XmlElement(ElementName = "Id")]
    public string Id { get; set; }
    [XmlElement(ElementName = "Name")]
    public string Name { get; set; }
    [XmlElement(ElementName = "Number")]
    public string Number { get; set; }
    [XmlElement(ElementName = "Spaces")]
    public Spaces Spaces { get; set; }
    [XmlElement(ElementName = "AreaType")]
    public AreaType AreaType { get; set; }
}

[XmlRoot(ElementName = "CadSpaces")]
public class CadSpaces
{
    [XmlElement(ElementName = "Category")]
    public List<Category> Category { get; set; }
}

[XmlRoot(ElementName = "Data")]
public class Data
{
    [XmlElement(ElementName = "AreaTypeList")]
    public AreaTypeList AreaTypeList { get; set; }
    [XmlElement(ElementName = "AreaTypeGroupList")]
    public AreaTypeGroupList AreaTypeGroupList { get; set; }
    [XmlElement(ElementName = "CadSpaces")]
    public CadSpaces CadSpaces { get; set; }
}

[XmlRoot(ElementName = "Message")]
public class Message
{
    [XmlElement(ElementName = "Info")]
    public Info Info { get; set; }
    [XmlElement(ElementName = "Data")]
    public Data Data { get; set; }
    [XmlAttribute(AttributeName = "xsi", Namespace = "http://www.w3.org/2000/xmlns/")]
    public string Xsi { get; set; }
    [XmlAttribute(AttributeName = "xsd", Namespace = "http://www.w3.org/2000/xmlns/")]
    public string Xsd { get; set; }
}

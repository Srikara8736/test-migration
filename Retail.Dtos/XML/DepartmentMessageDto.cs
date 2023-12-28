using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Retail.DTOs.XML;

[XmlRoot(ElementName = "Header")]
public class Header
{
    [XmlAttribute(AttributeName = "Name")]
    public string Name { get; set; }
    [XmlAttribute(AttributeName = "FileName")]
    public string FileName { get; set; }
    [XmlAttribute(AttributeName = "Version")]
    public string Version { get; set; }
    [XmlAttribute(AttributeName = "Createdby")]
    public string Createdby { get; set; }
    [XmlAttribute(AttributeName = "CreationDate")]
    public string CreationDate { get; set; }
}

[XmlRoot(ElementName = "Customer")]
public class DepartmentCustomer
{
    [XmlAttribute(AttributeName = "No")]
    public string No { get; set; }
    [XmlAttribute(AttributeName = "Name")]
    public string Name { get; set; }
    [XmlAttribute(AttributeName = "IsVisible")]
    public string IsVisible { get; set; }
}

[XmlRoot(ElementName = "Store")]
public class DepartmentStore
{
    [XmlAttribute(AttributeName = "No")]
    public string No { get; set; }
    [XmlAttribute(AttributeName = "Name")]
    public string Name { get; set; }
    [XmlAttribute(AttributeName = "IsVisible")]
    public string IsVisible { get; set; }
}

[XmlRoot(ElementName = "Project")]
public class Project
{
    [XmlAttribute(AttributeName = "No")]
    public string No { get; set; }
    [XmlAttribute(AttributeName = "Name")]
    public string Name { get; set; }
    [XmlAttribute(AttributeName = "IsVisible")]
    public string IsVisible { get; set; }
}

[XmlRoot(ElementName = "CADXml")]
public class CADXml
{
    [XmlAttribute(AttributeName = "Group")]
    public string Group { get; set; }
    [XmlAttribute(AttributeName = "Type")]
    public string Type { get; set; }
    [XmlAttribute(AttributeName = "SubType")]
    public string SubType { get; set; }
    [XmlAttribute(AttributeName = "IsVisible")]
    public string IsVisible { get; set; }
}

[XmlRoot(ElementName = "InteriorXml")]
public class InteriorXml
{
    [XmlAttribute(AttributeName = "Id")]
    public string Id { get; set; }
    [XmlAttribute(AttributeName = "Name")]
    public string Name { get; set; }
    [XmlAttribute(AttributeName = "IsVisible")]
    public string IsVisible { get; set; }
}



[XmlRoot(ElementName = "CadPackage")]
public class CadPackage
{
    [XmlAttribute(AttributeName = "Name")]
    public string Name { get; set; }
    [XmlAttribute(AttributeName = "IsVisible")]
    public string IsVisible { get; set; }
}

[XmlRoot(ElementName = "ExtendedData")]
public class ExtendedData
{
    [XmlAttribute(AttributeName = "Availablility")]
    public string Availablility { get; set; }
    [XmlAttribute(AttributeName = "IsVisible")]
    public string IsVisible { get; set; }
}

[XmlRoot(ElementName = "Version")]
public class Version
{
    [XmlAttribute(AttributeName = "IsVisible")]
    public string IsVisible { get; set; }
    [XmlText]
    public string Text { get; set; }
}

[XmlRoot(ElementName = "CADUniqueID")]
public class CADUniqueID
{
    [XmlAttribute(AttributeName = "IsVisible")]
    public string IsVisible { get; set; }
    [XmlText]
    public string Text { get; set; }
}

[XmlRoot(ElementName = "MandatoryProperties")]
public class MandatoryProperties
{
    [XmlElement(ElementName = "Header")]
    public Header Header { get; set; }
    [XmlElement(ElementName = "Customer")]
    public DepartmentCustomer Customer { get; set; }
    [XmlElement(ElementName = "Store")]
    public DepartmentStore Store { get; set; }
    [XmlElement(ElementName = "Project")]
    public Project Project { get; set; }
    [XmlElement(ElementName = "CADXml")]
    public CADXml CADXml { get; set; }
    [XmlElement(ElementName = "InteriorXml")]
    public InteriorXml InteriorXml { get; set; }
    [XmlElement(ElementName = "ArchicadFile")]
    public ArchicadFile ArchicadFile { get; set; }
    [XmlElement(ElementName = "CadPackage")]
    public CadPackage CadPackage { get; set; }
    [XmlElement(ElementName = "ExtendedData")]
    public ExtendedData ExtendedData { get; set; }
    [XmlElement(ElementName = "Version")]
    public Version Version { get; set; }
    [XmlElement(ElementName = "CADUniqueID")]
    public CADUniqueID CADUniqueID { get; set; }
    [XmlAttribute(AttributeName = "IsBold")]
    public string IsBold { get; set; }
    [XmlAttribute(AttributeName = "IsUnderscored")]
    public string IsUnderscored { get; set; }
}


[XmlRoot(ElementName = "MetaData")]
public class MetaData
{
    [XmlElement(ElementName = "MandatoryProperties")]
    public MandatoryProperties MandatoryProperties { get; set; }
    [XmlElement(ElementName = "DynamicProperties")]
    public DynamicProperties DynamicProperties { get; set; }
}



[XmlRoot(ElementName = "Message")]
public class DepartmentMessageDto
{
    [XmlElement(ElementName = "MetaData")]
    public MetaData MetaData { get; set; }
    [XmlElement(ElementName = "Data")]
    public Data Data { get; set; }
    [XmlAttribute(AttributeName = "xsi", Namespace = "http://www.w3.org/2000/xmlns/")]
    public string Xsi { get; set; }
    [XmlAttribute(AttributeName = "xsd", Namespace = "http://www.w3.org/2000/xmlns/")]
    public string Xsd { get; set; }
}

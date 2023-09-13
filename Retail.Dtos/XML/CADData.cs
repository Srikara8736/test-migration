using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retail.DTOs.XML;

/* 
 Licensed under the Apache License, Version 2.0

 http://www.apache.org/licenses/LICENSE-2.0
 */
using System;
using System.Xml.Serialization;
using System.Collections.Generic;

    [XmlRoot(ElementName = "FileHeaderStyle")]
    public class FileHeaderStyle
    {
        [XmlElement(ElementName = "IsBold")]
        public string IsBold { get; set; }
        [XmlElement(ElementName = "IsUnderscored")]
        public string IsUnderscored { get; set; }
    }

    [XmlRoot(ElementName = "FileHeader")]
    public class FileHeader
    {
        [XmlElement(ElementName = "FileHeaderText")]
        public string FileHeaderText { get; set; }
        [XmlElement(ElementName = "FileHeaderStyle")]
        public FileHeaderStyle FileHeaderStyle { get; set; }
    }

    [XmlRoot(ElementName = "CustomerNo")]
    public class CustomerNo
    {
        [XmlAttribute(AttributeName = "IsVisible")]
        public string IsVisible { get; set; }
        [XmlAttribute(AttributeName = "Value")]
        public string Value { get; set; }
    }

    [XmlRoot(ElementName = "CustomerName")]
    public class CustomerName
    {
        [XmlAttribute(AttributeName = "IsVisible")]
        public string IsVisible { get; set; }
        [XmlAttribute(AttributeName = "Value")]
        public string Value { get; set; }
    }

    [XmlRoot(ElementName = "ProjectNo")]
    public class ProjectNo
    {
        [XmlAttribute(AttributeName = "IsVisible")]
        public string IsVisible { get; set; }
        [XmlAttribute(AttributeName = "Value")]
        public string Value { get; set; }
    }

    [XmlRoot(ElementName = "ProjectName")]
    public class ProjectName
    {
        [XmlAttribute(AttributeName = "IsVisible")]
        public string IsVisible { get; set; }
        [XmlAttribute(AttributeName = "Value")]
        public string Value { get; set; }
    }

    [XmlRoot(ElementName = "InteriorXmlId")]
    public class InteriorXmlId
    {
        [XmlAttribute(AttributeName = "IsVisible")]
        public string IsVisible { get; set; }
        [XmlAttribute(AttributeName = "Value")]
        public string Value { get; set; }
    }

    [XmlRoot(ElementName = "InteriorXmlName")]
    public class InteriorXmlName
    {
        [XmlAttribute(AttributeName = "IsVisible")]
        public string IsVisible { get; set; }
        [XmlAttribute(AttributeName = "Value")]
        public string Value { get; set; }
    }

    [XmlRoot(ElementName = "Createdby")]
    public class CadCreatedby
    {
        [XmlAttribute(AttributeName = "IsVisible")]
        public string IsVisible { get; set; }
        [XmlAttribute(AttributeName = "Value")]
        public string Value { get; set; }
    }

    [XmlRoot(ElementName = "CreationDate")]
    public class CadCreationDate
    {
        [XmlAttribute(AttributeName = "IsVisible")]
        public string IsVisible { get; set; }
        [XmlAttribute(AttributeName = "Value")]
        public string Value { get; set; }
    }

    [XmlRoot(ElementName = "ArchicadFile")]
    public class ArchicadFile
    {
        [XmlAttribute(AttributeName = "IsVisible")]
        public string IsVisible { get; set; }
        [XmlAttribute(AttributeName = "Value")]
        public string Value { get; set; }
    }

    [XmlRoot(ElementName = "CadPackageName")]
    public class CadPackageName
    {
        [XmlAttribute(AttributeName = "IsVisible")]
        public string IsVisible { get; set; }
        [XmlAttribute(AttributeName = "Value")]
        public string Value { get; set; }
    }

    [XmlRoot(ElementName = "ExtendedDataAvailable")]
    public class ExtendedDataAvailable
    {
        [XmlAttribute(AttributeName = "IsVisible")]
        public string IsVisible { get; set; }
        [XmlAttribute(AttributeName = "Value")]
        public string Value { get; set; }
    }

    [XmlRoot(ElementName = "MandatoryPoperties")]
    public class MandatoryPoperties
    {
        [XmlElement(ElementName = "CustomerNo")]
        public CustomerNo CustomerNo { get; set; }
        [XmlElement(ElementName = "CustomerName")]
        public CustomerName CustomerName { get; set; }
        [XmlElement(ElementName = "ProjectNo")]
        public ProjectNo ProjectNo { get; set; }
        [XmlElement(ElementName = "ProjectName")]
        public ProjectName ProjectName { get; set; }
        [XmlElement(ElementName = "InteriorXmlId")]
        public InteriorXmlId InteriorXmlId { get; set; }
        [XmlElement(ElementName = "InteriorXmlName")]
        public InteriorXmlName InteriorXmlName { get; set; }
        [XmlElement(ElementName = "Createdby")]
        public CadCreatedby Createdby { get; set; }
        [XmlElement(ElementName = "CreationDate")]
        public CadCreationDate CreationDate { get; set; }
        [XmlElement(ElementName = "ArchicadFile")]
        public ArchicadFile ArchicadFile { get; set; }
        [XmlElement(ElementName = "CadPackageName")]
        public CadPackageName CadPackageName { get; set; }
        [XmlElement(ElementName = "ExtendedDataAvailable")]
        public ExtendedDataAvailable ExtendedDataAvailable { get; set; }
        [XmlAttribute(AttributeName = "IsBold")]
        public string IsBold { get; set; }
        [XmlAttribute(AttributeName = "IsUnderscored")]
        public string IsUnderscored { get; set; }
    }

    [XmlRoot(ElementName = "Property")]
    public class Property
    {
        [XmlAttribute(AttributeName = "Name")]
        public string Name { get; set; }
        [XmlAttribute(AttributeName = "Value")]
        public string Value { get; set; }
    }

    [XmlRoot(ElementName = "DynamicProperties")]
    public class DynamicProperties
    {
        [XmlElement(ElementName = "Property")]
        public Property Property { get; set; }
        [XmlAttribute(AttributeName = "IsBold")]
        public string IsBold { get; set; }
        [XmlAttribute(AttributeName = "IsUnderscored")]
        public string IsUnderscored { get; set; }
    }

    [XmlRoot(ElementName = "Properties")]
    public class Properties
    {
        [XmlElement(ElementName = "PropertyName")]
        public string PropertyName { get; set; }
        [XmlElement(ElementName = "PropertyValue")]
        public string PropertyValue { get; set; }
    }

    [XmlRoot(ElementName = "MessageData")]
    public class MessageData
    {
        [XmlElement(ElementName = "Properties")]
        public List<Properties> Properties { get; set; }
    }

    [XmlRoot(ElementName = "Messages")]
    public class Messages
    {
        [XmlElement(ElementName = "MessageData")]
        public MessageData MessageData { get; set; }
    }

    [XmlRoot(ElementName = "MessageBlock")]
    public class MessageBlock
    {
        [XmlElement(ElementName = "MandatoryPoperties")]
        public MandatoryPoperties MandatoryPoperties { get; set; }
        [XmlElement(ElementName = "DynamicProperties")]
        public DynamicProperties DynamicProperties { get; set; }
        [XmlElement(ElementName = "Messages")]
        public Messages Messages { get; set; }
        [XmlElement(ElementName = "RelatedDocuments")]
        public string RelatedDocuments { get; set; }
        [XmlAttribute(AttributeName = "IsBold")]
        public string IsBold { get; set; }
        [XmlAttribute(AttributeName = "IsUnderscored")]
        public string IsUnderscored { get; set; }
    }

    [XmlRoot(ElementName = "CADData")]
    public class CADData
    {
        [XmlElement(ElementName = "Version")]
        public string Version { get; set; }
        [XmlElement(ElementName = "FileHeader")]
        public FileHeader FileHeader { get; set; }
        [XmlElement(ElementName = "MessageBlock")]
        public MessageBlock MessageBlock { get; set; }
        [XmlAttribute(AttributeName = "xsi", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Xsi { get; set; }
        [XmlAttribute(AttributeName = "xsd", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Xsd { get; set; }
    }



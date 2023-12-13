using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Retail.DTOs.XML;

[XmlRoot(ElementName = "FileHeaderStyle")]
public class OrderFileHeaderStyle
{
    [XmlElement(ElementName = "IsBold")]
    public string IsBold { get; set; }
    [XmlElement(ElementName = "IsUnderscored")]
    public string IsUnderscored { get; set; }
}

[XmlRoot(ElementName = "FileHeader")]
public class OrderFileHeader
{
    [XmlElement(ElementName = "FileHeaderText")]
    public string FileHeaderText { get; set; }
    [XmlElement(ElementName = "FileHeaderStyle")]
    public OrderFileHeaderStyle FileHeaderStyle { get; set; }
}

[XmlRoot(ElementName = "CustomerNo")]
public class OrderCustomerNo
{
    [XmlAttribute(AttributeName = "IsVisible")]
    public string IsVisible { get; set; }
    [XmlAttribute(AttributeName = "Value")]
    public string Value { get; set; }
}

[XmlRoot(ElementName = "CustomerName")]
public class OrderCustomerName
{
    [XmlAttribute(AttributeName = "IsVisible")]
    public string IsVisible { get; set; }
    [XmlAttribute(AttributeName = "Value")]
    public string Value { get; set; }
}

[XmlRoot(ElementName = "ProjectNo")]
public class OrderProjectNo
{
    [XmlAttribute(AttributeName = "IsVisible")]
    public string IsVisible { get; set; }
    [XmlAttribute(AttributeName = "Value")]
    public string Value { get; set; }
}

[XmlRoot(ElementName = "ProjectName")]
public class OrderProjectName
{
    [XmlAttribute(AttributeName = "IsVisible")]
    public string IsVisible { get; set; }
    [XmlAttribute(AttributeName = "Value")]
    public string Value { get; set; }
}

[XmlRoot(ElementName = "InteriorXmlId")]
public class OrderInteriorXmlId
{
    [XmlAttribute(AttributeName = "IsVisible")]
    public string IsVisible { get; set; }
    [XmlAttribute(AttributeName = "Value")]
    public string Value { get; set; }
}

[XmlRoot(ElementName = "InteriorXmlName")]
public class OrderInteriorXmlName
{
    [XmlAttribute(AttributeName = "IsVisible")]
    public string IsVisible { get; set; }
    [XmlAttribute(AttributeName = "Value")]
    public string Value { get; set; }
}

[XmlRoot(ElementName = "Createdby")]
public class OrderCreatedby
{
    [XmlAttribute(AttributeName = "IsVisible")]
    public string IsVisible { get; set; }
    [XmlAttribute(AttributeName = "Value")]
    public string Value { get; set; }
}

[XmlRoot(ElementName = "CreationDate")]
public class OrderCreationDate
{
    [XmlAttribute(AttributeName = "IsVisible")]
    public string IsVisible { get; set; }
    [XmlAttribute(AttributeName = "Value")]
    public string Value { get; set; }
}

[XmlRoot(ElementName = "ArchicadFile")]
public class OrderArchicadFile
{
    [XmlAttribute(AttributeName = "IsVisible")]
    public string IsVisible { get; set; }
    [XmlAttribute(AttributeName = "Value")]
    public string Value { get; set; }
}

[XmlRoot(ElementName = "CadPackageName")]
public class OrderCadPackageName
{
    [XmlAttribute(AttributeName = "IsVisible")]
    public string IsVisible { get; set; }
    [XmlAttribute(AttributeName = "Value")]
    public string Value { get; set; }
}

[XmlRoot(ElementName = "ExtendedDataAvailable")]
public class OrderExtendedDataAvailable
{
    [XmlAttribute(AttributeName = "IsVisible")]
    public string IsVisible { get; set; }
    [XmlAttribute(AttributeName = "Value")]
    public string Value { get; set; }
}

[XmlRoot(ElementName = "MandatoryPoperties")]
public class OrderMandatoryPoperties
{
    [XmlElement(ElementName = "CustomerNo")]
    public OrderCustomerNo CustomerNo { get; set; }
    [XmlElement(ElementName = "CustomerName")]
    public OrderCustomerName CustomerName { get; set; }
    [XmlElement(ElementName = "ProjectNo")]
    public OrderProjectNo ProjectNo { get; set; }
    [XmlElement(ElementName = "ProjectName")]
    public OrderProjectName ProjectName { get; set; }
    [XmlElement(ElementName = "InteriorXmlId")]
    public OrderInteriorXmlId InteriorXmlId { get; set; }
    [XmlElement(ElementName = "InteriorXmlName")]
    public OrderInteriorXmlName InteriorXmlName { get; set; }
    [XmlElement(ElementName = "Createdby")]
    public OrderCreatedby Createdby { get; set; }
    [XmlElement(ElementName = "CreationDate")]
    public OrderCreationDate CreationDate { get; set; }
    [XmlElement(ElementName = "ArchicadFile")]
    public OrderArchicadFile ArchicadFile { get; set; }
    [XmlElement(ElementName = "CadPackageName")]
    public OrderCadPackageName CadPackageName { get; set; }
    [XmlElement(ElementName = "ExtendedDataAvailable")]
    public OrderExtendedDataAvailable ExtendedDataAvailable { get; set; }
    [XmlAttribute(AttributeName = "IsBold")]
    public string IsBold { get; set; }
    [XmlAttribute(AttributeName = "IsUnderscored")]
    public string IsUnderscored { get; set; }
}

[XmlRoot(ElementName = "Property")]
public class OrderProperty
{
    [XmlAttribute(AttributeName = "Name")]
    public string Name { get; set; }
    [XmlAttribute(AttributeName = "Value")]
    public string Value { get; set; }
}

[XmlRoot(ElementName = "DynamicProperties")]
public class OrderDynamicProperties
{
    [XmlElement(ElementName = "Property")]
    public OrderProperty Property { get; set; }
    [XmlAttribute(AttributeName = "IsBold")]
    public string IsBold { get; set; }
    [XmlAttribute(AttributeName = "IsUnderscored")]
    public string IsUnderscored { get; set; }
}

[XmlRoot(ElementName = "Properties")]
public class OrderProperties
{
    [XmlElement(ElementName = "PropertyName")]
    public string PropertyName { get; set; }
    [XmlElement(ElementName = "PropertyValue")]
    public string PropertyValue { get; set; }
    [XmlAttribute(AttributeName = "IsVisible")]
    public string IsVisible { get; set; }
    [XmlAttribute(AttributeName = "Unit")]
    public string Unit { get; set; }
}

[XmlRoot(ElementName = "MessageData")]
public class OrderMessageData
{
    [XmlElement(ElementName = "Properties")]
    public List<OrderProperties> Properties { get; set; }
}

[XmlRoot(ElementName = "Messages")]
public class OrderMessages
{
    [XmlElement(ElementName = "MessageData")]
    public List<OrderMessageData> MessageData { get; set; }
}

[XmlRoot(ElementName = "MessageBlock")]
public class OrderMessageBlock
{
    [XmlElement(ElementName = "MandatoryPoperties")]
    public OrderMandatoryPoperties MandatoryPoperties { get; set; }
    [XmlElement(ElementName = "DynamicProperties")]
    public OrderDynamicProperties DynamicProperties { get; set; }
    [XmlElement(ElementName = "Messages")]
    public OrderMessages Messages { get; set; }
    [XmlElement(ElementName = "RelatedDocuments")]
    public string RelatedDocuments { get; set; }
    [XmlAttribute(AttributeName = "IsBold")]
    public string IsBold { get; set; }
    [XmlAttribute(AttributeName = "IsUnderscored")]
    public string IsUnderscored { get; set; }
}

[XmlRoot(ElementName = "CADData")]
public class CADOrder
{
    [XmlElement(ElementName = "Version")]
    public string Version { get; set; }
    [XmlElement(ElementName = "FileHeader")]
    public OrderFileHeader FileHeader { get; set; }
    [XmlElement(ElementName = "MessageBlock")]
    public OrderMessageBlock MessageBlock { get; set; }
    [XmlAttribute(AttributeName = "xsi", Namespace = "http://www.w3.org/2000/xmlns/")]
    public string Xsi { get; set; }
    [XmlAttribute(AttributeName = "xsd", Namespace = "http://www.w3.org/2000/xmlns/")]
    public string Xsd { get; set; }
}

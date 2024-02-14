namespace Retail.DTOs.Stores;

public class StoreDataHistoryDto
{
    public string Name { get; set; }
    public string VersionNumber { get; set; }
    public Guid StoreId { get; set;}

    public Guid StoreDataId { get; set;}

    public string Status { get; set;}
    public Guid StatusId { get; set;}
    public string UploadType { get; set;}
    public Guid UploadTypeId { get; set;}

    public List<DocumentHistoryDto> documentHistories { get; set; } = new();
}

public class DocumentHistoryDto
{
    public Guid StoreDocumentId { get; set;}

    public Guid DocumentId { get; set;}

    public string FileType { get; set;}

    public Guid FileTypeId { get; set;}

    public string Path { get; set;}
}

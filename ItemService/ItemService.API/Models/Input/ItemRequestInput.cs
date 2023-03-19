namespace ItemService.API.Models.Input;

public class ItemRequestInput
{
    public Guid ItemId { get; set; }
    public bool GetImages { get; set; }
    public bool GetExtraInformation { get; set; }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemService.API.Models.Input;

public class ItemRequestInput
{
    public Guid ItemId { get; set; }
    public bool GetImages { get; set; }
    public bool GetExtraInformation { get; set; }

}

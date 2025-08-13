using MinimalAPI_Pages.Models;

namespace MinimalAPI_Pages.Requests
{
    /// <summary>
    /// ItemRequest is used to serialise an ItemModel without a primary key. Used for formatting json requests with required fields.
    /// </summary>
    public class ItemRequest
    {
        public string Description { get; set; }
        public float Price { get; set; }
        public long SupplierID { get; set; }

        /// <summary>
        /// Constructor to turn an ItemModel into an ItemRequest
        /// </summary>
        /// <param name="model"></param>
        public ItemRequest(ItemModel model)
        {
            Description = model.Description;
            Price = model.Price;
            SupplierID = model.SupplierID;
        }
    }
}

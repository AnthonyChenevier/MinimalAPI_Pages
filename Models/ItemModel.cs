using MinimalAPI_Pages.Requests;
using Supabase.Postgrest.Models;

namespace MinimalAPI_Pages.Models
{
    [Supabase.Postgrest.Attributes.Table("items")]
    public class ItemModel : BaseModel
    {
        [Supabase.Postgrest.Attributes.PrimaryKey("item_id")]
        public long ItemID { get; init; }

        [Supabase.Postgrest.Attributes.Column("description")]
        public string Description { get; init; }

        [Supabase.Postgrest.Attributes.Column("price")]
        public float Price { get; init; }

        [System.ComponentModel.DataAnnotations.Schema.ForeignKey("supplier_id"), Supabase.Postgrest.Attributes.Column("supplier_id")]
        public long SupplierID { get; init; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public ItemModel()
        {
            ItemID = 0;
            Description = "";
            Price = 0;
            SupplierID = 0;
        }

        /// <summary>
        /// Constructor for all model properties
        /// </summary>
        /// <param name="itemID"></param>
        /// <param name="description"></param>
        /// <param name="price"></param>
        /// <param name="supplierID"></param>
        public ItemModel(long itemID, string description, float price, long supplierID)
        {
            ItemID = itemID;
            Description = description;
            Price = price;
            SupplierID = supplierID;
        }

        /// <summary>
        /// ItemRequest to ItemModel Constructor. Does not set ItemID
        /// </summary>
        /// <param name="request"></param>
        public ItemModel(ItemRequest request) : this(0, request.Description, request.Price, request.SupplierID) { }

        /// <summary>
        /// ItemId and ItemRequest to ItemModel Constructor.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        public ItemModel(long id, ItemRequest request) : this(id, request.Description, request.Price, request.SupplierID) { }
    }
}

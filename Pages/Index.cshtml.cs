using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MinimalAPI_Pages.Controllers;
using MinimalAPI_Pages.Models;
using MinimalAPI_Pages.Requests;
using Supabase;

namespace MinimalAPI_Pages.Pages
{
    public class ItemsCrudPageModel : PageModel
    {
        private readonly ItemsController _itemsController;

        public ItemsCrudPageModel(ItemsController itemsController) { _itemsController = itemsController; }

        public IEnumerable<ItemModel> Items { get; set; }
        public ItemRequest NewItem { get; set; } = new();
        public ItemModel EditItem { get; set; } = new();

        public async Task OnGetAsync()
        {
            Client client = HttpContext.RequestServices.GetService<Client>();

            IActionResult response = await _itemsController.OnGetAsync(1, client);

            Items = (response as OkObjectResult).Value as IEnumerable<ItemModel> ?? [];
        }

        public async Task<IActionResult> OnPostCreateAsync(ItemRequest request)
        {
            Client client = HttpContext.RequestServices.GetService<Client>();

            IActionResult response = await _itemsController.OnPostAsync(request, client);

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostEditAsync(long id)
        {
            Client client = HttpContext.RequestServices.GetService<Client>();

            IActionResult response = await _itemsController.OnGetAsync(id, client);

            EditItem = (response as OkObjectResult)?.Value as ItemModel;

            return Page();
        }

        public async Task<IActionResult> OnPostUpdateAsync(ItemModel item)
        {
            Client client = HttpContext.RequestServices.GetService<Client>();

            IActionResult response = await _itemsController.OnPutAsync(item.ItemID, new ItemRequest(item), client);

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAsync(long id)
        {
            Client client = HttpContext.RequestServices.GetService<Client>();

            await _itemsController.OnDeleteAsync(id, client);

            return RedirectToPage();
        }
    }
}
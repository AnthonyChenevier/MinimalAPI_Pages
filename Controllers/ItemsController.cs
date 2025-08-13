using Microsoft.AspNetCore.Mvc;
using MinimalAPI_Pages.Models;
using MinimalAPI_Pages.Requests;
using Supabase;
using Supabase.Postgrest.Responses;

namespace MinimalAPI_Pages.Controllers;

[ApiController]
[Route("[controller]")]
public class ItemsController : ControllerBase
{
    // READ
    [HttpGet("{id}")]
    public async Task<IActionResult> OnGetAsync(int id, [FromServices] Client client)
    {
        ModeledResponse<ItemModel> response = await client.From<ItemModel>().Where(i => i.ItemID == id).Get();

        ItemModel? item = response.Models.FirstOrDefault();

        if (item is null)
            return NotFound();

        return Ok(item);
    }

    // CREATE
    [HttpPost]
    public async Task<IActionResult> OnPostAsync([FromBody] ItemRequest request, [FromServices] Client client)
    {
        ItemModel item = new ItemModel(request);

        ModeledResponse<ItemModel> response = await client.From<ItemModel>().Insert(item);

        ItemModel newItem = response.Models.First();

        return Ok(newItem);
    }

    // UPDATE
    [HttpPut("{id}")]
    public async Task<IActionResult> OnPutAsync(long id, [FromBody] ItemRequest request, [FromServices] Client client)
    {
        ItemModel item = new ItemModel(id, request);

        ModeledResponse<ItemModel> response = await client.From<ItemModel>().Where(i => i.ItemID == id).Update(item);

        ItemModel? updatedItemResponse = response.Models.FirstOrDefault();

        if (updatedItemResponse is null)
            return NotFound();

        return Ok(updatedItemResponse);
    }
    
    // DELETE
    [HttpDelete]
    public async Task<IActionResult> OnDeleteAsync(long id, [FromServices] Client client)
    {
        await client.From<ItemModel>().Where(i => i.ItemID == id).Delete();

        return NoContent();
    }
}
using Microsoft.AspNetCore.Http.HttpResults;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class TrnAssetController : ControllerBase
{
    private readonly AppDbContext _context;

    public TrnAssetController(AppDbContext context)
    {
        _context = context;
    }

    // Get hardware by ASSETCODE and include the employee information
    [HttpGet("{ASSETCODE}")]
    public async Task<ActionResult<TRNASSETMODEL>> GetTrnAssetById(string ASSETCODE)
    {
        var trndtlAsset = await _context.TRN_ASSET
            // .Include(x => x.EMPLOYEE) // Eagerly load the related employee information
            .Where(x => x.ASSETCODE == ASSETCODE)
            .FirstOrDefaultAsync(x => x.ASSETCODE == ASSETCODE);

        if (trndtlAsset == null)
        {
            return Ok("Asset not found");
        }

        return Ok(trndtlAsset);
    }

    [HttpGet]
    public async Task<ActionResult<List<TRNASSETMODEL>>> GetAllAsset()
    {
        var trnAsset = await _context.TRN_ASSET
            // .Include(x => x.EMPLOYEE)
            .OrderByDescending(x => x.ADDEDDATE)
            .ToListAsync();

        return Ok(trnAsset);
    }

    // Get all hardware
    [HttpGet("search")]
    public async Task<ActionResult<List<TRNASSETMODEL>>> GetSearchAsset([FromQuery] string? search = null, [FromQuery] int page = 1, [FromQuery] int pageSize = 7)
    {
        var query = _context.TRN_ASSET
            // .Include(x => x.EMPLOYEE)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            search = search.ToLower();
            query = query.Where(x => (x.ASSETBRAND + " " + x.ASSETMODEL + " " + x.ASSETSERIES).ToLower().Contains(search) ||
                                    x.ASSETCATEGORY.ToLower().Contains(search) ||
                                    x.ASSETSERIALNUMBER.ToLower().Contains(search) ||
                                    x.ASSETTYPE.ToLower().Contains(search) ||
                                    x.CONDITION.ToLower().Contains(search) || 
                                    x.ASSETCODE.ToLower().Contains(search));
        }

        var transAsset = await query.ToListAsync();

        // Return the results
        return Ok(transAsset);
    }

    // [HttpPost]
    // public async Task<ActionResult<TRNASSETMODEL>> PostTrnAsset([FromBody] TRNASSETMODEL trnAsset)
    // {   
    //     //Auto increment MaxIdAsset 
    //     var maxIDAsset = await _context.TRN_ASSET
    //         .MaxAsync(e => (int?)e.IDASSET) ?? 0;
    //     trnAsset.IDASSET = maxIDAsset + 1;

    //     // Fetch category mapping from MST_GCM
    //     var categoryMapping = await _context.MST_GCM
    //     .FirstOrDefaultAsync(x => x.TYPEGCM == trnAsset.ASSETCATEGORY);

    //     //Auto set Date
    //     trnAsset.ADDEDDATE = DateOnly.FromDateTime(DateTime.Now);

    //     trnAsset.ACTIVE = "y";
    //     trnAsset.CONDITION = "GREAT";

    //     var assetCategoryInitial = trnAsset.ASSETCATEGORY[..1].ToUpper(); // First letter of ASSETCATEGORY
    //     string addedDateStr = trnAsset.ADDEDDATE.ToString("yyMM"); // Format ADDEDDATE as YYYYMMDD

    //     // Get the last asset created in the current month
    //     var lastAsset = await _context.TRN_ASSET
    //         .Where(e => e.ADDEDDATE.Year == trnAsset.ADDEDDATE.Year &&
    //          e.ADDEDDATE.Month == trnAsset.ADDEDDATE.Month && 
    //          e.ASSETCATEGORY == trnAsset.ASSETCATEGORY && 
    //          e.ASSETTYPE == trnAsset.ASSETTYPE)
    //         .OrderByDescending(e => e.IDASSET)
    //         .FirstOrDefaultAsync();


    //     int counter;
    //     if (lastAsset != null)
    //     {
    //         counter = int.Parse(lastAsset.ASSETCODE.Substring(lastAsset.ASSETCODE.Length - 3)) + 1;
    //     }
    //     else
    //     {
    //         counter = 1;
    //     }

    //     string formattedCounter = counter.ToString("D3");

    //     trnAsset.ASSETCODE = $"{trnAsset.ASSETTYPE}{assetCategoryInitial}{addedDateStr}{formattedCounter}";


    //     // Add Asset
    //     _context.TRN_ASSET.Add(trnAsset);
    //     await _context.SaveChangesAsync();

    //     return CreatedAtAction("GetAllAsset", new { id = trnAsset.IDASSET }, trnAsset);
    // }

    [HttpPost]
    public async Task<ActionResult<TRNASSETMODEL>> PostTrnAsset([FromBody] TRNASSETMODEL trnAsset)
    {
        // Auto increment MaxIdAsset
        var maxIDAsset = await _context.TRN_ASSET
            .MaxAsync(e => (int?)e.IDASSET) ?? 0;
        trnAsset.IDASSET = maxIDAsset + 1;

        // Fetch category mapping from MST_GCM
        var categoryMapping = await _context.MST_GCM
            .FirstOrDefaultAsync(x => x.TYPEGCM == trnAsset.ASSETCATEGORY && x.CONDITION == "ASSET_CATEGORY_MAPING");

        // Validate mapping exists
        if (categoryMapping == null)
        {
            return BadRequest($"Category '{trnAsset.ASSETCATEGORY}' is not mapped in the master data (ASSET_CATEGORY_MAPING).");
        }

        // Use the mapped code from MST_GCM
        string assetCategoryCode = categoryMapping.DESCRIPTION;

        // Auto set Date
        trnAsset.ADDEDDATE = DateOnly.FromDateTime(DateTime.Now);
        trnAsset.ACTIVE = "y";
        trnAsset.CONDITION = "GREAT";

        // Generate Asset Code
        string addedDateStr = trnAsset.ADDEDDATE.ToString("yyMM"); // Format ADDEDDATE as YYMM

        // Get the last asset created in the current month
        var lastAsset = await _context.TRN_ASSET
            .Where(e => e.ADDEDDATE.Year == trnAsset.ADDEDDATE.Year &&
                        e.ADDEDDATE.Month == trnAsset.ADDEDDATE.Month &&
                        e.ASSETCATEGORY == trnAsset.ASSETCATEGORY &&
                        e.ASSETTYPE == trnAsset.ASSETTYPE)
            .OrderByDescending(e => e.IDASSET)
            .FirstOrDefaultAsync();

        int counter;
        if (lastAsset != null)
        {
            counter = int.Parse(lastAsset.ASSETCODE.Substring(lastAsset.ASSETCODE.Length - 3)) + 1;
        }
        else
        {
            counter = 1;
        }

        string formattedCounter = counter.ToString("D3");

        // Combine parts to generate final ASSETCODE
        trnAsset.ASSETCODE = $"{trnAsset.ASSETTYPE}{assetCategoryCode}{addedDateStr}{formattedCounter}";

        // Add Asset
        _context.TRN_ASSET.Add(trnAsset);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetAllAsset", new { id = trnAsset.IDASSET }, trnAsset);
    }


    [HttpPut("update-nipp/{ASSETCODE}")]
    public async Task<IActionResult> UpdateNipp(string ASSETCODE, [FromBody] int? newNipp)
    {
        var existingAsset = await _context.TRN_ASSET
            .FirstOrDefaultAsync(x => x.ASSETCODE == ASSETCODE);

        if (existingAsset == null)
        {
            return NotFound("Asset not found");
        }

        existingAsset.NIPP = newNipp;
        existingAsset.DATEUPDATED = DateOnly.FromDateTime(DateTime.Now);

        _context.Entry(existingAsset).State = EntityState.Modified;
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!TrnAssetExists(ASSETCODE))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return Ok(existingAsset);
    }

    [HttpPut("{ASSETCODE}")]
    public async Task<IActionResult> PutTrnAsset(string ASSETCODE, [FromBody] TRNASSETMODEL trnAsset)
    {
        var existingAsset = await _context.TRN_ASSET
            .FirstOrDefaultAsync(x => x.ASSETCODE == ASSETCODE);

        if (existingAsset == null)
        {
            return NotFound("Asset not found");
        }

        // Update properties explicitly
        existingAsset.ASSETBRAND = trnAsset.ASSETBRAND;
        existingAsset.ASSETMODEL = trnAsset.ASSETMODEL;
        existingAsset.ASSETSERIES = trnAsset.ASSETSERIES;
        existingAsset.ASSETSERIALNUMBER = trnAsset.ASSETSERIALNUMBER;
        existingAsset.CONDITION = trnAsset.CONDITION;
        existingAsset.DATEUPDATED = DateOnly.FromDateTime(DateTime.Now);
        existingAsset.PICUPDATED = trnAsset.PICUPDATED;
        existingAsset.ACTIVE = trnAsset.ACTIVE;
        
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!TrnAssetExists(ASSETCODE))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return Ok(trnAsset);
    }

    private bool TrnAssetExists(string ASSETCODE)
    {
        return _context.TRN_ASSET.Any(e => e.ASSETCODE == ASSETCODE);
    }
}
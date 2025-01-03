using System.Net;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]

public class AssetHistoryController : ControllerBase{
    private readonly AppDbContext _context;

    public AssetHistoryController(AppDbContext context){
        _context = context;
    }

    [HttpGet("{ASSETCODE}")]
    public async Task<ActionResult<List<TRNASSETHISTORYMODEL>>> GetAssetHistory(string ASSETCODE){
        var assetHistory = await _context.TRN_HIST_ASSET
            // .Include(x => x.EMPLOYEE) // Eagerly load the related employee information
            .Where(x => x.ASSETCODE == ASSETCODE)
            .OrderByDescending(x => x.DATEADDED) // Order the list in descending order
            .ToListAsync();

        if (assetHistory == null || !assetHistory.Any())
        {
            return Ok("Asset not found");
        }
        
        return Ok(assetHistory);
    }

    [HttpPost]
    public async Task<ActionResult<TRNASSETHISTORYMODEL>> PostAssetHistory(TRNASSETHISTORYMODEL assetHistory){

        // assetHistory.IDASSETHISTORY = _context.TRN_HIST_ASSET.Max(x => x.IDASSETHISTORY) + 1;

        var maxIdAssetHistory = await _context.TRN_HIST_ASSET
            .MaxAsync(e => (int?)e.IDASSETHISTORY) ?? 0;
        assetHistory.IDASSETHISTORY = maxIdAssetHistory + 1;

        // assetHistory.PICADDED = "Dava";
        assetHistory.DATEADDED = DateOnly.FromDateTime(DateTime.Now);
        // assetHistory.EMPLOYEE = null;
        assetHistory.TRNASSET = null;

        _context.TRN_HIST_ASSET.Add(assetHistory);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetAssetHistory", new { ASSETCODE = assetHistory.ASSETCODE }, assetHistory);
    }
    
    [HttpPut("{IDASSETHISTORY}")]
    public async Task<IActionResult> PutAssetHistory(int IDASSETHISTORY, TRNASSETHISTORYMODEL assetHistory){
        if (IDASSETHISTORY != assetHistory.IDASSETHISTORY){
            return BadRequest();
        }

        _context.Entry(assetHistory).State = EntityState.Modified;

        try{
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException){
            if (!AssetHistoryExists(IDASSETHISTORY)){
                return NotFound();
            }
            else{
                throw;
            }
        }

        return NoContent();
    }
    private bool AssetHistoryExists(int IDASSETHISTORY){
        return _context.TRN_HIST_ASSET.Any(e => e.IDASSETHISTORY == IDASSETHISTORY);
    }

}
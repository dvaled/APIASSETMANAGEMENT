using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]

public class TrnSoftwareController : ControllerBase{
    private readonly AppDbContext _context;
    public TrnSoftwareController(AppDbContext context){
        _context = context;
    }

    [HttpGet("{ASSETCODE}")]
    public async Task<ActionResult<List<TRNSOFTWAREMODEL>>> GetTrnSoftware(string ASSETCODE){
        var trnSoftware = await _context.TRN_DTL_SOFTWARE.Where(e => e.ASSETCODE == ASSETCODE).ToListAsync();

        if (trnSoftware == null){
            return Ok("No Software Data Available");
        }

        return Ok(trnSoftware);
    }   

    [HttpPost]
    public async Task<ActionResult<TRNSOFTWAREMODEL>> PostTrnSoftware(TRNSOFTWAREMODEL trnSoftware){
        //Auto increment MaxIdAsset 
        var maxIDSoftware = await _context.TRN_DTL_SOFTWARE
            .MaxAsync(e => (int?)e.IDASSETSOFTWARE) ?? 0;
        trnSoftware.IDASSETSOFTWARE = maxIDSoftware + 1;


        trnSoftware.ACTIVE = "Y";
        trnSoftware.DATEADDED = DateOnly.FromDateTime(DateTime.Now);
        trnSoftware.DATEUPDATED = null;
        trnSoftware.PICUPDATED = null;
        trnSoftware.TRNASSET = null;

        _context.TRN_DTL_SOFTWARE.Add(trnSoftware);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetTrnSoftware", new { assetcode = trnSoftware.ASSETCODE }, trnSoftware);
    }

    [HttpPut("{IDASSETSOFTWARE:int}")]
    public async Task<ActionResult<TRNSOFTWAREMODEL>> PutTrnSoftware(int IDASSETSOFTWARE, TRNSOFTWAREMODEL trnSoftware){

        if(IDASSETSOFTWARE != trnSoftware.IDASSETSOFTWARE){
            return BadRequest();
        }

        var existingSoftware = await _context.TRN_DTL_SOFTWARE.FindAsync(IDASSETSOFTWARE);
        if (existingSoftware == null)
        {
            return NotFound();
        }

        
        trnSoftware.DATEADDED = existingSoftware.DATEADDED;
        trnSoftware.PICADDED = existingSoftware.PICADDED;
        trnSoftware.DATEUPDATED = DateOnly.FromDateTime(DateTime.Now);

        _context.Entry(existingSoftware).CurrentValues.SetValues(trnSoftware);
        _context.Entry(existingSoftware).Property(x => x.DATEADDED).IsModified = false;
        _context.Entry(existingSoftware).Property(x => x.PICADDED).IsModified = false;


        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
             if (!SoftwarerExists(IDASSETSOFTWARE)){
                return Ok();
            }
            else{throw;}
        }
        return Ok(trnSoftware);
    }

      [HttpGet("{IDASSETSOFTWARE:int}")]
    public async Task<ActionResult<TRNSOFTWAREMODEL>> GetIdSoftware(int IDASSETSOFTWARE){
        var trngetspec = await _context.TRN_DTL_SOFTWARE.Where(x => x.IDASSETSOFTWARE == IDASSETSOFTWARE).FirstOrDefaultAsync();
        
        if (trngetspec == null)
        {
            return NotFound();
        }

        return Ok(trngetspec);
    }
    private bool SoftwarerExists(int idAssetSoftware){
        return _context.TRN_DTL_SOFTWARE.Any(e => e.IDASSETSOFTWARE == idAssetSoftware);

    }
}       
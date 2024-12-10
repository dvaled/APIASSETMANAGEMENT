using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using Master_GCM.ViewModels;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]

public class TrnAssetDtlPictureController : ControllerBase
{
    private readonly AppDbContext _context;

    public TrnAssetDtlPictureController(AppDbContext context)
    {
        _context = context;
    }

    // Get hardware by ASSETCODE and include the employee information
    [HttpGet("{ASSETCODE}")]
    public async Task<ActionResult<List<TRNASSETPICTUREMODEL>>> GetTrnHardware(string assetcode)
    {
        var trnassetcode = await _context.TRN_DTL_PICTURE
                                        .Where(e => e.ASSETCODE == assetcode)
                                        .ToListAsync();

        if(trnassetcode == null || !trnassetcode.Any()){
            return Ok("Not Found");
        }                                
        return Ok(trnassetcode);
    }


    [HttpPut("{IDASSETPIC}")]
    public async Task<IActionResult> UpdateAssetImage(int IDASSETPIC, [FromForm] AssetViewModel model)
    {
        try
        {
            // Retrieve the existing asset from the database
            var existingAsset = await _context.TRN_DTL_PICTURE
                .FirstOrDefaultAsync(a => a.IDASSETPIC == IDASSETPIC);

            if (existingAsset == null)
            {
                return NotFound("Asset not found.");
            }

            // Update asset properties
            existingAsset.ACTIVE = model.ACTIVE;
            existingAsset.PICADDED = model.PICADDED;
            existingAsset.DATEADDED = DateOnly.FromDateTime(DateTime.Now);

            // Handle file upload if a new image is provided
            if (model.ASSETIMG != null && model.ASSETIMG.Count > 0)
            {
                var folderName = @"/network_share/AssetManagementSystem/Image/Asset"; 
                var pathToSave = folderName;

                if (!Directory.Exists(pathToSave))
                    Directory.CreateDirectory(pathToSave);

                foreach (var file in model.ASSETIMG)
                {
                    if (file.Length > 0)
                    {
                        string assetCode = model.ASSETCODE;
                        string searchPattern = $"{assetCode}-*.jpg"; 
                        var existingFiles = Directory.GetFiles(pathToSave, searchPattern);
                        var existingNumbers = existingFiles
                            .Select(f => Path.GetFileNameWithoutExtension(f))
                            .Select(name => name.Substring(assetCode.Length + 1))
                            .Where(num => int.TryParse(num, out _))
                            .Select(int.Parse)
                            .ToList();

                        int nextNumber = existingNumbers.Any() ? existingNumbers.Max() + 1 : 1;
                        var newFileName = $"{assetCode}-{nextNumber}.jpg";
                        var fullPath = Path.Combine(pathToSave, newFileName);

                        using (var stream = new FileStream(fullPath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        // Update the asset picture path
                        existingAsset.ASSETPIC = newFileName;
                    }
                    else
                    {
                        return BadRequest("Invalid file.");
                    }
                }
            }

            // Save changes to the database
            _context.TRN_DTL_PICTURE.Update(existingAsset);
            await _context.SaveChangesAsync();

            return Ok("Asset updated successfully.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("{IDASSETPIC:int}")]
    public async Task<ActionResult<TRNASSETPICTUREMODEL>> GetIDImage(int IDASSETPIC){
        var trngetspec = await _context.TRN_DTL_PICTURE.Where(x => x.IDASSETPIC == IDASSETPIC).FirstOrDefaultAsync();
        
        if (trngetspec == null)
        {
            return NotFound();
        }

        return Ok(trngetspec);
    }
    private bool TrnPictureExists(int IDASSETPIC)
    {
        return _context.TRN_DTL_PICTURE.Any(e => e.IDASSETPIC == IDASSETPIC);
    }

    [HttpPost]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UploasdAssetImage([FromForm] AssetViewModel model)
    {
        try
        {
            if (model.ASSETIMG != null && model.ASSETIMG.Count > 0)
            {
                
                var folderName = @"/network_share/AssetManagementSystem/Image/Asset"; 
                var pathToSave = folderName;

                
                if (!Directory.Exists(pathToSave))
                    Directory.CreateDirectory(pathToSave);

                foreach (var file in model.ASSETIMG)
                {
                    // Ensure the file is valid
                    if (file.Length > 0)
                    {
                        string assetCode = model.ASSETCODE; // Get the asset code from the model
                        string searchPattern = $"{assetCode}-*.jpg"; // Define the search pattern

                        // Get existing files that match the pattern
                        var existingFiles = Directory.GetFiles(pathToSave, searchPattern);

                        // Extract numbers from existing file names
                        var existingNumbers = existingFiles
                            .Select(f => Path.GetFileNameWithoutExtension(f))
                            .Select(name => name.Substring(assetCode.Length + 1)) // Remove asset code and dash
                            .Where(num => int.TryParse(num, out _)) // Ensure it's a number
                            .Select(int.Parse)
                            .ToList();

                        // Determine the next number
                        int nextNumber = existingNumbers.Any() ? existingNumbers.Max() + 1 : 1;

                        // Construct the new file name
                        var newFileName = $"{assetCode}-{nextNumber}.jpg";
                        var fullPath = Path.Combine(pathToSave, newFileName);
                        var relativePath = newFileName;

                        // Save the image to the folder
                        using (var stream = new FileStream(fullPath, FileMode.Create))
                        {
                            Console.WriteLine($"Saving file to: {fullPath}");
                            await file.CopyToAsync(stream);
                        }

                        // Save only the file name to the database, not the path
                        var trnPicture = new TRNASSETPICTUREMODEL
                        {
                            ASSETCODE = model.ASSETCODE,
                            ACTIVE = model.ACTIVE,  
                            ASSETPIC = relativePath,  
                            PICADDED = model.PICADDED,  
                            DATEADDED = DateOnly.FromDateTime(DateTime.Now)  
                        };

                        // Add image data to the database
                        _context.TRN_DTL_PICTURE.Add(trnPicture);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        return BadRequest("Invalid file.");
                    }
                }   

                return Ok("File(s) uploaded successfully.");
            }
            else
            {
                return BadRequest("No files uploaded.");
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}
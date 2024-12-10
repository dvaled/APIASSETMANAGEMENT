using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]

public class EmployeeController : ControllerBase{
    private readonly AppDbContext _context;
    public EmployeeController(AppDbContext context){
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MSTEMPLOYEEMODEL>>> GetEmployee(){
    return await _context.MST_EMPLOYEE.ToListAsync();
    }

    [HttpGet("search")]
    public async Task<ActionResult<List<MSTEMPLOYEEMODEL>>> SearchEmployees(string? search)
    {
        var query = _context.MST_EMPLOYEE.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            search = search.ToLower();
            query = query.Where(x => x.NAME.ToLower().Contains(search));
        }

        var employees = await query.ToListAsync();

        return Ok(employees);
    }
       
}
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MovieTicketsBookings.Models;

using static System.Runtime.InteropServices.JavaScript.JSType;

public class IndexModel : PageModel
{
    private readonly Prn222FinalProjectContext _projectContext;

    public IndexModel(Prn222FinalProjectContext projectContext)
    { 
        _projectContext = projectContext;
    }

    public List<Movie> OpeningMovies { get; set; }

    public List<Movie> ComingSoonMovies { get; set; }

    public List<Movie> PosterMovies { get; set; }

    public async Task OnGetAsync()
    {
        var currentDate = DateTime.Today;

        //Filter "Opening This Week" movies based on status and release date
        OpeningMovies = await _projectContext.Movies
            .Include(o => o.Category)
            .Where(o => o.Status == "Active" && o.ReleaseDate <= currentDate)
            .ToListAsync();
        ComingSoonMovies = await _projectContext.Movies
            .Include(o => o.Category)
            .Where(o => o.Status == "Active" && o.ReleaseDate > currentDate)
            .ToListAsync();

        PosterMovies = await _projectContext.Movies
            .Include(o => o.Category)
            .Where(o => o.Status == "Poster")
            .OrderBy(o => Guid.NewGuid())
            .Take(5)
            .ToListAsync();
    }

}
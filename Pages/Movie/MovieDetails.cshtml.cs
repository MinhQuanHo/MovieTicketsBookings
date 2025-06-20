using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MovieTicketsBookings.Models;

namespace MovieTicketsBookings.Pages.Movie
{
    public class MovieDetailsModel : PageModel
    {
        private readonly Prn222FinalProjectContext _projectContext;

        public Models.Movie? Movie { get; set; }

        public List<Feedback> Feedbacks { get; set; }
        public int TotalReviews { get; set; }
        public double AverageRating { get; set; }
        [BindProperty]
        public string CommentText { get; set; } = string.Empty;
        [BindProperty]
        public int Rate { get; set; }
        public bool CanComment { get; private set; } = false;

        public MovieDetailsModel(Prn222FinalProjectContext projectContext)
        {
            _projectContext = projectContext;
        }


        public async Task OnGetAsync(int id)
        {
            Movie = await _projectContext.Movies
                .Include(o => o.Category)
                .FirstOrDefaultAsync(o => o.Id == id);

            Feedbacks = await _projectContext.Feedbacks
                .Where(o => o.MovieId == id)
                .Include(o => o.User)
                .OrderByDescending(o => o.CreateAt)
                .ToListAsync();

            HttpContext.Session.SetInt32("MovieId", Movie.Id);

            if (Feedbacks.Any())
            {
                AverageRating = Feedbacks.Average(o => o.Rate ?? 0);
                TotalReviews = Feedbacks.Count;
            }

            var userIdClaim = User.FindFirst("UserId")?.Value;
            if (userIdClaim != null && int.TryParse(userIdClaim, out int userId))
            {
                CanComment = await _projectContext.Bookings
                    .AnyAsync(b => b.UserId == userId
                    && b.Showtime.MovieId == id
                    && b.Status == "Confirmed");
            }
            else
            {
                CanComment = false;
            }
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var userIdClaim = User.FindFirst("UserId")?.Value;
            if (userIdClaim != null || !int.TryParse(userIdClaim, out int userId))
            {
                return RedirectToPage("/Login");

            }
            var newFeedback = new Feedback
            {
                MovieId = id,
                UserId = userId,
                Comments = CommentText,
                Rate = Rate,
                CreateAt = DateTime.Now
            };
            _projectContext.Feedbacks.Add(newFeedback);
            await _projectContext.SaveChangesAsync();

            return RedirectToPage("/Movie/MovieDetails", new { id = id });
        }

        public string GetEmbedUrl(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return "";
            }

            // Xử lý URL dạng youtube.com/watch?v=
            if (url.Contains("watch?v="))
            {
                var videoId = url.Split("watch?v=")[1];
                return $"https://www.youtube.com/embed/{videoId}";
            }
            // Xử lý URL dạng youtu.be/
            else if (url.Contains("youtu.be/"))
            {
                var videoId = url.Split("youtu.be/")[1];
                return $"https://www.youtube.com/embed/{videoId}";
            }

            return url; // Trả về nguyên URL nếu không match các pattern trên
        }
    }
}
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Pages.areas._207.Pages
{
    [BindProperty]
    public class IndexModel : PageModel
    {
        public int[] Integers { get; private set; } = new int[] { 1, 2, 3, 4 };

        public string Host { get; private set; }

        [ModelBinder(BinderType = typeof(RandomModelBinder))]
        public string FirstName { get; set; } = "John";

        [BindProperty(SupportsGet = true)]
        public string LastName { get; set; } = "Smith";

        public string ReadOnlyName => $"{FirstName} {LastName}";

        [FromForm(Name = "Name")]
        public string FullName { get; set; } = "Gene Kelly";

        [BindProperty]
        public string Required { get; set; }

        public IActionResult OnGet([FromHeader, Required] string host)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Host = host;
            ModelState.Clear();

            return Page();
        }

        public IActionResult OnPost([MinLength(6, ErrorMessage = "No host is that short!")] string host)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Host = host;
            ModelState.Clear();

            return Page();
        }

        public IActionResult OnPostIntegral([MinLength(6), Required] int[] ints)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Integers = ints;
            ModelState.Clear();

            return Page();
        }
    }
}

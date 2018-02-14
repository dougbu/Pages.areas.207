using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Pages.areas._207.Pages
{
    [BindProperty]
    public class IndexModel : PageModel
    {
        private string _realHost;
        private bool _isRealHostSet;

        [BindProperty]
        [MinLength(6)]
        [Required]
        public int[] Integers { get; set; } = new int[] { 1, 2, 3, 4 };

        [FromHeader(Name = "Host")]
        public string Host { get; set; }

        [BindProperty]
        [MinLength(6, ErrorMessage = "No host is that short!")]
        [Required]
        public string RealHost
        {
            get
            {
                if (!_isRealHostSet)
                {
                    return Host;
                }

                return _realHost;
            }
            set
            {
                _realHost = value;
                _isRealHostSet = true;
            }
        }

        [ModelBinder(BinderType = typeof(RandomModelBinder))]
        public string FirstName { get; set; } = "John";

        [BindProperty(SupportsGet = true)]
        public string LastName { get; set; } = "Smith";

        [BindProperty]
        public string ReadOnlyName => $"{FirstName} {LastName}";

        [FromForm(Name = "Name")]
        public string FullName { get; set; } = "Gene Kelly";

        [BindProperty]
        [BindRequired] // ignored !!!
        [Required] // Ignored !!!
        public string Required { get; set; }

        public IActionResult OnGet()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ModelState.Clear();

            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ModelState.Clear();

            return Page();
        }
    }
}

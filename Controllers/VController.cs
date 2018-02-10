using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
namespace Pages.areas._207.Controllers
{
    [BindProperty] // ignored !!!
    public class VController : Controller
    {
        public int[] Integers { get; set; } = new int[] { 1, 2, 3, 4 };

        public string Host { get; set; }

        [ModelBinder(BinderType = typeof(RandomModelBinder))]
        public string FirstName { get; set; } = "John";

        [BindProperty(SupportsGet = true)]
        public string LastName { get; set; } = "Smith";

        [BindProperty]
        public string ReadOnlyName => $"{FirstName} {LastName}";

        [FromForm(Name = "Name")]
        public string FullName { get; set; } = "Gene Kelly";

        [BindProperty]
        [BindNever]
        [Required]
        public string Required { get; set; }

        [Route("/View")]
        public IActionResult Index([FromHeader, Required] string host)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Host = host;
            ModelState.Clear();

            return View(model: this);
        }

        [HttpPost("/View")]
        public IActionResult PostIndex([MinLength(6, ErrorMessage = "No host is that short!")] string host)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Host = host;
            ModelState.Clear();

            return View(viewName: "Index", model: this);
        }

        [HttpPost("/Ints")]
        public IActionResult Ints([MinLength(6), Required] int[] ints)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Integers = ints;
            ModelState.Clear();

            return View(viewName: "Index", model: this);
        }
    }
}

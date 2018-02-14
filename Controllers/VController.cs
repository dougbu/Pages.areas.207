using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
namespace Pages.areas._207.Controllers
{
    [BindProperty] // ignored !!!
    public class VController : Controller
    {
        private string _realHost;
        private bool _isRealHostSet;

        [BindProperty]
        [MinLength(6)]
        [Required]
        public int[] Integers { get; set; } = new int[] { 1, 2, 3, 4, 5, 6 };

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
        [BindNever]
        [Required]
        public string Required { get; set; }

        [Route("/View")]
        public IActionResult Index()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ModelState.Clear();

            return View(model: this);
        }

        [HttpPost("/View")]
        public IActionResult PostIndex()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ModelState.Clear();

            return View(viewName: "Index", model: this);
        }
    }
}

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
namespace Pages.areas._207.Controllers
{
    [ApiController]
    [BindProperty] // ignored !!!
    [ApiExplorerSettings(IgnoreApi = false)]
    public class VController : ControllerBase
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
        public IActionResult Index(
            [Required] string hosta,
            [FromServices] IApiDescriptionGroupCollectionProvider apiDescriptionProvider,
            [FromServices] ILoggerFactory loggerFactory)
        {
            var logger = loggerFactory.CreateLogger<VController>();
            var groups = apiDescriptionProvider.ApiDescriptionGroups;
            logger.LogInformation(0, "Version: {Version}", groups.Version);
            var i = 0;
            foreach (var group in groups.Items)
            {
                logger.LogInformation(1, "Group {GroupIndex}: {GroupName}", i, group.GroupName);
                var j = 0;
                foreach (var item in group.Items)
                {
                    logger.LogInformation(2, "API Description {GroupIndex} / {APIIndex}: {APIName}", i, j, item.ActionDescriptor.DisplayName);
                    var k = 0;
                    foreach (var parameter in item.ParameterDescriptions)
                    {
                        logger.LogInformation(3, "Parameter {GroupIndex} / {APIIndex} / {ParameterIndex}: {ParameterName}, {IsRequired}", i, j, k, parameter.Name, parameter.ModelMetadata.IsRequired);
                        k++;
                    }

                    j++;
                }

                i++;
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ModelState.Clear();

            return Ok();
        }

        [HttpPost("/View")]
        public IActionResult PostIndex()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ModelState.Clear();

            return Ok();
        }

        [HttpGet("/string")]
        public IActionResult StringLength([Required, StringLength(2, ErrorMessage = "Go away too-tall.")] string value)
        {
            return Ok();
        }
    }
}

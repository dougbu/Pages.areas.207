﻿using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Pages.areas._207.MyArea.Pages
{
    public class ContactModel : PageModel
    {
        public string Message { get; set; }

        public void OnGet()
        {
            Message = "Your contact page.";
        }
    }
}

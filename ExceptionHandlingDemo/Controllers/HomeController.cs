using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ExceptionHandlingDemo.Models;
using System.IO;

namespace ExceptionHandlingDemo.Controllers
{ [TypeFilter(typeof(CustomExceptionFilter))]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Divide()
        {   return View();

        }
        [HttpPost]
        public IActionResult Divide(string no1, string no2)
        {

            int x = int.Parse(no1);
            int y = int.Parse(no2);
            int res = x / y;
            ViewBag.result = res;
            return View();
        }
        public IActionResult Index(int? id)
        {
            if (id.HasValue)
            {
                if (id == 0)
                {

                    int x = 10;
                    int? res = x / id;
                    throw new DivideByZeroException();
                }

                if (id == 1)
                    throw new FileNotFoundException("This file do not exist");


                else if (id == 2)
                    return StatusCode(500);
            }
            return View();
        }

        	[TypeFilter(typeof(CustomExceptionFilter))]  
 	public IActionResult Failing()
 	{  
  	    throw new Exception("Testing custom exception filter.");  
 	}


    public IActionResult MyStatusCode(int code)
 	{  
 	    if (code == 404)  
 	    {  
  	        ViewBag.ErrorMessage = "The requested page not found.";  
 	    }  
 	    else if (code == 500)  
 	    {  
 	        ViewBag.ErrorMessage = "My custom 500 error message.";  
 	    }  
 	    else  
 	    {  
 	        ViewBag.ErrorMessage = "An error occurred while processing your request.";  
 	    }  
 	  
 	    ViewBag.RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;  
     ViewBag.ShowRequestId = !string.IsNullOrEmpty(ViewBag.RequestId);  
 	    ViewBag.ErrorStatusCode = code;  
 	  
 	    return View();  
 	} 

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

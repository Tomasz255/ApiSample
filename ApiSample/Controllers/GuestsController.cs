using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiSample.DAL;
using ApiSample.DTOs.Guests;
using Microsoft.AspNetCore.Http;

using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ApiSample.Controllers
{
    [Route("api/guests")]
    [ApiController]
    public class GuestsController : ControllerBase
    {
        private readonly IExampleService _service;

        // ctor + TAB + TAB
        public GuestsController(IExampleService service)
        {
            _service = service;
        }

        //... api/guests/test
        [HttpGet("test")]
        public IActionResult Test()
        {
            return Ok(_service.Test());
        }

        // .../api/guests?lastname=Kowalski
        // .../api/guests
        [HttpGet]
        public IActionResult GetGuests(string lastname)
        {
            //tutaj będziemy zwracać listę gości
            return Ok(_service.GetGuestsCollection(lastname));
        }

        //... api/guests/4
        [HttpGet("{id:int}")]
        public IActionResult GetGuest(int id)
        {
            var result -_service.GetGuestById(id);
             if  (result !=null)
               return Ok(result);
             else 
                return NoFound("Something went wrong")

        }

        //... api/guests [+body]
        [HttpPost]
        public IActionResult AddGuest(GuestRequestDto requestDto)
        {
            if (_service.AddGuest(requestDto))
                return Ok("New guest created");
            else
                return BadRequest("Something went wrong");
        }

        //... api/guests/1 [+body]
        [HttpPut("{id:int}")]
        public IActionResult UpdateGuest(int id, GuestRequestDto requestDto)
        {
            if (_service.UpdateGuest(id, requestDto))
                return Ok($"Guest with id {id} has been updated.");
            else
                return BadRequest("Guest not found!");
        }

        //... api/guests/1
        [HttpDelete("{id:int}")]
        public IActionResult DeleteGuest(int id)
        {
            if (_service.DeleteGuest(id))
                return Ok($"Guest with id {id} has been deleted.");
            else
                return BadRequest("Guest not found!");
        }
    }
}
using Application.Core;
using Application.Users;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistnce;

namespace API.Controllers
{
    public class UsersController : BasicApiController
    {
        private static Random _rnd = new Random();

        [HttpGet]
        public async Task<ActionResult<List<User>>> GetUsers(int page = 1, int pageSize = 100)
        {
           //нужно проверить 500 на дашборде
           var n = _rnd.Next(100);
           if (n == 50 || n == 75)
              return StatusCode(500);

           return await Mediator.Send(new List.Query{PageNum = page, PageSize = pageSize});
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(Guid id)
        {
           //нужно проверить 500 на дашборде
           var n = _rnd.Next(100);
           if (n == 50 || n == 75)
              return StatusCode(500);

            return await Mediator.Send(new Details.Query{Id = id}); 
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(User user)
        {
            user.BirthDate = user.BirthDate.SetKindUtc();
            return Ok(await Mediator.Send(new Create.Command {User = user}));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> EditUser(Guid id, User user)
        {
            user.Id = id;
            return Ok(await Mediator.Send(new Edit.Command{User = user}));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(Guid id)
        {
            return Ok(await Mediator.Send(new Delete.Command{Id = id}));
        }


    }
}
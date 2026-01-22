using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class BuggyController : BaseApiController
{
    [HttpGet("auth")]
    public ActionResult<string> GetAuth()
    {
        return Unauthorized("Not authorized");
    }
    
    [HttpGet("not-found")]
    public ActionResult<string> GetNotFound()
    {
        return NotFound("Not found");
    }
    
    [HttpGet("bad-request")]
    public ActionResult<string> GetBadRequest()
    {
        return BadRequest("Bad request");
    }

    [HttpGet("server-error")]
    public ActionResult<string> GetServerError()
    {
        throw new Exception("Server error");
    }
}
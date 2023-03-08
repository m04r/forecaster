using Microsoft.AspNetCore.Mvc;

namespace jh_banno_assignment.Controllers;

[ApiController]
[Route("[controller]")]
public class FooController : ControllerBase
{
    private readonly ILogger<FooController> _logger;

    public FooController(ILogger<FooController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetFoo")]
    public Foo Get()
    {
        return new Foo() { Id = Guid.NewGuid(), Name = "Some One", CreatedAt = DateTime.Now };
    }
}

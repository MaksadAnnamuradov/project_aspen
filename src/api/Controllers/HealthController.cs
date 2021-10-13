﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Api.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class HealthController : ControllerBase
  {

    private readonly ILogger<HealthController> _logger;

    public HealthController(ILogger<HealthController> logger)
    {
      _logger = logger;
    }

    [HttpGet]
    public String Get()
    {
      return "ur good";
    }
  }
}

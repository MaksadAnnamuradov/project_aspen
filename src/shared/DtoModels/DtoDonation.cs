﻿using Swashbuckle.AspNetCore.Annotations;

namespace shared.DtoModels;

public record DtoDonation
{
    [SwaggerSchema(ReadOnly = true)]
    public long ID { get; set; }
    public long EventID { get; set; }
    public long? TeamID { get; set; }
    public long? PersonID { get; set; }
    public DateTime Date { get; set; }
    public decimal Amount { get; set; }
    public bool IsPending { get; set; } = false;
    public string? TeamName { get; set; }
    public string? PersonName { get; set; }
}

using System.Collections.Generic;

namespace DoñaTere.Core.Validators;

public class ValidationResult
{
    public List<string> Errors { get; set; } = new List<string>();
    public bool IsValid => Errors.Count == 0;
}
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using FluentValidation.Results;
namespace Project.Validation.Models
{
    public class ValidationResultModel 
    {
        public HttpStatusCode STATUSCODE { get; set; } = HttpStatusCode.BadRequest;

        public string MESSAGE { get; set; } = "Error";

        public string RESPONSES {get;set;} = string.Empty;

        public List<ValidationError>? Errors { get; }

        public ValidationResultModel(ValidationResult validationResult)
        {
            Errors = validationResult.Errors
                .Select(error => new ValidationError(error.PropertyName, error.ErrorMessage))
                .ToList();
        }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}

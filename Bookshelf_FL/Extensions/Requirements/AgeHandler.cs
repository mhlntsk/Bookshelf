using Microsoft.AspNetCore.Authorization;

namespace Bookshelf_FL.Extensions.Requirements
{
    public class AgeHandler : AuthorizationHandler<AgeRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AgeRequirement ageRequirement)
        {
            var dateOfBirthClaim = context.User.FindFirst("dateofbirth");

            if (dateOfBirthClaim != null && DateTime.TryParse(dateOfBirthClaim.Value, out var birthDate))
            {
                var today = DateTime.Today;
                var age = today.Year - birthDate.Year;

                if (birthDate.Date > today.Date.AddYears(-age))
                {
                    age--;
                }

                if (age >= ageRequirement.Age)
                {
                    context.Succeed(ageRequirement);
                }
            }

            return Task.CompletedTask;
        }
    }
}

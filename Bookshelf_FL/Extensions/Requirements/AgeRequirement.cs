using Microsoft.AspNetCore.Authorization;

namespace Bookshelf_FL.Extensions.Requirements
{
    public class AgeRequirement : IAuthorizationRequirement
    {
        protected internal int Age { get; set; }
        public AgeRequirement(int age)
        {
            Age = age;
        }
    }
}

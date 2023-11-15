using Microsoft.AspNetCore.Authorization;

namespace WebApp_UnderTheHood.Authorization
{
    public class HRManagerProbationRequirement: IAuthorizationRequirement
    {
        public HRManagerProbationRequirement(int probationMonth)
        {
            ProbationMonths=probationMonth;
                
        }

        public int ProbationMonths { get; }

        public class HRManagerProbationRequirementHandler : AuthorizationHandler<HRManagerProbationRequirement>

        {
            protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, HRManagerProbationRequirement requirement)
            {
                if (!context.User.HasClaim(x=> x.Type== "EmploymentDate"))
                {
                    return Task.CompletedTask;
                }
               
                if(DateTime.TryParse(context.User.FindFirst(x=>x.Type== "EmploymentDate")?.Value, out DateTime employementDate))
                {

                    var period = DateTime.Now - employementDate;
                    if (period.Days > 30 * requirement.ProbationMonths)
                        context.Succeed(requirement);
                }

                return Task.CompletedTask;

            }
        }
    }
}

using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace League_Of_Programmers.Controllers
{
    /// <summary>
    /// extension funciton if it only use at controllers
    /// </summary>
    public static class Extensions
    {
        public static ModelStateDictionary AddMessageError(this ModelStateDictionary modelState, params string[] errorMessages)
        {
            foreach (string message in errorMessages)
            {
                modelState.AddModelError(LOPController.MODEL_KEY, message);
            }
            return modelState;
        }
    }
}

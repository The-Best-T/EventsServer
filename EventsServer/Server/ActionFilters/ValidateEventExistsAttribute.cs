using Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Server.ActionFilters
{
    public class ValidateEventExistsAttribute : IAsyncActionFilter
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        public ValidateEventExistsAttribute(IRepositoryManager repository,
            ILoggerManager logger)
        {
            _repository = repository;
            _logger = logger;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context,
            ActionExecutionDelegate next)
        {
            var trackChanges = context.HttpContext.Request.Method.Equals("PUT");
            var id = (Guid)context.ActionArguments["id"];
            var chekEvent = await _repository.Event.GetEventByIdAsync(id, trackChanges);

            if (chekEvent == null)
            {
                _logger.LogInfo($"Event with id: {id} doesn't exist in the database.");
                context.Result = new NotFoundResult();
            }

            else
            {
                context.HttpContext.Items.Add("checkEvent", chekEvent);
                await next();
            }
        }
    }
}

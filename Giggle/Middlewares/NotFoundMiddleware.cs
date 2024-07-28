namespace Giggle.Middlewares
{
    public class NotFoundMiddleware
    {
        private readonly RequestDelegate _next;

        public NotFoundMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            await _next(context);

            if (context.Response.StatusCode == 404 || context.Response.StatusCode == 401)
            {
                context.Response.Redirect("/Error/NotFound");
            }
        }
    }
}

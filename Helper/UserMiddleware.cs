namespace QuanLyKhoaTu.Helper
{
    public class UserMiddleware
    {
        private readonly RequestDelegate _next;

        public UserMiddleware(RequestDelegate next)
        {
            _next = next;
        }


        public async Task Invoke(HttpContext context)
        {
            var path = context.Request.Path.Value?.ToLower();
            var userId = context.Request.Cookies["userId"];

            // Nếu là request AJAX thì bỏ qua middleware (không redirect)
            if (context.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                await _next(context);
                return;
            }
            if (string.IsNullOrEmpty(userId) && path != "/dang-nhap")
            {
                context.Response.Redirect("/dang-nhap");
                return;
            }
            if (!string.IsNullOrEmpty(userId) && path == "/dang-nhap")
            {
                context.Response.Redirect("/");
                return;
            }

            await _next(context);
        }
    }
}
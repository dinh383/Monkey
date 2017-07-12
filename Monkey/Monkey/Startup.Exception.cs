using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace Monkey
{
    public partial class Startup
    {
        public static class Exception
        {
            public static void Middleware(IApplicationBuilder app)
            {
                if (Environment.IsDevelopment())
                {
                    app.UseDeveloperExceptionPage(); 
                }
                else
                    app.UseExceptionHandler("/Home/Error");
            }
        }
    }
}
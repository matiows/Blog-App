using Blog_App.Services.Blogs;
using Blog_App.Services.UserAccounts;
using Blog_App.Services.Tags;

namespace Blog_App
{
    public static class AppServicesRegistration
    {
        public static void AddAppServices(this IServiceCollection services)
        {
            services.AddScoped<IBlogService, BlogService>();
            services.AddScoped<IUserAccountService, UserAccountService>();
            services.AddScoped<ITagService, TagService>();
        }

    }
}

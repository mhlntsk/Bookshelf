﻿using Bookshelf_SL.Repositories;
using Bookshelf_SL.Repositories.IntermediateModelsRepositories;
using Bookshelf_TL;
using Bookshelf_TL.Models;
using Bookshelf_TL.Models.IntermediateModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Bookshelf_FL.Extensions.Services;
using Bookshelf_FL.Extensions.Services.AccountServices;
using Bookshelf_FL.Extensions.Services.Factories;

namespace Bookshelf_FL.Extensions
{
    public static class ServiceExtensions
    {
        /// <summary>
        /// Configures Entity Framework DbContext for the project.
        /// </summary>
        public static void ConfigureEntityFrameworkCore(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddDbContext<BookshelfDbContext>(builder =>
                builder.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("Bookshelf_FL")));

        }

        /// <summary>
        /// Configures identity services.
        /// </summary>
        public static void ConfigureIdentity(this IServiceCollection services)
        {
            services.AddIdentity<User, IdentityRole>(opts =>
            {
                opts.Password.RequireLowercase = true;
                opts.SignIn.RequireConfirmedAccount = false;
                opts.User.RequireUniqueEmail = true;
            })
                .AddEntityFrameworkStores<BookshelfDbContext>()
                .AddDefaultTokenProviders();
        }

        /// <summary>
        /// Configures policy services.
        /// </summary>
        public static void ConfigurePolicy(this IServiceCollection services)
        {
            services.AddAuthorization(opts =>
            {
                opts.AddPolicy("OnlyForMicrosoft", policy =>
                {
                    policy.RequireClaim("Company", "Microsoft"); // [Authorize(Policy = "OnlyForMicrosoft")]
                });

                opts.AddPolicy("AgeLimit", policy =>
                {
                    policy.AddRequirements(new AgeRequirement(18));
                });
            });
        }

        /// <summary>
        /// Configures dependencies
        /// </summary>
        public static void ConfigureDependencies(this IServiceCollection services)
        {
            services.AddScoped<IRepository<Book>, BookRepository>();
            services.AddScoped<IRepository<Author>, AuthorRepository>();
            services.AddScoped<IRepository<User>, UserRepository>();
            services.AddScoped<IRepository<Category>, CategoryRepository>();
            services.AddScoped<IIntermediateRepository<BookAuthor>, BookAuthorRepository>();
            services.AddScoped<IIntermediateRepository<BookUser>, BookUserRepository>();
            services.AddScoped<IIntermediateRepository<BookCategory>, BookCategoryRepository>();
            services.AddTransient<IAuthorizationHandler, AgeHandler>();
            services.AddScoped<BookScoreService, BookScoreService>();
            services.AddScoped<CoverImageService, CoverImageService>();
            services.AddScoped<RelationEntitiesService, RelationEntitiesService>();
            services.AddScoped<FactoryOfBookService, FactoryOfBookService>();
            services.AddScoped<FactoryOfAuthorService, FactoryOfAuthorService>();
            services.AddScoped<FactoryOfUserService, FactoryOfUserService>();
            services.AddScoped<RegistrationService, RegistrationService>();
            services.AddScoped<LoginService, LoginService>();
            services.AddScoped<ChangePasswordService, ChangePasswordService>();
            services.AddScoped<ForgotPasswordService, ForgotPasswordService>();
            services.AddScoped<AdminService, AdminService>();

            services.AddScoped<IEmailSender, EmailSender>();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddScoped<IUrlHelper>(x =>
            {
                var actionContext = x.GetRequiredService<IActionContextAccessor>().ActionContext;
                var factory = x.GetRequiredService<IUrlHelperFactory>();
                return factory.GetUrlHelper(actionContext);
            });
        }


    }
}

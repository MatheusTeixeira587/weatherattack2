﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using WeatherAttack.Application.Command.Character;
using WeatherAttack.Application.Command.Character.Handlers;
using WeatherAttack.Application.Command.Spell;
using WeatherAttack.Application.Command.Spell.Handlers;
using WeatherAttack.Application.Command.User;
using WeatherAttack.Application.Command.User.Handlers;
using WeatherAttack.Application.Command.Weather;
using WeatherAttack.Application.Command.Weather.Handlers;
using WeatherAttack.Application.Comparer.User;
using WeatherAttack.Application.Mapper.Character;
using WeatherAttack.Application.Mapper.Spell;
using WeatherAttack.Application.Mapper.SpellRule;
using WeatherAttack.Application.Mapper.User;
using WeatherAttack.Application.Mapper.Weather;
using WeatherAttack.Contracts.Command;
using WeatherAttack.Contracts.Dtos.Character;
using WeatherAttack.Contracts.Dtos.Spell.Request;
using WeatherAttack.Contracts.Dtos.Spell.Response;
using WeatherAttack.Contracts.Dtos.SpellRule.Request;
using WeatherAttack.Contracts.Dtos.User.Request;
using WeatherAttack.Contracts.Dtos.User.Response;
using WeatherAttack.Contracts.Dtos.Weather.Request;
using WeatherAttack.Contracts.interfaces;
using WeatherAttack.Contracts.Interfaces;
using WeatherAttack.Contracts.Mapper;
using WeatherAttack.Domain.Contracts;
using WeatherAttack.Domain.Entities;
using WeatherAttack.Domain.Entities.Weather;
using WeatherAttack.Hub.Hubs.Challenge;
using WeatherAttack.Hub.Repositories;
using WeatherAttack.Infra;
using WeatherAttack.Infra.Repositories;
using WeatherAttack.Infra.Services;
using WeatherAttack.Security.Commands;
using WeatherAttack.Security.Commands.Handlers;
using WeatherAttack.Security.Entities;
using WeatherAttack.Security.Services;

namespace Weatherattack.WebApi
{
    public sealed class Startup
    {
        public Startup(IConfiguration configuration) => Configuration = configuration;

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(Configuration["SecuritySettings:SigningKey"])
                    )
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var accessToken = context.Request.Query["access_token"];

                            var path = context.HttpContext.Request.Path;

                            if (!string.IsNullOrEmpty(accessToken) &&
                                path.StartsWithSegments("/challenge"))
                            {
                                context.Token = accessToken;
                            }

                            return Task.CompletedTask;
                        },
                    };
                });

            services.AddControllers();
            services.AddMvc();

            //services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("v1", new Info { Title = "WeatherAttackAPI", Version = "v1" });

            //    c.AddSecurityDefinition("Bearer", new ApiKeyScheme
            //    {
            //        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
            //        Name = "Authorization",
            //        In = "header",
            //        Type = "apiKey"
            //    });

            //    c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
            //    {
            //        { "Bearer", new string[] { } } 
            //    });
            //});

            ConfigureDatabase(services);
            ConfigureRepositories(services);
            ConfigureMappers(services);
            ConfigureCommonServices(services);
            ConfigureActionHandlers(services);
            ConfigureComparers(services);

            services.AddSignalR(s => s.EnableDetailedErrors = true);
        }

        public void ConfigureRepositories(IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ISpellRepository, SpellRepository>();
            services.AddScoped<ICharacterRepository, CharacterRepository>();
            services.AddSingleton<IConnectionRepository<UserResponseDto>, ConnectionInMemoryDatabase>();
        }

        public void ConfigureDatabase(IServiceCollection services)
        {
            var connectionString = Configuration.GetConnectionString("WeatherAttack");

            services.AddDbContext<WeatherAttackContext>(options => options.UseSqlServer(connectionString));
        }

        public void ConfigureCommonServices(IServiceCollection services)
        {
            string OpenWeatherMapApiKey = Configuration["WebServices:OpenWeatherMap:ApiKey"];
            string OpenWeatherMapUrl = Configuration["WebServices:OpenWeatherMap:Url"];

            services.AddTransient<IPasswordService, PasswordService>();
            services.AddTransient<IAuthenticationService, AuthenticationService>();
            services.AddTransient<IOpenWeatherMapService, OpenWeatherMapWebService>(p => new OpenWeatherMapWebService(OpenWeatherMapUrl, OpenWeatherMapApiKey));
            services.AddSingleton(options =>
                new SecuritySettings
                {
                    SaltWorkFactor = int.Parse(Configuration["SecuritySettings:SaltWorkFactor"]),
                        SigningKey = Configuration["SecuritySettings:SigningKey"],
                        TokenExpirationTime = int.Parse(Configuration["SecuritySettings:TokenExpirationTime"])
                });
        }

        public void ConfigureMappers(IServiceCollection services)
        {
            services.AddTransient<IMapper<User, UserRequestDto, UserResponseDto>, UserEntityMapper>();
            services.AddTransient<IMapper<SpellRule, SpellRuleRequestDto, SpellRuleRequestDto>, SpellRuleEntityMapper>();
            services.AddTransient<IMapper<Spell, SpellRequestDto, SpellResponseDto>, SpellEntityMapper>();
            services.AddTransient<IMapper<Character, CharacterDto, CharacterDto>, CharacterEntityMapper>();
            services.AddTransient<IMapper<Wind, WindRequestDto, WindRequestDto>, WindEntityMapper>();
            services.AddTransient<IMapper<Rain, RainRequestDto, RainRequestDto>, RainEntityMapper>();
            services.AddTransient<IMapper<Coordinates, CoordinatesRequestDto, CoordinatesRequestDto>, CoordinatesEntityMapper>();
            services.AddTransient<IMapper<Weather, WeatherRequestDto, WeatherRequestDto>, WeatherEntityMapper>();
            services.AddTransient<IMapper<Main, MainRequestDto, MainRequestDto>, MainWeatherEntityMapper>();
            services.AddTransient<IMapper<CurrentWeather, CurrentWeatherRequestDto, CurrentWeatherRequestDto>, CurrentWeatherEntityMapper>();
            services.AddTransient<IMapper<CountryInfo, CountryInfoRequestDto, CountryInfoRequestDto>, CountryInfoEntityMapper>();
        }

        public void ConfigureComparers(IServiceCollection services)
        {
            services.AddTransient<IEqualityComparer<UserResponseDto>, UserDtoComparer>();
        }

        public void ConfigureActionHandlers(IServiceCollection services)
        {
            services.AddTransient<IActionHandlerAsync<LoginCommand>, LoginActionHandler>();

            services.AddTransient<IActionHandlerAsync<AddUserCommand>, AddUserActionHandler>();
            services.AddTransient<IActionHandlerAsync<GetPagedUsersCommand>, GetPagedUsersActionHandler>();
            services.AddTransient<IActionHandlerAsync<GetUserCommand>, GetUserActionHandler>();
            services.AddTransient<IActionHandlerAsync<DeleteUserCommand>, DeleteUserActionHandler>();

            services.AddTransient<IActionHandlerAsync<AddSpellCommand>, AddSpellActionHandler>();
            services.AddTransient<IActionHandlerAsync<GetSpellCommand>, GetSpellActionHandler>();
            services.AddTransient<IActionHandlerAsync<DeleteSpellCommand>, DeleteSpellActionHandler>();
            services.AddTransient<IActionHandlerAsync<GetSpellsForLocationCommand>, GetSpellsForLocationActionHandler>();
            services.AddTransient<IActionHandlerAsync<GetPagedSpellsCommand>, GetPagedSpellsActionHandler>();

            services.AddTransient<IActionHandlerAsync<GetCharacterCommand>, GetCharacterActionHandler>();

            services.AddTransient<IActionHandlerAsync<GetCurrentWeatherCommand>, GetCurrentWeatherActionHandler>();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting();

            app.UseCors(builder =>
                builder.WithOrigins("http://localhost:3000", "http://api.openweathermap.org")
                .SetPreflightMaxAge(TimeSpan.FromSeconds(5000))
                .SetIsOriginAllowedToAllowWildcardSubdomains()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseHttpsRedirection();

            app.UseEndpoints(app =>
            {
                app.MapHub<Challenge>("/challenge");
                app.MapControllers();
            });
        }
    }
}
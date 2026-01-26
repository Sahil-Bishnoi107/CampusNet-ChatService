using System;
using ChatService.Domain.Interfaces;
using ChatService.Infrastructure.Messaging;
using ChatService.Infrastructure.Persistence;
using ChatService.Infrastructure.Repositories;
using ChatService.Infrastructure.Jwt;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace ChatService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ChatDbContext>(opt =>
            opt.UseNpgsql(configuration.GetConnectionString("AppDb")));

       
        services.AddScoped<IEventPublisher, RabbitMqPublisher>();
        
        // Repositories
        services.AddScoped<IMessageRepository, MessageRepository>();
        services.AddScoped<IUserRelationRepository, UserRelationRepository>();
        services.AddScoped<IChatReadStateRepository, ChatReadStateRepository>();
        services.AddScoped<IJwtRepository, JwtRepository>();
        
        // UnitOfWork (ChatDbContext implements it)
        services.AddScoped<IUnitOfWork>(provider => provider.GetRequiredService<ChatDbContext>());

        return services;
    }
}

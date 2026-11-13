using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using MusicPortal.Application.Interfaces;
using MusicPortal.Application.Profiles;
using MusicPortal.Application.Services;

namespace MusicPortal.Application.DependencyInjection;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IArtistService, ArtistService>();
        services.AddScoped<IGenreService, GenreService>();
        services.AddScoped<IRecordService, RecordService>();
        services.AddScoped<IPlaylistService, PlaylistService>();
        services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>());
        return services;
    }
}

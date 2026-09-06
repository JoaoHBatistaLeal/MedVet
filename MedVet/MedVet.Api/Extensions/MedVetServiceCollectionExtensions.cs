using MedVet.Application.Interfaces.Repositories;
using MedVet.Application.Services.Implementations;
using MedVet.Application.Services.Interfaces;
using MedVet.Infrastructure.Persistence;
using MedVet.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MedVet.Api.Extensions;

public static class MedVetServiceCollectionExtensions
{
    public static IServiceCollection AddMedVetDbContext(
        this IServiceCollection services,
        IConfiguration configuration,
        string connectionStringName = "OracleConnection")
    {
        var useSqlite = configuration.GetValue<bool>("Database:UseSqlite");
        if (useSqlite)
        {
            var sqliteConnectionString = configuration.GetConnectionString("MedVetSqlite")
                ?? "Data Source=medvet-dev.db";

            services.AddDbContext<MedVetContext>(options =>
                options.UseSqlite(sqliteConnectionString));

            return services;
        }

        var oracleConnectionString = configuration.GetConnectionString(connectionStringName)
            ?? throw new InvalidOperationException($"Connection string '{connectionStringName}' nao encontrada.");

        services.AddDbContext<MedVetContext>(options =>
            options.UseOracle(oracleConnectionString));

        return services;
    }

    public static IServiceCollection AddMedVetRepositories(this IServiceCollection services)
    {
        services.AddScoped<IDonoRepository, DonoRepository>();
        services.AddScoped<IPetRepository, PetRepository>();
        services.AddScoped<IVeterinarioRepository, VeterinarioRepository>();
        services.AddScoped<IConsultaRepository, ConsultaRepository>();
        services.AddScoped<IPrescricaoRepository, PrescricaoRepository>();
        services.AddScoped<IMedicamentoRepository, MedicamentoRepository>();
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

        return services;
    }

    public static IServiceCollection AddMedVetApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IDonoService, DonoService>();
        services.AddScoped<IPetService, PetService>();
        services.AddScoped<IMedicamentoService, MedicamentoService>();
        services.AddScoped<IVeterinarioService, VeterinarioService>();
        services.AddScoped<IConsultaService, ConsultaService>();

        return services;
    }
}

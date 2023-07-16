using Microsoft.EntityFrameworkCore;
using MiniValidation;
using VehiclesMinimalAPI.Data;
using VehiclesMinimalAPI.Models;
using NetDevPack.Identity;
using NetDevPack.Identity.Jwt;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using NetDevPack.Identity.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

#region Configuração de Serviços

builder.Services.AddDbContext<MinimalContextDb>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentityEntityFrameworkContextConfiguration(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
    b => b.MigrationsAssembly("VehiclesMinimalAPI")));

builder.Services.AddIdentityConfiguration();
builder.Services.AddJwtConfiguration(builder.Configuration, "Appsettings");

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Insira o token JWT desta maneira: Bearer {seu token}",
        Name = "Authorization",
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

var app = builder.Build();

#endregion

#region Configuração de Pipeline

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthConfiguration();
app.UseHttpsRedirection();

MapActions(app);

app.Run();

#endregion

#region Configuração de Endpoints

void MapActions(WebApplication app)
{
    app.MapPost("/cadastro", [AllowAnonymous] async (
    SignInManager<IdentityUser> signInManager,
    UserManager<IdentityUser> userManager,
    IOptions<AppJwtSettings> appJwtSettings,
    RegisterUser registerUser) =>
    {
        if (registerUser == null)
            return Results.BadRequest("Usuário não informado");

        if (!MiniValidator.TryValidate(registerUser, out var errors))
            return Results.ValidationProblem(errors);

        var user = new IdentityUser
        {
            UserName = registerUser.Email,
            Email = registerUser.Email,
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(user, registerUser.Password);

        if (!result.Succeeded)
            return Results.BadRequest(result.Errors);

        var jwt = new JwtBuilder()
                    .WithUserManager(userManager)
                    .WithJwtSettings(appJwtSettings.Value)
                    .WithEmail(user.Email)
                    .WithJwtClaims()
                    .WithUserClaims()
                    .WithUserRoles()
                    .BuildUserResponse();

        return Results.Ok(jwt);
    })
    .ProducesValidationProblem()
    .Produces(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status400BadRequest)
    .WithName("CadastroUsuario")
    .WithTags("Usuario");

    app.MapPost("/login", [AllowAnonymous] async (
        SignInManager<IdentityUser> signInManager,
        UserManager<IdentityUser> userManager,
        IOptions<AppJwtSettings> appJwtSettings,
        LoginUser loginUser) =>
    {
        if (loginUser == null)
            return Results.BadRequest("Usuário não informado");

        if (!MiniValidator.TryValidate(loginUser, out var errors))
            return Results.ValidationProblem(errors);

        var result = await signInManager.PasswordSignInAsync(loginUser.Email, loginUser.Password, false, true);

        if (result.IsLockedOut)
            return Results.BadRequest("Usuário bloqueado");

        if (!result.Succeeded)
            return Results.BadRequest("Usuário ou senha inválidos");

        var jwt = new JwtBuilder()
                    .WithUserManager(userManager)
                    .WithJwtSettings(appJwtSettings.Value)
                    .WithEmail(loginUser.Email)
                    .WithJwtClaims()
                    .WithUserClaims()
                    .WithUserRoles()
                    .BuildUserResponse();

        return Results.Ok(jwt);

    }).ProducesValidationProblem()
      .Produces(StatusCodes.Status200OK)
      .Produces(StatusCodes.Status400BadRequest)
      .WithName("LoginUsuario")
      .WithTags("Usuario");


    app.MapGet("/automovel", [AllowAnonymous] async (
        MinimalContextDb context) =>
        await context.Automoveis.ToListAsync())
        .WithName("GetAutomovel")
        .WithTags("Automovel");

    app.MapGet("/automovel/{id}", [Authorize] async (
        int id,
        MinimalContextDb context) =>
        await context.Automoveis.FindAsync(id)
            is Automovel automovel
                ? Results.Ok(automovel)
                : Results.NotFound())
        .Produces<Automovel>(StatusCodes.Status200OK)
        .Produces<Automovel>(StatusCodes.Status404NotFound)
        .WithName("GetAutomovelPorId")
        .WithTags("Automovel");

    app.MapPost("/automovel", [Authorize] async (
        MinimalContextDb context,
        Automovel automovel) =>
    {
        if (!MiniValidator.TryValidate(automovel, out var errors))
            return Results.ValidationProblem(errors);

        context.Automoveis.Add(automovel);
        var result = await context.SaveChangesAsync();

        return result > 0
            ? Results.CreatedAtRoute("GetAutomovelPorId", new { id = automovel.Id }, automovel)
            : Results.BadRequest("Houve um problema ao salvar o registro!");

    }).ProducesValidationProblem()
        .Produces<Automovel>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest)
        .WithName("PostAutomovel")
        .WithTags("Automovel");

    app.MapPut("/automovel/{id}", [Authorize] async (
        int id,
        MinimalContextDb context,
        Automovel automovel) =>
    {
        var automovelAlocado = await context.Automoveis.AsNoTracking<Automovel>().FirstOrDefaultAsync(a => a.Id == id);
        if (automovelAlocado == null) return Results.NotFound();

        if (!MiniValidator.TryValidate(automovel, out var errors))
            return Results.ValidationProblem(errors);

        context.Automoveis.Update(automovel);
        var result = await context.SaveChangesAsync();

        return result > 0
            ? Results.NoContent()
            : Results.BadRequest("Houve um problema ao salvar o registro!");

    }).ProducesValidationProblem()
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status400BadRequest)
        .WithName("PutAutomovel")
        .WithTags("Automovel");

    app.MapDelete("/automovel/{id}", [Authorize] async (
        int id,
        MinimalContextDb context) =>
    {
        var automovel = await context.Automoveis.FindAsync(id);
        if (automovel == null) return Results.NotFound();

        context.Automoveis.Remove(automovel);
        var result = await context.SaveChangesAsync();

        return result > 0
            ? Results.NoContent()
            : Results.BadRequest("Houve um problema ao salvar o registro!");

    }).Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status204NoContent)
        .WithName("DeleteAutomovel")
        .WithTags("Automovel");
}
#endregion
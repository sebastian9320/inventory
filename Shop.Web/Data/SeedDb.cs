namespace Shop.Web.Data
{
using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Entities;
    using Helpers;
    using Microsoft.AspNetCore.Identity;

    public class SeedDb
{
    private readonly DataContext context;
    private readonly IUserHelper userHelper;
    private readonly Random random;

    public SeedDb(DataContext context, IUserHelper userHelper)
    {
        this.context = context;
        this.userHelper = userHelper;
        this.random = new Random();
    }

    public async Task SeedAsync()
    {
        await this.context.Database.EnsureCreatedAsync();

        await this.CheckRoles();

        if (!this.context.Countries.Any())
        {
            await this.AddCountriesAndCitiesAsync();
        }

        await this.CheckUserAsync("sebastian@gmail.com", "Juan", "Sebastian", "Admin");
        await this.CheckUserAsync("adriana@gmail.com", "Adriana", "Rodriguez", "Customer");
        var user = await this.CheckUserAsync("damjebecerra@gmail.com", "Damian", "Becerra", "Admin");

        // Add products
        if (!this.context.Products.Any())
        {
            this.AddProduct("Aceite Girasol", 13000, user);
            this.AddProduct("Aromatizante", 2500, user);            
            this.AddProduct("Emapandas Nana Legalizadas", 1000, user);
            this.AddProduct("Escoba", 5000, user);
            this.AddProduct("frijol", 3000, user);
            this.AddProduct("Jabon en Barra", 2000, user);
            this.AddProduct("Jabon en Polvo Rindex", 3500, user);
            this.AddProduct("Leche en Polvo", 6000, user);
            this.AddProduct("Mantequilla Campi", 5000, user);
            this.AddProduct("Papel Higienico Triple Hoja x6", 10000, user);
            this.AddProduct("Pastas la Muñeca", 7000, user);
            this.AddProduct("Trapero Ecologico", 8000, user);
            this.AddProduct("Arroz Roa", 1500, user);
            await this.context.SaveChangesAsync();
        }
    }

    private async Task<User> CheckUserAsync(string userName, string firstName, string lastName, string role)
    {
        // Add user
        var user = await this.userHelper.GetUserByEmailAsync(userName);
        if (user == null)
        {
            user = await this.AddUser(userName, firstName, lastName, role);
        }

        var isInRole = await this.userHelper.IsUserInRoleAsync(user, role);
        if (!isInRole)
        {
            await this.userHelper.AddUserToRoleAsync(user, role);
        }

        return user;
    }

    private async Task<User> AddUser(string userName, string firstName, string lastName, string role)
    {
        var user = new User
        {
            FirstName = firstName,
            LastName = lastName,
            Email = userName,
            UserName = userName,
            Address = "Avenida Siempre viva 742",
            PhoneNumber = "350 634 2747",
            CityId = this.context.Countries.FirstOrDefault().Cities.FirstOrDefault().Id,
            City = this.context.Countries.FirstOrDefault().Cities.FirstOrDefault()
        };

        var result = await this.userHelper.AddUserAsync(user, "123456");
        if (result != IdentityResult.Success)
        {
            throw new InvalidOperationException("Could not create the user in seeder");
        }

        await this.userHelper.AddUserToRoleAsync(user, role);
        var token = await this.userHelper.GenerateEmailConfirmationTokenAsync(user);
        await this.userHelper.ConfirmEmailAsync(user, token);
        return user;
    }

    private async Task AddCountriesAndCitiesAsync()
    {
        this.AddCountry("Colombia", new string[] { "Ibagué", "Bogota", "Calí", "Barranquilla", "Bucaramanga", "Cartagena", "Pereira" });
        this.AddCountry("Argentina", new string[] { "Córdoba", "Buenos Aires", "Rosario", "Tandil", "Salta", "Mendoza" });
        this.AddCountry("Estados Unidos", new string[] { "New York", "Los Ángeles", "Chicago", "Washington", "San Francisco", "Miami", "Boston" });
        this.AddCountry("Ecuador", new string[] { "Quito", "Guayaquil", "Ambato", "Manta", "Loja", "Santo" });
        this.AddCountry("Peru", new string[] { "Lima", "Arequipa", "Cusco", "Trujillo", "Chiclayo", "Iquitos" });
        this.AddCountry("Chile", new string[] { "Santiago", "Valdivia", "Concepcion", "Puerto Montt", "Temucos", "La Sirena" });
        this.AddCountry("Uruguay", new string[] { "Montevideo", "Punta del Este", "Colonia del Sacramento", "Las Piedras" });
        this.AddCountry("Bolivia", new string[] { "La Paz", "Sucre", "Potosi", "Cochabamba" });
        this.AddCountry("Venezuela", new string[] { "Caracas", "Valencia", "Maracaibo", "Ciudad Bolivar", "Maracay", "Barquisimeto" });
        this.AddCountry("Paraguay", new string[] { "Asunción", "Ciudad del Este", "Encarnación", "San  Lorenzo", "Luque", "Areguá" });
        this.AddCountry("Brasil", new string[] { "Rio de Janeiro", "São Paulo", "Salvador", "Porto Alegre", "Curitiba", "Recife", "Belo Horizonte", "Fortaleza" });
        this.AddCountry("Panamá", new string[] { "Chitré", "Santiago", "La Arena", "Agua Dulce", "Monagrillo", "Ciudad de Panamá", "Colón", "Los Santos" });
        this.AddCountry("México", new string[] { "Guadalajara", "Ciudad de México", "Monterrey", "Ciudad Obregón", "Hermosillo", "La Paz", "Culiacán", "Los Mochis" });
        await this.context.SaveChangesAsync();
    }

    private void AddCountry(string country, string[] cities)
    {
        var theCities = cities.Select(c => new City { Name = c }).ToList();
        this.context.Countries.Add(new Country
        {
            Cities = theCities,
            Name = country
        });
    }

    private async Task CheckRoles()
    {
        await this.userHelper.CheckRoleAsync("Admin");
        await this.userHelper.CheckRoleAsync("Customer");
    }

    private void AddProduct(string name, decimal price, User user)
    {
        this.context.Products.Add(new Product
        {
            Name = name,
            Price = price,
            IsAvailabe = true,
     
            User = user,
            ImageUrl = $"~/images/Products/{name}.png"
        });
    }
}}

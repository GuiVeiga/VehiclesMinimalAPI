using VehiclesMinimalAPI.Models;

namespace VehiclesMinimalAPI.Seeds
{
    public static class AutomovelSeed
    {
        public static List<Automovel> Seed { get; set; } = new List<Automovel>()
        {
            new Automovel()
            {
                Id = 1,
                Modelo = "Joy",
                Marca = "Chevrolet",
                Cor = "Preto",
                Placa = "ABC1234",
                Disponivel = true
            },
            new Automovel()
            {
                Id = 2,
                Modelo = "Gol",
                Marca = "Volkswagen",
                Cor = "Prata",
                Placa = "DEF5678",
                Disponivel = true
            },
            new Automovel()
            {
                Id = 3,
                Modelo = "Ka",
                Marca = "Ford",
                Cor = "Branco",
                Placa = "GHI9101",
                Disponivel = true
            }
        };
    }
}

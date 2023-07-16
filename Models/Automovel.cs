namespace VehiclesMinimalAPI.Models
{
    public class Automovel
    {
        public int Id { get; set; }
        public string? Modelo { get; set; }
        public string? Marca { get; set; }
        public string? Cor { get; set; }
        public string? Placa { get; set; }
        public bool Disponivel { get; set; }
    }
}

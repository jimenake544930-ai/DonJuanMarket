namespace DonJuanMarket
{
    /// <summary>
    /// Los tres tipos de cliente. El número indica la prioridad:
    /// mientras más pequeño, más rápido lo atienden.
    /// </summary>
    public enum TipoCliente
    {
        VIP = 1,
        ADULTO_MAYOR = 2,
        REGULAR = 3
    }

    /// <summary>
    /// Representa un cliente que espera ser atendido en la caja.
    /// </summary>
    public class Cliente
    {
        // Contador compartido por TODOS los clientes: cada cliente nuevo recibe
        // el siguiente número (1, 2, 3...). Sirve para desempatar con exactitud.
        private static long contadorLlegadas = 0;

        public string Nombre { get; private set; }
        public TipoCliente Tipo { get; private set; }
        public int NumeroProductos { get; private set; }
        public DateTime HoraLlegada { get; private set; }
        public long NumeroLlegada { get; private set; }

        // Convierte el tipo en su número: VIP = 1, ADULTO_MAYOR = 2, REGULAR = 3.
        public int Prioridad => (int)Tipo;

        // Constructor: la hora y el número de llegada se guardan solos.
        public Cliente(string nombre, TipoCliente tipo, int numeroProductos)
        {
            Nombre = nombre;
            Tipo = tipo;
            NumeroProductos = numeroProductos;
            HoraLlegada = DateTime.Now;
            NumeroLlegada = ++contadorLlegadas;
        }

        // Muestra los datos del cliente en pantalla.
        public void Mostrar()
        {
            Console.WriteLine($"Nombre: {Nombre}");
            Console.WriteLine($"Tipo: {Tipo}");
            Console.WriteLine($"Productos: {NumeroProductos}");
            Console.WriteLine($"Prioridad: {Prioridad}");
            Console.WriteLine($"Hora de llegada: {HoraLlegada:HH:mm:ss}");
        }
    }
}
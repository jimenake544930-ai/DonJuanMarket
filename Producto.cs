namespace DonJuanMarket
{
    /// <summary>
    /// Representa un producto del inventario de Don Juan Market.
    /// </summary>
    public class Producto
    {
        // El código solo se puede leer desde fuera (private set),
        // porque el PDF dice que no se puede modificar después de crearlo.
        public string Codigo { get; private set; }
        public string Nombre { get; set; }
        public string Categoria { get; set; }
        public double Precio { get; set; }
        public int Stock { get; set; }

        // Constructor: se ejecuta al crear un producto nuevo.
        public Producto(string codigo, string nombre, string categoria, double precio, int stock)
        {
            Codigo = codigo;
            Nombre = nombre;
            Categoria = categoria;
            Precio = precio;
            Stock = stock;
        }

        // Muestra los datos del producto en pantalla.
        public void Mostrar()
        {
            Console.WriteLine($"Código: {Codigo}");
            Console.WriteLine($"Nombre: {Nombre}");
            Console.WriteLine($"Categoría: {Categoria}");
            Console.WriteLine($"Precio: Q{Precio:F2}");
            Console.WriteLine($"Stock: {Stock}");
        }
    }
}
namespace DonJuanMarket
{
    /// <summary>
    /// Clase principal que integra las dos estructuras de datos:
    /// el Árbol AVL (inventario) y la Cola de Prioridad (clientes).
    /// El menú le pide las operaciones a esta clase; ella valida los datos
    /// y usa el árbol y el heap. Si algo está mal, lanza un ArgumentException
    /// con un mensaje claro para que el menú lo muestre.
    /// </summary>
    public class Supermercado
    {
        private ArbolAVL inventario = new ArbolAVL();
        private ColaPrioridad colaClientes = new ColaPrioridad();
        private int clientesAtendidos = 0;

        // ===================== INVENTARIO (ÁRBOL AVL) =====================

        /// <summary>
        /// Registra un producto nuevo. Lanza error si los datos no son válidos
        /// o si el código ya existe.
        /// </summary>
        public void RegistrarProducto(string codigo, string nombre, string categoria, double precio, int stock)
        {
            codigo = NormalizarCodigo(codigo);

            if (string.IsNullOrWhiteSpace(codigo))
            {
                throw new ArgumentException("El código no puede estar vacío.");
            }

            ValidarDatosProducto(nombre, categoria, precio, stock);

            Producto producto = new Producto(codigo, nombre.Trim(), categoria.Trim(), precio, stock);

            // Insertar devuelve false si el código ya estaba registrado.
            if (!inventario.Insertar(producto))
            {
                throw new ArgumentException($"Ya existe un producto con el código {codigo}.");
            }
        }

        /// <summary>
        /// Busca un producto por código. Devuelve null si no existe.
        /// </summary>
        public Producto? BuscarProducto(string codigo)
        {
            return inventario.Buscar(NormalizarCodigo(codigo));
        }

        /// <summary>
        /// Actualiza nombre, categoría, precio y stock de un producto existente.
        /// El código no se puede cambiar.
        /// </summary>
        public void ActualizarProducto(string codigo, string nombre, string categoria, double precio, int stock)
        {
            codigo = NormalizarCodigo(codigo);

            if (inventario.Buscar(codigo) == null)
            {
                throw new ArgumentException($"No existe un producto con el código {codigo}.");
            }

            ValidarDatosProducto(nombre, categoria, precio, stock);
            inventario.Actualizar(codigo, nombre.Trim(), categoria.Trim(), precio, stock);
        }

        /// <summary>
        /// Elimina un producto por su código. Lanza error si no existe.
        /// </summary>
        public void EliminarProducto(string codigo)
        {
            codigo = NormalizarCodigo(codigo);

            if (!inventario.Eliminar(codigo))
            {
                throw new ArgumentException($"No existe un producto con el código {codigo}.");
            }
        }

        /// <summary>
        /// Muestra el inventario ordenado por código (recorrido in-orden).
        /// </summary>
        public void MostrarInventario()
        {
            inventario.MostrarInventario();
        }

        // ================= ATENCIÓN DE CLIENTES (HEAP) =================

        /// <summary>
        /// Agrega un cliente a la cola de prioridad y devuelve su posición estimada.
        /// </summary>
        public int AgregarCliente(string nombre, TipoCliente tipo, int numeroProductos)
        {
            if (string.IsNullOrWhiteSpace(nombre))
            {
                throw new ArgumentException("El nombre del cliente no puede estar vacío.");
            }

            if (numeroProductos <= 0)
            {
                throw new ArgumentException("El número de productos debe ser mayor que 0.");
            }

            Cliente cliente = new Cliente(nombre.Trim(), tipo, numeroProductos);
            colaClientes.Agregar(cliente);
            return colaClientes.PosicionEnCola(cliente);
        }

        /// <summary>
        /// Saca de la cola al siguiente cliente según su prioridad.
        /// Devuelve null si no hay clientes esperando.
        /// </summary>
        public Cliente? AtenderSiguiente()
        {
            Cliente? cliente = colaClientes.Extraer();

            if (cliente != null)
            {
                clientesAtendidos++;
            }

            return cliente;
        }

        /// <summary>
        /// Muestra los clientes en espera, en el orden en que serán atendidos.
        /// </summary>
        public void MostrarClientesEnEspera()
        {
            colaClientes.MostrarEnEspera();
        }

        // ========================= REPORTES =========================

        /// <summary>
        /// Muestra estadísticas del inventario y de la atención de clientes.
        /// </summary>
        public void MostrarEstadisticas()
        {
            List<Producto> productos = inventario.ObtenerProductos();

            int unidades = 0;
            double valorTotal = 0;

            foreach (Producto p in productos)
            {
                unidades += p.Stock;
                valorTotal += p.Precio * p.Stock;
            }

            Console.WriteLine("=== ESTADÍSTICAS DEL SISTEMA ===");
            Console.WriteLine();
            Console.WriteLine("INVENTARIO");
            Console.WriteLine($"Productos registrados : {productos.Count}");
            Console.WriteLine($"Unidades en stock     : {unidades}");
            Console.WriteLine($"Valor del inventario  : Q{valorTotal:N2}");
            Console.WriteLine($"Altura del árbol AVL  : {inventario.AlturaArbol}");
            Console.WriteLine($"Código en la raíz     : {inventario.RaizCodigo ?? "(inventario vacío)"}");
            Console.WriteLine();
            Console.WriteLine("CLIENTES");
            Console.WriteLine($"Clientes en espera    : {colaClientes.Cantidad}");
            Console.WriteLine($"Clientes atendidos    : {clientesAtendidos}");
        }

        // ======================= MÉTODOS PRIVADOS =======================

        // Quita espacios y pasa a mayúsculas, para que "prod001" y "PROD001" sean el mismo código.
        private static string NormalizarCodigo(string codigo)
        {
            return (codigo ?? "").Trim().ToUpper();
        }

        // Validaciones comunes de nombre, categoría, precio y stock (se usan al registrar y al actualizar).
        private static void ValidarDatosProducto(string nombre, string categoria, double precio, int stock)
        {
            if (string.IsNullOrWhiteSpace(nombre))
            {
                throw new ArgumentException("El nombre no puede estar vacío.");
            }

            if (string.IsNullOrWhiteSpace(categoria))
            {
                throw new ArgumentException("La categoría no puede estar vacía.");
            }

            if (precio <= 0)
            {
                throw new ArgumentException("El precio debe ser mayor que 0.");
            }

            if (stock < 0)
            {
                throw new ArgumentException("El stock no puede ser negativo.");
            }
        }
    }
}
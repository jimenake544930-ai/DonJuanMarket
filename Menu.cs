using System.Globalization;

namespace DonJuanMarket
{
    /// <summary>
    /// Error especial que se lanza cuando el usuario escribe "volver"
    /// en cualquier pregunta. El menú principal lo atrapa y regresa al inicio.
    /// </summary>
    public class OperacionCancelada : Exception
    {
    }

    /// <summary>
    /// Maneja la interfaz de consola: muestra el menú, hace las preguntas,
    /// valida lo que escribe el usuario y le pide el trabajo a Supermercado.
    /// </summary>
    public class Menu
    {
        private Supermercado tienda = new Supermercado();

        /// <summary>
        /// Ciclo principal: muestra el menú y ejecuta la opción elegida
        /// hasta que el usuario elige salir.
        /// </summary>
        public void Iniciar()
        {
            bool salir = false;

            while (!salir)
            {
                MostrarMenuPrincipal();
                Console.Write("Elige una opción (1-10): ");
                string? entrada = Console.ReadLine();

                // Si la entrada se cerró (null), terminamos para no quedar en un ciclo infinito.
                if (entrada == null)
                {
                    break;
                }

                Console.WriteLine();

                try
                {
                    switch (entrada.Trim())
                    {
                        case "1": RegistrarProducto(); break;
                        case "2": BuscarProducto(); break;
                        case "3": ActualizarProducto(); break;
                        case "4": EliminarProducto(); break;
                        case "5": MostrarInventario(); break;
                        case "6": AgregarCliente(); break;
                        case "7": AtenderSiguiente(); break;
                        case "8": MostrarClientesEnEspera(); break;
                        case "9": MostrarEstadisticas(); break;
                        case "10":
                            salir = true;
                            MostrarExito("¡Gracias por usar Don Juan Market! Hasta pronto.");
                            break;
                        default:
                            MostrarError("Opción no válida. Escribe un número del 1 al 10.");
                            break;
                    }
                }
                catch (OperacionCancelada)
                {
                    MostrarInfo("Operación cancelada. Regresando al menú principal.");
                }
                catch (ArgumentException ex)
                {
                    // Errores de datos inválidos que lanza Supermercado (código repetido, precio negativo...).
                    MostrarError(ex.Message);
                }
                catch (Exception ex)
                {
                    MostrarError($"Ocurrió un error inesperado: {ex.Message}");
                }

                if (!salir)
                {
                    Pausar();
                }
            }
        }

        // ===================== PANTALLA DEL MENÚ =====================

        private void MostrarMenuPrincipal()
        {
            Console.Clear();
            Escribir("=======================================", ConsoleColor.Cyan);
            Escribir("            Don Juan MARKET", ConsoleColor.Cyan);
            Escribir("    SISTEMA DE GESTIÓN DE SUPERMERCADO", ConsoleColor.Cyan);
            Escribir("=======================================", ConsoleColor.Cyan);
            Console.WriteLine();

            Escribir("----------- INVENTARIO (AVL) -----------", ConsoleColor.Yellow);
            Console.WriteLine();
            Console.WriteLine("1. Registrar producto");
            Console.WriteLine("2. Buscar producto por código");
            Console.WriteLine("3. Actualizar producto");
            Console.WriteLine("4. Eliminar producto");
            Console.WriteLine("5. Mostrar el inventario ordenado");
            Console.WriteLine();

            Escribir("------- ATENCIÓN DE CLIENTES -(HEAP) -----", ConsoleColor.Yellow);
            Console.WriteLine();
            Console.WriteLine("6. - Agregar cliente a la cola");
            Console.WriteLine("7. - Atender siguiente cliente");
            Console.WriteLine("8. - Mostrar clientes en espera");
            Console.WriteLine();

            Escribir("-------------- REPORTES ----------------", ConsoleColor.Yellow);
            Console.WriteLine();
            Console.WriteLine("9. Mostrar estadísticas del sistema");
            Console.WriteLine();

            Escribir("--------------- SISTEMA ----------------", ConsoleColor.Yellow);
            Console.WriteLine();
            Console.WriteLine("10. Salir");
            Console.WriteLine();
            Escribir("=======================================", ConsoleColor.Cyan);
            Escribir("Tip: Si deseas regresar escribe la palabra 'volver' para regresar a este menú.", ConsoleColor.DarkGray);
            Console.WriteLine();
        }

        // ===================== OPCIONES DE INVENTARIO =====================

        private void RegistrarProducto()
        {
            MostrarTitulo("REGISTRAR PRODUCTO");

            string codigo = LeerTexto("Código (ejemplo: PROD001): ");
            string nombre = LeerTexto("Nombre: ");
            string categoria = LeerTexto("Categoría (ejemplo: Alimentos): ");
            double precio = LeerPrecio("Precio Moneda(Q): ");
            int stock = LeerEntero("Cantidad en stock: ", 0);

            tienda.RegistrarProducto(codigo, nombre, categoria, precio, stock);

            Console.WriteLine();
            MostrarExito($"Producto {codigo.ToUpper()} insertado correctamente.");
        }

        private void BuscarProducto()
        {
            MostrarTitulo("BUSCAR PRODUCTO");

            string codigo = LeerTexto("Código a buscar: ");
            Producto? producto = tienda.BuscarProducto(codigo);

            Console.WriteLine();

            if (producto == null)
            {
                MostrarInfo("Producto no encontrado.");
                return;
            }

            MostrarExito("Producto encontrado:");
            Console.WriteLine();
            MostrarFicha(producto);
        }

        private void ActualizarProducto()
        {
            MostrarTitulo("ACTUALIZAR PRODUCTO");

            string codigo = LeerTexto("Código del producto a actualizar: ");

            // Buscamos primero para no pedir todos los datos si el producto no existe.
            Producto? actual = tienda.BuscarProducto(codigo);

            if (actual == null)
            {
                throw new ArgumentException($"No existe un producto con el código {codigo.ToUpper()}.");
            }

            Console.WriteLine();
            Console.WriteLine("Datos actuales:");
            MostrarFicha(actual);

            Console.WriteLine();
            Console.WriteLine("Escribe los datos nuevos (el código no se puede cambiar):");
            string nombre = LeerTexto("Nombre nuevo: ");
            string categoria = LeerTexto("Categoría nueva: ");
            double precio = LeerPrecio("Precio nuevo (Q): ");
            int stock = LeerEntero("Stock nuevo: ", 0);

            tienda.ActualizarProducto(codigo, nombre, categoria, precio, stock);

            Console.WriteLine();
            MostrarExito("Producto actualizado correctamente.");
            Console.WriteLine();

            Producto? nuevo = tienda.BuscarProducto(codigo);
            if (nuevo != null)
            {
                MostrarFicha(nuevo);
            }
        }

        private void EliminarProducto()
        {
            MostrarTitulo("ELIMINAR PRODUCTO");

            string codigo = LeerTexto("Código del producto a eliminar: ");
            tienda.EliminarProducto(codigo);

            Console.WriteLine();
            MostrarExito($"Producto {codigo.ToUpper()} eliminado correctamente.");
        }

        private void MostrarInventario()
        {
            tienda.MostrarInventario();
        }

        // ===================== OPCIONES DE CLIENTES =====================

        private void AgregarCliente()
        {
            MostrarTitulo("AGREGAR CLIENTE A LA COLA");

            string nombre = LeerTexto("Nombre del cliente: ");
            TipoCliente tipo = LeerTipoCliente();
            int productos = LeerEntero("Número de productos que va a comprar: ", 1);

            int posicion = tienda.AgregarCliente(nombre, tipo, productos);

            Console.WriteLine();
            MostrarExito($"Cliente {nombre} agregado correctamente.");
            Console.WriteLine($"Posición estimada en la cola: {posicion}");
        }

        private void AtenderSiguiente()
        {
            Cliente? cliente = tienda.AtenderSiguiente();

            if (cliente == null)
            {
                MostrarInfo("No hay clientes en espera.");
                return;
            }

            Console.WriteLine("Atendiendo cliente:");
            Console.WriteLine();
            Console.WriteLine($"Nombre    : {cliente.Nombre}");
            Console.WriteLine($"Tipo      : {cliente.Tipo}");
            Console.WriteLine($"Productos : {cliente.NumeroProductos}");
            Console.WriteLine();
            MostrarExito("Cliente atendido correctamente.");
        }

        private void MostrarClientesEnEspera()
        {
            tienda.MostrarClientesEnEspera();
        }

        private void MostrarEstadisticas()
        {
            tienda.MostrarEstadisticas();
        }

        // ===================== LECTURA Y VALIDACIÓN =====================

        // Pide un texto que no esté vacío. Si el usuario escribe "volver", cancela la operación.
        private string LeerTexto(string mensaje)
        {
            while (true)
            {
                Console.Write(mensaje);
                string? entrada = Console.ReadLine();

                if (entrada == null || entrada.Trim().Equals("volver", StringComparison.OrdinalIgnoreCase))
                {
                    throw new OperacionCancelada();
                }

                if (!string.IsNullOrWhiteSpace(entrada))
                {
                    return entrada.Trim();
                }

                MostrarError("Este dato no puede estar vacío.");
            }
        }

        // Pide un precio: número decimal mayor que 0. Acepta punto o coma (45.99 o 45,99).
        private double LeerPrecio(string mensaje)
        {
            while (true)
            {
                string texto = LeerTexto(mensaje).Replace(',', '.');

                // TryParse intenta convertir el texto en número y devuelve true o false
                // (sin lanzar error si el texto no es un número).
                if (double.TryParse(texto, NumberStyles.Float, CultureInfo.InvariantCulture, out double precio)
                    && double.IsFinite(precio) && precio > 0)
                {
                    return precio;
                }

                MostrarError("Escribe un número mayor que 0 (ejemplo: 45.99).");
            }
        }

        // Pide un número entero que sea mayor o igual al mínimo indicado.
        private int LeerEntero(string mensaje, int minimo)
        {
            while (true)
            {
                string texto = LeerTexto(mensaje);

                if (int.TryParse(texto, out int numero) && numero >= minimo)
                {
                    return numero;
                }

                MostrarError($"Escribe un número entero mayor o igual a {minimo}.");
            }
        }

        // Pide el tipo de cliente. Acepta el número (1, 2, 3) o el nombre (VIP, ADULTO_MAYOR, REGULAR).
        private TipoCliente LeerTipoCliente()
        {
            Console.WriteLine("Tipo de cliente:");
            Console.WriteLine("  1. VIP");
            Console.WriteLine("  2. Adulto mayor (65 años o más)");
            Console.WriteLine("  3. Regular");

            while (true)
            {
                string texto = LeerTexto("Elige el tipo (1-3): ").ToUpper();

                switch (texto)
                {
                    case "1":
                    case "VIP":
                        return TipoCliente.VIP;
                    case "2":
                    case "ADULTO_MAYOR":
                        return TipoCliente.ADULTO_MAYOR;
                    case "3":
                    case "REGULAR":
                        return TipoCliente.REGULAR;
                }

                MostrarError("Tipo no válido. Escribe 1, 2 o 3.");
            }
        }

        // ===================== AYUDAS DE PANTALLA =====================

        // Muestra los datos de un producto (se usa al buscar y al actualizar).
        private void MostrarFicha(Producto p)
        {
            Console.WriteLine($"Código    : {p.Codigo}");
            Console.WriteLine($"Nombre    : {p.Nombre}");
            Console.WriteLine($"Categoría : {p.Categoria}");
            Console.WriteLine($"Precio    : Q{p.Precio:F2}");
            Console.WriteLine($"Stock     : {p.Stock}");
        }

        private void MostrarTitulo(string titulo)
        {
            Escribir($"=== {titulo} ===", ConsoleColor.Cyan);
            Console.WriteLine();
        }

        private void MostrarExito(string texto)
        {
            Escribir(texto, ConsoleColor.Green);
        }

        private void MostrarError(string texto)
        {
            Escribir($"Error: {texto}", ConsoleColor.Red);
        }

        private void MostrarInfo(string texto)
        {
            Escribir(texto, ConsoleColor.Yellow);
        }

        // Escribe una línea con color y luego regresa al color normal.
        private void Escribir(string texto, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(texto);
            Console.ResetColor();
        }

        // Espera a que el usuario presione Enter, para que alcance a leer el resultado.
        private void Pausar()
        {
            Console.WriteLine();
            Console.Write("Presiona Enter para volver al menú...");
            Console.ReadLine();
        }
    }
}
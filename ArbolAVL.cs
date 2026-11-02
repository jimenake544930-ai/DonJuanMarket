namespace DonJuanMarket
{
    /// <summary>
    /// Árbol AVL que guarda el inventario de productos.
    /// El código del producto es la clave que ordena los nodos:
    /// los códigos menores van a la izquierda y los mayores a la derecha.
    /// Después de cada inserción o eliminación el árbol se balancea con rotaciones.
    /// </summary>
    public class ArbolAVL
    {
        // La raíz es el nodo de más arriba. Si es null, el árbol está vacío.
        private NodoAVL? raiz;

        // Altura total del árbol (sirve para nuestras pruebas).
        public int AlturaArbol => ObtenerAltura(raiz);

        // Código de la raíz (sirve para comprobar que el árbol se balanceó).
        public string? RaizCodigo => raiz?.Dato.Codigo;        /// <summary>
        /// Devuelve todos los productos en una lista, ordenados por código
        /// (recorrido in-orden). No modifica el árbol.
        /// </summary>
        public List<Producto> ObtenerProductos()
        {
            List<Producto> lista = new List<Producto>();    
            RecogerProductos(raiz, lista);
            return lista;
        }

        // Recorrido in-orden que va guardando los productos en la lista.
        private void RecogerProductos(NodoAVL? nodo, List<Producto> lista)
        {
            if (nodo == null)
            {
                return;
            }

            RecogerProductos(nodo.Izquierdo, lista);
            lista.Add(nodo.Dato);
            RecogerProductos(nodo.Derecho, lista);
        }

        /// <summary>
        /// Agrega un producto al inventario.
        /// Devuelve true si se agregó, o false si el código ya existía.
        /// </summary>
        public bool Insertar(Producto producto)
        {
            // Primero revisamos que el código no esté repetido.
            if (Buscar(producto.Codigo) != null)
            {
                return false;
            }

            raiz = InsertarRecursivo(raiz, producto);
            return true;
        }

        // Método recursivo: baja por el árbol hasta encontrar el lugar vacío
        // y, al regresar, balancea cada nodo del camino.
        private NodoAVL InsertarRecursivo(NodoAVL? nodo, Producto producto)
        {
            // Caso base: encontramos un espacio vacío, aquí va el nodo nuevo.
            if (nodo == null)
            {
                return new NodoAVL(producto);
            }

            // Si el código es menor, se va por la izquierda; si no, por la derecha.
            if (string.Compare(producto.Codigo, nodo.Dato.Codigo, StringComparison.Ordinal) < 0)
            {
                nodo.Izquierdo = InsertarRecursivo(nodo.Izquierdo, producto);
            }
            else
            {
                nodo.Derecho = InsertarRecursivo(nodo.Derecho, producto);
            }

            // Al regresar, actualizamos la altura y revisamos si hay que balancear.
            return Balancear(nodo);
        }

        /// <summary>
        /// Busca un producto por su código. Devuelve el producto o null si no existe.
        /// No modifica el árbol.
        /// </summary>
        public Producto? Buscar(string codigo)
        {
            NodoAVL? actual = raiz;

            while (actual != null)
            {
                int comparacion = string.Compare(codigo, actual.Dato.Codigo, StringComparison.Ordinal);

                if (comparacion == 0)
                {
                    return actual.Dato;          // Lo encontramos.
                }
                else if (comparacion < 0)
                {
                    actual = actual.Izquierdo;   // Es menor: vamos a la izquierda.
                }
                else
                {
                    actual = actual.Derecho;     // Es mayor: vamos a la derecha.
                }
            }

            return null;   // Llegamos a un espacio vacío: no existe.
        }

        /// <summary>
        /// Actualiza nombre, categoría, precio y stock de un producto existente.
        /// El código NO se puede cambiar.
        /// Devuelve true si se actualizó, o false si el código no existe.
        /// Lanza ArgumentException si el precio o el stock no son válidos.
        /// </summary>
        public bool Actualizar(string codigo, string nombre, string categoria, double precio, int stock)
        {
            Producto? producto = Buscar(codigo);

            if (producto == null)
            {
                return false;
            }

            // Validamos ANTES de cambiar algo, para no dejar el producto a medias.
            if (precio <= 0)
            {
                throw new ArgumentException("El precio debe ser mayor que 0.");
            }

            if (stock < 0)
            {
                throw new ArgumentException("El stock no puede ser negativo.");
            }

            producto.Nombre = nombre;
            producto.Categoria = categoria;
            producto.Precio = precio;
            producto.Stock = stock;
            return true;
        }

        /// <summary>
        /// Elimina un producto por su código y rebalancea el árbol.
        /// Devuelve true si se eliminó, o false si el código no existe.
        /// </summary>
        public bool Eliminar(string codigo)
        {
            if (Buscar(codigo) == null)
            {
                return false;
            }

            raiz = EliminarRecursivo(raiz, codigo);
            return true;
        }

        // Baja hasta el nodo a eliminar, lo quita según su caso
        // y, al regresar, balancea cada nodo del camino.
        private NodoAVL? EliminarRecursivo(NodoAVL? nodo, string codigo)
        {
            if (nodo == null)
            {
                return null;
            }

            int comparacion = string.Compare(codigo, nodo.Dato.Codigo, StringComparison.Ordinal);

            if (comparacion < 0)
            {
                nodo.Izquierdo = EliminarRecursivo(nodo.Izquierdo, codigo);
            }
            else if (comparacion > 0)
            {
                nodo.Derecho = EliminarRecursivo(nodo.Derecho, codigo);
            }
            else
            {
                // Encontramos el nodo a eliminar. Hay tres casos:

                // Caso 1 y 2: sin hijo izquierdo. Si no tiene hijos (hoja) devuelve null;
                // si solo tiene hijo derecho, ese hijo ocupa su lugar.
                if (nodo.Izquierdo == null)
                {
                    return nodo.Derecho;
                }

                // Caso 2: solo tiene hijo izquierdo, ese hijo ocupa su lugar.
                if (nodo.Derecho == null)
                {
                    return nodo.Izquierdo;
                }

                // Caso 3: tiene dos hijos. Buscamos el sucesor (el menor de la derecha),
                // copiamos sus datos aquí y eliminamos el sucesor de su lugar original.
                NodoAVL sucesor = ObtenerMenor(nodo.Derecho);
                nodo.Dato = sucesor.Dato;
                nodo.Derecho = EliminarRecursivo(nodo.Derecho, sucesor.Dato.Codigo);
            }

            // Al regresar, actualizamos la altura y rebalanceamos si hace falta.
            return Balancear(nodo);
        }

        // El menor de un subárbol es el que está más a la izquierda.
        private NodoAVL ObtenerMenor(NodoAVL nodo)
        {
            while (nodo.Izquierdo != null)
            {
                nodo = nodo.Izquierdo;
            }

            return nodo;
        }

        // Si el nodo es null su altura es 0; si no, la altura que tiene guardada.
        private int ObtenerAltura(NodoAVL? nodo)
        {
            return nodo == null ? 0 : nodo.Altura;
        }

        // La altura de un nodo = 1 + la altura del hijo más alto.
        private void ActualizarAltura(NodoAVL nodo)
        {
            nodo.Altura = 1 + Math.Max(ObtenerAltura(nodo.Izquierdo), ObtenerAltura(nodo.Derecho));
        }

        // Factor de balance = altura izquierda - altura derecha.
        private int ObtenerBalance(NodoAVL? nodo)
        {
            if (nodo == null)
            {
                return 0;
            }

            return ObtenerAltura(nodo.Izquierdo) - ObtenerAltura(nodo.Derecho);
        }

        // Rotación simple a la derecha. Se usa cuando el árbol pesa a la izquierda.
        // El hijo izquierdo (x) sube y el nodo desbalanceado (y) baja a su derecha.
        private NodoAVL RotarDerecha(NodoAVL y)
        {
            NodoAVL x = y.Izquierdo!;
            NodoAVL? subarbolMovido = x.Derecho;

            x.Derecho = y;                 // y baja y queda a la derecha de x.
            y.Izquierdo = subarbolMovido;  // lo que colgaba a la derecha de x pasa a y.

            // Primero se actualiza el que quedó abajo (y) y luego el de arriba (x).
            ActualizarAltura(y);
            ActualizarAltura(x);

            return x;                      // x es la nueva raíz de este pedazo.
        }

        // Rotación simple a la izquierda. Se usa cuando el árbol pesa a la derecha.
        // El hijo derecho (y) sube y el nodo desbalanceado (x) baja a su izquierda.
        private NodoAVL RotarIzquierda(NodoAVL x)
        {
            NodoAVL y = x.Derecho!;
            NodoAVL? subarbolMovido = y.Izquierdo;

            y.Izquierdo = x;               // x baja y queda a la izquierda de y.
            x.Derecho = subarbolMovido;    // lo que colgaba a la izquierda de y pasa a x.

            ActualizarAltura(x);
            ActualizarAltura(y);

            return y;                      // y es la nueva raíz de este pedazo.
        }

        // Revisa el factor de balance del nodo y aplica la rotación que haga falta.
        private NodoAVL Balancear(NodoAVL nodo)
        {
            ActualizarAltura(nodo);
            int balance = ObtenerBalance(nodo);

            // Pesa a la izquierda (balance mayor a 1).
            if (balance > 1)
            {
                // Si el hijo izquierdo pesa a la derecha, es zigzag (Izquierda-Derecha):
                // primero rotamos ese hijo a la izquierda.
                if (ObtenerBalance(nodo.Izquierdo) < 0)
                {
                    nodo.Izquierdo = RotarIzquierda(nodo.Izquierdo!);
                }

                return RotarDerecha(nodo);
            }

            // Pesa a la derecha (balance menor a -1).
            if (balance < -1)
            {
                // Si el hijo derecho pesa a la izquierda, es zigzag (Derecha-Izquierda):
                // primero rotamos ese hijo a la derecha.
                if (ObtenerBalance(nodo.Derecho) > 0)
                {
                    nodo.Derecho = RotarDerecha(nodo.Derecho!);
                }

                return RotarIzquierda(nodo);
            }

            return nodo;   // Balance correcto: no se hace nada.
        }

        /// <summary>
        /// Devuelve los códigos de todos los productos en orden, separados por espacio.
        /// Sirve para comprobar el orden en las pruebas.
        /// </summary>
        public string CodigosEnOrden()
        {
            List<string> codigos = new List<string>();
            RecogerCodigos(raiz, codigos);
            return string.Join(" ", codigos);
        }

        // Recorrido in-orden que va guardando los códigos en una lista.
        private void RecogerCodigos(NodoAVL? nodo, List<string> codigos)
        {
            if (nodo == null)
            {
                return;
            }

            RecogerCodigos(nodo.Izquierdo, codigos);
            codigos.Add(nodo.Dato.Codigo);
            RecogerCodigos(nodo.Derecho, codigos);
        }

        /// <summary>
        /// Muestra todos los productos ordenados por código (recorrido in-orden).
        /// </summary>
        public void MostrarInventario()
        {
            Console.WriteLine("=============================================");
            Console.WriteLine("INVENTARIO DEL SUPERMERCADO");
            Console.WriteLine("=============================================");

            if (raiz == null)
            {
                Console.WriteLine("El inventario está vacío.");
                return;
            }

            Console.WriteLine();
            Console.WriteLine($"{"Código",-10}{"Nombre",-22}{"Categoría",-14}{"Precio",-10}Stock");
            Console.WriteLine("---------------------------------------------------------");

            InOrden(raiz);
        }

        // Recorrido in-orden: primero la izquierda, luego el nodo, luego la derecha.
        private void InOrden(NodoAVL? nodo)
        {
            if (nodo == null)
            {
                return;   // Caso base: no hay nada que mostrar, se termina.
            }

            InOrden(nodo.Izquierdo);

            Producto p = nodo.Dato;
            string precio = $"Q{p.Precio:F2}";
            Console.WriteLine($"{p.Codigo,-10}{p.Nombre,-22}{p.Categoria,-14}{precio,-10}{p.Stock}");

            InOrden(nodo.Derecho);
        }
    }
}
namespace DonJuanMarket
{
    /// <summary>
    /// Cola de prioridad implementada con un Heap (árbol binario guardado en una lista).
    /// Hay UNA sola cola: la raíz (posición 0) siempre es el siguiente cliente a atender.
    /// Orden: VIP, luego ADULTO_MAYOR, luego REGULAR; si empatan, gana el que llegó primero.
    /// </summary>
    public class ColaPrioridad
    {
        // El heap vive en una lista. Para el nodo en la posición i:
        //   padre = (i - 1) / 2   |   hijo izquierdo = 2i + 1   |   hijo derecho = 2i + 2
        private List<Cliente> heap = new List<Cliente>();

        public int Cantidad => heap.Count;
        public bool EstaVacia => heap.Count == 0;

        /// <summary>
        /// Agrega un cliente al final del heap y lo hace "subir" hasta su lugar.
        /// Complejidad O(log n).
        /// </summary>
        public void Agregar(Cliente cliente)
        {
            heap.Add(cliente);
            SubirBurbuja(heap.Count - 1);
        }

        /// <summary>
        /// Saca y devuelve al cliente de mayor prioridad (la raíz).
        /// Devuelve null si no hay clientes. Complejidad O(log n).
        /// </summary>
        public Cliente? Extraer()
        {
            if (heap.Count == 0)
            {
                return null;
            }

            Cliente primero = heap[0];

            // El último de la lista pasa a la raíz y luego "baja" hasta su lugar.
            int ultimo = heap.Count - 1;
            heap[0] = heap[ultimo];
            heap.RemoveAt(ultimo);

            if (heap.Count > 0)
            {
                BajarBurbuja(0);
            }

            return primero;
        }

        /// <summary>
        /// Devuelve al siguiente cliente SIN sacarlo de la cola.
        /// </summary>
        public Cliente? VerSiguiente()
        {
            return heap.Count == 0 ? null : heap[0];
        }

        /// <summary>
        /// Calcula en qué posición será atendido un cliente:
        /// 1 + la cantidad de clientes con más prioridad que él.
        /// </summary>
        public int PosicionEnCola(Cliente cliente)
        {
            int posicion = 1;

            foreach (Cliente otro in heap)
            {
                if (otro != cliente && TieneMasPrioridad(otro, cliente))
                {
                    posicion++;
                }
            }

            return posicion;
        }

        /// <summary>
        /// Muestra los clientes en espera, en el orden en que serán atendidos.
        /// No modifica la cola: trabaja sobre una copia.
        /// </summary>
        public void MostrarEnEspera()
        {
            Console.WriteLine("=== CLIENTES EN ESPERA ===");
            Console.WriteLine();

            if (heap.Count == 0)
            {
                Console.WriteLine("No hay clientes en espera.");
                return;
            }

            Console.WriteLine($"{"Posición",-9}| {"Nombre",-14}| {"Tipo",-15}| Productos");
            Console.WriteLine(new string('-', 53));

            // Una lista del heap NO está ordenada por completo, así que sacamos
            // los clientes uno por uno de una copia para mostrarlos en orden.
            ColaPrioridad copia = new ColaPrioridad();
            copia.heap = new List<Cliente>(heap);

            int posicion = 1;
            while (!copia.EstaVacia)
            {
                Cliente c = copia.Extraer()!;
                Console.WriteLine($"{posicion,-9}| {c.Nombre,-14}| {c.Tipo,-15}| {c.NumeroProductos}");
                posicion++;
            }
        }

        // Regla de prioridad: ¿el cliente a debe ir antes que el cliente b?
        private bool TieneMasPrioridad(Cliente a, Cliente b)
        {
            // 1) Menor número de prioridad = más prioridad (VIP = 1).
            if (a.Prioridad != b.Prioridad)
            {
                return a.Prioridad < b.Prioridad;
            }

            // 2) Misma prioridad: gana el que llegó primero (FIFO).
            if (a.HoraLlegada != b.HoraLlegada)
            {
                return a.HoraLlegada < b.HoraLlegada;
            }

            // 3) Misma hora exacta: gana el de menor número de llegada.
            return a.NumeroLlegada < b.NumeroLlegada;
        }

        // BUBBLE UP: mientras el cliente tenga más prioridad que su padre, se intercambian.
        private void SubirBurbuja(int indice)
        {
            while (indice > 0)
            {
                int padre = (indice - 1) / 2;

                if (TieneMasPrioridad(heap[indice], heap[padre]))
                {
                    Intercambiar(indice, padre);
                    indice = padre;     // Seguimos revisando desde la nueva posición.
                }
                else
                {
                    break;              // Ya está en su lugar.
                }
            }
        }

        // BUBBLE DOWN: el cliente se intercambia con su hijo de mayor prioridad
        // mientras alguno de sus hijos tenga más prioridad que él.
        private void BajarBurbuja(int indice)
        {
            while (true)
            {
                int izquierdo = 2 * indice + 1;
                int derecho = 2 * indice + 2;
                int mejor = indice;

                if (izquierdo < heap.Count && TieneMasPrioridad(heap[izquierdo], heap[mejor]))
                {
                    mejor = izquierdo;
                }

                if (derecho < heap.Count && TieneMasPrioridad(heap[derecho], heap[mejor]))
                {
                    mejor = derecho;
                }

                if (mejor == indice)
                {
                    break;              // Ningún hijo le gana: ya está en su lugar.
                }

                Intercambiar(indice, mejor);
                indice = mejor;
            }
        }

        // Intercambia dos clientes de lugar dentro de la lista.
        private void Intercambiar(int i, int j)
        {
            Cliente temporal = heap[i];
            heap[i] = heap[j];
            heap[j] = temporal;
        }
    }
}
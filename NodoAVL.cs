namespace DonJuanMarket
{
    /// <summary>
    /// Representa un nodo del árbol AVL. Cada nodo guarda un producto,
    /// una referencia a su hijo izquierdo, otra a su hijo derecho y su altura.
    /// </summary>
    public class NodoAVL
    {
        public Producto Dato { get; set; }
        public NodoAVL? Izquierdo { get; set; }
        public NodoAVL? Derecho { get; set; }
        public int Altura { get; set; }

        // Constructor: un nodo nuevo siempre nace como "hoja" (sin hijos),
        // por eso su altura inicial es 1.
        public NodoAVL(Producto dato)
        {
            Dato = dato;
            Izquierdo = null;
            Derecho = null;
            Altura = 1;
        }
    }
}
=====================================================
DON JUAN MARKET
Sistema de gestión de inventario y atención de clientes
=====================================================

Proyecto final - Procesos y Algoritmos II 
segundo semestre 2026
Universidad Regional de Guatemala
Estudiante: Kimberly Jimena Escobar Pacheco
Carné: 2683247
Fecha: 14 de Noviembre 2026


1. DESCRIPCIÓN

Aplicación de consola en C# para la tienda Don Juan Market.
Usa dos estructuras de datos implementadas desde cero:

- Árbol AVL: guarda el inventario de productos. El código del
  producto es la clave de ordenamiento y el árbol se balancea
  solo con rotaciones después de insertar o eliminar.
- Cola de prioridad (Heap): organiza la fila de clientes.
  Atiende primero a los VIP, luego a los Adultos Mayores y
  luego a los Regulares. Si dos clientes tienen la misma
  prioridad, se atiende primero al que llegó antes (FIFO).


2. REQUISITOS

- .NET SDK instalado (el proyecto fue creado con la versión
  10.0).
- Lenguaje: C# (versión 9 o superior).
- Editor recomendado: Visual Studio Code con la extensión
  "C# Dev Kit". También funciona con Visual Studio.
- No usa librerías externas.

Para comprobar que tienes el SDK, abrir la terminal y escribir:

    dotnet --version


3. CÓMO COMPILAR

1. Abrir la terminal.
2. Entrar a la carpeta del proyecto (la que contiene el archivo
   DonJuanMarket.csproj).

    cd ruta/de/la/carpeta/DonJuanMarket

3. escribir:

    dotnet build

Si todo está bien, el resultado termina con "Build succeeded".


4. CÓMO EJECUTAR

Dentro de la misma carpeta, escribir:

    dotnet run

Aparece el menú principal. Para usarlo, escribe el número de
la opción y presiona Enter.

También se puede abrir la carpeta del proyecto en Visual Studio
Code, abrir una terminal desde Terminal > New Terminal y escribir
dotnet run.


5. OPCIONES DEL MENÚ

Inventario (Árbol AVL)
 1. Registrar producto
 2. Buscar producto por código
 3. Actualizar producto
 4. Eliminar producto
 5. Mostrar inventario ordenado

Atención de clientes (Heap)
 6. Agregar cliente a la cola
 7. Atender siguiente cliente
 8. Mostrar clientes en espera

Reportes
 9. Mostrar estadísticas del sistema

Sistema
10. Salir

En cualquier pregunta se puede escribir la palabra "volver"
para cancelar y regresar al menú principal.


6. ESTRUCTURA DEL PROYECTO

Program.cs        Punto de entrada del programa.
Producto.cs       Datos de un producto.
NodoAVL.cs        Nodo del árbol AVL.
ArbolAVL.cs       Árbol AVL con sus operaciones y rotaciones.
Cliente.cs        Datos de un cliente y su prioridad.
ColaPrioridad.cs  Cola de prioridad implementada con un Heap.
Supermercado.cs   Clase que integra el árbol y la cola.
Menu.cs           Menú interactivo y validación de entradas.


7. REPOSITORIO

GitHub: https://github.com/jimenake544930-ai/DonJuanMarket
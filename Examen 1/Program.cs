

using System;
using System.Globalization;

internal class Program
{
    
    const int MAX_PROD = 50;                 
    static string[] nombres = new string[MAX_PROD];
    static double[] precios = new double[MAX_PROD];
    static int[,] datos = new int[MAX_PROD, 2]; 

    
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        CargarProductosDemo();               

        int opcion;
        do
        {
            Console.Clear();
            MostrarMenu();
            opcion = LeerEntero("Seleccione una opción: ", 1, 5);

            switch (opcion)
            {
                case 1: RegistrarVenta(); break;
                case 2: ReabastecerStock(); break;
                case 3: AgregarProducto(); break;
                case 4: MostrarReportes(); break;
                case 5: Console.WriteLine("\n¡Hasta luego!"); break;
            }

            if (opcion != 5)
            {
                Console.WriteLine("\nPresione cualquier tecla para continuar...");
                Console.ReadKey();
            }

        } while (opcion != 5);
    }

  -
    static void MostrarMenu()
    {
        Console.WriteLine("╔══════════════════════════════════════╗");
        Console.WriteLine("║   Sistema de Ventas – Café Campus    ║");
        Console.WriteLine("╚══════════════════════════════════════╝\n");
        Console.WriteLine("1. Registrar venta");
        Console.WriteLine("2. Reabastecer stock");
        Console.WriteLine("3. Agregar nuevo producto");
        Console.WriteLine("4. Reportes y estadísticas");
        Console.WriteLine("5. Salir\n");
    }

    
    static void RegistrarVenta()
    {
        Console.WriteLine("\n--- Registrar venta ---");
        if (nProductos == 0) { Console.WriteLine("No hay productos registrados."); return; }

        ListarProductos();
        int idx = LeerEntero("Ingrese el número de producto: ", 0, nProductos - 1);
        int maxCant = datos[idx, 0];
        if (maxCant == 0) { Console.WriteLine("Sin stock disponible."); return; }

        int cant = LeerEntero($"Cantidad (máx {maxCant}): ", 1, maxCant);

        datos[idx, 0] -= cant;   
        datos[idx, 1] += cant;   

        double monto = cant * precios[idx];
        Console.WriteLine($"✔ Venta registrada: {cant} × {nombres[idx]} = ₡{monto:N0}");
    }

 
    static void ReabastecerStock()
    {
        Console.WriteLine("\n--- Reabastecer stock ---");
        if (nProductos == 0) { Console.WriteLine("No hay productos registrados."); return; }

        ListarProductos();
        int idx = LeerEntero("Seleccione producto a reabastecer: ", 0, nProductos - 1);
        int cant = LeerEntero("Unidades a agregar: ", 1, 10_000);

        datos[idx, 0] += cant;
        Console.WriteLine($"✔ Stock actualizado. Nuevo stock de {nombres[idx]}: {datos[idx, 0]}");
    }

 
    static void AgregarProducto()
    {
        Console.WriteLine("\n--- Agregar nuevo producto ---");
        if (nProductos >= MAX_PROD) { Console.WriteLine("Capacidad máxima alcanzada."); return; }

        Console.Write("Nombre del producto: ");
        string nombre = Console.ReadLine()?.Trim() ?? "";
        if (nombre == "")
        {
            Console.WriteLine("Nombre vacío; operación cancelada."); return;
        }

        double precio = LeerDouble("Precio de venta (₡): ", 100, 100_000);
        int stock = LeerEntero("Stock inicial: ", 0, 10_000);

        
        nombres[nProductos] = nombre;
        precios[nProductos] = precio;
        datos[nProductos, 0] = stock;
        datos[nProductos, 1] = 0;

        Console.WriteLine($"✔ Producto agregado con índice {nProductos}: {nombre} (₡{precio:N0})");
        nProductos++;
    }

    -- Operación 4: Reportes ----------------
    static void MostrarReportes()
    {
        Console.WriteLine("\n--- Reportes y estadísticas ---");
        if (nProductos == 0) { Console.WriteLine("No hay información disponible."); return; }

        double totalIngresos = 0;
        int totalVendidos = 0;
        int idxMasVendido = -1;
        int maxVendidos = -1;

        Console.WriteLine("Producto                       Vendidos    Ingreso");
        Console.WriteLine("-------------------------------------------------------");

        for (int i = 0; i < nProductos; i++)
        {
            int vendidos = datos[i, 1];
            double ingreso = vendidos * precios[i];
            Console.WriteLine($"{i,-2} {nombres[i],-25} {vendidos,8}   ₡{ingreso,10:N0}");

            totalIngresos += ingreso;
            totalVendidos += vendidos;

            if (vendidos > maxVendidos)
            {
                maxVendidos = vendidos;
                idxMasVendido = i;
            }
        }

        Console.WriteLine("-------------------------------------------------------");
        Console.WriteLine($"Total unidades vendidas: {totalVendidos}");
        Console.WriteLine($"Total recaudado:         ₡{totalIngresos:N0}");

        if (idxMasVendido != -1)
        {
            Console.WriteLine($"Producto más vendido:    {nombres[idxMasVendido]} ({maxVendidos} unidades)");
        }
    }

    
    static void ListarProductos()
    {
        Console.WriteLine("\nIdx Producto                 Stock Precio");
        Console.WriteLine("----------------------------------------------");
        for (int i = 0; i < nProductos; i++)
        {
            Console.WriteLine($"{i,-3} {nombres[i],-22} {datos[i, 0],5}  ₡{precios[i],8:N0}");
        }
    }

    static int LeerEntero(string msg, int min, int max)
    {
        int valor;
        bool ok;
        do
        {
            Console.Write(msg);
            ok = int.TryParse(Console.ReadLine(), out valor) && valor >= min && valor <= max;
            if (!ok) Console.WriteLine($"⚠ Ingrese un número entre {min} y {max}.");
        } while (!ok);
        return valor;
    }

    static double LeerDouble(string msg, double min, double max)
    {
        double valor;
        bool ok;
        do
        {
            Console.Write(msg);
            ok = double.TryParse(Console.ReadLine(), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out valor)
                 && valor >= min && valor <= max;
            if (!ok) Console.WriteLine($"⚠ Ingrese un valor entre {min} y {max}.");
        } while (!ok);
        return valor;
    }

    static void CargarProductosDemo()
    {
        
        string[] demoNombres = { "Café negro", "Capuchino", "Sándwich", "Brownie", "CocaCola" };
        double[] demoPrecios = { 850, 1150, 1500, 1000, 1000 };
        int[] demoStock = { 20, 15, 10, 12, 18 };

        for (int i = 0; i < demoNombres.Length; i++)
        {
            nombres[i] = demoNombres[i];
            precios[i] = demoPrecios[i];
            datos[i, 0] = demoStock[i];
            datos[i, 1] = 0;
        }
        nProductos = demoNombres.Length;
    }
}

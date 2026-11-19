// Cifrado vigenere

string texto = "Si la vida me da de nuevo el placer";
string clave = "OtroAtardecer"; // clave: una palabra, solo cuentan las letras
string alfabeto = "ABCDEFGHIJKLMNÑOPQRSTUVWXYZ"; // para el alfabeto de 26 letras quita la Ñ

// Permite que la consola muestre bien la Ñ
Console.OutputEncoding = System.Text.Encoding.UTF8;

// Validar los datos antes de cifrar
if (texto == null || texto == "")
{
    Console.WriteLine("Error: escribe un texto para cifrar.");
    return;
}

if (alfabeto == null || alfabeto == "")
{
    Console.WriteLine("Error: el alfabeto no puede estar vacio.");
    return;
}

if (clave == null)
{
    Console.WriteLine("Error: escribe una clave.");
    return;
}

// La clave se pasa a mayusculas y se quitan los simbolos
string claveLimpia = LimpiarClave(clave, alfabeto);

if (claveLimpia == "")
{
    Console.WriteLine("Error: la clave debe tener al menos una letra del alfabeto.");
    return;
}

// Cifrar
string cifrado = Cifrar(texto, claveLimpia, alfabeto);

Console.WriteLine("Texto original: " + texto);

Console.WriteLine("Clave utilizada: " + claveLimpia);
Console.WriteLine("Texto cifrado: " + cifrado);

// Descifrar el texto que acabamos de cifrar
string descifrado = Descifrar(cifrado, claveLimpia, alfabeto);

Console.WriteLine("Texto descifrado: " + descifrado);

// Cifrar es sumar el valor de la letra de la clave a cada letra del texto
static string Cifrar(string texto, string clave, string alfabeto)
{
    return Transformar(texto, clave, alfabeto, 1);
}

// Descifrar es restar el valor de la letra de la clave a cada letra del texto
static string Descifrar(string texto, string clave, string alfabeto)
{
    return Transformar(texto, clave, alfabeto, -1);
}

// Deja solo las letras de la clave que estan en el alfabeto, en mayusculas
static string LimpiarClave(string clave, string alfabeto)
{

    string resultado = "";

    for (int i = 0; i < clave.Length; i++)
    {
        char letra = char.ToUpperInvariant(clave[i]);

        if (alfabeto.Contains(letra))
        {
            resultado = resultado + letra;
        }
    }
    return resultado;
}

/*
 Cada letra del texto se desplaza segun la letra de la clave, que se repite en bucle.
 La clave solo avanza cuando el texto tiene una letra; los espacios, numeros y signos se dejan igual. Respeta mayusculas y minusculas.
 sentido = 1 para cifrar, sentido = -1 para descifrar
*/
static string Transformar(string texto, string clave, string alfabeto, int sentido)
{
    int cantidad = alfabeto.Length;
    int posicionClave = 0;
    string resultado = "";

    for (int i = 0; i < texto.Length; i++)
    {
        char letra = texto[i];
        int posicion = alfabeto.IndexOf(char.ToUpperInvariant(letra));

        if (posicion == -1)
        {
            // No es una letra del alfabeto, se deja igual y la clave no avanza
            resultado = resultado + letra;
        }
        else
        {
            char letraClave = clave[posicionClave % clave.Length];
            int desplazamiento = alfabeto.IndexOf(letraClave);
            posicionClave = posicionClave + 1;

            int nuevaPosicion = ((posicion + sentido * desplazamiento) % cantidad + cantidad) % cantidad;
            char nueva = alfabeto[nuevaPosicion];

            if (char.IsLower(letra))
            {
                nueva = char.ToLowerInvariant(nueva);
            }

            resultado = resultado + nueva;
        }
    }

    return resultado;
}

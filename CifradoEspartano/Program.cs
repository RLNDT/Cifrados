// CIFRADO ESPARTANO

string texto = "Hola mundo, este es un cifrado espartano";
int caras = 5; // clave: numero de caras (filas) de la vara, minimo 2

// Validar los datos antes de cifrar
if (texto == null || texto == "")
{
    Console.WriteLine("Error: escribe un texto para cifrar.");
    return;
}

if (caras < 2)
{
    Console.WriteLine("Error: la clave debe ser 2 o mayor.");
    return;
}

// Cifrar
string cifrado = Cifrar(texto, caras);

Console.WriteLine("Texto original: " + texto);
Console.WriteLine("Clave (caras de la vara): " + caras);

Console.WriteLine("Texto cifrado: " + cifrado);

// Descifrar el texto que acabamos de cifrar
string descifrado = Descifrar(cifrado, caras);

Console.WriteLine("Texto descifrado: " + descifrado);

// Escribe el texto por filas (una por cada cara de la vara) y lo lee por columnas
static string Cifrar(string texto, int caras)
{
    int largo = texto.Length;
    int columnas = (largo + caras - 1) / caras;
    string resultado = "";

    for (int col = 0; col < columnas; col++)
    {
        for (int fila = 0; fila < caras; fila++)
        {
            int indice = fila * columnas + col;

            // La ultima fila puede quedar incompleta, esas posiciones se saltan
            if (indice < largo)
            {
                resultado = resultado + texto[indice];
            }
        }
    }

    return resultado;
}

// Recorre la vara en el mismo orden que Cifrar y devuelve cada letra a su lugar original
static string Descifrar(string cifrado, int caras)
{
    int largo = cifrado.Length;
    int columnas = (largo + caras - 1) / caras;
    char[] letras = new char[largo];

    int siguiente = 0;

    for (int col = 0; col < columnas; col++)
    {
        for (int fila = 0; fila < caras; fila++)
        {
            int indice = fila * columnas + col;

            if (indice < largo)
            {
                letras[indice] = cifrado[siguiente];
                siguiente = siguiente + 1;

            }
        }
    }

    return new string(letras);
}

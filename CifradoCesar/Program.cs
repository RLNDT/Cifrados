// Cifrado de cesar

string texto = "Hello World";
int desplazamiento = 5; // clave: cuantas letras se desplaza cada letra
string alfabeto = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"; //para el alfabeto de 26 letras quita la Ñ

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

// Cifrar
string cifrado = Cifrar(texto, desplazamiento, alfabeto);

Console.WriteLine("Texto original: " + texto);
Console.WriteLine("Desplazamiento: " + desplazamiento);
Console.WriteLine("Texto cifrado: " + cifrado);

// Descifrar el texto que acabamos de cifrar
string descifrado = Descifrar(cifrado, desplazamiento, alfabeto);

Console.WriteLine("Texto descifrado: " + descifrado);

// Cifrar es desplazar las letras hacia adelante
static string Cifrar(string texto, int desplazamiento, string alfabeto)
{
    return Transformar(texto, desplazamiento, alfabeto);
}

// Descifrar es desplazar las letras hacia atras
static string Descifrar(string texto, int desplazamiento, string alfabeto)
{
    return Transformar(texto, -desplazamiento, alfabeto);
}


// Desplaza cada letra del alfabeto. Respeta mayusculas y minusculas, y deja igual los numeros, espacios y signos. Los acentos se pierden en el proceso
static string Transformar(string texto, int desplazamiento, string alfabeto)
{
    int cantidad = alfabeto.Length;

    // Deja el desplazamiento entre 0 y cantidad - 1 (tambien sirve si es negativo)
    int d = ((desplazamiento % cantidad) + cantidad) % cantidad;

    string resultado = "";

    for (int i = 0; i < texto.Length; i++)
    {
        char original = texto[i];
        char letra = QuitarAcento(original, alfabeto);
        int posicion = alfabeto.IndexOf(char.ToUpperInvariant(letra));

        if (posicion == -1)
        {
            // No es una letra del alfabeto, se deja igual
            resultado = resultado + original;
        }
        else
        {
            char nueva = alfabeto[(posicion + d) % cantidad];

            if (char.IsLower(letra))
            {
                nueva = char.ToLowerInvariant(nueva);
            }

            resultado = resultado + nueva;
        }
    }
    return resultado;
}

// Cambia las letras con acento por su letra normal y si la letra ya esta en el alfabeto (por ejemplo la Ñ) se queda igual
static char QuitarAcento(char c, string alfabeto)
{
    if (alfabeto.Contains(char.ToUpperInvariant(c)) || char.IsSurrogate(c))
    {
        return c;
    }

    string descompuesto = c.ToString().Normalize(System.Text.NormalizationForm.FormD);
    return descompuesto[0];
}

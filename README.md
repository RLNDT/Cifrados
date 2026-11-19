# Cifrados clásicos en C# (.NET 10)

Tres programas de consola independientes, cada uno en su propia carpeta:

| Carpeta | Cifrado | Clave |
|---|---|---|
| `CifradoEspartano` | Espartano (escítala) | Un número: las caras (filas) de la vara |
| `CifradoCesar` | César | Un número: cuántas letras se desplaza cada letra |
| `CifradoVigenere` | Vigenère | Una palabra |

Cada programa cifra un texto, muestra el resultado y después lo descifra para comprobar que se recupera el texto original.

## Requisitos

- [.NET 10 SDK](https://dotnet.microsoft.com/download). Para comprobar que está instalado:

```
dotnet --version
```

## Cómo ejecutar

Desde la carpeta que contiene este archivo, entra a la carpeta del cifrado que quieras y ejecuta `dotnet run`:

```
cd CifradoEspartano
dotnet run
```

```
cd CifradoCesar
dotnet run
```

```
cd CifradoVigenere
dotnet run
```

Para volver a la carpeta anterior usa `cd ..`.

La primera vez, `dotnet run` compila el proyecto y crea las carpetas `bin` y `obj`. Es normal, no hay que tocarlas.

## Cómo cambiar el texto y la clave

Los programas no piden datos por consola. Para probar otro mensaje, abre el `Program.cs` del cifrado, cambia las variables que están al inicio del archivo, guarda y vuelve a ejecutar `dotnet run`.

| Programa | Variables al inicio de `Program.cs` |
|---|---|
| Espartano | `texto`, `caras` |
| César | `texto`, `desplazamiento`, `alfabeto` |
| Vigenère | `texto`, `clave`, `alfabeto` |

Si el texto está vacío, o la clave no es válida, el programa muestra un mensaje de error y termina.

---

## 1. Cifrado espartano (escítala)

**Idea.** Los espartanos enrollaban una tira de cuero alrededor de una vara y escribían el mensaje a lo largo de la vara. Al desenrollar la tira, las letras quedaban desordenadas, y solo se podía leer volviéndola a enrollar en una vara del mismo grosor. Es un cifrado de **transposición**: no cambia las letras, solo las reordena.

**Cómo funciona en el programa.**

1. La clave `caras` es el número de caras (filas) de la vara.
2. El texto se escribe **por filas** en una rejilla con ese número de filas.
3. El texto cifrado se obtiene leyendo la rejilla **por columnas**, de arriba hacia abajo.

**Ejemplo** con el texto `ATAQUEALAMANECER` y la clave `4`:

```
A T A Q
U E A L
A M A N
E C E R
```

Leyendo las columnas: `AUAE` `TEMC` `AAAE` `QLNR`, es decir, **`AUAETEMCAAAEQLNR`**.

**Descifrar.** Se hace el recorrido contrario: con la misma clave se vuelven a colocar las letras del texto cifrado, columna por columna, y se lee la rejilla por filas. Sin conocer la clave correcta, el texto sale desordenado.

**Detalles del programa.**

- Se reordenan todos los caracteres, incluidos los espacios y los signos.
- No se añade relleno: si la última fila queda incompleta, las posiciones que faltan se saltan, y así el descifrado siempre recupera el texto original.
- La clave debe ser 2 o mayor. Si es igual o mayor que la longitud del texto, el texto no cambia.

**Salida con los valores por defecto** (`texto = "Hola mundo, este es un cifrado espartano"`, `caras = 5`):

```
Texto original: Hola mundo, este es un cifrado espartano
Clave (caras de la vara): 5
Texto cifrado: Hd isooefpl,sraa  ar eudtmsnoaut  nneceo
Texto descifrado: Hola mundo, este es un cifrado espartano
```

---

## 2. Cifrado César

**Idea.** Se atribuye a Julio César. Cada letra del mensaje se sustituye por la que está un número fijo de posiciones más adelante en el alfabeto. Es un cifrado de **sustitución**. Con desplazamiento 3, la `A` pasa a ser `D`, la `B` pasa a ser `E`, y así sucesivamente. Al llegar al final del alfabeto se vuelve a empezar por la `A`.

**Cómo funciona en el programa.**

1. La clave `desplazamiento` indica cuántas posiciones se mueve cada letra.
2. Para **cifrar** se avanza ese número de posiciones. Para **descifrar** se retrocede el mismo número.
3. Las mayúsculas y minúsculas se respetan. Todo lo que no es una letra del alfabeto (números, espacios, signos) queda igual.
4. El desplazamiento puede ser mayor que el alfabeto o negativo. Por ejemplo, `29` es lo mismo que `3` en un alfabeto de 26 letras.
5. Las letras con tilde se convierten en la letra sin tilde (`á` pasa a `a`) antes de cifrarse, así que las tildes se pierden.

**Ejemplo** con el texto `HOLA` y el desplazamiento `3` (alfabeto de 26 letras):

| Letra | H | O | L | A |
|---|---|---|---|---|
| Cifrada | K | R | O | D |

El resultado es **`KROD`**. Para descifrar se retrocede 3 posiciones: `KROD` pasa a `HOLA`.

**El alfabeto.** La variable `alfabeto` define con qué letras se trabaja. En este programa viene con las 26 letras estándar. Si se le agrega la `Ñ` (`"ABCDEFGHIJKLMNÑOPQRSTUVWXYZ"`), el alfabeto es el español de 27 letras y los resultados cambian. Hay que usar el mismo alfabeto para cifrar y para descifrar.

**Salida con los valores por defecto** (`texto = "Hello World"`, `desplazamiento = 5`):

```
Texto original: Hello World
Desplazamiento: 5
Texto cifrado: Mjqqt Btwqi
Texto descifrado: Hello World
```

**Limitación.** Solo existen tantas claves como letras tiene el alfabeto, así que se puede romper probando todas las posibilidades.

---

## 3. Cifrado Vigenère

**Idea.** Es una mejora del César: en vez de un único desplazamiento, se usa una **palabra clave**, y cada letra de la clave da un desplazamiento distinto. Así, la misma letra del mensaje puede cifrarse de formas diferentes según su posición.

**Cómo funciona en el programa.**

1. Cada letra del alfabeto tiene un número: `A = 0`, `B = 1`, `C = 2`, etc.
2. La clave se limpia: se pasa a mayúsculas y se descartan los caracteres que no están en el alfabeto. Por ejemplo, `"OtroAtardecer"` pasa a `OTROATARDECER`.
3. La clave se repite en bucle y se alinea con las letras del texto.
4. Para **cifrar** se suma el número de la letra de la clave al de la letra del texto. Para **descifrar** se resta. Si el resultado pasa del final del alfabeto, se vuelve a empezar por la `A`.
5. La clave solo avanza cuando el texto tiene una letra. Los espacios, números y signos se dejan igual y no consumen letras de la clave.
6. Se respetan las mayúsculas y minúsculas. Los caracteres que no están en el alfabeto, como las letras con tilde, se dejan sin cambiar.

**Ejemplo** con el texto `HOLA` y la clave `SOL` (alfabeto español de 27 letras, con `A = 0` y `Z = 26`):

| Texto | H (7) | O (15) | L (11) | A (0) |
|---|---|---|---|---|
| Clave (se repite) | S (19) | O (15) | L (11) | S (19) |
| Suma | 26 | 30 → 3 | 22 | 19 |
| Cifrada | Z | D | V | S |

El resultado es **`ZDVS`**. Para descifrar se resta el valor de la clave letra por letra: `ZDVS` pasa a `HOLA`.

**El alfabeto.** En este programa `alfabeto` viene con las 27 letras del español (incluida la `Ñ`). Si se quita la `Ñ`, se usa el alfabeto estándar de 26 letras, que es el de la mayoría de las calculadoras en línea, y los resultados cambian. Hay que usar el mismo alfabeto para cifrar y para descifrar.

**Salida con los valores por defecto** (`texto = "Si la vida me da de nuevo el placer"`, `clave = "OtroAtardecer"`):

```
Texto original: Si la vida me da de nuevo el placer
Clave utilizada: OTROATARDECER
Texto cifrado: Hb co vbdr oi fe us gmsvi ec socgvg
Texto descifrado: Si la vida me da de nuevo el placer
```

**Limitación.** Es más difícil de romper que el César, pero con un texto largo y una clave corta también se puede descifrar sin conocer la clave.

---

## Notas

- La carpeta `bin` y la carpeta `obj` las genera .NET al compilar. No hace falta incluirlas al entregar el trabajo: bastan los archivos `Program.cs` y `.csproj` de cada proyecto.
- Si la `Ñ` no se ve bien en la consola, los programas de César y Vigenère ya incluyen la línea `Console.OutputEncoding = System.Text.Encoding.UTF8;` para mostrarla correctamente.

using System.Collections.Generic;
using System.Drawing;
using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace COMPILADOR_JONATHAN_MADELENE
{
    public partial class Form1 : Form
    {
        Compilador c;
        public Form1()
        {
            InitializeComponent();
            c = new Compilador();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Dentro de la clase Form1
        private void button2_Click(object sender, EventArgs e)
        {
            string entrada = richTextBox1.Text;
            ProcessInput(entrada);
            
        }


        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            HighlighResevedWords();
            HighlighNonResevedWords();

        }

        //Reconoce palabras reservadas en c.KeyWordPattern
        private void HighlighResevedWords()
        {
            // Guarda el estado actual de la selecci�n para restaurarlo despu�s de aplicar el resaltado
            int inicioSeleccion = richTextBox1.SelectionStart;
            int longitudSeleccion = richTextBox1.SelectionLength;
            Color colorOriginal = richTextBox1.SelectionColor;

            // Creamos una expresi�n regular para buscar las palabras reservadas
            Regex regex = new Regex(c.keywordPattern);

            // Iteramos sobre cada coincidencia encontrada en el texto
            foreach (Match match in regex.Matches(richTextBox1.Text))
            {
                // Resaltamos la palabra reservada en verde
                richTextBox1.Select(match.Index, match.Length);
                richTextBox1.SelectionColor = Color.Green;
            }

            // Restauramos el estado de la selecci�n al estado original
            richTextBox1.SelectionStart = inicioSeleccion;
            richTextBox1.SelectionLength = longitudSeleccion;
            richTextBox1.SelectionColor = colorOriginal;
        }
        private void HighlighNonResevedWords()
        {
            int inicioSeleccion = richTextBox1.SelectionStart;
            int longitudSeleccion = richTextBox1.SelectionLength;
            Color colorOriginal = richTextBox1.SelectionColor;

            // Creamos una expresi�n regular que busca todas las palabras que no coincidan con las palabras reservadas
            string regexPattern = $@"\b(?!{c.keywordPattern}\b)\w+\b";
            Regex regex = new Regex(regexPattern);

            // Iteramos sobre cada coincidencia encontrada en el texto
            foreach (Match match in regex.Matches(richTextBox1.Text))
            {
                // Resalta la palabra no reservada en rojo
                richTextBox1.Select(match.Index, match.Length);
                richTextBox1.SelectionColor = Color.Red;
            }

            // Restauramos el estado de la selecci�n al estado original
            richTextBox1.SelectionStart = inicioSeleccion;
            richTextBox1.SelectionLength = longitudSeleccion;
            richTextBox1.SelectionColor = colorOriginal;
        }
        private void Operations(string entrada)
        {
            richTextBox1.Text = entrada;
            // Diccionario para almacenar los valores asignados a cada identificador
            Dictionary<string, int> variables = new Dictionary<string, int>();

            // Expresi�n regular para buscar declaraciones de enteros y expresiones de asignaci�n
            string patronAsignacion = @"INTEGER\s+(\w+)\s*=\s*(\d+)\s*;";
            string patronOperacion = @"INTEGER\s+(\w+)\s*=\s*(\w+)\s*(\+|-|\*|/)\s*(\w+)\s*;";
            string patronPrint = @"PRINT\s+(\w+)\s*;";

            // Buscar declaraciones de enteros y asignar valores a las variables
            MatchCollection matchesAsignacion = Regex.Matches(entrada, patronAsignacion);
            foreach (Match match in matchesAsignacion)
            {
                string identificador = match.Groups[1].Value;
                int valor = int.Parse(match.Groups[2].Value);
                variables[identificador] = valor;
            }

            // Evaluar expresiones de asignaci�n y realizar operaciones
            MatchCollection matchesOperacion = Regex.Matches(entrada, patronOperacion);
            foreach (Match match in matchesOperacion)
            {
                string identificador = match.Groups[1].Value;
                string operando1 = match.Groups[2].Value;
                string operador = match.Groups[3].Value;
                string operando2 = match.Groups[4].Value;

                int valor1 = variables.ContainsKey(operando1) ? variables[operando1] : int.Parse(operando1);
                int valor2 = variables.ContainsKey(operando2) ? variables[operando2] : int.Parse(operando2);

                int resultado = 0;
                switch (operador)
                {
                    case "+":
                        resultado = valor1 + valor2;
                        break;
                    case "-":
                        resultado = valor1 - valor2;
                        break;
                    case "*":
                        resultado = valor1 * valor2;
                        break;
                    case "/":
                        resultado = valor1 / valor2;
                        break;
                }

                variables[identificador] = resultado;
            }

            // Imprimir el valor de la variable especificada
            Match matchPrint = Regex.Match(entrada, patronPrint);
            if (matchPrint.Success)
            {
                string identificador = matchPrint.Groups[1].Value;
                if (variables.ContainsKey(identificador))
                {
                    MessageBox.Show($"El valor de {identificador} es: {variables[identificador]}");
                }
                else
                {
                    MessageBox.Show($"Variable {identificador} no encontrada.");
                }
            }
        }
        private void OperationsFloat(string entrada)
        {
            richTextBox1.Text = entrada;
            // Diccionario para almacenar los valores asignados a cada identificador
            Dictionary<string, double> variables = new Dictionary<string, double>();

            // Expresi�n regular para buscar declaraciones de n�meros flotantes y expresiones de asignaci�n
            string patronAsignacion = @"FLOAT\s+(\w+)\s*=\s*([\d.]+)\s*;";
            string patronOperacion = @"FLOAT\s+(\w+)\s*=\s*(\w+)\s*(\+|-|\*|/)\s*(\w+)\s*;";
            string patronPrint = @"PRINT\s+(\w+)\s*;";

            // Buscar declaraciones de n�meros flotantes y asignar valores a las variables
            MatchCollection matchesAsignacion = Regex.Matches(entrada, patronAsignacion);
            foreach (Match match in matchesAsignacion)
            {
                string identificador = match.Groups[1].Value;
                double valor = double.Parse(match.Groups[2].Value);
                variables[identificador] = valor;
            }

            // Evaluar expresiones de asignaci�n y realizar operaciones
            MatchCollection matchesOperacion = Regex.Matches(entrada, patronOperacion);
            foreach (Match match in matchesOperacion)
            {
                string identificador = match.Groups[1].Value;
                string operando1 = match.Groups[2].Value;
                string operador = match.Groups[3].Value;
                string operando2 = match.Groups[4].Value;

                double valor1 = variables.ContainsKey(operando1) ? variables[operando1] : double.Parse(operando1);
                double valor2 = variables.ContainsKey(operando2) ? variables[operando2] : double.Parse(operando2);

                double resultado = 0;
                switch (operador)
                {
                    case "+":
                        resultado = valor1 + valor2;
                        break;
                    case "-":
                        resultado = valor1 - valor2;
                        break;
                    case "*":
                        resultado = valor1 * valor2;
                        break;
                    case "/":
                        if (valor2 != 0)
                            resultado = valor1 / valor2;
                        else
                            MessageBox.Show("Divisi�n por cero no permitida.");
                        break;
                }

                variables[identificador] = resultado;
            }

            // Imprimir el valor de la variable especificada
            Match matchPrint = Regex.Match(entrada, patronPrint);
            if (matchPrint.Success)
            {
                string identificador = matchPrint.Groups[1].Value;
                if (variables.ContainsKey(identificador))
                {
                    MessageBox.Show($"El valor de {identificador} es: {variables[identificador]}");
                }
                else
                {
                    MessageBox.Show($"Variable {identificador} no encontrada.");
                }
            }
        }
        private void Concat(string entrada)
        {
            entrada = richTextBox1.Text;
            // Diccionario para almacenar los valores asignados a cada identificador
            Dictionary<string, object> variables = new Dictionary<string, object>();

            // Expresi�n regular para buscar declaraciones de cadenas, enteros y expresiones de concatenaci�n
            string patronAsignacionString = @"STRING\s+(\w+)\s*=\s*""([^""]+)""\s*;";
            string patronAsignacionInteger = @"INTEGER\s+(\w+)\s*=\s*(\d+)\s*;";
            string patronConcatenacion = @"PRINT\s+(\w+)\s*\+\s*(\w+)\s*;";

            // Buscar declaraciones de cadenas y asignar valores a las variables
            MatchCollection matchesAsignacionString = Regex.Matches(entrada, patronAsignacionString);
            foreach (Match match in matchesAsignacionString)
            {
                string identificador = match.Groups[1].Value;
                string valor = match.Groups[2].Value;
                variables[identificador] = valor;
            }

            // Buscar declaraciones de enteros y asignar valores a las variables
            MatchCollection matchesAsignacionInteger = Regex.Matches(entrada, patronAsignacionInteger);
            foreach (Match match in matchesAsignacionInteger)
            {
                string identificador = match.Groups[1].Value;
                int valor = int.Parse(match.Groups[2].Value);
                variables[identificador] = valor;
            }

            // Evaluar expresiones de concatenaci�n y realizar operaciones
            MatchCollection matchesConcatenacion = Regex.Matches(entrada, patronConcatenacion);
            foreach (Match match in matchesConcatenacion)
            {
                string identificador1 = match.Groups[1].Value;
                string identificador2 = match.Groups[2].Value;

                object valor1 = variables.ContainsKey(identificador1) ? variables[identificador1] : identificador1;
                object valor2 = variables.ContainsKey(identificador2) ? variables[identificador2] : identificador2;

                string resultado = valor1.ToString() + valor2.ToString();

                MessageBox.Show("El resultado de la concatenaci�n es: " + resultado);
            }
        }
        private void ProcessInput(string entrada)
        {
            // Comprobar si la entrada contiene una operación aritmética
            if (Regex.IsMatch(entrada, @"INTEGER\s+\w+\s*=\s*\w+\s*[+\-*/]\s*\w+\s*;"))
            {
                Operations(entrada);
            }
            else if (Regex.IsMatch(entrada, @"FLOAT\s+(\w+)\s*=\s*(\w+)\s*(\+|-|\*|/)\s*(\w+)\s*;"))
            {
                OperationsFloat(entrada);
            }
            else if (Regex.IsMatch(entrada, @"DECIMAL\s+(\w+)\s*=\s*(\w+)\s*(\+|-|\*|/)\s*(\w+)\s*;"))
            {
                OperationsDecimal(entrada);
            }
            // Comprobar si la entrada contiene una concatenación de cadenas
            else if (Regex.IsMatch(entrada, @"PRINT\s+\w+\s*\+\s*\w+\s*;"))
            {
                Concat(entrada);
            }
            else if (Regex.IsMatch(entrada, @"FOR\s*\(\s*INTEGER\s+([a-zA-Z_]\w*)\s*=\s*(\d+)\s*;\s*([a-zA-Z_]\w*)\s*(<|<=)\s*(\d+)\s*;\s*([a-zA-Z_]\w*)\s*(\+\+|--)?\s*\)\s*\{\s*PRINT\s*([a-zA-Z_]\w*)\s*;\s*\}"))
            {
                ForMethod(entrada);
            }
            // Comprobar si la entrada contiene una estructura IF-ELSE
            else if (Regex.IsMatch(entrada, @"(INTEGER|DECIMAL|FLOAT)\s+(\w+)\s*=\s*([-+]?\d*\.?\d+(?:[eE][-+]?\d+)?)\s*;\s*(INTEGER|DECIMAL|FLOAT)\s+(\w+)\s*=\s*([-+]?\d*\.?\d+(?:[eE][-+]?\d+)?)\s*;\s*IF\s+(\w+)\s*([><=])\s*(\w+)\s*{\s*PRINT\s*""(.*?)"";\s*}\s*ELSE\s*{\s*PRINT\s*""(.*?)"";\s*}"))
            {
                ValidateIfExp(entrada);
            }
            else
            {
                MessageBox.Show("La entrada no es una operación aritmética, una concatenación de cadenas ni una estructura IF-ELSE válida.");
            }
        }
        private static void ForMethod(string entrada)
        {
            string patron = @"FOR\s*\(\s*INTEGER\s+([a-zA-Z_]\w*)\s*=\s*(\d+)\s*;\s*([a-zA-Z_]\w*)\s*(<|<=)\s*(\d+)\s*;\s*([a-zA-Z_]\w*)\s*(\+\+|--)?\s*\)\s*\{\s*PRINT\s*([a-zA-Z_]\w*)\s*;\s*\}";

            Regex regex = new Regex(patron, RegexOptions.Singleline);
            MatchCollection matches = regex.Matches(entrada);

            foreach (Match match in matches)
            {
                string variable = match.Groups[1].Value;
                int inicio = int.Parse(match.Groups[2].Value);
                string operador = match.Groups[4].Value;
                int condicion = int.Parse(match.Groups[5].Value);
                string aumento = match.Groups[7].Value;
                string imprimir = match.Groups[8].Value;

                Console.WriteLine($"Bucle for encontrado: {variable} desde {inicio} hasta {(operador == "<" ? condicion - 1 : condicion)} con incremento {(aumento == "++" ? 1 : -1)}");
                Console.WriteLine("Resultados:");
                for (int i = inicio; (operador == "<" ? i < condicion : i <= condicion); i += (aumento == "++" ? 1 : -1))
                {
                    MessageBox.Show(i.ToString());
                }
            }
        }
        private void OperationsDecimal(string entrada)
        {
            richTextBox1.Text = entrada;
            // Diccionario para almacenar los valores asignados a cada identificador
            Dictionary<string, decimal> variables = new Dictionary<string, decimal>();

            // Expresi�n regular para buscar declaraciones de decimales y expresiones de asignaci�n
            string patronAsignacion = @"DECIMAL\s+(\w+)\s*=\s*([\d.]+)\s*;";
            string patronOperacion = @"DECIMAL\s+(\w+)\s*=\s*(\w+)\s*(\+|-|\*|/)\s*(\w+)\s*;";
            string patronPrint = @"PRINT\s+(\w+)\s*;";

            // Buscar declaraciones de decimales y asignar valores a las variables
            MatchCollection matchesAsignacion = Regex.Matches(entrada, patronAsignacion);
            foreach (Match match in matchesAsignacion)
            {
                string identificador = match.Groups[1].Value;
                decimal valor = decimal.Parse(match.Groups[2].Value);
                variables[identificador] = valor;
            }

            // Evaluar expresiones de asignaci�n y realizar operaciones
            MatchCollection matchesOperacion = Regex.Matches(entrada, patronOperacion);
            foreach (Match match in matchesOperacion)
            {
                string identificador = match.Groups[1].Value;
                string operando1 = match.Groups[2].Value;
                string operador = match.Groups[3].Value;
                string operando2 = match.Groups[4].Value;

                decimal valor1 = variables.ContainsKey(operando1) ? variables[operando1] : decimal.Parse(operando1);
                decimal valor2 = variables.ContainsKey(operando2) ? variables[operando2] : decimal.Parse(operando2);

                decimal resultado = 0;
                switch (operador)
                {
                    case "+":
                        resultado = valor1 + valor2;
                        break;
                    case "-":
                        resultado = valor1 - valor2;
                        break;
                    case "*":
                        resultado = valor1 * valor2;
                        break;
                    case "/":
                        if (valor2 != 0)
                            resultado = valor1 / valor2;
                        else
                            MessageBox.Show("Divisi�n por cero no permitida.");
                        break;
                }

                variables[identificador] = resultado;
            }

            // Imprimir el valor de la variable especificada
            Match matchPrint = Regex.Match(entrada, patronPrint);
            if (matchPrint.Success)
            {
                string identificador = matchPrint.Groups[1].Value;
                if (variables.ContainsKey(identificador))
                {
                    MessageBox.Show($"El valor de {identificador} es: {variables[identificador]}");
                }
                else
                {
                    MessageBox.Show($"Variable {identificador} no encontrada.");
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            richTextBox1.Font = new Font(richTextBox1.Font.FontFamily, 12); // Ajusta el tama�o de la fuente
            richTextBox1.ForeColor = Color.White;
            richTextBox1.Text.ToUpper();
        }
        public static void ValidateIfExp(string entrada)
        {
            // Expresión regular para encontrar la estructura IF-ELSE
            Regex regexIfElse = new Regex(@"(INTEGER|DECIMAL|FLOAT)\s+(\w+)\s*=\s*([-+]?\d*\.?\d+(?:[eE][-+]?\d+)?)\s*;\s*(INTEGER|DECIMAL|FLOAT)\s+(\w+)\s*=\s*([-+]?\d*\.?\d+(?:[eE][-+]?\d+)?)\s*;\s*IF\s+(\w+)\s*([><=])\s*(\w+)\s*{\s*PRINT\s*""(.*?)"";\s*}\s*ELSE\s*{\s*PRINT\s*""(.*?)"";\s*}");

            // Buscar la estructura IF-ELSE y evaluar la comparación
            Match matchIfElse = regexIfElse.Match(entrada);
            if (matchIfElse.Success)
            {
                string tipoA = matchIfElse.Groups[1].Value;
                string nombreA = matchIfElse.Groups[2].Value;
                string valorA = matchIfElse.Groups[3].Value;
                string tipoB = matchIfElse.Groups[4].Value;
                string nombreB = matchIfElse.Groups[5].Value;
                string valorB = matchIfElse.Groups[6].Value;
                string variableA = matchIfElse.Groups[7].Value;
                string operador = matchIfElse.Groups[8].Value;
                string variableB = matchIfElse.Groups[9].Value;
                string mensajeMayor = matchIfElse.Groups[10].Value;
                string mensajeMenor = matchIfElse.Groups[11].Value;

                // Convertir los valores a los tipos adecuados
                object objA = ConvertToType(tipoA, valorA);
                object objB = ConvertToType(tipoB, valorB);

                // Evaluar la comparación
                bool comparacion;
                if (objA is IComparable && objB is IComparable)
                {
                    comparacion = CompareObjects((IComparable)objA, (IComparable)objB, operador);
                }
                else
                {
                    MessageBox.Show("Los tipos de datos no son comparables.");
                    return;
                }

                // Mostrar el mensaje correspondiente
                if (comparacion)
                {
                    MessageBox.Show(mensajeMayor);
                }
                else
                {
                    MessageBox.Show(mensajeMenor);
                }
            }
            else
            {
                MessageBox.Show("La entrada no es una estructura IF-ELSE válida.");
            }
        }

        // Función para convertir el valor a su tipo adecuado
        private static object ConvertToType(string tipo, string valor)
        {
            switch (tipo)
            {
                case "INTEGER":
                    return int.Parse(valor);
                case "DECIMAL":
                    return decimal.Parse(valor);
                case "FLOAT":
                    return float.Parse(valor);
                default:
                    throw new ArgumentException("Tipo de dato no válido.");
            }
        }

        // Función para comparar dos objetos comparables
        private static bool CompareObjects(IComparable a, IComparable b, string operador)
        {
            switch (operador)
            {
                case "<":
                    return a.CompareTo(b) < 0;
                case ">":
                    return a.CompareTo(b) > 0;
                case "=":
                    return a.CompareTo(b) == 0;
                default:
                    throw new ArgumentException("Operador no válido.");
            }
        }

    }
}

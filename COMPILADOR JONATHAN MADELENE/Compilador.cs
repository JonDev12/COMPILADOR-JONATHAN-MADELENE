using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMPILADOR_JONATHAN_MADELENE
{
    public class Compilador
    {
        //Patrones en expresiones regulares 
        //Identificador - variable 
        public string identifierPattern = @"[a-zA-Z_]\w*";
        //Patron entero
        public string integerPattern = @"\d+";
        //Patrones de operacion
        public string operatorPattern = @"\+|\-|\*|\/";
        //Patton de palabras reservadas
        public string keywordPattern = @"\b(INTEGER|IF|ELSE|FLOAT|PRINT|FLOAT|DECIMAL|DOUBLE|STRING)\b";
        //Patrones de espacio en blanco
        public string whitespacePattern = @"\s+";
        //Patrones de puntuacion
        public string punctuationPattern = @"\;|\=";
        //Patron glotantes Float
        public string floatPattern = @"\b\d+\.\d+\b";
        //Patron notacion scientifica
        public string scientificNotationPattern = @"\b\d+(\.\d+)?([eE][+-]?\d+)?\b";

    }
}

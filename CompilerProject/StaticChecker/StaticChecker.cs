﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticChecker
{
    public class StaticChecker
    {
        public static IDictionary<string, string> reservedWords = new Dictionary<string, string>();
        public static IDictionary<string, string> reservedSymbols = new Dictionary<string, string>();
        public static IDictionary<string, string> reservedTypes = new Dictionary<string, string>();
        public static int linha = 1;

        static void Main(string[] args)
        {
            string documentNameOrPath, path, name;


            Console.WriteLine("Digite o nome do arquivo ou caminho completo: ");
            documentNameOrPath = Console.ReadLine();

            var splittedName = documentNameOrPath.Split('\\');

            //"inteligência" para saber se foi informado um path completo ou apenas o nome do documento.
            if (splittedName.Length > 1)
            {
                var aux = splittedName[splittedName.Length-1];
                var aux1 = aux.Split('.');
                name = aux1[0];
                name.ToUpper();
                path = documentNameOrPath;
                if (!File.Exists(path))
                {
                    Console.WriteLine("Nenhum arquivo com esse nome foi encontrado. Verifique se o nome do arquivo ou caminho dele estão corretos e tente novamente. Pressione ENTER para continuar");
                    Console.ReadKey();
                    return;
                }
            }
            else
            {
                name = documentNameOrPath;
                name.ToUpper();
                path = documentNameOrPath + ".201";
                if (!File.Exists(path))
                {
                    Console.WriteLine("Nenhum arquivo com esse nome foi encontrado. Verifique se o nome do arquivo ou caminho dele estão corretos e tente novamente. Pressione ENTER para continuar");
                    Console.ReadKey();
                    return;
                }
            }

            StreamReader reader;
            List<Token> tokensList = new List<Token>();
            List<Token> tokensListLexicalAnalisys = new List<Token>();
            char item;
            int count = 0;

            //Carrega elementos padrões da linguagem.
            LoadReservedSymbols();
            LoadReservedTypes();
            LoadReservedWords();
            reader = new StreamReader(@path);
            LexicalAnalysis lexical = new LexicalAnalysis(reader);
            Token token;
            
            do
            {
                item = (char)reader.Read();
                token = lexical.Analysis(item);

                switch (token.Code)
                {
                    case "INE":
                        break;
                    case "COM":
                        break;

                    default:
                        if (tokensList.Exists(x => x.Lexeme == token.Lexeme))
                        {
                            Token t = tokensList.Find(x => x.Lexeme == token.Lexeme);
                            if (t.AppearedInLines.Count < 5)
                                t.AppearedInLines.Add(token.AppearedInLines[0]);
                        }
                        else
                        {
                            List<int> lines = new List<int>();
                            foreach (int line in token.AppearedInLines)
                            {
                                lines.Add(line);
                            }
                            tokensList.Add(new Token(token, lines));
                        }
                        tokensListLexicalAnalisys.Add(new Token(token));
                        break;
                }
                count++;
                lexical.ClearToken();

            } while (!reader.EndOfStream);

            reader.Close();
            reader.Dispose();

            Report report = new Report();

            //Relatório da análise léxica:
            report.CreateLexicalReport(tokensListLexicalAnalisys, name, tokensList);

            //Relatório da tabela de símbolos:
            report.CreateSymbolTableReport(name, tokensList);


            Console.WriteLine("Programa finalizado. Pressione ENTER para continuar");
            Console.ReadKey();
        }

        public static void LoadReservedSymbols()
        {
            reservedSymbols.Add("!=", "410");
            reservedSymbols.Add("#", "410");
            reservedSymbols.Add("&", "411");
            reservedSymbols.Add("(", "412");
            reservedSymbols.Add("/", "413");
            reservedSymbols.Add(";", "414");
            reservedSymbols.Add("[", "415");
            reservedSymbols.Add("{", "416");
            reservedSymbols.Add("+", "417");
            reservedSymbols.Add("<=", "418");
            reservedSymbols.Add("=", "419");
            reservedSymbols.Add(">=", "420");
            reservedSymbols.Add("!", "421");
            reservedSymbols.Add("%", "422");
            reservedSymbols.Add(")", "423");
            reservedSymbols.Add("*", "424");
            reservedSymbols.Add(",", "425");
            reservedSymbols.Add("]", "426");
            reservedSymbols.Add("|", "427");
            reservedSymbols.Add("}", "428");
            reservedSymbols.Add("<", "429");
            reservedSymbols.Add("==", "430");
            reservedSymbols.Add("-", "432");
        }

        public static void LoadReservedTypes()
        {
            reservedTypes.Add("CHARACTER", "510");
            reservedTypes.Add("CONSTANT-STRING", "511");
            reservedTypes.Add("FLOAT-NUMBER", "512");
            reservedTypes.Add("FUNCTION", "513");
            reservedTypes.Add("IDENTIFIER", "514");
            reservedTypes.Add("INTEGER-NUMBER", "515");
        }

        public static void LoadReservedWords()
        {
            reservedWords.Add("BOOL", "310");
            reservedWords.Add("WHILE", "311");
            reservedWords.Add("BREAK", "312");
            reservedWords.Add("VOID", "313");
            reservedWords.Add("CHAR", "314");
            reservedWords.Add("TRUE", "315");
            reservedWords.Add("ELSE", "316");
            reservedWords.Add("STRING", "317");
            reservedWords.Add("END", "318");
            reservedWords.Add("RETURN", "319");
            reservedWords.Add("FALSE", "320");
            reservedWords.Add("PROGRAM", "321");
            reservedWords.Add("FLOAT", "322");
            reservedWords.Add("INT", "323");
            reservedWords.Add("IF", "324");
            reservedWords.Add("BEGIN", "325");
        }
    }
}

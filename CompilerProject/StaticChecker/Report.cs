using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticChecker
{
    public class Report
    {
        StreamWriter lexicalReport;
        StreamWriter tableReport;
        public Report()
        {

        }

        public void CreateLexicalReport(List<Token> tokenList, string name, List<Token> symbolTable)
        {
            lexicalReport = File.CreateText(name + ".LEX");
            lexicalReport.WriteLine("RELATORIO DA ANALISE LEXICA");
            lexicalReport.WriteLine("CODIGO DA EQUIPE: E03");
            lexicalReport.WriteLine("COMPONENTES:");
            lexicalReport.WriteLine("LUCAS CASSIMIRO TRANZILLO NOGUEIRA  E-MAIL: LCASSIMIRO1@GMAIL.COM    CONTATO: (71) 9 9926-2746");
            lexicalReport.WriteLine("LUCAS EDUARDO SANTANA DANTAS        E-MAIL: LDSANTANAE@GMAIL.COM     CONTATO: (71) 9 9369-9139");
            lexicalReport.WriteLine("PEDRO HENRIQUE ANDRADE COSTA        E-MAIL: PEDROHAMC21@GMAIL.COM    CONTATO: (71) 9 9195-4695");
            lexicalReport.WriteLine("******");
            lexicalReport.WriteLine("******");
            lexicalReport.WriteLine("LEXEME | CÓDIGO DO ÁTOMO | ÍNDICE NA TABELA DE SÍMBOLOS");

            int k = 1;
            foreach (Token token in tokenList)
            {
                int symbolTableIndex = symbolTable.FindIndex(x => x.Code == token.Code);
                lexicalReport.WriteLine($"{k} | {token.Lexeme} | {token.Category.Code} | {symbolTableIndex}");
                k++;
            }
            lexicalReport.Close();
        }

        public void CreateSymbolTableReport(string name, List<Token> symbolTable)
        {
            tableReport = File.CreateText(name + ".TAB");
            tableReport.WriteLine("RELATORIO DA TABELA DE SÍMBOLOS");
            tableReport.WriteLine("CODIGO DA EQUIPE: E03");
            tableReport.WriteLine("COMPONENTES:");
            tableReport.WriteLine("LUCAS CASSIMIRO TRANZILLO NOGUEIRA  E-MAIL: LCASSIMIRO1@GMAIL.COM    CONTATO: (71) 9 9926-2746");
            tableReport.WriteLine("LUCAS EDUARDO SANTANA DANTAS        E-MAIL: LDSANTANAE@GMAIL.COM     CONTATO: (71) 9 9369-9139");
            tableReport.WriteLine("PEDRO HENRIQUE ANDRADE COSTA        E-MAIL: PEDROHAMC21@GMAIL.COM    CONTATO: (71) 9 9195-4695");
            tableReport.WriteLine("******");
            tableReport.WriteLine("******");
            tableReport.WriteLine("ÍNDICE | CÓDIGO | LEXEME | TAM ANTES DE TRUNCAR | TAM DEPOIS DE TRUNCAR | CATEGORIA | 5 PRIMEIRAS LINHAS");

            int i = 0;
            foreach(Token token in symbolTable)
            {
                tableReport.Write($"{i} | {token.Category.Code} | {token.Lexeme} | {token.Size1} | {token.Size2} | {token.Code} |  ");
                for (int j = 0; j < token.AppearedInLines.Count; j++)
                {
                    tableReport.Write(token.AppearedInLines[j].ToString() + ",");
                }
                tableReport.WriteLine();
                i++;
            }
            tableReport.Close();
        }
    }
}

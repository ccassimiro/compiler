using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticChecker
{
    public class LexicalAnalysis
    {
        public StreamReader Reader { get; set; }
        public Token Token { get; set; }
        public int State { get; set; }
        public int ContChar { get; set; }
        public string AuxStr { get; set; }
        public bool ExistsChar { get; set; }

        public LexicalAnalysis(StreamReader reader)
        {
            this.Reader = reader;
            this.State = 0;
            this.ContChar = 0;
            this.ExistsChar = false;
            Token = new Token() { Code = "COM" };
        }

        public void ClearToken()
        {
            this.State = 0;
            this.Token.Character = ' ';
            this.AuxStr = string.Empty;
            this.Token.Lexeme = string.Empty;
            this.Token.Code = string.Empty;
            this.Token.AppearedInLines.Clear();
        }

        public void CheckSize(string auxStr)
        {
            if(auxStr.Length > 30)
            {
                Token.Size1 = 30;
                Token.Size1 = auxStr.Length;
                Token.Lexeme = auxStr.Substring(0, 30);
            }
            else
            {
                Token.Size1 = auxStr.Length;
                Token.Size2 = 0;
            }
        }

        public void AddLine(int line)
        {
            Token.AppearedInLines.Add(line);
        }

        public Token Analysis(char item)
        {
            item = Char.ToUpper(item);

            //Início da Análise
            while (true)
            {
                switch (State)
                {
                    case 0:
                        if (item == ' ' || item == '\n' || item == '\t' || item == '\r')
                        {
                            State = 0;
                            if (item == '\n')
                                StaticChecker.linha++;
                            item = (char)Reader.Read();

                            break;
                        }
                        else if (char.IsLetter(item))
                        {
                            State = 1;
                        }
                        else if (char.IsDigit(item))
                        {
                            State = 2;
                        }
                        else if (item == '(')
                        {
                            State = 3;
                        }
                        else if (item == ')')
                        {
                            State = 4;
                        }
                        else if (item == '+')
                        {
                            State = 5;
                        }
                        else if (item == '-')
                        {
                            State = 6;
                        }
                        else if (item == '&')
                        {
                            State = 7;
                        }
                        else if (item == '#')
                        {
                            State = 8;
                        }
                        else if (item == '<')
                        {
                            State = 9;
                        }
                        else if (item == '>')
                        {
                            State = 10;
                        }
                        else if (item == '=')
                        {
                            State = 11;
                        }
                        else if (item == '!')
                        {
                            State = 12;
                        }
                        else if (item == ';')
                        {
                            State = 14;
                        }
                        else if (item == ',')
                        {
                            State = 15;
                        }
                        else if (item == '[')
                        {
                            State = 16;
                        }
                        else if (item == ']')
                        {
                            State = 17;
                        }
                        else if (item == '{')
                        {
                            State = 18;
                        }
                        else if (item == '}')
                        {
                            State = 22;
                        }
                        else if (item == '%')
                        {
                            State = 23;
                        }
                        else if (item == '*')
                        {
                            State = 25;
                        }
                        else if (item == '|')
                        {
                            State = 26;
                        }
                        else if (item == '/')
                        {
                            State = 27;
                        }
                        else if (item == '\'')
                        {
                            State = 34;
                            AuxStr = AuxStr + item;
                        }
                        else if (item == '\"')
                        {
                            State = 35;
                            AuxStr = AuxStr + item;
                            ContChar++;
                        }
                        else if (item =='_')
                        {
                            State = 36;
                            AuxStr = AuxStr + item;
                            ContChar++;
                        }
                        else
                        {
                            Token.Category = new Category() { Name = "INEXISTENTE", Code = "NUL" };
                            Token.Code = "INE";
                            Token.Lexeme = "";
                            AddLine(StaticChecker.linha);
                            State = 0;

                            return Token;
                        }
                        break;

                    case 1:
                        if(char.IsLetterOrDigit(item))
                        {
                            AuxStr = AuxStr + item;
                            if (!(char.IsLetterOrDigit((char)Reader.Peek())))
                            {
                                if (item != '-')
                                    State = 24;
                                else
                                {
                                    if (item == '\n')
                                        StaticChecker.linha++;
                                    item = (char)Reader.Read();
                                }
                            }
                        }
                        break;

                    case 2:
                        if(!(char.IsDigit(item)))
                        {
                            AuxStr = AuxStr + item;
                        }
                        else if(item == '.')
                        {
                            AuxStr = AuxStr + item;
                            State = 30;
                        }

                        if(!(char.IsDigit((char)Reader.Peek())) && (char)Reader.Peek() != '.' && State != 30)
                        {
                            State = 31;
                        }
                        else
                        {
                            if (item == '\n')
                                StaticChecker.linha++;
                            if (State != 30)
                                item = (char)Reader.Read();
                        }
                        break;

                    case 3:
                        Token.Category = new Category() { Name = "ABRE_PARENTESES", Code = StaticChecker.reservedSymbols[item.ToString()] };
                        Token.Lexeme = item.ToString();
                        CheckSize(Token.Lexeme);
                        Token.Code = "SR";
                        AddLine(StaticChecker.linha);
                        State = 0;
                        return Token;

                    case 4:
                        Token.Category = new Category() { Name = "FECHA_PARENTESES", Code = StaticChecker.reservedSymbols[item.ToString()] };
                        Token.Lexeme = item.ToString();
                        CheckSize(Token.Lexeme);
                        Token.Code = "SR";
                        AddLine(StaticChecker.linha);
                        State = 0;
                        return Token;

                    case 5:
                        Token.Category = new Category() { Name = "MAIS", Code = StaticChecker.reservedSymbols[item.ToString()] };
                        Token.Lexeme = item.ToString();
                        CheckSize(Token.Lexeme);
                        Token.Code = "SR";
                        AddLine(StaticChecker.linha);
                        State = 0;
                        return Token;

                    case 6:
                        Token.Category = new Category() { Name = "MENOS", Code = StaticChecker.reservedSymbols[item.ToString()] };
                        Token.Lexeme = item.ToString();
                        CheckSize(Token.Lexeme);
                        Token.Code = "SR";
                        AddLine(StaticChecker.linha);
                        State = 0;
                        return Token;

                    case 7:
                        Token.Category = new Category() { Name = "E_COMERCIAL", Code = StaticChecker.reservedSymbols[item.ToString()] };
                        Token.Lexeme = item.ToString();
                        CheckSize(Token.Lexeme);
                        Token.Code = "SR";
                        AddLine(StaticChecker.linha);
                        State = 0;
                        return Token;

                    case 8:
                        Token.Category = new Category() { Name = "HASHTAG", Code = StaticChecker.reservedSymbols[item.ToString()] };
                        Token.Lexeme = item.ToString();
                        CheckSize(Token.Lexeme);
                        Token.Code = "SR";
                        AddLine(StaticChecker.linha);
                        State = 0;
                        return Token;

                    case 9:
                        if((char)Reader.Peek() == '=')
                        {
                            State = 19;
                            AuxStr = AuxStr + item;
                            item = (char)Reader.Read();
                            AuxStr = AuxStr + item;
                        }
                        else
                        {
                            Token.Category = new Category() { Name = "MENOR", Code = StaticChecker.reservedSymbols[item.ToString()]};
                            Token.Lexeme = item.ToString();
                            CheckSize(Token.Lexeme);
                            Token.Code = "SR";
                            AddLine(StaticChecker.linha);
                            State = 0;
                            return Token;
                        }
                        break;

                    case 10:
                        if((char)Reader.Peek() == '=')
                        {
                            AuxStr = AuxStr + item;
                            State = 20;
                            item = (char)Reader.Read();
                            AuxStr = AuxStr + item;
                        }
                        else
                        {
                            Token.Category = new Category() { Name = "MAIOR", Code = StaticChecker.reservedSymbols[item.ToString()] };
                            Token.Lexeme = item.ToString();
                            CheckSize(Token.Lexeme);
                            Token.Code = "SR";
                            AddLine(StaticChecker.linha);
                            State = 0;
                            return Token;
                        }
                        break;

                    case 11:
                        if ((char)Reader.Peek() == '=')
                        {
                            AuxStr = AuxStr + item;
                            State = 21;
                            item = (char)Reader.Read();
                            AuxStr = AuxStr + item;
                        }
                        else
                        {
                            Token.Category = new Category() { Name = "ATRIBUICAO", Code = StaticChecker.reservedSymbols[item.ToString()] };
                            Token.Lexeme = item.ToString();
                            CheckSize(Token.Lexeme);
                            Token.Code = "SR";
                            AddLine(StaticChecker.linha);
                            State = 0;
                            return Token;
                        }
                        break;


                    case 12:
                        if ((char)Reader.Peek() == '=')
                        {
                            AuxStr = AuxStr + item;
                            State = 13;
                            item = (char)Reader.Read();
                            AuxStr = AuxStr + item;
                        }
                        else
                        {
                            Token.Category = new Category() { Name = "EXCLAMACAO", Code = StaticChecker.reservedSymbols[item.ToString()] };
                            Token.Lexeme = item.ToString();
                            CheckSize(Token.Lexeme);
                            Token.Code = "SR";
                            AddLine(StaticChecker.linha);
                            State = 0;
                            return Token;
                        }
                        break;

                    case 13:
                        Token.Category = new Category() { Name = "DIFERENTE", Code = StaticChecker.reservedSymbols[item.ToString()] };
                        Token.Lexeme = AuxStr;
                        CheckSize(Token.Lexeme);
                        Token.Code = "SR";
                        AddLine(StaticChecker.linha);
                        State = 0;
                        return Token;

                    case 14:
                        Token.Category = new Category() { Name = "PONTO_E_VIRGULA", Code = StaticChecker.reservedSymbols[item.ToString()] };
                        Token.Lexeme = item.ToString();
                        CheckSize(Token.Lexeme);
                        Token.Code = "SR";
                        AddLine(StaticChecker.linha);
                        State = 0;
                        return Token;

                    case 15:
                        Token.Category = new Category() { Name = "VIRGULA", Code = StaticChecker.reservedSymbols[item.ToString()] };
                        Token.Lexeme = item.ToString();
                        CheckSize(Token.Lexeme);
                        Token.Code = "SR";
                        AddLine(StaticChecker.linha);
                        State = 0;
                        return Token;

                    case 16:
                        Token.Category = new Category() { Name = "ABRE_COLCHETES", Code = StaticChecker.reservedSymbols[item.ToString()] };
                        Token.Lexeme = item.ToString();
                        CheckSize(Token.Lexeme);
                        Token.Code = "SR";
                        AddLine(StaticChecker.linha);
                        State = 0;
                        return Token;

                    case 17:
                        Token.Category = new Category() { Name = "FECHA_COLCHETES", Code = StaticChecker.reservedSymbols[item.ToString()] };
                        Token.Lexeme = item.ToString();
                        CheckSize(Token.Lexeme);
                        Token.Code = "SR";
                        AddLine(StaticChecker.linha);
                        State = 0;
                        return Token;

                    case 18:
                        Token.Category = new Category() { Name = "ABRE_CHAVES", Code = StaticChecker.reservedSymbols[item.ToString()] };
                        Token.Lexeme = item.ToString();
                        CheckSize(Token.Lexeme);
                        Token.Code = "SR";
                        AddLine(StaticChecker.linha);
                        State = 0;
                        return Token;

                    case 22:
                        Token.Category = new Category() { Name = "FECHA_CHAVES", Code = StaticChecker.reservedSymbols[item.ToString()] };
                        Token.Lexeme = item.ToString();
                        CheckSize(Token.Lexeme);
                        Token.Code = "SR";
                        AddLine(StaticChecker.linha);
                        State = 0;
                        return Token;

                    case 19:
                        Token.Category = new Category() { Name = "MENOR_IGUAL", Code = StaticChecker.reservedSymbols[item.ToString()] };
                        Token.Lexeme = AuxStr;
                        CheckSize(Token.Lexeme);
                        Token.Code = "SR";
                        AddLine(StaticChecker.linha);
                        State = 0;
                        AuxStr = string.Empty;
                        return Token;

                    case 20:
                        Token.Category = new Category() { Name = "MAIOR_IGUAL", Code = StaticChecker.reservedSymbols[item.ToString()] };
                        Token.Lexeme = AuxStr;
                        CheckSize(Token.Lexeme);
                        Token.Code = "SR";
                        AddLine(StaticChecker.linha);
                        State = 0;
                        AuxStr = string.Empty;
                        return Token;

                    case 23:
                        Token.Category = new Category() { Name = "PERCENTUAL", Code = StaticChecker.reservedSymbols[item.ToString()] };
                        Token.Lexeme = item.ToString();
                        CheckSize(Token.Lexeme);
                        Token.Code = "SR";
                        AddLine(StaticChecker.linha);
                        State = 0;
                        return Token;

                    case 24:
                        if (StaticChecker.reservedWords.ContainsKey(AuxStr))
                        {
                            Token.Category = new Category() { Name = AuxStr, Code = StaticChecker.reservedWords[AuxStr] };
                            Token.Lexeme = AuxStr;
                            CheckSize(Token.Lexeme);
                            Token.Code = "PR";
                            AddLine(StaticChecker.linha);
                            AuxStr = string.Empty;
                            State = 0;
                            return Token;
                        }
                        else if (StaticChecker.reservedTypes.ContainsKey(AuxStr))
                        {
                            Token.Category = new Category() { Name = AuxStr, Code = StaticChecker.reservedWords[AuxStr] };
                            Token.Lexeme = AuxStr;
                            CheckSize(Token.Lexeme);
                            Token.Code = "PR";
                            AddLine(StaticChecker.linha);
                            AuxStr = string.Empty;
                            State = 0;
                            return Token;
                        }
                        else if (!(AuxStr.Contains("-")))
                        {
                            State = 36;
                        }
                        else
                        {
                            AuxStr = string.Empty;
                            item = (char)Reader.Read();
                            State = 0;
                        }
                        break;

                    case 25:
                        Token.Category = new Category() { Name = "ASTERISCO", Code = StaticChecker.reservedSymbols[item.ToString()] };
                        Token.Lexeme = item.ToString();
                        CheckSize(Token.Lexeme);
                        Token.Code = "SR";
                        AddLine(StaticChecker.linha);
                        State = 0;
                        return Token;

                    case 26:
                        Token.Category = new Category() { Name = "PIPE", Code = StaticChecker.reservedSymbols[item.ToString()] };
                        Token.Lexeme = item.ToString();
                        CheckSize(Token.Lexeme);
                        Token.Code = "SR";
                        AddLine(StaticChecker.linha);
                        State = 0;
                        return Token;

                    case 27:
                        if((char)Reader.Peek() == '/')
                        {
                            State = 28;
                            item = (char)Reader.Read();
                        }
                        else if((char)Reader.Peek() == '*')
                        {
                            State = 29;
                            item = (char)Reader.Read();
                            item = (char)Reader.Read();
                        }
                        else
                        {
                            Token.Category = new Category() { Name = "DIVISAO", Code = StaticChecker.reservedSymbols[item.ToString()] };
                            Token.Lexeme = item.ToString();
                            CheckSize(Token.Lexeme);
                            Token.Code = "SR";
                            AddLine(StaticChecker.linha);
                            State = 0;
                            return Token;
                        }
                        break;

                    case 28:
                        if(item == '\n')
                        {
                            State = 0;
                            StaticChecker.linha++;
                        }
                        else
                        {
                            item = (char)Reader.Read();
                        }
                        break;

                    case 29:
                        if(item == '*')
                        {
                            item = (char)Reader.Read();
                            if(item == '/')
                            {
                                State = 0;
                                item = (char)Reader.Read();
                            }
                        }
                        else
                        {
                            if (item == '\n')
                                StaticChecker.linha++;
                            item = (char)Reader.Read();
                        }

                        item = (char)Reader.Read();
                        State = 0;
                        break;

                    case 30:
                        if (char.IsDigit((char)Reader.Peek()))
                        {
                            item = (char)Reader.Read();
                            State = 32;
                        }
                        else
                        {
                            Token.Category = new Category() { Name = "INEXISTENTE", Code = "NUL" };
                            Token.Lexeme = AuxStr;
                            CheckSize(Token.Lexeme);
                            Token.Code = "INE";
                            AddLine(StaticChecker.linha);
                            return Token;
                        }
                        break;

                    case 31:
                        Token.Category = new Category() { Name = "INTEIRO", Code = "INT" };
                        Token.Lexeme = AuxStr;
                        CheckSize(Token.Lexeme);
                        Token.Code = "INT";
                        CheckSize(AuxStr);
                        AddLine(StaticChecker.linha);
                        return Token;

                    case 32:
                        if (char.IsDigit((char)Reader.Peek()))
                        {
                            AuxStr = AuxStr + item;
                            item = (char)Reader.Read();
                        }
                        else
                        {
                            AuxStr = AuxStr + item;
                            State = 33;
                        }
                        break;

                    case 33:
                        Token.Category = new Category() { Name = "FLOAT", Code = "FLO" };
                        Token.Lexeme = AuxStr;
                        CheckSize(Token.Lexeme);
                        Token.Code = "FLO";
                        AddLine(StaticChecker.linha);
                        CheckSize(AuxStr);
                        return Token;

                    case 34:
                        if (Char.GetUnicodeCategory((char)Reader.Peek()) != UnicodeCategory.Control && (char)Reader.Peek() != '\\')
                        {
                            if(ExistsChar && (char)Reader.Peek() == '\'')
                            {
                                item = (char)Reader.Read();
                                AuxStr = AuxStr + item;
                                Token.Category = new Category() { Name = "CHAR", Code = "CH" };
                                Token.Lexeme = AuxStr;
                                CheckSize(Token.Code);
                                Token.Code = "CH";
                                AddLine(StaticChecker.linha);
                                State = 0;
                                ExistsChar = false;
                                AuxStr = string.Empty;
                                item = (char)Reader.Read();
                                return Token;
                            }
                            if (!ExistsChar)
                            {
                                item = (char)Reader.Read();
                                AuxStr = AuxStr + item;
                                ExistsChar = true;
                            }
                            else
                            {
                                if (item != '\'')
                                    item = (char)Reader.Read();
                                else
                                {
                                    if (item == '\n')
                                        StaticChecker.linha++;
                                    item = (char)Reader.Read();
                                    ExistsChar = false;
                                    AuxStr = string.Empty;
                                    State = 0;
                                }
                            }
                        }
                        else
                        {
                            Token.Category = new Category() { Name = "INEXISTENTE", Code = "NUL" };
                            Token.Lexeme = AuxStr;
                            CheckSize(Token.Lexeme);
                            Token.Code = "INE";
                            AuxStr = string.Empty;
                            AddLine(StaticChecker.linha);
                            return Token;
                        }
                        break;

                    case 35:
                        if (Char.GetUnicodeCategory((char)Reader.Peek()) != UnicodeCategory.Control && (char)Reader.Peek() != '\\')
                        {
                            if (item == '\n')
                                StaticChecker.linha++;
                            if((char)Reader.Peek() == '\"')
                            {
                                item = (char)Reader.Read();
                                AuxStr = AuxStr + item;
                                ContChar++;
                                Token.Category = new Category() { Name = "CONSTANT-STRING", Code = "ST" };
                                Token.Lexeme = AuxStr;
                                Token.Code = "ST";
                                AddLine(StaticChecker.linha);
                                Token.Size2 = ContChar;
                                State = 0;
                                AuxStr = string.Empty;
                                ContChar = 0;
                                item = (char)Reader.Read();
                                return Token;
                            }
                            if (AuxStr.Length <= 29)
                            {
                                item = (char)Reader.Read();
                                AuxStr = AuxStr + item;
                                ContChar++;
                            }
                            else
                            {
                                if (Reader.EndOfStream)
                                {
                                    Token.Category = new Category() { Name = "INEXISTENTE", Code = "NUL" };
                                    Token.Lexeme = "";
                                    Token.Code = "INE";
                                    AuxStr = string.Empty;
                                    State = 0;
                                    ContChar = 0;
                                    AddLine(StaticChecker.linha);
                                    return Token;
                                }
                                item = (char)Reader.Read();
                                ContChar++;
                            }
                        }
                        else
                        {
                            Token.Category = new Category() { Name = "INEXISTENTE", Code = "NUL" };
                            Token.Code = "INE";
                            Token.Lexeme = "";
                            AddLine(StaticChecker.linha);
                            State = 0;
                            ContChar = 0;
                            AuxStr = string.Empty;
                            return Token;
                        }
                        break;

                    case 36:
                        if ((char.IsLetterOrDigit((char)Reader.Peek())) || (char)Reader.Peek() == '_')
                        {
                            item = (char)Reader.Read();
                            if (AuxStr.Length < 30)
                                AuxStr = AuxStr + item;
                            ContChar++;
                        }
                        else
                        {
                            if ((char)Reader.Peek() == '(')
                            {
                                Token.Category = new Category() { Name = "FUNCTION", Code = "FUN" };
                                Token.Lexeme = AuxStr;
                                Token.Code = "FUN";
                                AddLine(StaticChecker.linha);
                                Token.Size2 = ContChar;
                            }
                            else
                            {
                                Token.Category = new Category() { Name = "IDENTIFIER", Code = "IDT" };
                                Token.Lexeme = AuxStr;
                                Token.Code = "IDT";
                                AddLine(StaticChecker.linha);
                                Token.Size2 = ContChar;
                            }
                            ClearToken();
                            State = 0;
                        }
                        break;

                    default:
                        break;
                }
            }

        }

    }
}

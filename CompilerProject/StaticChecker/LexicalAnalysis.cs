using System;
using System.Collections.Generic;
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

        public LexicalAnalysis(StreamReader reader)
        {
            this.Reader = reader;
            this.State = 0;
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


                    default:
                        break;
                }
            }

        }

    }
}

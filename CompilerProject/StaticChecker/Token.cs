using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticChecker
{
    public class Token
    {
        public char Character { get; set; }
        public string Lexeme { get; set; }
        public string Code { get; set; }
        public List<int> AppearedInLines { get; set; }
        public int Size1 { get; set; }
        public int Size2 { get; set; }
        public int NumberOfAppearances { get; set; }
        public Category Category { get; set; }

        public Token()
        {

        }

        public Token(Token t, List<int> appearedInLines)
        {
            this.Character = t.Character;
            this.Lexeme = t.Lexeme;
            this.Code = t.Code;
            this.AppearedInLines = appearedInLines;
            this.Size1 = t.Size1;
            this.Size2 = t.Size2;
            this.NumberOfAppearances = t.NumberOfAppearances;
            this.Category = t.Category;
        }

        public Token(Token t)
        {
            this.Character = t.Character;
            this.Lexeme = t.Lexeme;
            this.Code = t.Code;
            this.Size1 = t.Size1;
            this.Size2 = t.Size2;
            this.NumberOfAppearances = t.NumberOfAppearances;
            this.Category = t.Category;
        }

    }

    public class Category
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public Category()
        {

        }
    }
}

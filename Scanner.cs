//using System;
//using System.Collections.Generic;
//using System.Drawing.Drawing2D;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;


////fatema
//public enum Token_Class
//{
//    If, Then, Else, Repeat, Until, Read, Write, Return, Endl,
//    Int, Float, String,
//    End, Elseif, Main,
//    Semicolon, Comma, LParanthesis, RParanthesis,
//    EqualOp, LessThanOp, GreaterThanOp, NotEqualOp,
//    PlusOp, MinusOp, MultiplyOp, DivideOp,
//    Identifier, Constant, AssignmentOp
//    , Dot,
//    //hla
//    LCurlyBracket, RCurlyBracket,
//    //hla

//    //Eman
//    AndOp, OrOp
//        , Begin
//}
////fatema


//namespace JASON_Compiler
//{


//    public class Token
//    {
//        public string lex;
//        public Token_Class token_type;
//    }

//    public class Scanner
//    {
//        public List<Token> Tokens = new List<Token>();
//        //Hla - A list to store errors during scanning like tokens list 
//        public List<string> Errors = new List<string>();
//        //Hla

//        Dictionary<string, Token_Class> ReservedWords = new Dictionary<string, Token_Class>();
//        Dictionary<string, Token_Class> Operators = new Dictionary<string, Token_Class>();

//        public Scanner()
//        {

//            //fatema
//            ReservedWords.Clear();
//            ReservedWords.Add("if", Token_Class.If);
//            ReservedWords.Add("then", Token_Class.Then);
//            ReservedWords.Add("else", Token_Class.Else);
//            ReservedWords.Add("repeat", Token_Class.Repeat);
//            ReservedWords.Add("until", Token_Class.Until);
//            ReservedWords.Add("read", Token_Class.Read);
//            ReservedWords.Add("write", Token_Class.Write);
//            ReservedWords.Add("return", Token_Class.Return);
//            ReservedWords.Add("endl", Token_Class.Endl);
//            ReservedWords.Add("int", Token_Class.Int);
//            ReservedWords.Add("float", Token_Class.Float);
//            ReservedWords.Add("string", Token_Class.String);
//            ReservedWords.Add("end", Token_Class.End);
//            ReservedWords.Add("elseif", Token_Class.Elseif);
//            ReservedWords.Add("main", Token_Class.Main);
//            //fatema


//            Operators.Add(".", Token_Class.Dot);
//            Operators.Add(";", Token_Class.Semicolon);
//            Operators.Add(",", Token_Class.Comma);
//            Operators.Add("(", Token_Class.LParanthesis);
//            Operators.Add(")", Token_Class.RParanthesis);


//            //Hla
//            Operators.Add("{", Token_Class.LCurlyBracket);
//            Operators.Add("}", Token_Class.RCurlyBracket);
//            //Hla


//            Operators.Add("<", Token_Class.LessThanOp);
//            Operators.Add(">", Token_Class.GreaterThanOp);
//            Operators.Add("<>", Token_Class.NotEqualOp);

//            //rana
//            Operators.Add("=", Token_Class.EqualOp);
//            Operators.Add("+", Token_Class.PlusOp);
//            Operators.Add("-", Token_Class.MinusOp);
//            Operators.Add("*", Token_Class.MultiplyOp);
//            Operators.Add("/", Token_Class.DivideOp);
//            Operators.Add(":=", Token_Class.AssignmentOp);

//            //Eman 
//            Operators.Add("&&", Token_Class.AndOp);
//            Operators.Add("||", Token_Class.OrOp);

//        }

//        public void StartScanning(string SourceCode)
//        {
//            Tokens.Clear();
//            for (int i = 0; i < SourceCode.Length; i++)
//            {
//                int j = i;
//                char CurrentChar = SourceCode[i];
//                string CurrentLexeme = CurrentChar.ToString();

//                if (CurrentChar == ' ' || CurrentChar == '\r' || CurrentChar == '\n')
//                {
//                    continue;
//                }

//                //Hla - comment statement handling 
//                if (CurrentChar == '/' && i + 1 < SourceCode.Length && SourceCode[i + 1] == '*')
//                {
//                    i += 2; // skip '/*'
//                    while (i + 1 < SourceCode.Length)
//                    {
//                        if (SourceCode[i] == '*' && SourceCode[i + 1] == '/')
//                        {
//                            i += 1;
//                            break;
//                        }
//                        i++;
//                    }
//                    continue; //Ignore comment completely
//                }
//                //Hla



//                //fatema
//                if (char.IsLetter(CurrentChar))
//                {
//                    string lex = "";

//                    while (i < SourceCode.Length && char.IsLetterOrDigit(SourceCode[i]))
//                    {
//                        lex += SourceCode[i];
//                        i++;
//                    }

//                    i--; // step back
//                    FindTokenClass(lex);
//                }
//                //fatema




//                else if (CurrentChar >= '0' && CurrentChar <= '9')
//                {
//                    string lex = "";

//                    while (i < SourceCode.Length && (char.IsDigit(SourceCode[i]) || SourceCode[i] == '.'))
//                    {
//                        lex += SourceCode[i];
//                        i++;
//                    }

//                    i--;
//                    FindTokenClass(lex);
//                }

//                else if (CurrentChar == '"')
//                {
//                    string lex = "\"";
//                    i++;

//                    while (i < SourceCode.Length && SourceCode[i] != '"')
//                    {
//                        lex += SourceCode[i];
//                        i++;
//                    }

//                    if (i < SourceCode.Length)
//                    {
//                        lex += "\"";

//                    }
//                    else
//                    {
//                        Errors.Add("Unterminated string literal");
//                    }

//                    FindTokenClass(lex);
//                }


//                //rana
//                else
//                {
//                    // Assignment operator :=
//                    if (CurrentChar == ':')
//                    {
//                        if (i + 1 < SourceCode.Length && SourceCode[i + 1] == '=')
//                        {
//                            FindTokenClass(":=");
//                            i++; // skip '='
//                        }
//                        else
//                        {
//                            Errors.Add("Invalid operator ':'");
//                        }
//                    }
//                    //Eman 
//                    else if (CurrentChar == '&')
//                    {
//                        if (i + 1 < SourceCode.Length && SourceCode[i + 1] == '&')
//                        {
//                            FindTokenClass("&&");
//                            i++;
//                        }
//                        else
//                            Errors.Add("Invalid token: &");
//                    }
//                    else if (CurrentChar == '|')
//                    {
//                        if (i + 1 < SourceCode.Length && SourceCode[i + 1] == '|')
//                        {
//                            FindTokenClass("||");
//                            i++;
//                        }
//                        else
//                            Errors.Add("Invalid token: |");
//                    }
//                    //Eman
//                    else
//                    {
//                        string op = CurrentChar.ToString();

//                        // Arithmetic operators (+ - * / =)
//                        if (Operators.ContainsKey(op))
//                        {

//                            if (CurrentChar == '<')
//                            {
//                                if (i + 1 < SourceCode.Length && SourceCode[i + 1] == '>')
//                                {
//                                    FindTokenClass("<>");
//                                    i++;
//                                }
//                                else
//                                {
//                                    FindTokenClass("<");
//                                }
//                            }
//                            else
//                            {
//                                FindTokenClass(op);
//                            }
//                        }
//                        //Eman



//                        // Hla - Invalid token error handling
//                        else
//                        {
//                            Errors.Add("Invalid token: " + op);
//                        }
//                        //Hla 
//                    }



//                }
//            }

//            JASON_Compiler.TokenStream = Tokens;
//        }
//        void FindTokenClass(string Lex)
//        {
//            Token_Class TC;
//            Token Tok = new Token();
//            Tok.lex = Lex;


//            //fatema
//            //Is it a reserved word?
//            if (ReservedWords.ContainsKey(Lex))
//            {
//                Tok.token_type = ReservedWords[Lex];
//                Tokens.Add(Tok);
//                return;
//            }
//            //Is it an identifier?
//            if (isIdentifier(Lex))
//            {
//                Tok.token_type = Token_Class.Identifier;
//                Tokens.Add(Tok);
//                return;
//            }
//            //fatema



//            //Is it a Constant?

//            if (isConstant(Lex))
//            {
//                Tok.token_type = Token_Class.Constant;
//                Tokens.Add(Tok);
//                return;
//            }


//            if (Lex.StartsWith("\"") && Lex.EndsWith("\""))
//            {
//                Tok.token_type = Token_Class.Constant;
//                Tokens.Add(Tok);
//                return;
//            }

//            //Is it an operator? rana
//            if (Operators.ContainsKey(Lex))
//            {
//                Tok.token_type = Operators[Lex];
//                Tokens.Add(Tok);
//                return;
//            }

//            //Is it an undefined?
//        }



//        //fatema
//        bool isIdentifier(string lex)
//        {

//            if (!char.IsLetter(lex[0]))
//                return false;

//            foreach (char c in lex)
//            {
//                if (!char.IsLetterOrDigit(c))
//                    return false;
//            }

//            return true;
//        }
//        //fatema

//        bool isConstant(string lex)
//        {
//            bool hasDot = false;
//            foreach (char c in lex)
//            {
//                if (c == '.')
//                {
//                    if (hasDot) return false;
//                    hasDot = true;
//                }
//                else if (!char.IsDigit(c))
//                    return false;
//            }
//            return true;
//        }

//    }


//}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//public enum Token_Class
//{
//    Begin, Call, Declare, End, Do, Else, EndIf, EndUntil, EndWhile, If, Integer,
//    Parameters, Procedure, Program, Read, Real, Set, Then, Until, While, Write,
//    Dot, Semicolon, Comma, LParanthesis, RParanthesis, EqualOp, LessThanOp,
//    GreaterThanOp, NotEqualOp, PlusOp, MinusOp, MultiplyOp, DivideOp,
//    Idenifier, Constant, AssignmentOp
//}


//fatema
public enum Token_Class
{
    If, Then, Else, Repeat, Until, Read, Write, Return, Endl,
    Int, Float, String,
    End, Elseif, Main,
    Semicolon, Comma, LParanthesis, RParanthesis,
    EqualOp, LessThanOp, GreaterThanOp, NotEqualOp,
    PlusOp, MinusOp, MultiplyOp, DivideOp,
    Identifier, Constant, AssignmentOp
    , Dot,
    //hla
    LCurlyBracket, RCurlyBracket,
    //hla

    //Eman
    AndOp, OrOp
        , Begin, Program

}
//fatema


namespace JASON_Compiler
{


    public class Token
    {
        public string lex;
        public Token_Class token_type;
    }

    public class Scanner
    {
        public List<Token> Tokens = new List<Token>();
        //Hla 

        //Hla

        Dictionary<string, Token_Class> ReservedWords = new Dictionary<string, Token_Class>();
        Dictionary<string, Token_Class> Operators = new Dictionary<string, Token_Class>();

        public Scanner()
        {
            //ReservedWords.Add("IF", Token_Class.If);
            //ReservedWords.Add("BEGIN", Token_Class.Begin);
            //ReservedWords.Add("CALL", Token_Class.Call);
            //ReservedWords.Add("DECLARE", Token_Class.Declare);
            //ReservedWords.Add("END", Token_Class.End);
            //ReservedWords.Add("DO", Token_Class.Do);
            //ReservedWords.Add("ELSE", Token_Class.Else);
            //ReservedWords.Add("ENDIF", Token_Class.EndIf);
            //ReservedWords.Add("ENDUNTIL", Token_Class.EndUntil);
            //ReservedWords.Add("ENDWHILE", Token_Class.EndWhile);
            //ReservedWords.Add("INTEGER", Token_Class.Integer);
            //ReservedWords.Add("PARAMETERS", Token_Class.Parameters);
            //ReservedWords.Add("PROCEDURE", Token_Class.Procedure);
            //ReservedWords.Add("PROGRAM", Token_Class.Program);
            //ReservedWords.Add("READ", Token_Class.Read);
            //ReservedWords.Add("REAL", Token_Class.Real);
            //ReservedWords.Add("SET", Token_Class.Set);
            //ReservedWords.Add("THEN", Token_Class.Then);
            //ReservedWords.Add("UNTIL", Token_Class.Until);
            //ReservedWords.Add("WHILE", Token_Class.While);
            //ReservedWords.Add("WRITE", Token_Class.Write);

            //fatema
            ReservedWords.Clear();
            ReservedWords.Add("if", Token_Class.If);
            ReservedWords.Add("then", Token_Class.Then);
            ReservedWords.Add("else", Token_Class.Else);
            ReservedWords.Add("repeat", Token_Class.Repeat);
            ReservedWords.Add("until", Token_Class.Until);
            ReservedWords.Add("read", Token_Class.Read);
            ReservedWords.Add("write", Token_Class.Write);
            ReservedWords.Add("return", Token_Class.Return);
            ReservedWords.Add("endl", Token_Class.Endl);
            ReservedWords.Add("int", Token_Class.Int);
            ReservedWords.Add("float", Token_Class.Float);
            ReservedWords.Add("string", Token_Class.String);
            ReservedWords.Add("end", Token_Class.End);
            ReservedWords.Add("elseif", Token_Class.Elseif);
            ReservedWords.Add("main", Token_Class.Main);
            //fatema


            Operators.Add(".", Token_Class.Dot);
            Operators.Add(";", Token_Class.Semicolon);
            Operators.Add(",", Token_Class.Comma);
            Operators.Add("(", Token_Class.LParanthesis);
            Operators.Add(")", Token_Class.RParanthesis);


            //Hla
            Operators.Add("{", Token_Class.LCurlyBracket);
            Operators.Add("}", Token_Class.RCurlyBracket);
            //Hla


            Operators.Add("<", Token_Class.LessThanOp);
            Operators.Add(">", Token_Class.GreaterThanOp);
            Operators.Add("<>", Token_Class.NotEqualOp);

            //rana
            Operators.Add("=", Token_Class.EqualOp);
            Operators.Add("+", Token_Class.PlusOp);
            Operators.Add("-", Token_Class.MinusOp);
            Operators.Add("*", Token_Class.MultiplyOp);
            Operators.Add("/", Token_Class.DivideOp);
            Operators.Add(":=", Token_Class.AssignmentOp);

            //Eman 
            Operators.Add("&&", Token_Class.AndOp);
            Operators.Add("||", Token_Class.OrOp);

        }

        public void StartScanning(string SourceCode)
        {
            Tokens.Clear();

            Errors.Error_List.Clear();
            for (int i = 0; i < SourceCode.Length; i++)
            {
                //int j = i;
                char CurrentChar = SourceCode[i];
                string CurrentLexeme = CurrentChar.ToString();

                if (CurrentChar == ' ' || CurrentChar == '\r' || CurrentChar == '\n' || CurrentChar == '\t')
                {
                    continue;
                }

                //Hla - comment statement handling 
                if (CurrentChar == '/' && i + 1 < SourceCode.Length && SourceCode[i + 1] == '*')
                {
                    i += 2;
                    while (i + 1 < SourceCode.Length)
                    {
                        if (SourceCode[i] == '*' && SourceCode[i + 1] == '/')
                        {
                            i += 1;
                            break;
                        }
                        i++;
                    }
                    continue; //Ignore comment completely
                }
                //Hla



                //fatema
                if (char.IsLetter(CurrentChar))
                {
                    string lex = "";

                    while (i < SourceCode.Length && char.IsLetterOrDigit(SourceCode[i]))
                    {
                        lex += SourceCode[i];
                        i++;
                    }

                    i--;
                    FindTokenClass(lex);
                }
                //fatema




                else if (CurrentChar >= '0' && CurrentChar <= '9')
                {
                    string lex = "";

                    while (i < SourceCode.Length && (char.IsDigit(SourceCode[i]) || SourceCode[i] == '.'))
                    {
                        lex += SourceCode[i];
                        i++;
                    }

                    i--;
                    FindTokenClass(lex);
                }

                else if (CurrentChar == '"')
                {
                    string lex = "\"";
                    i++;

                    while (i < SourceCode.Length && SourceCode[i] != '"')
                    {
                        lex += SourceCode[i];
                        i++;
                    }

                    if (i < SourceCode.Length)
                    {
                        lex += "\"";

                    }
                    else
                    {

                        Errors.Error_List.Add("Unterminated string literal");
                    }

                    FindTokenClass(lex);
                }


                //rana
                else
                {
                    // Assignment operator :=
                    if (CurrentChar == ':')
                    {
                        if (i + 1 < SourceCode.Length && SourceCode[i + 1] == '=')
                        {
                            FindTokenClass(":=");
                            i++;
                        }
                        else
                        {

                            Errors.Error_List.Add("Invalid operator ':'");
                        }
                    }
                    //Eman 
                    else if (CurrentChar == '&')
                    {
                        if (i + 1 < SourceCode.Length && SourceCode[i + 1] == '&')
                        {
                            FindTokenClass("&&");
                            i++;
                        }
                        else
                        {

                            Errors.Error_List.Add("Invalid token: &");
                        }
                    }
                    else if (CurrentChar == '|')
                    {
                        if (i + 1 < SourceCode.Length && SourceCode[i + 1] == '|')
                        {
                            FindTokenClass("||");
                            i++;
                        }
                        else
                        {

                            Errors.Error_List.Add("Invalid token: |");
                        }
                    }
                    //Eman
                    else
                    {
                        string op = CurrentChar.ToString();

                        // Arithmetic operators (+ - * / =)
                        if (Operators.ContainsKey(op))
                        {

                            if (CurrentChar == '<')
                            {
                                if (i + 1 < SourceCode.Length && SourceCode[i + 1] == '>')
                                {
                                    FindTokenClass("<>");
                                    i++;
                                }
                                else
                                {
                                    FindTokenClass("<");
                                }
                            }
                            else
                            {
                                FindTokenClass(op);
                            }
                        }
                        //Eman



                        // Hla  
                        else
                        {
                            Errors.Error_List.Add("Invalid token: " + op);
                        }
                        //Hla 
                    }



                }
            }

            JASON_Compiler.TokenStream = Tokens;
        }
        void FindTokenClass(string Lex)
        {
            Token Tok = new Token();
            Tok.lex = Lex;

            // 1. Reserved word
            if (ReservedWords.ContainsKey(Lex))
            {
                Tok.token_type = ReservedWords[Lex];
                Tokens.Add(Tok);
                return;
            }

            // 2. Identifier
            if (isIdentifier(Lex))
            {
                Tok.token_type = Token_Class.Identifier;
                Tokens.Add(Tok);
                return;
            }

            // 3. Number
            if (isConstant(Lex))
            {
                Tok.token_type = Token_Class.Constant;
                Tokens.Add(Tok);
                return;
            }

            // 4. String
            if (Lex.StartsWith("\"") && Lex.EndsWith("\""))
            {
                Tok.token_type = Token_Class.Constant;
                Tokens.Add(Tok);
                return;
            }

            // 5. Operator
            if (Operators.ContainsKey(Lex))
            {
                Tok.token_type = Operators[Lex];
                Tokens.Add(Tok);
                return;
            }

            //  6. Undefined token 

            Errors.Error_List.Add("Undefined token: " + Lex);
        }


        //fatema
        bool isIdentifier(string lex)
        {
            if (string.IsNullOrEmpty(lex)) return false;

            if (!char.IsLetter(lex[0]))
                return false;

            foreach (char c in lex)
            {
                if (!char.IsLetterOrDigit(c))
                    return false;
            }

            return true;
        }
        //fatema


        //full lexeme is dig
        bool isConstant(string lex)
        {
            if (string.IsNullOrEmpty(lex))
                return false;

            bool hasDot = false;
            bool hasDigitBeforeDot = false;
            bool hasDigitAfterDot = false;

            for (int i = 0; i < lex.Length; i++)
            {
                char c = lex[i];

                if (char.IsDigit(c))
                {
                    if (!hasDot)
                        hasDigitBeforeDot = true;
                    else
                        hasDigitAfterDot = true;
                }
                else if (c == '.')
                {
                    // Only one dot allowed
                    if (hasDot)
                        return false;

                    hasDot = true;
                }
                else
                {
                    // Invalid character
                    return false;
                }
            }

            // If no dot → must be digits only
            if (!hasDot)
                return hasDigitBeforeDot;

            // If dot exists → must have digits on BOTH sides
            return hasDigitBeforeDot && hasDigitAfterDot;
        }

    }


}
// parserrrr
using JASON_Compiler;
using System.Collections.Generic;
using System.Windows.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace JASON_Compiler
{
    public class Node
    {
        public List<Node> Children = new List<Node>();

        public string Name;
        public Node(string N)
        {
            this.Name = N;
        }
    }
    public class Parser
    {
        int InputPointer = 0;
        List<Token> TokenStream;
        public Node root;

        public Node StartParsing(List<Token> TokenStream)
        {
            this.InputPointer = 0;
            this.TokenStream = TokenStream;
            root = new Node("Program");
            root.Children.Add(Program());
            if (InputPointer != TokenStream.Count) // if there are still tokens left unparsed, it's an error
            {
                Errors.Error_List.Add(
                    "Parsing Error: Extra tokens after program end");
            }
            return root;
        }


        // FATEMA/////////////////////////////////////
        Node Program()
        {

            Node program = new Node("Program");
            program.Children.Add(Function_List());
            program.Children.Add(Main_Function());
            return program;
        }

        Node Main_Function()
        {
            Node main = new Node("Main_Function");

            //main.Children.Add(match(Token_Class.Int));
            main.Children.Add(Datatype());
            main.Children.Add(match(Token_Class.Main));
            main.Children.Add(match(Token_Class.LParanthesis));
            main.Children.Add(match(Token_Class.RParanthesis));
            main.Children.Add(Function_Body());

            return main;
        }
        //Node Function_List()
        //{
        //    Node list = new Node("Function_List");
        //    if (InputPointer < TokenStream.Count &&
        //        TokenStream[InputPointer].token_type == Token_Class.Int)
        //    {
        //        list.Children.Add(Function_Statement());
        //        list.Children.Add(Function_List());
        //    }

        //    // epsilon case skip the if and return the list
        //    return list;
        //}

        Node Function_List()
        {
            Node list = new Node("Function_List");
            //if (InputPointer < TokenStream.Count &&
            //    TokenStream[InputPointer].token_type == Token_Class.Int &&
            //    InputPointer + 1 < TokenStream.Count &&
            //    TokenStream[InputPointer + 1].token_type != Token_Class.Main)
            if (InputPointer < TokenStream.Count &&
            (TokenStream[InputPointer].token_type == Token_Class.Int ||
            TokenStream[InputPointer].token_type == Token_Class.Float ||
            TokenStream[InputPointer].token_type == Token_Class.String) &&
            InputPointer + 1 < TokenStream.Count &&
            TokenStream[InputPointer + 1].token_type != Token_Class.Main)
            {
                list.Children.Add(Function_Statement());
                list.Children.Add(Function_List());
            }
            return list;
        }

        Node Function_Statement()
        {
            Node func = new Node("Function_Statement");

            func.Children.Add(Function_Declaration());
            func.Children.Add(Function_Body());

            return func;
        }
        Node Function_Body()
        {
            Node body = new Node("Function_Body");

            body.Children.Add(match(Token_Class.LCurlyBracket));
            body.Children.Add(Statements());
            body.Children.Add(Return_Statement());
            body.Children.Add(match(Token_Class.RCurlyBracket));

            return body;
        }
        Node Repeat_Statement()
        {
            Node repeat = new Node("Repeat_Statement");

            repeat.Children.Add(match(Token_Class.Repeat));
            repeat.Children.Add(Statements());
            repeat.Children.Add(match(Token_Class.Until));
            repeat.Children.Add(Condition_Statement());

            return repeat;
        }

        // FATEMA/////////////////////////////////////


        // Implement your logic here
        //Hla/////////////////////////////////////////////////////////////////////////////
        Node FunctionCall()
        {
            Node Functioncall = new Node("FunctionCall");
            Functioncall.Children.Add(match(Token_Class.Identifier));
            Functioncall.Children.Add(match(Token_Class.LParanthesis));
            Functioncall.Children.Add(Args());
            Functioncall.Children.Add(match(Token_Class.RParanthesis));
            return Functioncall;
        }

        //Node Args()
        //{
        //    Node args = new Node("Args");
        //    if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.Identifier)
        //    {
        //        args.Children.Add(match(Token_Class.Identifier));
        //        args.Children.Add(Args_Dash());
        //    }
        //    return args;
        //}

        Node Args()
        {
            Node args = new Node("Args");

            if (InputPointer < TokenStream.Count &&
               (TokenStream[InputPointer].token_type == Token_Class.Identifier ||
                TokenStream[InputPointer].token_type == Token_Class.Constant ||
                TokenStream[InputPointer].token_type == Token_Class.LParanthesis))
            {
                args.Children.Add(Expression());
                args.Children.Add(Args_Dash());
            }

            return args;
        }

        //Node Args_Dash()
        //{
        //    Node args_Dash = new Node("Args_Dash");
        //    if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.Comma)
        //    {
        //        args_Dash.Children.Add(match(Token_Class.Comma));
        //        args_Dash.Children.Add(match(Token_Class.Identifier));
        //        args_Dash.Children.Add(Args_Dash());
        //    }

        //    return args_Dash;
        //}

        Node Args_Dash()
        {
            Node args_Dash = new Node("Args_Dash");

            if (InputPointer < TokenStream.Count &&
                TokenStream[InputPointer].token_type == Token_Class.Comma)
            {
                args_Dash.Children.Add(match(Token_Class.Comma));
                args_Dash.Children.Add(Expression());
                args_Dash.Children.Add(Args_Dash());
            }

            return args_Dash;
        }


        //Node Term()
        //{
        //    Node term = new Node("Term");
        //    if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.Constant)
        //    {
        //        if (TokenStream[InputPointer].lex.StartsWith("\""))
        //        {
        //            term.Children.Add(new Node("String: " + TokenStream[InputPointer].lex));
        //        }
        //        else
        //        {
        //            term.Children.Add(new Node("Number: " + TokenStream[InputPointer].lex));
        //        }
        //        term.Children.Add(match(Token_Class.Constant));
        //    }
        //    else if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.Identifier)
        //    {
        //        term.Children.Add(match(Token_Class.Identifier));
        //        term.Children.Add(Term_Dash());

        //    }
        //    return term;
        //}
        //Node Term()
        //{
        //    Node term = new Node("Term");

        //    if (InputPointer < TokenStream.Count)
        //    {
        //        if (TokenStream[InputPointer].token_type == Token_Class.Constant &&
        //            !TokenStream[InputPointer].lex.StartsWith("\""))
        //        {
        //            term.Children.Add(match(Token_Class.Constant));
        //        }

        //        else if (TokenStream[InputPointer].token_type == Token_Class.Identifier)
        //        {
        //            term.Children.Add(match(Token_Class.Identifier));
        //            term.Children.Add(Term_Dash());
        //        }
        //    }
        //    else
        //    {
        //        Errors.Error_List.Add(
        //            "Parsing Error: Invalid Term near "
        //            + TokenStream[InputPointer].lex + "\n");
        //    }

        //    return term;
        //}
        Node Term()
        {
            Node term = new Node("Term");

            if (InputPointer >= TokenStream.Count)
            {
                Errors.Error_List.Add("Parsing Error: Expected Term\n");
                return term;
            }

            if (TokenStream[InputPointer].token_type == Token_Class.Constant &&
                !TokenStream[InputPointer].lex.StartsWith("\""))
            {
                term.Children.Add(match(Token_Class.Constant));
            }

            else if (TokenStream[InputPointer].token_type == Token_Class.Identifier)
            {
                term.Children.Add(match(Token_Class.Identifier));
                term.Children.Add(Term_Dash());
            }

            else
            {
                Errors.Error_List.Add(
                    "Parsing Error: Invalid Term near "
                    + TokenStream[InputPointer].lex + "\n");
                InputPointer++; // this line is to prevent infinite loops in case of invalid tokens, it moves the pointer forward to skip the problematic token and continue parsing the rest of the input
            }

            return term;
        }


        Node Term_Dash()
        {
            Node term_Dash = new Node("Term_Dash");
            if (InputPointer < TokenStream.Count &&
        TokenStream[InputPointer].token_type == Token_Class.LParanthesis)
            {
                term_Dash.Children.Add(match(Token_Class.LParanthesis));
                term_Dash.Children.Add(Args());
                term_Dash.Children.Add(match(Token_Class.RParanthesis));
            }
            return term_Dash;
        }

        Node Equation()
        {
            Node equation = new Node("Equation");
            equation.Children.Add(Factor());
            equation.Children.Add(Equation_Dash());
            return equation;
        }


        Node Equation_Dash()
        {
            Node equation_Dash = new Node("Equation_Dash");
            if (InputPointer < TokenStream.Count)
            {
                Token_Class t = TokenStream[InputPointer].token_type;

                if (t == Token_Class.PlusOp ||
                    t == Token_Class.MinusOp)
                {
                    equation_Dash.Children.Add(Addop());
                    equation_Dash.Children.Add(Factor());
                    equation_Dash.Children.Add(Equation_Dash());
                }
            }
            return equation_Dash;
        }
        //Node Addop()
        //{
        //    Node addop = new Node("Addop");
        //    if (InputPointer < TokenStream.Count)
        //    {
        //        Token_Class t = TokenStream[InputPointer].token_type;

        //        if (t == Token_Class.PlusOp)
        //            addop.Children.Add(match(Token_Class.PlusOp));

        //        else if (t == Token_Class.MinusOp)
        //            addop.Children.Add(match(Token_Class.MinusOp));
        //    }
        //    return addop;
        //}

        //Node Mulop()
        //{
        //    Node mulop = new Node("Mulop");
        //    if (InputPointer < TokenStream.Count)
        //    {
        //        Token_Class t = TokenStream[InputPointer].token_type;

        //        if (t == Token_Class.MultiplyOp)
        //            mulop.Children.Add(match(Token_Class.MultiplyOp));

        //        else if (t == Token_Class.DivideOp)
        //            mulop.Children.Add(match(Token_Class.DivideOp));
        //    }
        //    return mulop;
        //}

        Node Addop()
        {
            Node addop = new Node("Addop");
            if (InputPointer < TokenStream.Count)
            {
                Token_Class t = TokenStream[InputPointer].token_type;
                if (t == Token_Class.PlusOp)
                    addop.Children.Add(match(Token_Class.PlusOp));
                else if (t == Token_Class.MinusOp)
                    addop.Children.Add(match(Token_Class.MinusOp));
                else  
                    Errors.Error_List.Add(
                        "Parsing Error: Expected + or - but found "
                        + t.ToString() + "\r\n");
            }
            return addop;
        }

        Node Mulop()
        {
            Node mulop = new Node("Mulop");
            if (InputPointer < TokenStream.Count)
            {
                Token_Class t = TokenStream[InputPointer].token_type;
                if (t == Token_Class.MultiplyOp)
                    mulop.Children.Add(match(Token_Class.MultiplyOp));
                else if (t == Token_Class.DivideOp)
                    mulop.Children.Add(match(Token_Class.DivideOp));
                else
                    Errors.Error_List.Add(
                        "Parsing Error: Expected * or / but found "
                        + t.ToString() + "\r\n");
            }
            return mulop;
        }

        Node Factor()
        {
            Node factor = new Node("Factor");
            if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.LParanthesis)
            {
                factor.Children.Add(match(Token_Class.LParanthesis));
                factor.Children.Add(Equation());
                factor.Children.Add(match(Token_Class.RParanthesis));
            }
            else
            {
                factor.Children.Add(Term());
            }
            factor.Children.Add(Factor_Dash());
            return factor;
        }
        //Node Factor_Dash()
        //{
        //    Node factor_Dash = new Node("Factor_Dash");
        //    if (InputPointer < TokenStream.Count)
        //    {
        //        Token_Class t = TokenStream[InputPointer].token_type;

        //        if (t == Token_Class.MultiplyOp ||
        //            t == Token_Class.DivideOp)
        //        {
        //            factor_Dash.Children.Add(Mulop());
        //            //factor_Dash.Children.Add(Term());
        //            factor_Dash.Children.Add(Factor());
        //            factor_Dash.Children.Add(Factor_Dash());
        //        }
        //    }
        //    return factor_Dash;
        //}

        Node Factor_Dash()
        {
            Node factor_Dash = new Node("Factor_Dash");

            if (InputPointer < TokenStream.Count)
            {
                Token_Class t = TokenStream[InputPointer].token_type;

                if (t == Token_Class.MultiplyOp ||
                    t == Token_Class.DivideOp)
                {
                    factor_Dash.Children.Add(Mulop());

                    // modification
                    //factor_Dash.Children.Add(Term());
                    factor_Dash.Children.Add(Factor());

                    factor_Dash.Children.Add(Factor_Dash());
                }
            }

            return factor_Dash;
        }


        //Node Expression()
        //{
        //    Node expression = new Node("Expression");
        //    if (InputPointer < TokenStream.Count)
        //    {
        //        Token t = TokenStream[InputPointer];

        //        if (t.token_type == Token_Class.Constant)
        //        {
        //            if (t.lex.StartsWith("\""))
        //            {
        //                expression.Children.Add(new Node("String: " + t.lex));
        //            }
        //            else
        //            {
        //                expression.Children.Add(new Node("Number: " + t.lex));
        //            }

        //            expression.Children.Add(match(Token_Class.Constant));
        //        }
        //        else
        //        {
        //            expression.Children.Add(Equation());
        //        }
        //    }
        //    return expression;
        //}

        //Node Expression()
        //{
        //    Node expression = new Node("Expression");

        //    if (InputPointer < TokenStream.Count)
        //    {
        //        Token t = TokenStream[InputPointer];

        //        // string literal ONLY
        //        if (t.token_type == Token_Class.Constant &&
        //            t.lex.StartsWith("\""))
        //        {
        //            expression.Children.Add(match(Token_Class.Constant));
        //        }
        //        else
        //        {
        //            expression.Children.Add(Equation());
        //        }
        //    }

        //    return expression;
        //}
        Node Expression()
        {
            Node expression = new Node("Expression");

            if (InputPointer >= TokenStream.Count)
            {
                Errors.Error_List.Add("Parsing Error: Expected Expression\n");
                return expression;
            }

            Token_Class t = TokenStream[InputPointer].token_type;

            // string literal
            if (t == Token_Class.Constant &&
                TokenStream[InputPointer].lex.StartsWith("\""))
            {
                expression.Children.Add(match(Token_Class.Constant));
            }

            // valid Equation starters
            else if (t == Token_Class.Identifier ||
                     t == Token_Class.Constant ||
                     t == Token_Class.LParanthesis)
            {
                expression.Children.Add(Equation());
            }

            else
            {
                Errors.Error_List.Add(
                    "Parsing Error: Invalid Expression near "
                    + TokenStream[InputPointer].lex + "\n");

                InputPointer++; // this line is to prevent infinite loops in case of invalid tokens, it moves the pointer forward to skip the problematic token and continue parsing the rest of the input
            }

            return expression;
        }
        //Hla/////////////////////////////////////////////////////////////////////////////


        // RANA //////////////////////////////////////////////////////////////

        Node Statements()
        {
            Node statements = new Node("Statements");

            while (InputPointer < TokenStream.Count)
            {
                Token_Class t = TokenStream[InputPointer].token_type;

                if (t == Token_Class.Identifier)
                {
                    statements.Children.Add(Assignment_Statement());
                }
                else if (t == Token_Class.Write)
                {
                    statements.Children.Add(Write_Statement());
                }
                else if (t == Token_Class.Read)
                {
                    statements.Children.Add(Read_Statement());
                }
                else if (t == Token_Class.Repeat)
                {
                    statements.Children.Add(Repeat_Statement());
                }
                else if (t == Token_Class.If)
                {
                    statements.Children.Add(If_Statement());
                }
                else if (t == Token_Class.Int ||
                         t == Token_Class.Float ||
                         t == Token_Class.String)
                {
                    statements.Children.Add(Declaration_Statement());
                }
                else
                {
                    break;
                }
            }

            return statements;
        }
        //Node Statements()
        //{
        //    Node statements = new Node("Statements");

        //    if (InputPointer < TokenStream.Count)
        //    {
        //        Token_Class t = TokenStream[InputPointer].token_type;

        //        if (
        //            t == Token_Class.Identifier ||
        //            t == Token_Class.Write ||
        //            t == Token_Class.Read ||
        //            t == Token_Class.Repeat ||
        //            t == Token_Class.If ||
        //            t == Token_Class.Int ||
        //            t == Token_Class.Float ||
        //            t == Token_Class.String
        //           )
        //        {
        //            statements.Children.Add(Statement());
        //            statements.Children.Add(Statements());
        //        }
        //    }

        //    return statements;
        //}
        Node Statement()
        {
            Node stmt = new Node("Statement");

            Token_Class t = TokenStream[InputPointer].token_type;

            if (t == Token_Class.Identifier)
                stmt.Children.Add(Assignment_Statement());

            else if (t == Token_Class.Write)
                stmt.Children.Add(Write_Statement());

            else if (t == Token_Class.Read)
                stmt.Children.Add(Read_Statement());

            else if (t == Token_Class.Repeat)
                stmt.Children.Add(Repeat_Statement());

            else if (t == Token_Class.If)
                stmt.Children.Add(If_Statement());

            else if (t == Token_Class.Int ||
                     t == Token_Class.Float ||
                     t == Token_Class.String)
                stmt.Children.Add(Declaration_Statement());

            return stmt;
        }

        Node Assignment_Statement()
        {
            Node assign = new Node("Assignment_Statement");

            assign.Children.Add(match(Token_Class.Identifier));
            assign.Children.Add(match(Token_Class.AssignmentOp));
            assign.Children.Add(Expression());
            assign.Children.Add(match(Token_Class.Semicolon));

            return assign;
        }

        Node Write_Statement()
        {
            Node write = new Node("Write_Statement");

            write.Children.Add(match(Token_Class.Write));

            if (InputPointer < TokenStream.Count &&
                TokenStream[InputPointer].token_type == Token_Class.Endl)
            {
                write.Children.Add(match(Token_Class.Endl));
            }
            else
            {
                write.Children.Add(Expression());
            }

            write.Children.Add(match(Token_Class.Semicolon));

            return write;
        }

        Node Read_Statement()
        {
            Node read = new Node("Read_Statement");

            read.Children.Add(match(Token_Class.Read));
            read.Children.Add(match(Token_Class.Identifier));
            read.Children.Add(match(Token_Class.Semicolon));

            return read;
        }

        Node Return_Statement()
        {
            Node ret = new Node("Return_Statement");

            ret.Children.Add(match(Token_Class.Return));
            ret.Children.Add(Expression());
            ret.Children.Add(match(Token_Class.Semicolon));

            return ret;
        }

        // RANA //////////////////////////////////////////////////////////////








        // EMAN//////////////////

        // Condition → identifier Condition_Operator Term
        Node Condition()
        {
            Node condition = new Node("Condition");
            //condition.Children.Add(match(Token_Class.Identifier));
            //condition.Children.Add(Condition_Operator());
            //condition.Children.Add(Term());
            condition.Children.Add(Expression());
            condition.Children.Add(Condition_Operator());
            condition.Children.Add(Expression());
            return condition;
        }

        // Condition_Operator → < | > | = | <>
        //Node Condition_Operator()
        //{
        //    Node condOp = new Node("Condition_Operator");
        //    if (InputPointer < TokenStream.Count)
        //    {
        //        Token_Class t = TokenStream[InputPointer].token_type;
        //        if (t == Token_Class.LessThanOp)
        //            condOp.Children.Add(match(Token_Class.LessThanOp));
        //        else if (t == Token_Class.GreaterThanOp)
        //            condOp.Children.Add(match(Token_Class.GreaterThanOp));
        //        else if (t == Token_Class.EqualOp)
        //            condOp.Children.Add(match(Token_Class.EqualOp));
        //        else if (t == Token_Class.NotEqualOp)
        //            condOp.Children.Add(match(Token_Class.NotEqualOp));
        //        else
        //            Errors.Error_List.Add(
        //                "Parsing Error: Expected condition operator (<, >, =, <>) but found "
        //                + t.ToString() + "\r\n");
        //    }
        //    return condOp;
        //}

        Node Condition_Operator()
        {
            Node condOp = new Node("Condition_Operator");
            if (InputPointer >= TokenStream.Count) 
            {
                Errors.Error_List.Add(
                    "Parsing Error: Expected condition operator but reached end of input\r\n");
                return condOp;
            }
            Token_Class t = TokenStream[InputPointer].token_type;
            if (t == Token_Class.LessThanOp)
                condOp.Children.Add(match(Token_Class.LessThanOp));
            else if (t == Token_Class.GreaterThanOp)
                condOp.Children.Add(match(Token_Class.GreaterThanOp));
            else if (t == Token_Class.EqualOp)
                condOp.Children.Add(match(Token_Class.EqualOp));
            else if (t == Token_Class.NotEqualOp)
                condOp.Children.Add(match(Token_Class.NotEqualOp));
            else
                Errors.Error_List.Add(
                    "Parsing Error: Expected condition operator (<, >, =, <>) but found "
                    + t.ToString() + "\r\n");
            return condOp;
        }


        // Condition_Statement  → Condition Condition_Statement'
        // Condition_Statement' → Boolean_Operator Condition Condition_Statement' | ε
        Node Condition_Statement()
        {
            Node condStmt = new Node("Condition_Statement");
            condStmt.Children.Add(Condition());
            condStmt.Children.Add(Condition_Statement_Dash());
            return condStmt;
        }

        Node Condition_Statement_Dash()
        {
            Node condStmt_Dash = new Node("Condition_Statement_Dash");
            if (InputPointer < TokenStream.Count)
            {
                Token_Class t = TokenStream[InputPointer].token_type;
                if (t == Token_Class.AndOp || t == Token_Class.OrOp)
                {
                    condStmt_Dash.Children.Add(Boolean_Operator());
                    condStmt_Dash.Children.Add(Condition());
                    condStmt_Dash.Children.Add(Condition_Statement_Dash());
                }
            }
            // ε
            return condStmt_Dash;
        }

        // Boolean_Operator → && | ||
        //Node Boolean_Operator()
        //{
        //    Node boolOp = new Node("Boolean_Operator");
        //    if (InputPointer < TokenStream.Count)
        //    {
        //        Token_Class t = TokenStream[InputPointer].token_type;
        //        if (t == Token_Class.AndOp)
        //            boolOp.Children.Add(match(Token_Class.AndOp));
        //        else if (t == Token_Class.OrOp)
        //            boolOp.Children.Add(match(Token_Class.OrOp));
        //        else
        //            Errors.Error_List.Add(
        //                "Parsing Error: Expected boolean operator (&& or ||) but found "
        //                + t.ToString() + "\r\n");
        //    }
        //    return boolOp;
        //}

        Node Boolean_Operator()
        {
            Node boolOp = new Node("Boolean_Operator");
            if (InputPointer >= TokenStream.Count)
            {
                Errors.Error_List.Add(
                    "Parsing Error: Expected boolean operator but reached end of input\r\n");
                return boolOp;
            }
            Token_Class t = TokenStream[InputPointer].token_type;
            if (t == Token_Class.AndOp)
                boolOp.Children.Add(match(Token_Class.AndOp));
            else if (t == Token_Class.OrOp)
                boolOp.Children.Add(match(Token_Class.OrOp));
            else
                Errors.Error_List.Add(
                    "Parsing Error: Expected boolean operator (&& or ||) but found "
                    + t.ToString() + "\r\n");
            return boolOp;
        }

        // If_Statement → if Condition_Statement then Statements If_Tail
        // If_Tail      → Else_If_Statement | Else_Statement | end
        Node If_Statement()
        {
            Node ifStmt = new Node("If_Statement");
            ifStmt.Children.Add(match(Token_Class.If));
            ifStmt.Children.Add(Condition_Statement());
            ifStmt.Children.Add(match(Token_Class.Then));
            ifStmt.Children.Add(Statements());
            ifStmt.Children.Add(If_Tail());
            return ifStmt;
        }

        //Node If_Tail()
        //{
        //    Node ifTail = new Node("If_Tail");
        //    if (InputPointer < TokenStream.Count)
        //    {
        //        Token_Class t = TokenStream[InputPointer].token_type;
        //        if (t == Token_Class.Elseif)
        //            ifTail.Children.Add(Else_If_Statement());
        //        else if (t == Token_Class.Else)
        //            ifTail.Children.Add(Else_Statement());
        //        else if (t == Token_Class.End)
        //            ifTail.Children.Add(match(Token_Class.End));
        //        else
        //            Errors.Error_List.Add(
        //                "Parsing Error: Expected 'elseif', 'else', or 'end' but found "
        //                + t.ToString() + "\r\n");
        //    }
        //    return ifTail;
        //}
        Node If_Tail()
        {
            Node ifTail = new Node("If_Tail");
            if (InputPointer >= TokenStream.Count)
            {
                Errors.Error_List.Add(
                    "Parsing Error: Expected 'elseif', 'else', or 'end' but reached end of input\r\n");
                return ifTail;
            }
            Token_Class t = TokenStream[InputPointer].token_type;
            if (t == Token_Class.Elseif)
                ifTail.Children.Add(Else_If_Statement());
            else if (t == Token_Class.Else)
                ifTail.Children.Add(Else_Statement());
            else if (t == Token_Class.End)
                ifTail.Children.Add(match(Token_Class.End));
            else
                Errors.Error_List.Add(
                    "Parsing Error: Expected 'elseif', 'else', or 'end' but found "
                    + t.ToString() + "\r\n");
            return ifTail;
        }


        // Else_If_Statement → elseif Condition_Statement then Statements If_Tail
        Node Else_If_Statement()
        {
            Node elseIfStmt = new Node("Else_If_Statement");
            elseIfStmt.Children.Add(match(Token_Class.Elseif));
            elseIfStmt.Children.Add(Condition_Statement());
            elseIfStmt.Children.Add(match(Token_Class.Then));
            elseIfStmt.Children.Add(Statements());
            elseIfStmt.Children.Add(If_Tail());
            return elseIfStmt;
        }

        // Else_Statement → else Statements end
        Node Else_Statement()
        {
            Node elseStmt = new Node("Else_Statement");
            elseStmt.Children.Add(match(Token_Class.Else));
            elseStmt.Children.Add(Statements());
            elseStmt.Children.Add(match(Token_Class.End));
            return elseStmt;
        }

        // EMAN/////////////////




        //MUSTAFA////////////////////




        Node Declaration_Statement()
        {
            Node decl = new Node("Declaration_Statement");
            decl.Children.Add(Datatype());
            decl.Children.Add(DeclList());
            decl.Children.Add(match(Token_Class.Semicolon));
            return decl;
        }

        //Node Datatype()
        //{
        //    Node dt = new Node("Datatype");
        //    if (InputPointer < TokenStream.Count)
        //    {
        //        Token_Class t = TokenStream[InputPointer].token_type;
        //        if (t == Token_Class.Int)
        //            dt.Children.Add(match(Token_Class.Int));
        //        else if (t == Token_Class.Float)
        //            dt.Children.Add(match(Token_Class.Float));
        //        else if (t == Token_Class.String)
        //            dt.Children.Add(match(Token_Class.String));
        //    }
        //    return dt;
        //}
        Node Datatype()
        {
            Node dt = new Node("Datatype");
            if (InputPointer < TokenStream.Count)
            {
                Token_Class t = TokenStream[InputPointer].token_type;
                if (t == Token_Class.Int)
                    dt.Children.Add(match(Token_Class.Int));
                else if (t == Token_Class.Float)
                    dt.Children.Add(match(Token_Class.Float));
                else if (t == Token_Class.String)
                    dt.Children.Add(match(Token_Class.String));
                else  // ADD THIS
                    Errors.Error_List.Add(
                        "Parsing Error: Expected datatype (int/float/string) but found "
                        + t.ToString() + "\r\n");
            }
            else
                Errors.Error_List.Add("Parsing Error: Expected datatype but reached end of input\r\n");
            return dt;
        }

        Node DeclList()
        {
            Node declList = new Node("DeclList");
            declList.Children.Add(DeclItem());
            declList.Children.Add(DeclList_Dash());
            return declList;
        }

        Node DeclList_Dash()
        {
            Node declList_Dash = new Node("DeclList_Dash");
            if (InputPointer < TokenStream.Count &&
                TokenStream[InputPointer].token_type == Token_Class.Comma)
            {
                declList_Dash.Children.Add(match(Token_Class.Comma));
                declList_Dash.Children.Add(DeclItem());
                declList_Dash.Children.Add(DeclList_Dash());
            }
            // ε
            return declList_Dash;
        }

        Node DeclItem()
        {
            Node declItem = new Node("DeclItem");
            declItem.Children.Add(match(Token_Class.Identifier));
            declItem.Children.Add(DeclItem_Dash());
            return declItem;
        }

        Node DeclItem_Dash()
        {
            Node declItem_Dash = new Node("DeclItem_Dash");
            if (InputPointer < TokenStream.Count &&
                TokenStream[InputPointer].token_type == Token_Class.AssignmentOp)
            {
                declItem_Dash.Children.Add(match(Token_Class.AssignmentOp));
                declItem_Dash.Children.Add(Expression());
            }
            // ε
            return declItem_Dash;
        }

        // Rule 14: FunctionName → identifier
        Node FunctionName()
        {
            Node funcName = new Node("FunctionName");
            funcName.Children.Add(match(Token_Class.Identifier));
            return funcName;
        }

        // Rule 15: Parameter → Datatype identifier
        Node Parameter()
        {
            Node param = new Node("Parameter");
            param.Children.Add(Datatype());
            param.Children.Add(match(Token_Class.Identifier));
            return param;
        }

        // Rule 16: 

        Node Function_Declaration()
        {
            Node funcDecl = new Node("Function_Declaration");
            funcDecl.Children.Add(Datatype());
            funcDecl.Children.Add(FunctionName());
            funcDecl.Children.Add(match(Token_Class.LParanthesis));
            funcDecl.Children.Add(ParamList());
            funcDecl.Children.Add(match(Token_Class.RParanthesis));
            return funcDecl;
        }

        Node ParamList()
        {
            Node paramList = new Node("ParamList");
            if (InputPointer < TokenStream.Count)
            {
                Token_Class t = TokenStream[InputPointer].token_type;
                if (t == Token_Class.Int ||
                    t == Token_Class.Float ||
                    t == Token_Class.String)
                {
                    paramList.Children.Add(Parameter());
                    paramList.Children.Add(ParamList_Dash());
                }
            }
            // ε
            return paramList;
        }

        Node ParamList_Dash()
        {
            Node paramList_Dash = new Node("ParamList_Dash");
            if (InputPointer < TokenStream.Count &&
                TokenStream[InputPointer].token_type == Token_Class.Comma)
            {
                paramList_Dash.Children.Add(match(Token_Class.Comma));
                paramList_Dash.Children.Add(Parameter());
                paramList_Dash.Children.Add(ParamList_Dash());
            }
            // ε
            return paramList_Dash;
        }




        //MUSTAFA////////////////////






        public Node match(Token_Class ExpectedToken)
        {

            if (InputPointer < TokenStream.Count)
            {
                if (ExpectedToken == TokenStream[InputPointer].token_type)
                {
                    InputPointer++;
                    Node newNode = new Node(ExpectedToken.ToString());

                    return newNode;

                }

                else
                {
                    Errors.Error_List.Add("Parsing Error: Expected "
                        + ExpectedToken.ToString() + " and " +
                        TokenStream[InputPointer].token_type.ToString() +
                        "  found\r\n");
                    InputPointer++;
                    return null;
                }
            }
            else
            {
                Errors.Error_List.Add("Parsing Error: Expected "
                        + ExpectedToken.ToString() + "\r\n");
                //InputPointer++; 
                return null;
            }
            //else
            //{
            //    Errors.Error_List.Add(
            //        "Parsing Error: Expected "
            //        + ExpectedToken.ToString()
            //        + " and "
            //        + TokenStream[InputPointer].token_type.ToString()
            //        + " found\n");

            //    return null;
            //}
        }

        public static TreeNode PrintParseTree(Node root)
        {
            TreeNode tree = new TreeNode("Parse Tree");
            TreeNode treeRoot = PrintTree(root);
            if (treeRoot != null)
                tree.Nodes.Add(treeRoot);
            return tree;
        }
        static TreeNode PrintTree(Node root)
        {
            if (root == null || root.Name == null)
                return null;
            TreeNode tree = new TreeNode(root.Name);
            if (root.Children.Count == 0)
                return tree;
            foreach (Node child in root.Children)
            {
                if (child == null)
                    continue;
                tree.Nodes.Add(PrintTree(child));
            }
            return tree;
        }
    }
}

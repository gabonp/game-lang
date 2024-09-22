using System;
using static TokenType;

public class Parser
{
    private List<Token> tokens;

    public Parser(List<Token> tokens)
    {
        this.tokens = tokens;
    }

    public Stmt parse()
    {
        try
        {
            if(!isAtEnd)
                return parseStatement();
            else
                return null!;
        }
        catch
        {
            System.Console.WriteLine("error");
            return null!;
        }
    }

    private int curr = 0;

    // private
    private Stmt parseStatement()
    {
        if(check(PRINT))
        {
            advance();
            return parsePrint();
        }

        return parseExprStmt();
    }

    private Stmt parsePrint()
    {
        Expr value = parseExpression();
        lookfor(SEMICOLON, "Expected ';' after print statement.");
        return new stmtPrint(value);
    }

    private Stmt parseExprStmt()
    {
        Expr expr = parseExpression();
        lookfor(SEMICOLON, "Expected ';' after expression statement.");

        return new stmtExpr(expr);
    }

    private Expr parseExpression()
    {
        return parseRelation();
    }

    private Expr parseRelation()
    {
        Expr term = parseTerm();

        while(check(LESS, LESS_EQUAL))
        {
            Token oper = currToken;
            advance();

            Expr term_ = parseTerm();
            term = new exprBinary(term, oper, term_);
        }

        return term;
    }

    private Expr parseTerm()
    {
        Expr cast = parseCast();

        while( check(MINUS, PLUS) )
        {
            Token oper = currToken;
            advance();

            Expr cast_ = parseCast();
            cast = new exprBinary(cast, oper, cast_);
        }

        return cast;
    }

    private Expr parseCast()
    {
        if(check(INT))
        {
            Token to = currToken;
            advance();

            Expr cast_ = parseCast();
            return new exprCast(to, cast_);
        }
        
        return parseUnary();
    }

    private Expr parseUnary()
    {
        if(check(MINUS))
        {
            Token oper = currToken;
            advance();

            Expr expr = parseCast();
            return new exprUnary(oper, expr);
        }

        return parsePrimary();
    }

    private Expr parsePrimary()
    {
        if(check(FALSE))
        {
            advance();
            return new exprLiteral(false);
        }

        if(check(TRUE))
        {
            advance();
            return new exprLiteral(true);
        }

        if(check(NUMBER, STRING))
        {
            object value = currToken.literal!;
            advance();
            return new exprLiteral(value);
        }

        if(check(LEFT_PAREN))
        {
            advance();
            Expr expr = parseExpression();

            if( check(MARK) )
            {
                advance();
                Expr mainBranch = parseExpression();

                lookfor(COLON, "Expected ':' after expression in conditional operator.");

                Expr elseBranch = parseExpression();

                lookfor(RIGHT_PAREN, "Expected ')' after expression.");

                return new exprTernary(expr, mainBranch, elseBranch);
            }

            lookfor(RIGHT_PAREN, "Expected ')' after expression.");

            return new exprGroup(expr);
        }

        // [TODO] report an error. "Expected expression".
        throw new Exception();
    }

    // private tools
    private bool isAtEnd
    {
        get { return currToken.type == EOF; }
    }

    private void advance()
    {
        if(!isAtEnd) curr++;
    }

    private Token currToken
    {
        get { return tokens[curr]; }
    }

    private bool check(TokenType type)
    {
        if(isAtEnd) return false;

        return currToken.type == type;
    }

    private bool check(params TokenType[] types)
    {
        foreach(TokenType type in types)
        {
            if(check(type))
                return true;
        }

        return false;
    }

    private void lookfor(TokenType type, string message)
    {
        if(check(type))
        {
            advance();
            return;
        }

        // [TODO] report an error.
        System.Console.WriteLine(message);
        throw new Exception();
    }
}
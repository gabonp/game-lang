using static TokenType;

public class Scanner
{
    private string code;
    private List<Token> tokens = new List<Token>();

    public Scanner(string code)
    {
        this.code = code;
    }

    private int start = 0, curr = 0;
    private int line = 1, colStart = 1, colEnd = 1;

    public void scanTokens(ref Error error)
    {
        while(!isAtEnd)
        {
            identify(ref error);

            start = curr;
            colStart = colEnd;
        }

        tokens.Add( new Token(EOF, "", null, line, colStart, colStart));
    }

    public List<Token> getTokens()
    {
        return tokens;
    }

    // private
    private void identify(ref Error error)
    {
        char c = currChar;

        switch(c)
        {
            case ' ':
            case '\t':
                advance();
                return;

            case '\n':
                advance();
                line++;
                colEnd = 1;
                return;
        }

        if( isDigit(c) ) // it's a number
            readNumber(ref error); 
        else if( isAlpha(c) ) // it's an identifier or a keyword
            readIdentifier();
        else if( c == '\"' ) // it's a string
            readString(ref error);
        else // it's a symbol
            readSymbol(ref error);
    }

    private void readNumber(ref Error error)
    {
        string number = "";

        while( isDigit(currChar) )
        {
            number += currChar;
            advance();
        }

        try
        {
            addToken(NUMBER, number, long.Parse(number));
        }
        catch(System.OverflowException)
        {
            error.error(line, colStart, "This value is too large for an integer.");
        }
    }

    private void readIdentifier()
    {
        string ident = "";

        while(!isAtEnd && isAlpha(currChar))
        {
            ident += currChar;
            advance();
        }

        if(!isKeyword(ident))
            addToken(IDENTIFIER, ident);
    }

    private void readString(ref Error error)
    {
        advance(); // the opening "
        string str = "";

        while( !isAtEnd && currChar != '\n' && currChar != '\"' )
        {
            str += currChar;
            advance();
        }

        if(!isAtEnd && currChar == '\"')
        {
            advance();
            addToken(STRING, str, str);
            return;
        }

        error.error(line, colStart, "Unterminated string.");
    }

    private void readSymbol(ref Error error)
    {
        char c = currChar;
        advance();

        switch(c)
        {
            // single-character tokens
            case '(': // left parenthesis
                addToken(LEFT_PAREN, "(");
                return;

            case ')': // right parenthesis
                addToken(RIGHT_PAREN, ")");
                return;

            case ':': // colon
                addToken(COLON, ":");
                return;

            case '?': // mark
                addToken(MARK, "?");
                return;

            case '-': // minus
                addToken(MINUS, "-");
                return;

            case '+': // plus
                addToken(PLUS, "+");
                return;

            case ';': // semicolon
                addToken(SEMICOLON, ";");
                return;

            // one or two character tokens
            case '<': // less, less or equal
                if(currChar == '=')
                {
                    advance();
                    addToken(LESS_EQUAL, "<=");
                    return;
                }

                addToken(LESS, "<");
                return;

            case '/': // slash, comment
                if(currChar == '/')
                {
                    commentDetected();
                    return;
                }
                return;

            default:
                error.error(line, colStart, "Unknown character.");
                return;
        }
    }

    private bool isKeyword(string s)
    {
        switch(s)
        {
            case "false":
                addToken(FALSE, s, false);
                return true;

            case "int":
                addToken(INT, s);
                return true;

            case "print":
                addToken(PRINT, s);
                return true;

            case "true":
                addToken(TRUE, s, true);
                return true;

            default:
                return false;
        }
    }

    // private tools
    private bool isAtEnd
    {
        get { return curr >= code.Length; }
    }

    private void advance()
    {
        if(!isAtEnd) curr++;
        colEnd++;
    }

    private char currChar
    {
        get { return code[curr]; }
    }

    private bool isDigit(char c)
    {
        return '0' <= c && c <= '9';
    }

    private bool isAlpha(char c)
    {
        return (c == '_') || ('a' <= c && c <= 'z') || ('A' <= c && c <= 'Z');
    }

    private void addToken(TokenType type, string lexeme, object? literal)
    {
        tokens.Add( new Token(type, lexeme, literal, line, colStart, colEnd - 1) );
    }

    private void addToken(TokenType type, string lexeme)
    {
        addToken(type, lexeme, null);
    }

    private void commentDetected()
    {
        while(!isAtEnd && currChar != '\n') // ignore this line
            advance();
    }
}
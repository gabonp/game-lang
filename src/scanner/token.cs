public enum TokenType
{
    // single-character tokens
    LEFT_PAREN, RIGHT_PAREN,
    COLON, MARK, MINUS, PLUS,

    // one or two character tokens
    LESS, LESS_EQUAL,

    // literals
    IDENTIFIER, STRING, NUMBER,

    // keywords
    FALSE, INT, TRUE,

    EOF
}

public class Token
{
    public TokenType type;
    public string lexeme;
    public object? literal;

    public int line, column;

    public Token(TokenType type, string lexeme, object? literal, int line, int column)
    {
        this.type = type;
        this.lexeme = lexeme;
        this.literal = literal;

        this.line = line;
        this.column = column;
    }

    public override string ToString()
    {
        return type + " '" + lexeme + "' " + (literal == null ? "" : literal.ToString()) + " in " + line.ToString() + ", " + column.ToString();
    }
}
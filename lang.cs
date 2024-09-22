using System;
using System.IO;

public class GameLang
{
    private Error error = new Error();
    private RuntimeError runtimeError = new RuntimeError();
    private Interpreter interpreter = new Interpreter();
    private Stmt? stmt;

    public void init(string path)
    {
        List<string> code = readFile(path); // read the source code and store it
        error = new Error(code);
        runtimeError = new RuntimeError(code);
        
        List<Token> tokens = scan(code); // tokenize the code;

        if(error.hadError) return;

        stmt = parse(tokens);
    }

    public void interpret()
    {
        if(stmt != null)
            interpreter.interpret(stmt, ref runtimeError);
    }

    private List<string> readFile(string path)
    {
        if( File.Exists(path) )
        {
            string[] code_ = File.ReadAllLines(path);
            return code_.ToList();
        }

        // [TODO] report an error if code path doesn't exist
        return new List<string> ();
    }

    private List<Token> scan(List<string> code)
    {
        string code_ = "";
        foreach(string line in code)
        {
            code_ += line;
            code_ += '\n';
        }

        Scanner scanner = new Scanner(code_);
        
        scanner.scanTokens(ref error);

        return scanner.getTokens();
    }

    private Stmt parse(List<Token> tokens)
    {
        Parser parser = new Parser(tokens);

        return parser.parse();
    }
}
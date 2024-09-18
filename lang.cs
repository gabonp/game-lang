using System;
using System.IO;

public class GameLang
{
    private Error error = new Error();

    public void init(string path)
    {
        List<string> code = readFile(path); // read the source code and store it
        error.addCode(code);
        
        List<Token> tokens = scan(code); // tokenize the code;
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
}
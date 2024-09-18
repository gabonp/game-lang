public class Error
{
    public bool hadError = false;

    private List<string> code = new List<string>();

    public Error() {}
    public void addCode(List<string> code)
    {
        this.code = code;
    }

    public void error(int line, int column, string message)
    {
        report(line - 1, column - 1, message);
        hadError = true;
    }

    private void report(int line, int column, string message)
    {
        string code_ = code[line];
        string where = "";

        for(int i = 0; i < code_.Length; i++)
            where += ( i == column ? "^" : "." );

        System.Console.WriteLine(">> Error in line " + (line + 1) + ": " + message);
        System.Console.WriteLine("         " + code_);
        System.Console.WriteLine("         " + where);
    }
}
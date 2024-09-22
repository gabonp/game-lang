using static TokenType;

public class Interpreter : ExprVisitor, StmtVisitor
{
    RuntimeError error = new RuntimeError();

    public void interpret(Stmt stmt, ref RuntimeError error)
    {
        this.error = error;

        try
        {
            executeStmt(stmt);

            error = this.error; // update Lang's error handler in case errors occurred
        }
        catch {}
    }

    // StmtVisitor
    public void visitExpr(stmtExpr stmt)
    {
        evaluateExpr(stmt.expr);
    }

    public void visitPrint(stmtPrint stmt)
    {
        object value = evaluateExpr(stmt.expr);
        System.Console.WriteLine( stringify(value) );
    }

    // ExprVisitor
    public object visitBinary(exprBinary expr)
    {
        object left = evaluateExpr(expr.left);
        object right = evaluateExpr(expr.right);

        switch(expr.oper.type)
        {
            case MINUS:
            case PLUS:
                return arithmetic(left, expr.oper, right);
            
            case LESS:
                return relation(left, right);

            case LESS_EQUAL:
                return (bool) relation(left, right) || (bool) equality(left, right);

            default:
                return null!; // unreachable
        }
    }

    public object visitCast(exprCast cast)
    {
        switch(cast.to.type)
        {
            case INT:
                return toInt(cast);

            default:
                return null!; // unreachable
        }
    }

    public object visitGroup(exprGroup group)
    {
        return evaluateExpr(group.expr);
    }

    public object visitLiteral(exprLiteral expr)
    {
        return expr.value;
    }

    public object visitTernary(exprTernary expr)
    {
        object condition = evaluateExpr(expr.condition);

        if((bool) condition)
            return evaluateExpr(expr.mainBranch);
        else
            return evaluateExpr(expr.elseBranch);
    }

    public object visitUnary(exprUnary unary)
    {
        object value = evaluateExpr(unary.expr);

        switch(unary.oper.type)
        {
            case MINUS:
                return -(long)value;

            default:
                return null!; // unreachable
        }
    }

    // private
    private object evaluateExpr(Expr expr)
    {
        return expr.accept(this);
    }

    private void executeStmt(Stmt stmt)
    {
        stmt.accept(this);
    }

    // private tools
    private object arithmetic(object left, Token oper, object right)
    {
        switch(oper.type)
        {
            case MINUS:
                return (long) left - (long) right;

            case PLUS:
                if(left is long)
                    return (long) left + (long) right;
                // else left is str
                    return (string) left + (string) right;

            default:
                return null!; // unreachable
        }
    }

    private object relation(object left, object right)
    {
        if(left is long)
            return (long) left < (long) right;

        return null!; // unreachable
    }

    private object equality(object left, object right)
    {
        if(left is long)
            return (long) left == (long) right;

        return null!; // unreachable
    }

    private long toInt(exprCast cast)
    {
        object value = evaluateExpr(cast.expr);

        if(value is bool)
            return ((bool) value ? 1 : 0);

        if(value is string)
            try
            {
                return long.Parse((string) value);
            }
            catch(System.OverflowException)
            {
                error.error(cast.to, "This value is too large for an integer.");
            }
            catch(System.FormatException)
            {
                error.error(cast.to, "String \"" + value + "\" cannot be converted to integer.");
            }

        return (long) value;
    }

    private string stringify(object value)
    {
        if(value is bool)
            return (bool) value ? "true" : "false";

        return value.ToString()!;
    }
}
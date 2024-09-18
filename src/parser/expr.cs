public interface Expr
{
    public object accept(ExprVisitor visitor);
}

public interface ExprVisitor
{
    public object visitBinary(exprBinary expr);
    public object visitCast(exprCast expr);
    public object visitGroup(exprGroup expr);
    public object visitLiteral(exprLiteral expr);
    public object visitTernary(exprTernary expr);
    public object visitUnary(exprUnary expr);
}

public class exprBinary : Expr
{
    public Expr left;
    public Token oper;
    public Expr right;

    public exprBinary(Expr left, Token oper, Expr right)
    {
        this.left = left;
        this.oper = oper;
        this.right = right;
    }

    public object accept(ExprVisitor visitor)
    {
        return visitor.visitBinary(this);
    }
}

public class exprCast : Expr
{
    public Token to;
    public Expr expr;

    public exprCast(Token to, Expr expr)
    {
        this.to = to;
        this.expr = expr;
    }

    public object accept(ExprVisitor visitor)
    {
        return visitor.visitCast(this);
    }
}

public class exprGroup : Expr
{
    public Expr expr;

    public exprGroup(Expr expr)
    {
        this.expr = expr;
    }

    public object accept(ExprVisitor visitor)
    {
        return visitor.visitGroup(this);
    }
}

public class exprLiteral : Expr
{
    public object value;

    public exprLiteral(object value)
    {
        this.value = value;
    }

    public object accept(ExprVisitor visitor)
    {
        return visitor.visitLiteral(this);
    }
}

public class exprTernary : Expr
{
    public Expr condition;
    public Expr mainBranch;
    public Expr elseBranch;

    public exprTernary(Expr condition, Expr mainBranch, Expr elseBranch)
    {
        this.condition = condition;
        this.mainBranch = mainBranch;
        this.elseBranch = elseBranch;
    }

    public object accept(ExprVisitor visitor)
    {
        return visitor.visitTernary(this);
    }
}

public class exprUnary : Expr
{
    public Token oper;
    public Expr expr;

    public exprUnary(Token oper, Expr expr)
    {
        this.oper = oper;
        this.expr = expr;
    }

    public object accept(ExprVisitor visitor)
    {
        return visitor.visitUnary(this);
    }
}
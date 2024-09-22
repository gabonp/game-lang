public interface Stmt
{
    public void accept(StmtVisitor visitor);
}

public interface StmtVisitor
{
    public void visitExpr(stmtExpr stmt);
    public void visitPrint(stmtPrint stmt);
}

public class stmtExpr : Stmt
{
    public Expr expr;

    public stmtExpr(Expr expr)
    {
        this.expr = expr;
    }

    public void accept(StmtVisitor visitor)
    {
        visitor.visitExpr(this);
    }
}

public class stmtPrint : Stmt
{
    public Expr expr;

    public stmtPrint(Expr expr)
    {
        this.expr = expr;
    }

    public void accept(StmtVisitor visitor)
    {
        visitor.visitPrint(this);
    }
}
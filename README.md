# MathExpr

```csharp
using MathExpr

// ...

// Usage 1
float result = MathExpr.Eval("1 + 3 * (8 + 5)");
// => 40

// Usage 2
string[] exprs = MathExpr.Parse("1 + 3 * (8 + 5)");
// => ["1", "3", "8", "5", "+", "*", "+"]
float result = MathExpr.Eval(exprs);
// => 40
```

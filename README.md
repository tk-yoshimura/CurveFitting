# CurveFitting
 Curvefitting - linear, polynomial, pade, arbitrary function

## Requirement
 .NET 6.0
 
 ## Install
[Download DLL](https://github.com/tk-yoshimura/CurveFitting/releases)  
[Download nuget](https://www.nuget.org/packages/TYoshimura.CurveFitting/)

- Import Algebra(https://github.com/tk-yoshimura/Algebra)
- Import DoubleDouble(https://github.com/tk-yoshimura/DoubleDouble)
- To install, just import the DLL.
- This library does not change the environment at all.

## Usage
```csharp
ddouble[] xs = (new ddouble[64]).Select((_, i) => (ddouble)i).ToArray();
ddouble[] ys1 = new ddouble[xs.Length], ys2 = new ddouble[xs.Length];
Vector p1 = new(2, -1, 1, 5), p2 = new(1, 4, 3, -1);

for (int i = 0; i < xs.Length; i++) {
    ddouble x = xs[i];

    ys1[i] = p1[0] + p1[1] * x + p1[2] * x * x + p1[3] * x * x * x;
    ys2[i] = p2[0] + p2[1] * x + p2[2] * x * x + p2[3] * x * x * x;
}
ys1[32] = ys2[32] = -64;

RobustPolynomialFitter fitter1 = new(xs, ys1, 3);
RobustPolynomialFitter fitter2 = new(xs, ys2, 3, intercept: 1);

Assert.IsTrue((fitter1.ExecuteFitting() - p1).Norm < 1e-20);
Assert.IsTrue((fitter2.ExecuteFitting() - p2).Norm < 1e-20);
```

## Licence
[MIT](https://github.com/tk-yoshimura/CurveFitting/blob/main/LICENSE)

## Author

[T.Yoshimura](https://github.com/tk-yoshimura)

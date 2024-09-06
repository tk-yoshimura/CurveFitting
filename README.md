# CurveFitting
 Curvefitting - linear, polynomial, pade, arbitrary function

## Requirement
.NET 8.0  
[Algebra](https://github.com/tk-yoshimura/Algebra)  
[DoubleDouble](https://github.com/tk-yoshimura/DoubleDouble)
 
 ## Install
[Download DLL](https://github.com/tk-yoshimura/CurveFitting/releases)  
[Download nuget](https://www.nuget.org/packages/TYoshimura.CurveFitting/)


## Usage
```csharp
Vector p1 = new(2, -1, 1, 5), p2 = new(1, 4, 3, -1);
ddouble[] xs = { 1, 3, 4, 7, 8, 9, 13, 15, 20 };
ddouble[] ys1 = Vector.Polynomial(xs, p1), ys2 = Vector.Polynomial(xs, p2);

PolynomialFitter fitter1 = new(xs, ys1, 3);
PolynomialFitter fitter2 = new(xs, ys2, 3, intercept: 1);

Assert.IsTrue((fitter1.Fit() - p1).Norm < 1e-24);
Assert.IsTrue((fitter2.Fit() - p2).Norm < 1e-24);
```

## Licence
[MIT](https://github.com/tk-yoshimura/CurveFitting/blob/main/LICENSE)

## Author

[T.Yoshimura](https://github.com/tk-yoshimura)

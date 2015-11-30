using NUnit.Framework;
using RefactoringEssentials.VB.CodeRefactorings;

namespace RefactoringEssentials.Tests.VB.CodeRefactorings
{
    [TestFixture]
    public class InvertLogicalExpressionTests : VBCodeRefactoringTestBase
    {
        [Test]
        public void ConditionlAnd()
        {
            Test<InvertLogicalExpressionCodeRefactoringProvider>(@"
Class TestClass
    Public Sub F()
        Dim a As Boolean = True
        Dim b As Boolean = False
        If a $And b Then
        End If
    End Sub
End Class", @"
Class TestClass
    Public Sub F()
        Dim a As Boolean = True
        Dim b As Boolean = False
        If Not (Not a Or Not b) Then
        End If
    End Sub
End Class");
        }

        [Test]
        public void ConditionlAndReverse()
        {
            Test<InvertLogicalExpressionCodeRefactoringProvider>(@"
Class TestClass
    Public Sub F()
        Dim a As Boolean = True
        Dim b As Boolean = False
        If Not (Not a $And Not b) Then
        End If
    End Sub
End Class", @"
Class TestClass
    Public Sub F()
        Dim a As Boolean = True
        Dim b As Boolean = False
        If a Or b Then
        End If
    End Sub
End Class");
        }

        [Test]
        public void ConditionlAndAlso()
        {
            Test<InvertLogicalExpressionCodeRefactoringProvider>(@"
Class TestClass
    Public Sub F()
        Dim a As Boolean = True
        Dim b As Boolean = False
        If a $AndAlso b Then
        End If
    End Sub
End Class", @"
Class TestClass
    Public Sub F()
        Dim a As Boolean = True
        Dim b As Boolean = False
        If Not (Not a OrElse Not b) Then
        End If
    End Sub
End Class");
        }

        [Test]
        public void ConditionlAndAlsoReverse()
        {
            Test<InvertLogicalExpressionCodeRefactoringProvider>(@"
Class TestClass
    Public Sub F()
        Dim a As Boolean = True
        Dim b As Boolean = False
        If Not (Not a $AndAlso Not b) Then
        End If
    End Sub
End Class", @"
Class TestClass
    Public Sub F()
        Dim a As Boolean = True
        Dim b As Boolean = False
        If a OrElse b Then
        End If
    End Sub
End Class");
        }

        [Test]
        public void ConditionlOr()
        {
            Test<InvertLogicalExpressionCodeRefactoringProvider>(@"
Class TestClass
    Public Sub F()
        Dim a As Boolean = True
        Dim b As Boolean = False
        If a $Or b Then
        End If
    End Sub
End Class", @"
Class TestClass
    Public Sub F()
        Dim a As Boolean = True
        Dim b As Boolean = False
        If Not (Not a And Not b) Then
        End If
    End Sub
End Class");
        }

        [Test]
        public void ConditionlOrReverse()
        {
            Test<InvertLogicalExpressionCodeRefactoringProvider>(@"
Class TestClass
    Public Sub F()
        Dim a As Boolean = True
        Dim b As Boolean = False
        If Not (Not a $And Not b) Then
        End If
    End Sub
End Class", @"
Class TestClass
    Public Sub F()
        Dim a As Boolean = True
        Dim b As Boolean = False
        If a Or b Then
        End If
    End Sub
End Class");
        }

        [Test]
        public void ConditionlOrElse()
        {
            Test<InvertLogicalExpressionCodeRefactoringProvider>(@"
Class TestClass
    Public Sub F()
        Dim a As Boolean = True
        Dim b As Boolean = False
        If a $OrElse b Then
        End If
    End Sub
End Class", @"
Class TestClass
    Public Sub F()
        Dim a As Boolean = True
        Dim b As Boolean = False
        If Not (Not a AndAlso Not b) Then
        End If
    End Sub
End Class");
        }

        [Test]
        public void ConditionlOrElseReverse()
        {
            Test<InvertLogicalExpressionCodeRefactoringProvider>(@"
Class TestClass
    Public Sub F()
        Dim a As Boolean = True
        Dim b As Boolean = False
        If Not (Not a $AndAlso Not b) Then
        End If
    End Sub
End Class", @"
Class TestClass
    Public Sub F()
        Dim a As Boolean = True
        Dim b As Boolean = False
        If a OrElse b Then
        End If
    End Sub
End Class");
        }


        [Test]
        public void ConditionlAnd2()
        {
            Test<InvertLogicalExpressionCodeRefactoringProvider>(@"
Class TestClass
    Public Sub F()
        Dim a As Integer = 1
        Dim b As Boolean = True
        If (a > 1) $And b Then
        End If
    End Sub
End Class", @"
Class TestClass
    Public Sub F()
        Dim a As Integer = 1
        Dim b As Boolean = True
        If Not ((a <= 1) Or Not b) Then
        End If
    End Sub
End Class");
        }

        [Test]
        public void ConditionlOr2()
        {
            Test<InvertLogicalExpressionCodeRefactoringProvider>(@"
Class TestClass
    Public Sub F()
        Dim a As Integer = 1
        Dim b As Boolean = True
        If Not ((a > 1) $Or b) Then
        End If
    End Sub
End Class", @"
Class TestClass
    Public Sub F()
        Dim a As Integer = 1
        Dim b As Boolean = True
        If (a <= 1) And Not b Then
        End If
    End Sub
End Class");
        }

        [Test]
        public void Equals()
        {
            Test<InvertLogicalExpressionCodeRefactoringProvider>(@"
Class TestClass
    Public Sub F()
        Dim a As Boolean = True
        Dim b As Boolean = False
        If a $= b Then
        End If
    End Sub
End Class", @"
Class TestClass
    Public Sub F()
        Dim a As Boolean = True
        Dim b As Boolean = False
        If Not (a <> b) Then
        End If
    End Sub
End Class");
        }

        [Test]
        public void EqualsReverse()
        {
            Test<InvertLogicalExpressionCodeRefactoringProvider>(@"
Class TestClass
    Public Sub F()
        Dim a As Boolean = True
        Dim b As Boolean = False
        If Not (a $<> b) Then
        End If
    End Sub
End Class", @"
Class TestClass
    Public Sub F()
        Dim a As Boolean = True
        Dim b As Boolean = False
        If a = b Then
        End If
    End Sub
End Class");
        }


        [Test]
        public void TestNullCoalescing()
        {
            TestWrongContext<InvertLogicalExpressionCodeRefactoringProvider>(@"
Class Foo
    Sub Bar (i As Object, j As Object)
        Console.WriteLine ($If(i, j))
    End Sub
End Class
");
        }


        [Test]
        public void TestUnaryExpression()
        {
            Test<InvertLogicalExpressionCodeRefactoringProvider>(@"
Class Foo
    Sub Bar (a As Boolean, b As Boolean)
        Console.WriteLine ($Not (a And b))
    End Sub
End Class", @"
Class Foo
    Sub Bar (a As Boolean, b As Boolean)
        Console.WriteLine (Not a Or Not b)
    End Sub
End Class");
        }
    }
}

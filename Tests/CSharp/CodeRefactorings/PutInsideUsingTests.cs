using NUnit.Framework;
using RefactoringEssentials.CSharp.CodeRefactorings;

namespace RefactoringEssentials.Tests.CSharp.CodeRefactorings
{
    [TestFixture]
    public class PutInsideUsingTests : CSharpCodeRefactoringTestBase
    {
        [Test]
        public void Test()
        {
            Test<PutInsideUsingAction>(@"
interface ITest : System.IDisposable
{
    void Test ();
}
class TestClass
{
    void TestMethod (int i)
    {
        ITest obj $= null;
        obj.Test ();
        int a;
        if (i > 0)
            obj.Test ();
        a = 0;
    }
}", @"
interface ITest : System.IDisposable
{
    void Test ();
}
class TestClass
{
    void TestMethod (int i)
    {
        int a;
        using (ITest obj = null)
        {
            obj.Test();
            if (i > 0)
                obj.Test();
        }

        a = 0;
    }
}");
        }

        [Test]
        public void TestIDisposable()
        {
            Test<PutInsideUsingAction>(@"
class TestClass
{
    void TestMethod()
    {
        System.IDisposable obj $= null;
        obj.Method();
    }
}", @"
class TestClass
{
    void TestMethod()
    {
        using (System.IDisposable obj = null)
        {
            obj.Method();
        }
    }
}");
        }

        [Test]
        public void TestTypeParameter()
        {

            Test<PutInsideUsingAction>(@"
class TestClass
{
    void TestMethod<T> ()
        where T : System.IDisposable, new()
    {
        T obj $= new T ();
        obj.Method ();
    }
}", @"
class TestClass
{
    void TestMethod<T> ()
        where T : System.IDisposable, new()
    {
        using (T obj = new T())
        {
            obj.Method();
        }
    }
}");
        }

        [Test]
        public void TestMultipleVariablesDeclaration()
        {
            Test<PutInsideUsingAction>(@"
class TestClass
{
    void TestMethod ()
    {
        System.IDisposable obj, obj2 $= null, obj3;
        obj2.Method ();
    }
}", @"
class TestClass
{
    void TestMethod ()
    {
        System.IDisposable obj, obj3;
        using (System.IDisposable obj2 = null)
        {
            obj2.Method();
        }
    }
}");
        }

        [Test]
        public void TestNullInitializer()
        {
            TestWrongContext<PutInsideUsingAction>(@"
class TestClass
{
    void TestMethod ()
    {
        System.IDisposable $obj;
        obj.Method ();
    }
}");
        }

        [Test]
        public void TestMoveVariableDeclaration()
        {
            Test<PutInsideUsingAction>(@"
class TestClass
{
    void TestMethod ()
    {
        System.IDisposable obj $= null;
        int a, b;
        a = b = 0;
        obj.Method ();
        a++;
    }
}", @"
class TestClass
{
    void TestMethod ()
    {
        int a;
        using (System.IDisposable obj = null)
        {
            int b;
            a = b = 0;
            obj.Method();
        }

        a++;
    }
}");
        }

        [Test]
        public void TestMoveVariableDeclarationAndConvertInitializationToAssignment()
        {
            Test<PutInsideUsingAction>(@"
class TestClass
{
    void TestMethod ()
    {
        System.IDisposable obj $= null;
        int a = 10, b;        
        obj.Method ();
        a++;
    }
}", @"
class TestClass
{
    void TestMethod ()
    {
        int a;
        using (System.IDisposable obj = null)
        {
            int b;
            a = 10;
            obj.Method();
        }

        a++;
    }
}");
        }

        [Test]
        public void TestRemoveDisposeInvocation()
        {
            Test<PutInsideUsingAction>(@"
class TestClass
{
    void TestMethod ()
    {
        System.IDisposable obj $= null;
        obj.Method();
        obj.Dispose();
    }
}", @"
class TestClass
{
    void TestMethod ()
    {
        using (System.IDisposable obj = null)
        {
            obj.Method();
        }
    }
}");
        }

        [Test]
        public void TestNotAvailableOnNonDisposableVariable()
        {
            TestWrongContext<PutInsideUsingAction>(@"
class TestClass
{
    void TestMethod ()
    {
        System.String obj $= null;
        obj.Method ();
        obj.Dispose();
    }
}");
        }

        [Test]
        public void TestAllDeclaredVariablesAreUsedInsideUsingBlock()
        {
            Test<PutInsideUsingAction>(@"
class TestClass
{
    void TestMethod()
    {
        System.IDisposable obj $= null;
        int a, b;
        a = b = 0;
        a++;
        obj.Method();
    }
}", @"
class TestClass
{
    void TestMethod()
    {
        using (System.IDisposable obj = null)
        {
            int a, b;
            a = b = 0;
            a++;
            obj.Method();
        }
    }
}");
        }

        [Test]
        public void TestEmptyUsingBlock()
        {
            Test<PutInsideUsingAction>(@"
class TestClass
{
    void TestMethod()
    {
        System.IDisposable obj = null, $obj2 = null;
        int a, b;
        a = b = 0;
        obj.Method();
        a++;
    }
}", @"
class TestClass
{
    void TestMethod()
    {
        System.IDisposable obj = null;
        using (System.IDisposable obj2 = null)
        {
        }

        int a, b;
        a = b = 0;
        obj.Method();
        a++;
    }
}");
        }

        [Test]
        public void TestVariableWithComment()
        {
            Test<PutInsideUsingAction>(@"
class TestClass
{
    void TestMethod()
    {
        // This is a comment
        System.IDisposable obj $= null;
        int a, b;
        a = b = 0;
        obj.Method();
        a++;
    }
}", @"
class TestClass
{
    void TestMethod()
    {
        int a;
        // This is a comment
        using (System.IDisposable obj = null)
        {
            int b;
            a = b = 0;
            obj.Method();
        }

        a++;
    }
}");
        }

        [Test]
        public void TestLastCallInBlockIsStatic()
        {
            Test<PutInsideUsingAction>(@"
class TestClass
{
    void TestMethod()
    {
        System.IDisposable obj $= null;
        int a, b;
        a = b = 0;
        a++;
        System.String.Split('.', obj.Method());
    }
}", @"
class TestClass
{
    void TestMethod()
    {
        using (System.IDisposable obj = null)
        {
            int a, b;
            a = b = 0;
            a++;
            System.String.Split('.', obj.Method());
        }
    }
}");
        }
    }

   

}

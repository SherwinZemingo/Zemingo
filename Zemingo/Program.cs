using Microsoft.Extensions.Logging;
using Zemingo;

public class Program
{
    private static readonly ILogger _logger;

    static Program()
    {
        // Set up logging configuration
        var loggerFactory = LoggerFactory.Create(builder => { builder.AddConsole(); });

        // Create logger instance
        _logger = loggerFactory.CreateLogger<Program>();
    }

    public static void Main(string[] args)
    {
        var person1 = new Person("Test1",1, 500);
        var person2 = new Person("Test2", 2, 800);
        var person3 = new Person("Test3", 3, 200);
        var person4 = new Person("Test4", 4, 1000);
        var person5 = new Person("Test5", 5, 20);
        var person6 = new Person("Test6", 6, 50);

        var quickPopDataStructure = new QuickPopDataStructure<Person>(_logger);
        quickPopDataStructure.Push(person1);
        quickPopDataStructure.Push(person2);
        quickPopDataStructure.Push(person3);
        quickPopDataStructure.Push(person4);
        quickPopDataStructure.Push(person5);
        quickPopDataStructure.Push(person6);

        Console.WriteLine(quickPopDataStructure.Pop()?.TotalMark);
        Console.WriteLine(quickPopDataStructure.Pop()?.TotalMark);
        Console.WriteLine(quickPopDataStructure.Pop()?.TotalMark);
        Console.WriteLine(quickPopDataStructure.Pop()?.TotalMark);
        Console.WriteLine(quickPopDataStructure.Pop()?.TotalMark);
        Console.WriteLine(quickPopDataStructure.Pop()?.TotalMark);
        Console.WriteLine(quickPopDataStructure.Pop()?.TotalMark);

        //var quickPushDataStructure = new QuickPushDataStructure<Person>(_logger);
        //quickPushDataStructure.Push(person1);
        //quickPushDataStructure.Push(person2);
        //quickPushDataStructure.Push(person3);
        //quickPushDataStructure.Push(person4);
        //quickPushDataStructure.Push(person5);
        //quickPushDataStructure.Push(person6);

        //Console.WriteLine(quickPushDataStructure.Pop()?.TotalMark);
        //Console.WriteLine(quickPushDataStructure.Pop()?.TotalMark);
        //Console.WriteLine(quickPushDataStructure.Pop()?.TotalMark);
        //Console.WriteLine(quickPushDataStructure.Pop()?.TotalMark);
        //Console.WriteLine(quickPushDataStructure.Pop()?.TotalMark);
        //Console.WriteLine(quickPushDataStructure.Pop()?.TotalMark);
        //Console.WriteLine(quickPushDataStructure.Pop()?.TotalMark);


        //quickPushDataStructure.Push(person4);
        //quickPushDataStructure.Push(person5);
        //quickPushDataStructure.Push(person6);


        //Console.WriteLine(quickPushDataStructure.Pop()?.TotalMark);
        //Console.WriteLine(quickPushDataStructure.Pop()?.TotalMark);
        //Console.WriteLine(quickPushDataStructure.Pop()?.TotalMark);
        //Console.WriteLine(quickPushDataStructure.Pop()?.TotalMark);
        //Console.WriteLine(quickPushDataStructure.Pop()?.TotalMark);
        //Console.WriteLine(quickPushDataStructure.Pop()?.TotalMark);
    }

    public class Person : IComparable<Person>
    {
        public string Name { get; set; }

        public int Id { get; set; }

        public int TotalMark { get; set; }

        public Person(string name, int id, int totalMark)
        {
            Name = name;
            Id = id;
            TotalMark = totalMark;
        }

        public int CompareTo(Person person) => this.TotalMark.CompareTo(person.TotalMark);
    }
}
public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("The Time Complexity of QuickPopDataStructure is O(n) for Push and  O(1) for Pop");

        Console.WriteLine("The Time Complexity of QuickPushDataStructure is O(n) for Pop and  O(1) for Push");

        Console.WriteLine("This works under normal scenarios as expected, and I have added atomic operations to make it threadsafe which might alter the complexity in multithreaded environment.");

        Console.WriteLine("If the idea is to maintain the complexity at all times, we can use locks but it will affect the consistency.");

        Console.WriteLine("I have not thrown exception if the data structures are empty and we try to Pop them, as this can be the scenario during Multithreading, so just added logging to make the application smooth.");
    }
}

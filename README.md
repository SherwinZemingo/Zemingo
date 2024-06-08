The Time Complexity of QuickPopDataStructure is O(n) for Push and  O(1) for Pop

The Time Complexity of QuickPushDataStructure is O(n) for Pop and  O(1) for Push

This works under normal scenarios as expected, and I have added atomic operations to make it threadsafe which might alter the complexity in multithreaded environment.

If the idea is to maintain the complexity at all times, we can use locks but it will affect the consistency.

I have not thrown exception if the data structures are empty and we try to Pop them, as this can be the scenario during Multithreading, so just added logging to make the application smooth.

using Microsoft.Extensions.Logging;

namespace Zemingo
{
    public class QuickPopDataStructure<T> where T : IComparable<T>
    {
        private Node<T>? Head;
        private readonly ILogger _logger;

        public QuickPopDataStructure(ILogger logger)
        {
            _logger = logger;
        }

        public void Push(T value)
        {
            var newNode = new Node<T>(value);

            while (true)
            {
                var currentHead = Head;

                if (currentHead == null || currentHead.Value.CompareTo(value) < 0)
                {
                    newNode.NextNode = currentHead;
                    if (Interlocked.CompareExchange(ref Head, newNode, currentHead) == currentHead)
                    {
                        return;
                    }
                }
                else
                {
                    break;
                }
            }

            while (true)
            {
                var currentNode = Head;

                while (currentNode?.NextNode != null && currentNode.NextNode.Value.CompareTo(value) >= 0)
                {
                    currentNode = currentNode.NextNode;
                }

                var nextNode = currentNode?.NextNode;
                newNode.NextNode = nextNode;

                if (Interlocked.CompareExchange(ref currentNode.NextNode, newNode, nextNode) == nextNode)
                {
                    return;
                }
            }
        }

        public T Pop()
        {
            Node<T> biggestNode;

            do
            {
                biggestNode = Head;
                if (Head == null)
                {
                    _logger.LogError("The QuickPopDataStructure is empty.");
                    return default;
                }
            }
            while (Interlocked.CompareExchange(ref Head, biggestNode.NextNode, biggestNode) != biggestNode);

            return biggestNode.Value;
        }
    }
}

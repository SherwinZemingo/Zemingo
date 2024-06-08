using Microsoft.Extensions.Logging;
using System.Xml.Linq;

namespace Zemingo
{
    public class QuickPushDataStructure<T> where T : IComparable<T>
    {
        private Node<T>? Head;
        private readonly ILogger _logger;

        public QuickPushDataStructure(ILogger logger)
        {
            _logger = logger;
        }

        public void Push(T value)
        {
            var newHeadNode = new Node<T>(value);

            do
            {
                newHeadNode.NextNode = Head;
            }
            while (Interlocked.CompareExchange(ref Head, newHeadNode, newHeadNode.NextNode) != newHeadNode.NextNode);
        }

        public T Pop()
        {
            if (Head == null)
            {
                _logger.LogError("The QuickPushDataStructure is empty.");
                return default;
            }

            var biggestNode = Head;
            Node<T> previousNode = null;

            while (true)
            {
                var currentNode = Head;

                while (currentNode.NextNode != null)
                {
                    if (currentNode.NextNode.Value.CompareTo(biggestNode.Value) > 0)
                    {
                        biggestNode = currentNode.NextNode;
                        previousNode = currentNode;
                    }

                    currentNode = currentNode.NextNode;
                }

                if (previousNode == null)
                {
                    if (Interlocked.CompareExchange(ref Head, biggestNode.NextNode, biggestNode) == biggestNode)
                        break;
                }
                else
                {
                    if (Interlocked.CompareExchange(ref previousNode.NextNode, biggestNode.NextNode, biggestNode) == biggestNode)
                        break;
                }
            }

            return biggestNode.Value;
        }
    }
}

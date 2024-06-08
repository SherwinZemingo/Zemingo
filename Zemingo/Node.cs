namespace Zemingo
{
    public class Node<T> where T : IComparable<T>
    {
        public T Value;

        public Node<T>? NextNode;

        public Node(T value)
        {
            Value = value;
        }
    }
}

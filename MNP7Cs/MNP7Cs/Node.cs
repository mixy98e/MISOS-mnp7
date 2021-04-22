using System;

namespace MNP7Cs
{
    public class Node
    {
        public char? Current { get; set; }
        public char? Predecessor { get; set; }
        public int Weight { get; set; }
        public int Number { get; set; }
        public Node Left { get; set; }
        public Node Right { get; set; }
        public Node Parent { get; set; }
        public bool IsNYT { get; set; }

        public Node()
        { }

        public Node(Node parent)
        {
            Parent = parent;
        }

        public Node(Node parent, char pred, char curr)
        {
            Parent = parent;
            Predecessor = pred;
            Current = curr;
            Weight = 1;
        }

        public Node FindOrDefault(char pred, char curr)
        {
            if (Predecessor == pred && Current == curr)
                return this;

            Node result = Left?.FindOrDefault(pred, curr);
            if (result != null)
                return result;

            return Right?.FindOrDefault(pred, curr);
        }

        public string GetCode(Node searched)
        {
            return GetCode(searched, String.Empty);
        }

        private string GetCode(Node searched, string code)
        {
            if (Predecessor == searched.Predecessor && Current == searched.Current)
                return code;

            if (Left == null && Right == null)
                return null;

            string result = Left.GetCode(searched, code + "0");
            if (result != null)
                return result;

            return Right.GetCode(searched, code + "1");
        }

        public string GetNYTCode(string code)
        {
            if (IsNYT)
                return code;

            if (Left == null && Right == null)
                return null;

            string result = Left.GetNYTCode(code + "0");
            if (result != null)
                return result;

            return Right.GetNYTCode(code + "1");
        }

        public bool IsLeftSon(Node son)
        {
            return Left == son;
        }

        public bool IsRightSon(Node son)
        {
            return Right == son;
        }

        public bool IsLeaf() // symbol == null ??
        {
            return Left == null && Right == null;
        }
    }
}
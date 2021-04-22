using System;
using System.Text;

namespace MNP7Cs
{
    public class Tree
    {
        public Node Root { get; set; }

        private Node _nyt; // "not yet transfered"
        private Node[] _nodes;
        private int _nextNum;

        public Tree()
        {
            Reset();
        }

        public void Reset()
        {
            Root = new Node { Number = 512 };
            _nyt = Root;
            _nodes = new Node[513];
            _nodes[Root.Number] = Root;
            _nextNum = 511;
        }

        public string Encode(string text)
        {
            var result = new StringBuilder();

            int limit = (text.Length % 2 == 0) ? text.Length : text.Length - 1;
            for (int i = 1; i < limit; i += 2)
            {
                result.Append(Encode(text[i - 1], text[i]));
                //result.Append(" ");
            }
            if (text.Length % 2 == 1)
                // Poslednji karakter u stringu neparne duzine nema para,
                // pa se ne kompresuje
                result.Append(text[text.Length - 1]);

            return result.ToString();
        }

        public string Encode(char pred, char succ)
        {
            Node node = Root.FindOrDefault(pred, succ);

            string code = string.Empty;

            // Ako se cvor vec nalazi u stablu
            if (node != null)
            {
                code = Root.GetCode(node);
                node.Weight++;
            }
            else // Nema kompresije
            {
                //code = Root.GetNYTCode(string.Empty);
                code = Root.GetNYTCode(string.Empty);
                code += pred;
                code += succ;
                node = AddToNYT(pred, succ);
            }

            UpdateAll(node.Parent);

            return code;
        }

        public string Decode(string code)
        {
            var result = new StringBuilder();

            int index = 0;
            while (index < code.Length-2)
            {
                Node node;

                Node symbol = ReadChar(index, code, out int count);
                index += count;

                if (symbol == null)
                {
                    symbol = new Node();
                    symbol.Predecessor = code[index - 1];
                    symbol.Current = code[index];
                    node = AddToNYT(symbol.Predecessor.Value, symbol.Current.Value);
                }
                else
                {
                    node = Root.FindOrDefault(symbol.Predecessor.Value, symbol.Current.Value);
                    node.Weight++;
                }

                UpdateAll(node.Parent);

                result.Append(symbol.Predecessor.Value);
                result.Append(symbol.Current.Value);
            }
            if (code.Length % 2 == 0)
                result.Append(code[code.Length - 1]);

            return result.ToString();
        }

        private Node ReadChar(int index, string code, out int count)
        {
            Node current = Root;
            count = 0;

            while (true)
            {
                count++;

                if (current == _nyt)
                    return null;

                if (current.IsLeaf())
                {
                    count--;
                    return current;
                }

                char bit = code[index++];

                if (bit == '0')
                    current = current.Left;
                else if (bit == '1')
                    current = current.Right;
            }
        }

        private Node AddToNYT(char pred, char curr)
        {
            var node = new Node(_nyt, pred, curr)
            {
                Number = _nextNum
            };
            _nyt.Right = node;
            _nodes[_nextNum--] = node;

            var nyt = new Node(_nyt)
            {
                Number = _nextNum,
                IsNYT = true
            };
            _nyt.IsNYT = false;
            _nyt.Left = nyt;
            _nodes[_nextNum--] = nyt;

            _nyt = nyt;

            return node;
        }

        private void UpdateAll(Node node)
        {
            while (node != null)
            {
                Update(node);
                node = node.Parent;
            }
        }

        private void Update(Node node)
        {
            Node toReplace = NodeToReplace(node.Number, node.Weight);

            if (toReplace != null && node.Parent != toReplace)
                Replace(node, toReplace);

            node.Weight++;
        }

        private Node NodeToReplace(int startIndex, int weight)
        {
            startIndex++;
            Node found = null;

            for (int i = startIndex; i < _nodes.Length; i++)
                if (_nodes[i].Weight == weight)
                    found = _nodes[i];

            return found;
        }

        private void Replace(Node a, Node b)
        {
            ReplaceNumbers(a, b);
            ReplaceSons(a, b);
        }

        private void ReplaceNumbers(Node a, Node b)
        {
            Node temp = _nodes[a.Number];
            _nodes[a.Number] = _nodes[b.Number];
            _nodes[b.Number] = temp;

            int tempNum = a.Number;
            a.Number = b.Number;
            b.Number = tempNum;
        }

        private void ReplaceSons(Node a, Node b)
        {
            bool bIsLeftSon = b.Parent.IsLeftSon(b);

            if (a.Parent.IsLeftSon(a))
                a.Parent.Left = b;
            else
                a.Parent.Right = b;

            Node temp = b.Parent;
            b.Parent = a.Parent;
            a.Parent = temp;

            if (bIsLeftSon)
                temp.Left = a;
            else
                temp.Right = a;
        }

        /*private static string ToBinary(int num)
        {
            int bin = 0;
            int rem, i = 1;
            while (num != 0)
            {
                rem = num % 2;
                num /= 2;
                bin += rem * i;
                i *= 10;
            }
            return bin.ToString();
        }*/
    }
}
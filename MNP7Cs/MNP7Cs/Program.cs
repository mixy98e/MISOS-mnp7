using System;
using System.Diagnostics;

namespace MNP7Cs
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string text = "asd test1 asd aaaaaa";

            var sw = new Stopwatch();
            sw.Start();

            string stage1output = RLE.RunsIdentify(text);

            var tree = new Tree();
            string encoded = tree.Encode(stage1output);

            sw.Stop();
            Console.WriteLine($"Run length encoding: {stage1output}");
            Console.WriteLine($"Kodirani tekst: {encoded}");
            Console.WriteLine($"Velicina ulaznog teksta: {text.Length * 8} b");


            //Ovo je sa razmakom, realna velicina je jos manja
            Console.WriteLine($"Velicina kompresovanog teksta: {DataEncodedLength(encoded)} b");
            Console.WriteLine($"Vreme kompresovanja: {sw.ElapsedMilliseconds}ms");

            Console.WriteLine($"Odnos kompresija/original: {(float)DataEncodedLength(encoded) * 100 / (text.Length * 8)} %");
            Console.WriteLine($"Procenat kompresovanja: {(float)100.0 - (float)DataEncodedLength(encoded) * 100 / (text.Length * 8)}%");
            tree.Reset();
            string decoded = tree.Decode(encoded);
            Console.WriteLine($"Decoded tekst: {decoded}");
            string final = RLE.Decode(decoded);
            Console.WriteLine($"Dekompresovani tekst: {final}");
        }

        private static int DataEncodedLength(string text)
        {
            int length = 0;
            foreach (char c in text)
            {
                if (c == '1' || c == '0')
                    length++;
                else
                    length += 8;
            }
            return length;
        }
    }
}

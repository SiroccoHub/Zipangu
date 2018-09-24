﻿using System;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Zipangu;

namespace NetUnitTest
{
    [TestClass]
    public class VBStringsTest
    {
        static string ToMessage1(char c) => $"({(int)c:D5}-{(int)c:X4}) {c}";
        static string ToMessage2(char c) => $"{c} ({(int)c:D5}-{(int)c:X4})";

        [TestMethod]
        public void Katakana()
        {
            var changed = EnumerableHelper.RangeChars(char.MinValue, char.MaxValue)
                .Select(c => new { before = c, after = c.ToString().ToKatakana_VB().Single() })
                .Where(_ => _.before != _.after)
                .Where(_ => _.after != '?')
                .Select(_ => $"{ToMessage1(_.before)} > {ToMessage2(_.after)}")
                .ToArray();

            File.WriteAllLines("VBStrings-Katakana.txt", changed, Encoding.UTF8);
        }

        [TestMethod]
        public void ToWideHiragana_Original()
        {
            var changed = Enumerable.Range(0, char.MaxValue + 1)
                .Select(i => new { i, before = (char)i, after = ((char)i).ToString().ToWideHiragana_VB().Single() })
                .Where(_ => _.before != _.after)
                .Where(_ => _.after != '？' || _.before == '?')
                .ToArray();

            foreach (var _ in changed)
                Console.WriteLine($"{_.i:D5}-{_.i:X4}: {_.before} > {_.after}");
        }
    }
}

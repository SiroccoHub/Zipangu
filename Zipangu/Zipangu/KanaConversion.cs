﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Zipangu
{
    /// <summary>
    /// かなを変換するためのメソッドを提供します。
    /// </summary>
    public static class KanaConversion
    {
        /// <summary>半角カタカナ。</summary>
        internal static readonly string HalfKatakanas = EnumerableHelper.RangeChars('｡', 'ﾟ');
        /// <summary>ひらがな。</summary>
        internal const string HiraganasByHalfKatakana = "。「」、・をぁぃぅぇぉゃゅょっーあいうえおかきくけこさしすせそたちつてとなにぬねのはひふへほまみむめもやゆよらりるれろわん゛゜";

        /// <summary>半角カタカナの濁点。</summary>
        const char VoicedSoundMark = 'ﾞ';
        /// <summary>濁音となる半角カタカナ。</summary>
        internal const string VoiceableKatakanas = "ｳｶｷｸｹｺｻｼｽｾｿﾀﾁﾂﾃﾄﾊﾋﾌﾍﾎ";
        /// <summary>濁音のひらがな。</summary>
        internal const string VoicedHiraganas = "ヴがぎぐげござじずぜぞだぢづでどばびぶべぼ";

        /// <summary>半角カタカナの半濁点。</summary>
        const char SemiVoicedSoundMark = 'ﾟ';
        /// <summary>半濁音となる半角カタカナ。</summary>
        internal const string SemiVoiceableKatakanas = "ﾊﾋﾌﾍﾎ";
        /// <summary>半濁音のひらがな。</summary>
        internal const string SemiVoicedHiraganas = "ぱぴぷぺぽ";

        static readonly IDictionary<char, char> KanaMap = HalfKatakanas.ZipToDictionary(HiraganasByHalfKatakana);
        static readonly IDictionary<char, char> VoicedMap = VoiceableKatakanas.Zip(VoicedHiraganas, (x, y) => new { x, y }).ToDictionary(_ => _.x, _ => _.y);
        static readonly IDictionary<char, char> SemiVoicedMap = SemiVoiceableKatakanas.Zip(SemiVoicedHiraganas, (x, y) => new { x, y }).ToDictionary(_ => _.x, _ => _.y);

        static readonly Regex VoicedPattern = new Regex(".ﾞ");
        static readonly Regex SemiVoicedPattern = new Regex(".ﾟ");

        /// <summary>
        /// 半角カタカナをひらがなに変換します。
        /// </summary>
        /// <param name="value">変換対象の文字列。</param>
        /// <returns>変換後の文字列。</returns>
        public static string HalfKatakanaToHiragana(this string value)
        {
            if (value == null) return null;

            var current = value;
            current = VoicedPattern.Replace(current, m =>
            {
                var c = m.Value[0];
                return VoicedMap.ContainsKey(c) ? VoicedMap[c].ToString() : m.Value;
            });
            current = SemiVoicedPattern.Replace(current, m =>
            {
                var c = m.Value[0];
                return SemiVoicedMap.ContainsKey(c) ? SemiVoicedMap[c].ToString() : m.Value;
            });
            return string.Concat(current.Select(ToOneHiragana));
        }

        static char ToOneHiragana(char c) => KanaMap.ContainsKey(c) ? KanaMap[c] : c;
    }
}

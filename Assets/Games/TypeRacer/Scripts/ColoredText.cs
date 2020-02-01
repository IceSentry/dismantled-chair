using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.TypeRacer
{
    public enum TextColor
    {
        NORMAL,
        SUCCESS,
        FAILURE,
        CURSOR
    }

    public class ColoredText
    {
        public string rawText;
        public string coloredText;
        public List<ColoredCharData> list;

        public ColoredText(string text)
        {
            rawText = text;
            list = text.Select(c => new ColoredCharData(c)).ToList();
            coloredText = ComputeColoredText();
        }

        public string ComputeColoredText() => list.Aggregate("", (acc, charData) => acc + charData.coloredString);

        public void SetColorAt(int index, TextColor color)
        {
            var data = list[index];

            if (data.color == color)
                return;

            data.SetColor(color);
            coloredText = ComputeColoredText();
        }

        public class ColoredCharData
        {
            public TextColor color;
            public char c;
            public string coloredString;
            public KeyCode code;

            public ColoredCharData(char c)
            {
                this.c = c;
                code = MapCharToKeyCode(c);
                color = TextColor.NORMAL;
                coloredString = FormatColoredChar();
            }

            public void SetColor(TextColor color)
            {
                this.color = color;
                coloredString = FormatColoredChar();
            }

            private string FormatColoredChar() => 
                $"<color={GetColorFrom(color)}>{(color == TextColor.CURSOR ? "\u0331" : "") + c}</color>";

            private KeyCode MapCharToKeyCode(char c)
            {
                switch (c)
                {
                    case ' ':
                        return KeyCode.Space;
                    case ',':
                        return KeyCode.Comma;
                    case '.':
                        return KeyCode.Period;
                    default:
                        Enum.TryParse(c.ToString(), true, out KeyCode key);
                        return key;
                }
            }

            private string GetColorFrom(TextColor textColor)
            {
                switch (textColor)
                {
                    case TextColor.SUCCESS:
                        return "#5cac48";
                    case TextColor.FAILURE:
                        return "#832121";
                    case TextColor.CURSOR:
                        return "#e53366";
                    case TextColor.NORMAL:
                    default:
                        return "black";
                }
            }
        }
    }
}

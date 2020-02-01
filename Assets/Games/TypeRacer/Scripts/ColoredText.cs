using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.TypeRacer
{
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

        public void SetColorAt(int index, string color)
        {
            var data = list[index];

            if (data.color == color)
                return;

            data.SetColor(color);
            coloredText = ComputeColoredText();
        }

        public class ColoredCharData
        {
            public string color;
            public char c;
            public string coloredString;
            public KeyCode code;

            public ColoredCharData(char c)
            {
                this.c = c;
                code = MapCharToKeyCode(c);
                color = "black";
                coloredString = FormatColoredChar();
            }

            public void SetColor(string color)
            {
                this.color = color;
                coloredString = FormatColoredChar();
            }

            private string FormatColoredChar() => $"<color={color}>{c}</color>";

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
        }
    }
}

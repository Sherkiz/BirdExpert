using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine;

namespace BirdExpert
{
    public static class UIUtilities
    {
        public static TextMeshProUGUI GetText(this Button button) => button.transform.GetComponentInChildren<TextMeshProUGUI>();
        public static void SetCodeText(this Button button, string text)
        {
            TextMeshProUGUI buttonText = button.GetText();
            if (buttonText != null)
            {
                buttonText.SetCodeText(text);
            }
        }
        public static void SetCodeText(this TextMeshProUGUI text, string code) 
        {
            text.SetText(Language.GetLang(code));
            Language.OnLanguageChanged += () => text.SetText(Language.GetLang(code));
        }
        public static void SetAlpha(this Graphic image, float alpha)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
        }
        public static string FormatMinutesSeconds(this int source)
        {
            string minutes = (source / 60).ToString();
            string seconds = (source % 60).ToString();
            if (seconds.Length == 1) seconds = "0" + seconds;
            return minutes + ":" + seconds;
        }
        public static Vector2Int Size(this GridLayoutGroup grid)
        {
            int itemsCount = grid.transform.childCount;
            Vector2Int size = Vector2Int.zero;

            if (itemsCount == 0)
                return size;

            switch (grid.constraint)
            {
                case GridLayoutGroup.Constraint.FixedColumnCount:
                    size.x = grid.constraintCount;
                    size.y = GetAnotherAxisCount(itemsCount, size.x);
                    break;

                case GridLayoutGroup.Constraint.FixedRowCount:
                    size.y = grid.constraintCount;
                    size.x = GetAnotherAxisCount(itemsCount, size.y);
                    break;

                case GridLayoutGroup.Constraint.Flexible:
                    size = FlexibleSize(grid);
                    break;

                default:
                    throw new ArgumentOutOfRangeException($"Unexpected constraint: {grid.constraint}");
            }

            return size;
        }

        private static Vector2Int FlexibleSize(this GridLayoutGroup grid)
        {
            int itemsCount = grid.transform.childCount;
            float prevX = float.NegativeInfinity;
            int xCount = 0;

            for (int i = 0; i < itemsCount; i++)
            {
                Vector2 pos = ((RectTransform)grid.transform.GetChild(i)).anchoredPosition;
                if (pos.x > prevX)
                {
                    prevX = pos.x;
                    xCount++;
                }
            }

            int yCount = GetAnotherAxisCount(itemsCount, xCount);
            return new Vector2Int(xCount, yCount);
        }

        private static int GetAnotherAxisCount(int totalCount, int axisCount)
        {
            return totalCount / axisCount + Mathf.Min(1, totalCount % axisCount);
        }
        public static Vector2 RealSize(this GridLayoutGroup grid)
        {
            float XSize = grid.Size().x * (grid.cellSize.x + grid.spacing.x) + grid.padding.left + grid.padding.right - grid.spacing.x;
            float YSize = grid.Size().y * (grid.cellSize.y + grid.spacing.y) + grid.padding.top + grid.padding.bottom - grid.spacing.y;
            return new Vector2(XSize, YSize);
        }
    }
}

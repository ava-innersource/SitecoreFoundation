using Sitecore.Data.Items;
using SF.Foundation.Configuration;

namespace SF.Feature.Social
{
    public static class StringExtensions
    {
        /// <summary>
        /// Extension method for a string that given an Item will replace any placeholder defined within the string
        /// (starting with a $) with the value of the field of the same name as the placeholder. If your field
        /// name contains spaces, use underscores "_" and it will check both with underscores and spaces. 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public static string ReplacePlaceholders(this string value, Item item)
        {
            int placeholderStart = value.IndexOf('$');
            if (placeholderStart > -1)
            {
                int placeholderEnd = value.IndexOf(' ', placeholderStart);
                placeholderEnd = placeholderEnd == -1 ? value.Length : placeholderEnd;

                //advance by 1 to avoid $
                placeholderStart++;

                if (placeholderEnd <= placeholderStart)
                {
                    return value;
                }

                var placeHolder = value.Substring(placeholderStart, placeholderEnd - placeholderStart);
                if (item.HasField(placeHolder))
                {
                    if (!string.IsNullOrEmpty(item.Fields[placeHolder].Value))
                    {
                        return value.Replace("$" + placeHolder, item.Fields[placeHolder].Value);                        
                    }
                }

                placeHolder = placeHolder.Replace("_", " ");

                if (item.HasField(placeHolder))
                {
                    if (!string.IsNullOrEmpty(item.Fields[placeHolder].Value))
                    {
                        return value.Replace("$" + placeHolder, item.Fields[placeHolder].Value);
                    }
                }
            }
            return value;
        }
    }
}

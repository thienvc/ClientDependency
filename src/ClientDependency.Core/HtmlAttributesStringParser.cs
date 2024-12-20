using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Query.Dynamic;

namespace ClientDependency.Core
{
    internal static class HtmlAttributesStringParser
    {
        internal static void ParseIntoDictionary(string attributes, IDictionary<string, string> destination)
        {
            if (string.IsNullOrEmpty(attributes))
                return;

            var key = "";
            var val = "";
            var isKey = true;
            var isVal = false;
            var isValDelimited = false;
            var arrChars = attributes.ToCharArray();
            var cLength = arrChars.Length;
            var hasKeyDelim = false;
            for (var i = 0; i < cLength; i++)
            {
                var c = arrChars[i];
                if (isKey && (c == ':' || c == ','))
                {
                    isKey = false;
                    isVal = true;
                    hasKeyDelim = c == ',';
                    continue;
                }

                if (isKey)
                {
                    key += c;

                    // key without value
                    if (i == cLength - 1)
                    {
                        isKey = false;
                        isVal = true;
                        destination[key.Trim()] = null;
                        key = "";
                        val = "";
                        continue;
                    }
                }

                if (isVal)
                {
                    // key without value
                    if (hasKeyDelim)
                    {
                        hasKeyDelim = false;
                        isKey = true;
                        isVal = false;
                        destination[key.Trim()] = null;
                        key = "";
                        val = "";
                        continue;
                    }

                    if (c == '\'')
                    {
                        if (!isValDelimited)
                        {
                            isValDelimited = true;
                            continue;
                        }
                        else
                        {
                            isValDelimited = false;
                            if ((i == cLength - 1))
                            {
                                //if it the end, add/replace the value
                                destination[key.Trim()] = val;                                
                            }
                            continue;
                        }
                    }
                    
                    if (c == ',' && !isValDelimited)
                    {
                        //we've reached a comma and the value is not longer delimited, this means we create a new key
                        isKey = true;
                        isVal = false;

                        //now we can add/replace the current value to the dictionary
                        destination[key.Trim()] = val;
                        key = "";
                        val = "";
                        continue;
                    }
                    
                    val += c;

                    if ((i == attributes.Length - 1))
                    {
                        //if it the end, add/replace the value
                        destination[key] = val;
                    }
                }
            }

        }
    }
}

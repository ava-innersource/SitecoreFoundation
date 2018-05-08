using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Xml;

namespace SF.Feature.Doc.Helpers
{
    public class XDocHelper
    {
        public static string GetSummary(Type type)
        {
            try
            {
                var doc = XMLFromType(type);
                return doc["summary"].InnerText.Trim();
            }
            catch
            {
                return string.Empty;
            }
        }

        public static string GetSummary(MethodInfo methodInfo)
        {
            try
            {
                var doc = XMLFromMember(methodInfo);
                return doc["summary"].InnerText.Trim();
            }
            catch
            {
                return string.Empty;
            }
        }

        public static XmlElement XMLFromMember(MethodInfo methodInfo)
        {
            string parametersString = "";
            foreach (ParameterInfo parameterInfo in methodInfo.GetParameters())
            {
                if (parametersString.Length > 0)
                {
                    parametersString += ",";
                }

                parametersString += parameterInfo.ParameterType.FullName;
            }

            if (parametersString.Length > 0)
                return XMLFromName(methodInfo.DeclaringType, 'M', methodInfo.Name + "(" + parametersString + ")");
            else
                return XMLFromName(methodInfo.DeclaringType, 'M', methodInfo.Name);
        }

        public static XmlElement XMLFromMember(MemberInfo memberInfo)
        {
            return XMLFromName(memberInfo.DeclaringType, memberInfo.MemberType.ToString()[0], memberInfo.Name);
        }

        public static XmlElement XMLFromType(Type type)
        {
            return XMLFromName(type, 'T', "");
        }

        private static XmlElement XMLFromName(Type type, char prefix, string name)
        {
            string fullName;

            if (String.IsNullOrEmpty(name))
            {
                fullName = prefix + ":" + type.FullName;
            }
            else
            {
                fullName = prefix + ":" + type.FullName + "." + name;
            }

            XmlDocument xmlDocument = XMLFromAssembly(type.Assembly);

            XmlElement matchedElement = null;

            foreach (XmlElement xmlElement in xmlDocument["doc"]["members"])
            {
                if (xmlElement.Attributes["name"].Value.Equals(fullName))
                {
                    if (matchedElement != null)
                    {
                        throw new Exception("Multiple matches to query", null);
                    }

                    matchedElement = xmlElement;
                }
            }

            if (matchedElement == null)
            {
                throw new Exception("Could not find documentation for specified element", null);
            }

            return matchedElement;
        }

        static Dictionary<Assembly, XmlDocument> cache = new Dictionary<Assembly, XmlDocument>();

        static Dictionary<Assembly, Exception> failCache = new Dictionary<Assembly, Exception>();

        public static XmlDocument XMLFromAssembly(Assembly assembly)
        {
            if (failCache.ContainsKey(assembly))
            {
                throw failCache[assembly];
            }

            try
            {

                if (!cache.ContainsKey(assembly))
                {
                    cache[assembly] = XMLFromAssemblyNonCached(assembly);
                }

                return cache[assembly];
            }
            catch (Exception exception)
            {
                failCache[assembly] = exception;
                throw exception;
            }
        }

        private static XmlDocument XMLFromAssemblyNonCached(Assembly assembly)
        {
            string assemblyFilename = assembly.CodeBase;

            const string prefix = "file:///";

            if (assemblyFilename.StartsWith(prefix))
            {
                StreamReader streamReader;

                try
                {
                    streamReader = new StreamReader(Path.ChangeExtension(assemblyFilename.Substring(prefix.Length), ".xml"));
                }
                catch (FileNotFoundException exception)
                {
                    throw new Exception("XML documentation not present (make sure it is turned on in project properties when building)", exception);
                }

                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(streamReader);
                return xmlDocument;
            }
            else
            {
                throw new Exception("Could not ascertain assembly filename", null);
            }
        }
    }
}
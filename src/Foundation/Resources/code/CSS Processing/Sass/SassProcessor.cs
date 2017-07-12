using NSass;
using System;


namespace SF.Foundation.Resources
{
    public class SassProcessor : ICSSProcessor
    {
        public string Process(string input)
        {
            string output = input;
            try
            {
                var compiler = new SassCompiler();
                output = compiler.Compile(source:input,outputStyle: OutputStyle.Nested, sourceComments:true);

            }
            catch (Exception ex)
            {
                output += string.Format(@"/* Error Compiling Sass: {0} */", ex.ToString());
            }

            return output;
        }
    }
}

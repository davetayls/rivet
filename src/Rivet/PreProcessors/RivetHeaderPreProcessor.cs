
namespace Rivet.PreProcessors
{
    internal sealed class RivetHeaderPreProcessor : IPreProcessor
    {
        private const string Header = "/**** File Created using Rivet ****/";

        #region Implementation of IPreProcessor

        public string Process(string body, ParserOptions parserOptions)
        {
            return string.Format("{0}\n{1}", Header, body);
        }

        #endregion
    }
}

using FormulaObfuscator.BLL.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Xml;
using System.Xml.Linq;

namespace FormulaObfuscator.BLL
{
    public class ObfuscatorManager
    {
        public string UploadedText { get; set; }

        public ObfuscatorManager(string uploadedText)
        {
            this.UploadedText = uploadedText;
        }

        public Holder<XElement> obfuscate()
        {
            var reader = new HTMLReader(UploadedText);
            var htmlTreeHolder = reader.convertToTree();
            return htmlTreeHolder;
        }

    }
}

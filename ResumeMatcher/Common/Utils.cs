

using NPOI.XWPF.UserModel;
using System.Text;
using UglyToad.PdfPig;
using UglyToad.PdfPig.DocumentLayoutAnalysis.TextExtractor;
namespace ResumeMatcher.Common
{
    public static class Utils
    {
        public static StringBuilder FileToText(this IFormFile formFile)
        {
            var fileExtension = System.IO.Path.GetExtension(formFile.FileName).ToLower();
            // Determine the file type based on the extension
            if (fileExtension != null)
            {
                if (fileExtension == ".pdf")
                {
                    return PdfToString(formFile);
                }
                else if (fileExtension == ".doc")
                {
                    return DocToString(formFile);
                }
                else if (fileExtension == ".docx")
                {
                    return DocToString(formFile);
                }
                else if (fileExtension == ".txt")
                {
                    return TxtToString(formFile);
                }
            }
            return null;
        }
        private static StringBuilder TxtToString(IFormFile formFile) {
        
            using(var stream = formFile.OpenReadStream())
            {
                return new StringBuilder(new StreamReader(stream, Encoding.UTF8).ReadToEnd());
            }
        }
        private static StringBuilder DocToString(IFormFile formFile)
        {
            using (var stream = formFile.OpenReadStream())
            {
                var doc = new XWPFDocument(stream);
                StringBuilder sb = new StringBuilder();

                foreach (var paragraph in doc.Paragraphs)
                {
                    sb.AppendLine(paragraph.Text);
                }

                return sb;
            }
        }
        private static StringBuilder PdfToString(IFormFile formFile)
        {
            var text = new StringBuilder();
            using (var pdf = PdfDocument.Open(formFile.OpenReadStream()))
            {
                foreach (var page in pdf.GetPages())
                {
                    var data = ContentOrderTextExtractor.GetText(page);
                    // Either extract based on order in the underlying document with newlines and spaces.
                    text.Append(data);

                    // Or based on grouping letters into words.
                    var otherText = string.Join(" ", page.GetWords());

                    // Or the raw text of the page's content stream.
                    var rawText = page.Text;
                }
                return text;
            }
        }
    }
}
